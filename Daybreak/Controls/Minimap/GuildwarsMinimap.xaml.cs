using Daybreak.Models.Guildwars;
using Daybreak.Models;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;
using System.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Core.Extensions;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using Daybreak.Services.Pathfinding;
using Daybreak.Services.Pathfinding.Models;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using Daybreak.Services.Drawing;
using Daybreak.Services.Themes;
using Daybreak.Services.Metrics;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using Daybreak.Models.FocusView;
using Daybreak.Models.LaunchConfigurations;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Daybreak.Controls.Minimap;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Using source generators to auto-implement dependency properties")]
/// <summary>
/// Interaction logic for GuildwarsMinimap.xaml
/// </summary>
public partial class GuildwarsMinimap : UserControl
{
    private const string DrawingLatencyName = "Minimap Drawing latency";
    private const string DrawingLatencyUnitName = "ms";
    private const string DrawingLatencyDescription = "The amount of time spent drawing the minimap entities. Measured in ms";
    private const int MapDownscaleFactor = 10;
    private const int EntitySize = 100;
    private const float PositionRadius = 150;

    private readonly Histogram<double> drawingLatency;
    private readonly DispatcherTimer dispatcherTimer = new(DispatcherPriority.Render);
    private readonly List<Position> mainPlayerPositionHistory = [];
    private readonly IPathfinder pathfinder;
    private readonly IDrawingService drawingService;
    private readonly IThemeManager themeManager;
    private readonly Color outlineColor = Colors.Chocolate;
    private readonly TimeSpan offsetRevertDelay = TimeSpan.FromSeconds(3);

    private bool calculatingPathToObjectives = false;
    private bool resizeEntities;
    private bool dragging;
    private double mapVirtualMinWidth;
    private double mapVirtualMinHeight;
    private double mapWidth;
    private double mapHeight;
    private DateTime offsetRevertTime = DateTime.Now;
    private double offsetRevert = 0;
    private Point originPoint = new(0, 0);
    private Vector originOffset = new(0, 0);
    private Point initialClickPoint = new(0, 0);
    private Point lastPathfindingPoint = new(0, 0);
    private int pathfindingObjectiveCount = 0;
    private PathfindingCache? pathfindingCache;
    
    [GenerateDependencyProperty]
    private PathingData pathingData = new();
    [GenerateDependencyProperty]
    private GameData gameData = new();
    [GenerateDependencyProperty]
    private GameState gameState = new();
    [GenerateDependencyProperty]
    private double zoom = 0.08;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool drawPositionHistory = true;
    [GenerateDependencyProperty]
    private bool controlsVisible;
    [GenerateDependencyProperty]
    private bool canRotate;
    [GenerateDependencyProperty]
    private double angle;
    [GenerateDependencyProperty]
    private int targetEntityId;
    [GenerateDependencyProperty]
    private int targetEntityModelId;
    [GenerateDependencyProperty]
    private GuildWarsApplicationLaunchContext launchContext = default!;

    public event EventHandler? MaximizeClicked;
    public event EventHandler<QuestMetadata>? QuestMetadataClicked;
    public event EventHandler<LivingEntity>? LivingEntityClicked;
    public event EventHandler<PlayerInformation>? PlayerInformationClicked;
    public event EventHandler<MapIcon>? MapIconClicked;
    public event EventHandler<Profession>? ProfessionClicked;
    public event EventHandler<string?>? NpcNameClicked;

    public GuildwarsMinimap()
        :this(
             Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IPathfinder>(),
             Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IDrawingService>(),
             Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IThemeManager>(),
             Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IMetricsService>())
    {
    }

    public GuildwarsMinimap(
        IPathfinder pathfinder,
        IDrawingService drawingService,
        IThemeManager themeManager,
        IMetricsService metricsService)
    {
        this.pathfinder = pathfinder.ThrowIfNull();
        this.drawingService = drawingService.ThrowIfNull();
        this.themeManager = themeManager.ThrowIfNull();
        this.drawingLatency = metricsService.CreateHistogram<double>(DrawingLatencyName, DrawingLatencyUnitName, DrawingLatencyDescription, Models.Metrics.AggregationTypes.P95);

        this.dispatcherTimer.Tick += (_, _) => this.ApplyOffsetRevert();
        this.dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16);
        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (!this.IsEnabled)
        {
            return;
        }

        if (e.Property == ActualHeightProperty ||
            e.Property == ActualWidthProperty)
        {
            this.resizeEntities = true;
        }
        else if (e.Property == PathingDataProperty)
        {
            this.mainPlayerPositionHistory.Clear();
            this.lastPathfindingPoint = new Point(0, 0);
            this.pathfindingObjectiveCount = 0;
            this.UpdateGameData();
            this.DrawMap();
        }
        else if (e.Property == ZoomProperty)
        {
            this.UpdateGameData();
        }
        else if (e.Property == GameStateProperty &&
            this.GameState is not null)
        {
            this.UpdateStates();
            this.UpdateGameData();
        }
    }

    private void UpdateStates()
    {
        if (this.GameState is null ||
            this.GameData is null ||
            this.GameData.Valid is false)
        {
            return;
        }

        var mainPlayerState = this.GameState.States?.FirstOrDefault(state => state.Id == this.GameData.MainPlayer?.Id);
        this.GameData.MainPlayer!.Position = mainPlayerState?.Position ?? new Position();
        this.GameData.MainPlayer!.CurrentHealth = mainPlayerState?.Health ?? 0;
        this.GameData.MainPlayer!.CurrentEnergy = mainPlayerState?.Energy ?? 0;
        foreach (var worldPlayer in this.GameData.WorldPlayers!)
        {
            var worldPlayerState = this.GameState.States?.FirstOrDefault(state => state.Id == worldPlayer.Id);
            worldPlayer.Position = worldPlayerState?.Position ?? new Position();
            worldPlayer.CurrentHealth = worldPlayerState?.Health ?? 0;
            worldPlayer.CurrentEnergy = worldPlayerState?.Energy ?? 0;
        }

        foreach (var partyPlayer in this.GameData.Party!)
        {
            var partyPlayerState = this.GameState.States?.FirstOrDefault(state => state.Id == partyPlayer.Id);
            partyPlayer.Position = partyPlayerState?.Position ?? new Position();
            partyPlayer.CurrentHealth = partyPlayerState?.Health ?? 0;
            partyPlayer.CurrentEnergy = partyPlayerState?.Energy ?? 0;
        }

        foreach (var entity in this.GameData.LivingEntities!)
        {
            var state = this.GameState.States?.FirstOrDefault(state => state.Id == entity.Id);
            entity.Position = state?.Position ?? new Position();
            entity.State = state?.State ?? LivingEntityState.Unknown;
            entity.Health = state?.Health ?? 0;
            entity.Energy = state?.Energy ?? 0;
        }

        this.Angle = this.CanRotate ? (this.GameState.Camera.Yaw - (Math.PI / 2)) * (180 / Math.PI) : 0;
    }

    private void UpdateGameData()
    {
        if(this.GameData is null ||
            this.GameData.Valid is false)
        {
            return;
        }

        if (!double.IsFinite(this.mapWidth) ||
            !double.IsFinite(this.mapHeight) ||
            !double.IsFinite(this.mapVirtualMinWidth) ||
            !double.IsFinite(this.mapVirtualMinHeight) ||
            this.GameData.MainPlayer?.Position is null)
        {
            return;
        }

        this.TargetEntityId = this.GameData.CurrentTargetId ?? 0;
        this.TargetEntityModelId = (int?)this.GameData.LivingEntities?.FirstOrDefault(e => e.Id == this.TargetEntityId)?.ModelType ?? 0;
        var screenVirtualWidth = this.ActualWidth / this.Zoom;
        var screenVirtualHeight = this.ActualHeight / this.Zoom;
        var position = this.GameData?.MainPlayer?.Position ?? new Position();
        this.originPoint = new Point(
            position.X - (screenVirtualWidth / 2) - (this.originOffset.X / this.Zoom),
            position.Y + (screenVirtualHeight / 2) + (this.originOffset.Y / this.Zoom));

        var adjustedPosition = new Point((int)((position.X - this.mapVirtualMinWidth) * this.Zoom), (int)(this.mapHeight - position.Y + this.mapVirtualMinHeight) * this.Zoom);
        
        this.MapDrawingHost.Margin = new Thickness(
            -adjustedPosition.X + (this.ActualWidth / 2) + this.originOffset.X,
            -adjustedPosition.Y + (this.ActualHeight / 2) + this.originOffset.Y,
            0,
            0);
        this.MapDrawingHost.Height = this.mapHeight * this.Zoom;
        this.MapDrawingHost.Width = this.mapWidth * this.Zoom;
        this.ManageMainPlayerPositionHistory();
        this.DrawEntities();
    }

    private void ManageMainPlayerPositionHistory()
    {
        if (this.GameData is null)
        {
            return;
        }

        var currentPosition = this.GameData?.MainPlayer?.Position ?? throw new InvalidOperationException("Unexpected main player null position");
        if (this.mainPlayerPositionHistory.Any(oldPosition => PositionsCollide(oldPosition, currentPosition)))
        {
            return;
        }

        this.mainPlayerPositionHistory.Add(currentPosition);
    }

    private void DrawMap()
    {
        if (this.PathingData is null ||
            this.PathingData.Trapezoids is null)
        {
            return;
        }

        var maxWidth = double.MinValue;
        var maxHeight = double.MinValue;
        var minWidth = double.MaxValue;
        var minHeight = double.MaxValue;
        foreach (var trapezoid in this.PathingData.Trapezoids!)
        {
            if (trapezoid.YT < minHeight)
            {
                minHeight = trapezoid.YT;
            }

            if (trapezoid.YB > maxHeight)
            {
                maxHeight = trapezoid.YB;
            }

            if (trapezoid.XTL < minWidth)
            {
                minWidth = trapezoid.XTL;
            }

            if (trapezoid.XBL < minWidth)
            {
                minWidth = trapezoid.XBL;
            }

            if (trapezoid.XTR > maxWidth)
            {
                maxWidth = trapezoid.XTR;
            }

            if (trapezoid.XBR > maxWidth)
            {
                maxWidth = trapezoid.XBR;
            }
        }

        var width = maxWidth - minWidth;
        var height = maxHeight - minHeight;
        this.mapVirtualMinHeight = minHeight;
        this.mapVirtualMinWidth = minWidth;
        this.mapWidth = width;
        this.mapHeight = height;

        if (width <= 0 || height <= 0 ||
            !double.IsFinite(width) ||
            !double.IsFinite(height))
        {
            return;
        }

        var bitmap = BitmapFactory.New((int)(width / MapDownscaleFactor), (int)(height / MapDownscaleFactor));
        this.MapDrawingHost.Source = bitmap;
        this.MapDrawingHost.Width = width / MapDownscaleFactor;
        this.MapDrawingHost.Height = height / MapDownscaleFactor;

        using var bitmapContext = bitmap.GetBitmapContext();
        bitmap.Clear(Colors.Transparent);
        var color = this.themeManager.GetForegroundColor();
        foreach (var trapezoid in this.PathingData.Trapezoids)
        {
            var a = new Point((int)((trapezoid.XTL - minWidth) / MapDownscaleFactor), (int)((height - trapezoid.YT + minHeight) / MapDownscaleFactor));
            var b = new Point((int)((trapezoid.XTR - minWidth) / MapDownscaleFactor), (int)((height - trapezoid.YT + minHeight) / MapDownscaleFactor));
            var c = new Point((int)((trapezoid.XBR - minWidth) / MapDownscaleFactor), (int)((height - trapezoid.YB + minHeight) / MapDownscaleFactor));
            var d = new Point((int)((trapezoid.XBL - minWidth) / MapDownscaleFactor), (int)((height - trapezoid.YB + minHeight) / MapDownscaleFactor));

            bitmap.FillPolygon(new int[] { (int)a.X, (int)a.Y, (int)b.X, (int)b.Y, (int)c.X, (int)c.Y, (int)d.X, (int)d.Y, (int)a.X, (int)a.Y }, color);
        }
    }

    private void DrawEntities()
    {
        if (this.ActualWidth == 0 || this.ActualHeight == 0)
        {
            return;
        }

        if (this.EntitiesDrawingHost.Source is not WriteableBitmap bitmap ||
            this.resizeEntities ||
            bitmap.PixelHeight != (int)this.ActualHeight ||
            bitmap.PixelWidth != (int)this.ActualWidth)
        {
            bitmap = BitmapFactory.New((int)this.ActualWidth, (int)this.ActualHeight);
            this.EntitiesDrawingHost.Source = bitmap;
            this.resizeEntities = false;
        }

        if (this.TestDrawingHost.Source is not WriteableBitmap testBitmap ||
            this.resizeEntities ||
            testBitmap.PixelHeight != (int)this.ActualHeight ||
            testBitmap.PixelWidth != (int)this.ActualWidth)
        {
            testBitmap = BitmapFactory.New((int)this.ActualWidth, (int)this.ActualHeight);
            this.TestDrawingHost.Source = testBitmap;
            this.resizeEntities = false;
        }

        this.CalculatePathsToObjectives();

        var foregroundColor = this.FindResource("MahApps.Colors.ThemeBackground").Cast<Color>();
        bitmap.Clear(Colors.Transparent);
        using var context = bitmap.GetBitmapContext();
        var sw = Stopwatch.StartNew();
        bitmap.Lock();
        this.drawingService.UpdateDrawingParameters(
            (int)this.ActualWidth,
            (int)this.ActualHeight,
            this.originPoint,
            this.Zoom,
            PositionRadius,
            EntitySize,
            foregroundColor);
        this.drawingService.DrawEngagementArea(bitmap, this.GameData);
        this.drawingService.DrawMainPlayerPositionHistory(bitmap, this.mainPlayerPositionHistory);
        this.drawingService.DrawPaths(bitmap, this.pathfindingCache);
        this.drawingService.DrawQuestObjectives(bitmap, this.GameData.MainPlayer?.QuestLog ?? []);
        this.drawingService.DrawMapIcons(bitmap, this.GameData.MapIcons ?? []);
        this.drawingService.DrawEntities(bitmap, this.GameData, this.TargetEntityId);
        bitmap.Unlock();
        this.drawingLatency.Record(sw.ElapsedMilliseconds);

        //TODO: Delete
        using var testContext = testBitmap.GetBitmapContext();
        testBitmap.Clear(Colors.Transparent);
        foreach (var entity in this.GameData.LivingEntities!)
        {
            var x = (int)((entity.Position!.Value.X - this.originPoint.X) * this.Zoom);
            var y = 0 - (int)((entity.Position!.Value.Y - this.originPoint.Y) * this.Zoom);
            var entityPoint = new Point(x, y);
            var finalEntityPoint = this.RotateTransform.Transform(entityPoint);
            testBitmap.DrawEllipseCentered((int)finalEntityPoint.X, (int)finalEntityPoint.Y, 10, 10, Colors.Red);
        }
    }

    private bool MouseOverEntity(IPositionalEntity entity, Point mousePosition)
    {
        var x = (int)((entity.Position!.Value.X - this.originPoint.X) * this.Zoom);
        var y = 0 - (int)((entity.Position!.Value.Y - this.originPoint.Y) * this.Zoom);
        var entityPoint = new Point(x, y);
        var finalEntityPoint = this.RotateTransform.Transform(entityPoint);
        return Math.Pow(mousePosition.X - finalEntityPoint.X, 2) + Math.Pow(mousePosition.Y - finalEntityPoint.Y, 2) < Math.Pow(EntitySize * this.Zoom, 2);
    }

    private void DragMinimap()
    {
        if (this.dragging is false)
        {
            return;
        }

        var rad = -this.Angle * Math.PI / 180;
        var mousePosition = Mouse.GetPosition(this);
        var offset = mousePosition - this.initialClickPoint;
        var transformedOffset = new Vector((offset.X * Math.Cos(rad)) - (offset.Y * Math.Sin(rad)),
                                            (offset.X * Math.Sin(rad)) + (offset.Y * Math.Cos(rad)));
        this.offsetRevert = 0;
        this.originOffset = transformedOffset;
        this.offsetRevertTime = DateTime.Now + this.offsetRevertDelay;
        this.UpdateGameData();
    }

    private void ApplyOffsetRevert()
    {
        if (DateTime.Now < this.offsetRevertTime)
        {
            return;
        }

        if (this.ContextMenu?.IsOpen is true)
        {
            return;
        }

        this.offsetRevert = 0.0001 + (this.offsetRevert * 1.0001);
        this.offsetRevert = Math.Min(99, this.offsetRevert);
        this.originOffset /= 1 + this.offsetRevert;
        this.UpdateGameData();
    }

    private async void CalculatePathsToObjectives()
    {
        if (this.calculatingPathToObjectives)
        {
            return;
        }

        this.calculatingPathToObjectives = true;
        var currentMapQuests = this.GameData.MainPlayer?.QuestLog?.Where(q => IsValidPositionalEntity(q)).ToList();
        if (currentMapQuests is null)
        {
            this.calculatingPathToObjectives = false;
            return;
        }

        var playerPosition = new Point
        {
            X = this.GameData?.MainPlayer?.Position?.X ?? 0,
            Y = this.GameData?.MainPlayer?.Position?.Y ?? 0,
        };

        /*
         * If we already calculated paths for the current objectives, return early.
         */
        if (currentMapQuests.Count == this.pathfindingObjectiveCount &&
            (playerPosition - this.lastPathfindingPoint).Length < PositionRadius + PositionRadius)
        {
            this.calculatingPathToObjectives = false;
            return;
        }

        this.pathfindingObjectiveCount = currentMapQuests.Count;
        this.lastPathfindingPoint = playerPosition;
        var pathfindingTasks = currentMapQuests
            .Select(questMetaData =>
            {
                return this.pathfinder.CalculatePath(
                    this.PathingData,
                    playerPosition,
                    new Point
                    {
                        X = questMetaData.Position!.Value.X,
                        Y = questMetaData.Position!.Value.Y
                    },
                    CancellationToken.None);
            })
            .ToList();

        await Task.WhenAll(pathfindingTasks);
        var paths = new List<PathfindingResponse>();
        var colors = new List<Color>();
        for(var i = 0; i < currentMapQuests.Count; i++)
        {
            var quest = currentMapQuests[i];
            var result = await pathfindingTasks[i];
            if (result.TryExtractSuccess(out var pathfindingResponse))
            {
                paths.Add(pathfindingResponse!);
                var questColor = GetQuestColor(quest);
                var pathColor = Color.FromArgb(100, questColor.R, questColor.G, questColor.B);
                colors.Add(pathColor);
            }
        }

        this.pathfindingCache = new PathfindingCache { PathfindingResponses = paths, Colors = colors };
        this.calculatingPathToObjectives = false;
    }

    private IPositionalEntity? CheckMouseOverEntity(IEnumerable<IPositionalEntity>? maybeEntities)
    {
        if (maybeEntities is not IEnumerable<IPositionalEntity> entities)
        {
            return default;
        }

        if (this.GameData.Valid is false)
        {
            return default;
        }

        var mousePoint = Mouse.GetPosition(this);
        foreach(var entity in entities)
        {
            if (this.MouseOverEntity(entity, mousePoint))
            {
                this.Cursor = Cursors.Hand;
                return entity;
            }
        }

        this.Cursor = default;
        return default;
    }

    private Position ForceOnScreenPosition(Position entityPosition)
    {
        var screenVirtualWidth = this.ActualWidth / this.Zoom;
        var screenVirtualHeight = this.ActualHeight / this.Zoom;
        var finalEntitySize = EntitySize * this.Zoom;
        return new Position
        {
            X = (float)Math.Clamp(
                entityPosition.X,
                Math.Min(this.originPoint.X + finalEntitySize, this.originPoint.X + screenVirtualWidth - finalEntitySize),
                Math.Max(this.originPoint.X + finalEntitySize, this.originPoint.X + screenVirtualWidth - finalEntitySize)),
            Y = (float)Math.Clamp(
                entityPosition.Y,
                Math.Min(this.originPoint.Y - screenVirtualHeight + finalEntitySize, this.originPoint.Y - finalEntitySize),
                Math.Max(this.originPoint.Y - screenVirtualHeight + finalEntitySize, this.originPoint.Y - finalEntitySize)),
        };
    }

    private void GuildwarsMinimap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.initialClickPoint = Mouse.GetPosition(this);
        this.offsetRevert = 0;
        this.offsetRevertTime = DateTime.Now + this.offsetRevertDelay;
        this.dragging = true;
    }

    private void GuildwarsMinimap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.dragging = false;
    }

    private void GuildwarsMinimap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var maybeOffScreenQuestMetadata = this.CheckMouseOverEntity(
            this.GameData.MainPlayer?.QuestLog?
                .Where(q => IsValidPositionalEntity(q))
                .Where(entity => !this.drawingService.IsEntityOnScreen(entity.Position, out _, out _))
                .Select(oldQuestMetadata =>
                {
                    var onScreenPosition = this.ForceOnScreenPosition(oldQuestMetadata.Position!.Value);
                    return new QuestMetadata
                    {
                        From = oldQuestMetadata.From,
                        To = oldQuestMetadata.To,
                        Quest = oldQuestMetadata.Quest,
                        Position = onScreenPosition
                    };
                })
                .OfType<IPositionalEntity>());
        if (maybeOffScreenQuestMetadata is QuestMetadata offscreenQuestMetadata &&
            this.TryFindResource("QuestContextMenu") is ContextMenu offscreenQuestContextMenu)
        {
            this.ContextMenu = offscreenQuestContextMenu;
            this.ContextMenu.DataContext = offscreenQuestMetadata;
            this.ContextMenu.IsOpen = true;
            return;
        }

        var maybeQuestMetadata = this.CheckMouseOverEntity(
            this.GameData.MainPlayer?.QuestLog?
                .OfType<IPositionalEntity>().Where(IsValidPositionalEntity)
                .Where(entity => this.drawingService.IsEntityOnScreen(entity.Position, out _, out _)));
        if (maybeQuestMetadata is QuestMetadata questMetadata &&
            this.TryFindResource("QuestContextMenu") is ContextMenu questContextMenu)
        {
            this.ContextMenu = questContextMenu;
            this.ContextMenu.DataContext = questMetadata;
            this.ContextMenu.IsOpen = true;
            return;
        }

        if (this.CheckMouseOverEntity(Enumerable.Repeat(this.GameData.MainPlayer!.As<IEntity>()!, 1)) is MainPlayerInformation &&
            this.TryFindResource("MainPlayerContextMenu") is ContextMenu mainPlayerContextMenu)
        {
            this.ContextMenu = mainPlayerContextMenu;
            this.ContextMenu.DataContext = this.GameData.MainPlayer;
            this.ContextMenu.IsOpen = true;
            return;
        }

        var maybeWorldPlayer = this.CheckMouseOverEntity(this.GameData.WorldPlayers?.OfType<IEntity>().Where(IsValidPositionalEntity).OrderByDescending(p => p.Id == this.TargetEntityId));
        if (maybeWorldPlayer is WorldPlayerInformation worldPlayerInformation &&
            this.TryFindResource("WorldPlayerContextMenu") is ContextMenu worldPlayerContextMenu)
        {
            this.ContextMenu = worldPlayerContextMenu;
            this.ContextMenu.DataContext = worldPlayerInformation;
            this.ContextMenu.IsOpen = true;
            return;
        }

        var maybePartyMember = this.CheckMouseOverEntity(this.GameData.Party?.OfType<IEntity>().Where(IsValidPositionalEntity).OrderByDescending(p => p.Id == this.TargetEntityId));
        if (maybePartyMember is PlayerInformation partyMember &&
            this.TryFindResource("PlayerContextMenu") is ContextMenu playerContextMenu &&
            this.LaunchContext is not null)
        {
            this.ContextMenu = playerContextMenu;
            this.ContextMenu.DataContext = new PlayerContextMenuContext { Player = partyMember, GuildWarsApplicationLaunchContext = this.LaunchContext };
            this.ContextMenu.IsOpen = true;
            return;
        }

        var maybeLivingEntity = this.CheckMouseOverEntity(this.GameData.LivingEntities?.OfType<IEntity>().Where(IsValidPositionalEntity).OrderByDescending(p => p.Id == this.TargetEntityId));
        if (maybeLivingEntity is LivingEntity livingEntity &&
            this.TryFindResource("LivingEntityContextMenu") is ContextMenu livingEntityContextMenu &&
            this.LaunchContext is not null)
        {
            this.ContextMenu = livingEntityContextMenu;
            this.ContextMenu.DataContext = new LivingEntityContextMenuContext { LivingEntity = livingEntity, GuildWarsApplicationLaunchContext = this.LaunchContext };
            this.ContextMenu.IsOpen = true;
            return;
        }

        var maybeMapIcon = this.CheckMouseOverEntity(this.GameData.MapIcons?.OfType<IPositionalEntity>().Where(IsValidPositionalEntity));
        if (maybeMapIcon is MapIcon mapIcon &&
            this.TryFindResource("MapIconContextMenu") is ContextMenu mapIconContextMenu)
        {
            this.ContextMenu = mapIconContextMenu;
            this.ContextMenu.DataContext = mapIcon;
            this.ContextMenu.IsOpen = true;
            return;
        }

        this.ContextMenu = default;
    }

    private void GuildwarsMinimap_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
    }

    private void GuildwarsMinimap_MouseMove(object sender, MouseEventArgs e)
    {
        if (this.GameData is null ||
            this.GameData.Valid is false ||
            this.GameData.MainPlayer is null ||
            this.GameData.Party is null ||
            this.GameData.WorldPlayers is null)
        {
            return;
        }

        this.DragMinimap();
        if (this.CheckMouseOverEntity(this.GameData.Party.OfType<IEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.WorldPlayers.OfType<IEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(Enumerable.Repeat(this.GameData.MainPlayer.Cast<IEntity>(), 1)) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.LivingEntities?.OfType<IEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.MapIcons?.OfType<IPositionalEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.MainPlayer.QuestLog?
                .Where(entity => this.drawingService.IsEntityOnScreen(entity.Position, out _, out _))
                .OfType<IPositionalEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.MainPlayer.QuestLog?
                .Where(entity => !this.drawingService.IsEntityOnScreen(entity.Position, out _, out _))
                .Select(oldQuestMetadata =>
                {
                    var onScreenPosition = this.ForceOnScreenPosition(oldQuestMetadata.Position!.Value);
                    return new QuestMetadata
                    {
                        From = oldQuestMetadata.From,
                        To = oldQuestMetadata.To,
                        Quest = oldQuestMetadata.Quest,
                        Position = onScreenPosition
                    };
                })
                .OfType<IPositionalEntity>()) is not null)
        {
            return;
        }
    }

    private void GuildwarsMinimap_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        var delta = e.Delta > 0 ? 0.1 : -0.1;
        var previousZoom = this.Zoom;
        var newZoom = this.Zoom + (this.Zoom * delta);
        this.originOffset *= newZoom / previousZoom;
        this.initialClickPoint = new Point(
            this.initialClickPoint.X * newZoom / previousZoom,
            this.initialClickPoint.Y * newZoom / previousZoom);

        this.Zoom = newZoom;
    }

    private void GuildwarsMinimap_Loaded(object sender, RoutedEventArgs e)
    {
        this.dispatcherTimer.Start();
        this.DrawMap();
    }

    private void GuildwarsMinimap_Unloaded(object sender, RoutedEventArgs e)
    {
        this.dispatcherTimer.Stop();
    }

    private void QuestContextMenu_QuestContextMenuClicked(object _, QuestMetadata? quest)
    {
        if (quest is null)
        {
            return;
        }

        this.QuestMetadataClicked?.Invoke(this, quest);
    }

    private void PlayerContextMenu_PlayerContextMenuClicked(object _, (PlayerInformation? Player, string? Name) tuple)
    {
        if (tuple.Name?.IsNullOrWhiteSpace() is false)
        {
            this.NpcNameClicked?.Invoke(this, tuple.Name);
            return;
        }

        if (tuple.Player is null)
        {
            return;
        }

        this.PlayerInformationClicked?.Invoke(this, tuple.Player);
    }

    private void LivingEntityContextMenu_LivingEntityContextMenuClicked(object _, (LivingEntity? LivingEntity, string? Name) tuple)
    {
        if (tuple.Name?.IsNullOrWhiteSpace() is false)
        {
            this.NpcNameClicked?.Invoke(this, tuple.Name);
            return;
        }

        if (tuple.LivingEntity is null)
        {
            return;
        }

        this.LivingEntityClicked?.Invoke(this, tuple.LivingEntity);
    }

    private void LivingEntityContextMenu_LivingEntityProfessionContextMenuClicked(object _, Profession? e)
    {
        if (e is not Profession profession)
        {
            return;
        }

        this.ProfessionClicked?.Invoke(this, profession);
    }

    private void MapIconContextMenu_MapIconContextMenuClicked(object _, MapIcon? mapIcon)
    {
        if (mapIcon is null)
        {
            return;
        }

        this.MapIconClicked?.Invoke(this, mapIcon);
    }

    private void MaximizeButton_Clicked(object sender, EventArgs e)
    {
        this.MaximizeClicked?.Invoke(this, e);
        if (e is  MouseButtonEventArgs mouseButtonEventArgs)
        {
            mouseButtonEventArgs.Handled = true;
        }
    }

    private static Point RotatePointAroundPivot(Point point, Point pivot, double angle)
    {
        // Translate the point to the pivot point
        point = new Point(point.X - pivot.X, point.Y - pivot.Y);

        // Convert angle to radians for the rotation
        double radians = Math.PI / 180 * angle;

        // Rotate the point around the pivot
        double rotatedX = (point.X * Math.Cos(radians)) - (point.Y * Math.Sin(radians));
        double rotatedY = (point.X * Math.Sin(radians)) + (point.Y * Math.Cos(radians));

        // Translate the point back
        rotatedX += pivot.X;
        rotatedY += pivot.Y;

        return new Point(rotatedX, rotatedY);
    }


    private static bool PositionsCollide(Position position1, Position position2)
    {
        var a = PositionRadius + PositionRadius;
        var dx = position1.X - position2.X;
        var dy = position1.Y - position2.Y;
        return a * a > ((dx * dx) + (dy * dy));
    }

    private static bool IsValidPositionalEntity(IPositionalEntity entity)
    {
        if (entity.Position?.X == 0 &&
            entity.Position?.Y == 0)
        {
            return false;
        }

        return true;
    }

    private static Color GetQuestColor(QuestMetadata questMetadata)
    {
        return ColorPalette.Colors[questMetadata.Quest?.Id % ColorPalette.Colors.Count ?? 0];
    }
}
