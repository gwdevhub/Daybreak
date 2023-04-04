using Daybreak.Models.Guildwars;
using Daybreak.Models;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;
using System.Extensions;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.DependencyInjection;
using System.Core.Extensions;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using Daybreak.Services.Pathfinding;
using Daybreak.Services.Pathfinding.Models;
using System.Numerics;
using System.Windows.Threading;
using Daybreak.Utils;

namespace Daybreak.Controls;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Using source generators to auto-implement dependency properties")]
/// <summary>
/// Interaction logic for GuildwarsMinimap.xaml
/// </summary>
public partial class GuildwarsMinimap : UserControl
{
    private const int MapDownscaleFactor = 10;
    private const int EntitySize = 100;
    private const float PositionRadius = 150;

    private static readonly Lazy<(Point[] OuterPoints, Point[] InnerPoints)> StarCoordinates = new(GetStarGlyphPoints);

    private readonly DispatcherTimer dispatcherTimer = new(DispatcherPriority.ApplicationIdle);
    private readonly List<Position> mainPlayerPositionHistory = new();
    private readonly IPathfinder pathfinder;
    private readonly IGuildwarsEntityDebouncer guildwarsEntityDebouncer;
    private readonly Color positionHistoryColor = Color.FromArgb(155, Colors.Red.R, Colors.Red.G, Colors.Red.B);
    private readonly TimeSpan offsetRevertDelay = TimeSpan.FromSeconds(3);

    private bool resizeEntities;
    private bool dragging;
    private double mapVirtualMinWidth;
    private double mapVirtualMinHeight;
    private double mapWidth;
    private double mapHeight;
    private DateTime offsetRevertTime = DateTime.Now;
    private double offsetRevert = 0;
    private Point originPoint = new(0, 0);
    private System.Windows.Vector originOffset = new(0, 0);
    private Point initialClickPoint = new(0, 0);
    private DebounceResponse? cachedDebounceResponse;
    
    [GenerateDependencyProperty]
    private PathingData pathingData = new();
    [GenerateDependencyProperty]
    private GameData gameData = new();
    [GenerateDependencyProperty]
    private double zoom = 0.08;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool drawPositionHistory = true;
    [GenerateDependencyProperty]
    private bool controlsVisible;

    public event EventHandler? MaximizeClicked;
    public event EventHandler<QuestMetadata>? QuestMetadataClicked;

    public GuildwarsMinimap()
        :this(
             Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IPathfinder>(),
             Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IGuildwarsEntityDebouncer>())
    {
    }

    public GuildwarsMinimap(
        IPathfinder pathfinder,
        IGuildwarsEntityDebouncer guildwarsEntityDebouncer)
    {
        this.pathfinder = pathfinder.ThrowIfNull();
        this.guildwarsEntityDebouncer = guildwarsEntityDebouncer.ThrowIfNull();

        this.dispatcherTimer.Tick += (_, _) => this.ApplyOffsetRevert();
        this.dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16);
        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == ActualHeightProperty ||
            e.Property == ActualWidthProperty)
        {
            this.resizeEntities = true;
        }
        else if (e.Property == PathingDataProperty)
        {
            this.guildwarsEntityDebouncer.ClearCaches();
            this.mainPlayerPositionHistory.Clear();
            this.UpdateGameData();
            this.DrawMap();
        }
        else if (e.Property == ZoomProperty)
        {
            this.UpdateGameData();
        }
        else if (e.Property == GameDataProperty &&
                this.GameData.Valid)
        {
            this.UpdateGameData();
        }
    }

    private void UpdateGameData()
    {
        if(this.GameData.Valid is false)
        {
            return;
        }

        var debounceResponse = this.guildwarsEntityDebouncer.DebounceEntities(this.GameData);
        if (!double.IsFinite(this.mapWidth) ||
            !double.IsFinite(this.mapHeight) ||
            !double.IsFinite(this.mapVirtualMinWidth) ||
            !double.IsFinite(this.mapVirtualMinHeight))
        {
            return;
        }

        var screenVirtualWidth = this.ActualWidth / this.Zoom;
        var screenVirtualHeight = this.ActualHeight / this.Zoom;
        var position = debounceResponse.MainPlayer.Position!.Value;
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
        this.MapPathfindingHost.Margin = this.MapDrawingHost.Margin;
        this.MapPathfindingHost.Height = this.mapHeight * this.Zoom;
        this.MapPathfindingHost.Width = this.mapWidth * this.Zoom;
        this.ManageMainPlayerPositionHistory();
        this.DrawEntities(debounceResponse);
        this.cachedDebounceResponse = debounceResponse;
    }

    private void ManageMainPlayerPositionHistory()
    {
        if (this.cachedDebounceResponse is null)
        {
            return;
        }

        var currentPosition = this.cachedDebounceResponse.MainPlayer.Position ?? throw new InvalidOperationException("Unexpected main player null position");
        if (this.mainPlayerPositionHistory.Any(oldPosition => PositionsCollide(oldPosition, currentPosition)))
        {
            return;
        }

        this.mainPlayerPositionHistory.Add(currentPosition);
    }

    private void DrawMap()
    {
        if (this.PathingData.Trapezoids is null)
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

        var colors = new Color[this.PathingData.Trapezoids.Count];
        foreach (var trapezoid in this.PathingData.Trapezoids!)
        {
            var color = Color.FromArgb(255, (byte)(2.55 * Random.Shared.Next(0, 100)), (byte)(2.55 * Random.Shared.Next(0, 100)), (byte)(2.55 * Random.Shared.Next(0, 100)));

            if (colors[trapezoid.Id] == default)
            {
                var queue = new Queue<int>();
                queue.Enqueue(trapezoid.Id);
                while(queue.TryDequeue(out var currentId))
                {
                    if (colors[currentId] != default)
                    {
                        continue;
                    }

                    colors[currentId] = color;
                    foreach(var adjacentId in this.PathingData.AdjacencyArray[currentId])
                    {
                        queue.Enqueue(adjacentId);
                    }
                }
            }

            color = colors[trapezoid.Id];
            var a = new Point((int)((trapezoid.XTL - minWidth) / MapDownscaleFactor), (int)((height - trapezoid.YT + minHeight) / MapDownscaleFactor));
            var b = new Point((int)((trapezoid.XTR - minWidth) / MapDownscaleFactor), (int)((height - trapezoid.YT + minHeight) / MapDownscaleFactor));
            var c = new Point((int)((trapezoid.XBR - minWidth) / MapDownscaleFactor), (int)((height - trapezoid.YB + minHeight) / MapDownscaleFactor));
            var d = new Point((int)((trapezoid.XBL - minWidth) / MapDownscaleFactor), (int)((height - trapezoid.YB + minHeight) / MapDownscaleFactor));

            bitmap.FillPolygon(new int[] { (int)a.X, (int)a.Y, (int)b.X, (int)b.Y, (int)c.X, (int)c.Y, (int)d.X, (int)d.Y, (int)a.X, (int)a.Y }, color);
        }
    }
    
    private void DrawPath(WriteableBitmap bitmap, params (PathfindingResponse PathfindingResponse, Color Color)[] paths)
    {
        if (this.mapWidth <= 0 ||
            this.mapHeight <= 0 ||
            !double.IsFinite(this.mapWidth) ||
            !double.IsFinite(this.mapHeight))
        {
            return;
        }

        if (paths.Length <= 0)
        {
            return;
        }

        foreach((var response, var color) in paths)
        {
            foreach (var segment in response.Pathing!)
            {
                var startPosition = new Position { X = (float)segment.StartPoint.X, Y = (float)segment.StartPoint.Y };
                var endPosition = new Position { X = (float)segment.EndPoint.X, Y = (float)segment.EndPoint.Y };
                this.EntityOnScreen(startPosition, bitmap, out var startX, out var startY);
                this.EntityOnScreen(endPosition, bitmap, out var endX, out var endY);
                bitmap.DrawLineAa(startX, startY, endX, endY, color, (int)(EntitySize * this.Zoom));
            }
        }
    }

    private void DrawEntities(DebounceResponse debounceResponse)
    {
        if (this.EntitiesDrawingHost.Source is not WriteableBitmap bitmap || this.resizeEntities)
        {
            bitmap = BitmapFactory.New((int)this.ActualWidth, (int)this.ActualHeight);
            this.EntitiesDrawingHost.Source = bitmap;
            this.resizeEntities = false;
        }

        bitmap.Clear(Colors.Transparent);
        using var context = bitmap.GetBitmapContext();

        this.DrawMainPlayerPositionHistory(bitmap);

        this.FillEllipse(debounceResponse.MainPlayer.Position, bitmap, Colors.Green);

        foreach (var partyMember in debounceResponse.Party.Where(p => IsValidPositionalEntity(p)))
        {
            this.FillEllipse(partyMember.Position, bitmap, Colors.Green);
        }

        foreach (var player in debounceResponse.WorldPlayers.Where(p => IsValidPositionalEntity(p)))
        {
            this.FillEllipse(player.Position, bitmap, Colors.CornflowerBlue);
        }

        foreach (var livingEntity in debounceResponse.LivingEntities.Where(p => IsValidPositionalEntity(p)))
        {
            if (livingEntity.State is LivingEntityState.ToBeCleanedUp)
            {
                continue;
            }
            else if (livingEntity.State is LivingEntityState.Dead)
            {
                this.FillEllipse(livingEntity.Position, bitmap, Colors.Gray);
            }
            else if (livingEntity.State is LivingEntityState.Boss)
            {
                this.FillStar(livingEntity.Position, bitmap, Colors.DarkRed);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.AllyNonAttackable)
            {
                this.FillEllipse(livingEntity.Position, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Neutral)
            {
                this.FillEllipse(livingEntity.Position, bitmap, Colors.LightSteelBlue);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Enemy)
            {
                this.FillTriangle(livingEntity.Position, bitmap, Colors.Red);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.SpiritOrPet)
            {
                this.FillTriangle(livingEntity.Position, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Minion)
            {
                this.FillTriangle(livingEntity.Position, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.NpcOrMinipet)
            {
                this.FillTriangle(livingEntity.Position, bitmap, Colors.LimeGreen);
            }
        }

        this.DrawQuestObjectives(bitmap, debounceResponse);
    }

    private void DrawQuestObjectives(WriteableBitmap writeableBitmap, DebounceResponse debounceResponse)
    {
        if (this.PathingData.Trapezoids is null)
        {
            return;
        }

        var currentMapQuests = this.GameData.MainPlayer!.Value.QuestLog!
            .Where(v => v.Position.GetValueOrDefault().X != 0 && v.Position.GetValueOrDefault().Y != 0);
        foreach (var questMetaData in currentMapQuests)
        {
            var position = this.ForceOnScreenPosition(questMetaData.Position!.Value);
            if (!this.EntityOnScreen(questMetaData.Position, writeableBitmap, out var x, out var y))
            {
                var questPoint = new Point(questMetaData.Position!.Value.X, questMetaData.Position.Value.Y);
                

                this.FillStar(position, writeableBitmap, Colors.BlueViolet);
            }
            else
            {
                this.FillStar(questMetaData.Position, writeableBitmap, Colors.BlueViolet);
            }
        }

        //var pathingResponses = currentMapQuests
        //    .Select(questMetaData =>
        //    {
        //        return this.pathfinder.CalculatePath(
        //            this.PathingData,
        //            new Point
        //            {
        //                X = debounceResponse.MainPlayer.Position!.Value.X,
        //                Y = debounceResponse.MainPlayer.Position!.Value.Y,
        //            },
        //            new Point
        //            {
        //                X = questMetaData.Position!.Value.X,
        //                Y = questMetaData.Position!.Value.Y
        //            });
        //    })
        //    .Select(result =>
        //    {
        //        if (result.TryExtractSuccess(out var response))
        //        {
        //            return response;
        //        }

        //        return default;
        //    })
        //    .OfType<PathfindingResponse>()
        //    .Select(response => (response, Colors.DarkOrange))
        //    .ToArray();

        //this.DrawPath(writeableBitmap, pathingResponses);
    }

    private void DrawMainPlayerPositionHistory(WriteableBitmap writeableBitmap)
    {
        if (!this.DrawPositionHistory)
        {
            return;
        }

        if (this.cachedDebounceResponse is null)
        {
            return;
        }

        foreach(var position in this.mainPlayerPositionHistory)
        {
            this.FillEllipse(position, writeableBitmap, this.positionHistoryColor);
        }
    }

    private void FillStar(Position? position, WriteableBitmap bitmap, Color color)
    {
        if (!this.EntityOnScreen(position, bitmap, out var x, out var y))
        {
            return;
        }

        (var outerPoints, var innerPoints) = StarCoordinates.Value;

        for(var i = 1; i <= 5; i++)
        {
            var outerPoint = outerPoints[i % 5];
            var innerPointPrev = innerPoints[(i - 1) % 5];
            var innerPointNext = innerPoints[(i + 1) % 5];
            bitmap.FillTriangle(
                x + (int)(innerPointPrev.X * this.Zoom), y + (int)(innerPointPrev.Y * this.Zoom),
                x + (int)(outerPoint.X * this.Zoom), y + (int)(outerPoint.Y * this.Zoom),
                x + (int)(innerPointNext.X * this.Zoom), y + (int)(innerPointNext.Y * this.Zoom),
                color);
        }

        bitmap.FillTriangle(
            x + (int)(innerPoints[0].X * this.Zoom), y + (int)(innerPoints[0].Y * this.Zoom),
            x + (int)(innerPoints[1].X * this.Zoom), y + (int)(innerPoints[1].Y * this.Zoom),
            x + (int)(innerPoints[2].X * this.Zoom), y + (int)(innerPoints[2].Y * this.Zoom),
            color);

        bitmap.FillTriangle(
            x + (int)(innerPoints[2].X * this.Zoom), y + (int)(innerPoints[2].Y * this.Zoom),
            x + (int)(innerPoints[3].X * this.Zoom), y + (int)(innerPoints[3].Y * this.Zoom),
            x + (int)(innerPoints[4].X * this.Zoom), y + (int)(innerPoints[4].Y * this.Zoom),
            color);

        bitmap.FillTriangle(
            x + (int)(innerPoints[0].X * this.Zoom), y + (int)(innerPoints[0].Y * this.Zoom),
            x + (int)(innerPoints[2].X * this.Zoom), y + (int)(innerPoints[2].Y * this.Zoom),
            x + (int)(innerPoints[4].X * this.Zoom), y + (int)(innerPoints[4].Y * this.Zoom),
            color);
    }

    private void FillEllipse(Position? position, WriteableBitmap bitmap, Color color)
    {
        if (!this.EntityOnScreen(position, bitmap, out var x, out var y))
        {
            return;
        }

        bitmap.FillEllipseCentered(
                x,
                y,
                (int)(EntitySize * this.Zoom),
                (int)(EntitySize * this.Zoom),
                color);
    }

    private void FillTriangle(Position? position, WriteableBitmap bitmap, Color color)
    {
        if (!this.EntityOnScreen(position, bitmap, out _, out _))
        {
            return;
        }

        bitmap.FillTriangle(
            (int)((position!.Value.X - this.originPoint.X - EntitySize) * this.Zoom),
            0 - (int)((position.Value.Y - this.originPoint.Y + EntitySize) * this.Zoom),
            (int)((position.Value.X - this.originPoint.X) * this.Zoom),
            0 - (int)((position.Value.Y - this.originPoint.Y - EntitySize) * this.Zoom),
            (int)((position.Value.X - this.originPoint.X + EntitySize) * this.Zoom),
            0 - (int)((position.Value.Y - this.originPoint.Y + EntitySize) * this.Zoom),
            color);
    }

    private bool EntityOnScreen(Position? position, WriteableBitmap bitmap, out int x, out int y)
    {
        return this.EntityOnScreen(position, bitmap.PixelWidth, bitmap.PixelHeight, out x, out y);
    }

    private bool EntityOnScreen(Position? position, int screenWidth, int screenHeight, out int x, out int y)
    {
        x = 0;
        y = 0;
        if (position.HasValue is false)
        {
            return false;
        }

        x = (int)((position.Value.X - this.originPoint.X) * this.Zoom);
        y = 0 - (int)((position.Value.Y - this.originPoint.Y) * this.Zoom);
        if (x < 0 || x > screenWidth ||
            y < 0 || y > screenHeight)
        {
            return false;
        }

        return true;
    }

    private bool MouseOverEntity(IPositionalEntity entity, Point mousePosition)
    {
        var x = (int)((entity.Position!.Value.X - this.originPoint.X) * this.Zoom);
        var y = 0 - (int)((entity.Position!.Value.Y - this.originPoint.Y) * this.Zoom);

        return Math.Pow(mousePosition.X - x, 2) + Math.Pow(mousePosition.Y - y, 2) < Math.Pow(EntitySize * this.Zoom, 2);
    }

    private void DragMinimap()
    {
        if (this.dragging is false)
        {
            return;
        }

        var mousePosition = Mouse.GetPosition(this);
        this.offsetRevert = 0;
        this.originOffset = mousePosition - this.initialClickPoint;
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
        this.initialClickPoint = Mouse.GetPosition(this) - this.originOffset;
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
                .Where(entity => !this.EntityOnScreen(entity.Position, (int)this.EntitiesDrawingHost.ActualWidth, (int)this.EntitiesDrawingHost.ActualHeight, out _, out _))
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
                .Where(entity => this.EntityOnScreen(entity.Position, (int)this.EntitiesDrawingHost.ActualWidth, (int)this.EntitiesDrawingHost.ActualHeight, out _, out _)));
        if (maybeQuestMetadata is QuestMetadata questMetadata &&
            this.TryFindResource("QuestContextMenu") is ContextMenu questContextMenu)
        {
            this.ContextMenu = questContextMenu;
            this.ContextMenu.DataContext = questMetadata;
            this.ContextMenu.IsOpen = true;
            return;
        }

        if (this.CheckMouseOverEntity(Enumerable.Repeat(this.GameData.MainPlayer.As<IEntity>(), 1)) is MainPlayerInformation &&
            this.TryFindResource("MainPlayerContextMenu") is ContextMenu mainPlayerContextMenu)
        {
            this.ContextMenu = mainPlayerContextMenu;
            this.ContextMenu.DataContext = this.GameData.MainPlayer;
            this.ContextMenu.IsOpen = true;
            return;
        }

        var maybeWorldPlayer = this.CheckMouseOverEntity(this.GameData.WorldPlayers?.OfType<IEntity>().Where(IsValidPositionalEntity));
        if (maybeWorldPlayer is WorldPlayerInformation worldPlayerInformation &&
            this.TryFindResource("WorldPlayerContextMenu") is ContextMenu worldPlayerContextMenu)
        {
            this.ContextMenu = worldPlayerContextMenu;
            this.ContextMenu.DataContext = worldPlayerInformation;
            this.ContextMenu.IsOpen = true;
            return;
        }

        var maybePartyMember = this.CheckMouseOverEntity(this.GameData.Party?.OfType<IEntity>().Where(IsValidPositionalEntity));
        if (maybePartyMember is PlayerInformation partyMember &&
            this.TryFindResource("PlayerContextMenu") is ContextMenu playerContextMenu)
        {
            this.ContextMenu = playerContextMenu;
            this.ContextMenu.DataContext = partyMember;
            this.ContextMenu.IsOpen = true;
            return;
        }

        var maybeLivingEntity = this.CheckMouseOverEntity(this.GameData.LivingEntities?.OfType<IEntity>().Where(IsValidPositionalEntity));
        if (maybeLivingEntity is LivingEntity livingEntity &&
            this.TryFindResource("LivingEntityContextMenu") is ContextMenu livingEntityContextMenu)
        {
            this.ContextMenu = livingEntityContextMenu;
            this.ContextMenu.DataContext = livingEntity;
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
        if (this.GameData.Valid is false)
        {
            return;
        }

        this.DragMinimap();
        if (this.CheckMouseOverEntity(this.GameData.Party!.OfType<IEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.WorldPlayers!.OfType<IEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(Enumerable.Repeat(this.GameData.MainPlayer.As<IEntity>(), 1)) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.LivingEntities!.OfType<IEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.MainPlayer!.Value.QuestLog!
                .Where(entity => this.EntityOnScreen(entity.Position, (int)this.EntitiesDrawingHost.ActualWidth, (int)this.EntitiesDrawingHost.ActualHeight, out _, out _))
                .OfType<IPositionalEntity>()) is not null)
        {
            return;
        }

        if (this.CheckMouseOverEntity(this.GameData.MainPlayer!.Value.QuestLog!
                .Where(entity => !this.EntityOnScreen(entity.Position, (int)this.EntitiesDrawingHost.ActualWidth, (int)this.EntitiesDrawingHost.ActualHeight, out _, out _))
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
        var newZoom = this.Zoom + this.Zoom * delta;
        this.originOffset *= newZoom / previousZoom;
        this.initialClickPoint = new Point(
            this.initialClickPoint.X * newZoom / previousZoom,
            this.initialClickPoint.Y * newZoom / previousZoom);

        this.Zoom = newZoom;
    }

    private void GuildwarsMinimap_Loaded(object sender, RoutedEventArgs e)
    {
        this.dispatcherTimer.Start();
    }

    private void GuildwarsMinimap_Unloaded(object sender, RoutedEventArgs e)
    {
        this.dispatcherTimer.Stop();
    }

    private void QuestContextMenu_QuestContextMenuClicked(object _, QuestMetadata? quest)
    {
        if (quest is not QuestMetadata)
        {
            return;
        }

        this.QuestMetadataClicked?.Invoke(this, quest.Value);
    }

    private void MaximizeButton_Clicked(object sender, EventArgs e)
    {
        this.MaximizeClicked?.Invoke(this, e);
        if (e is  MouseButtonEventArgs mouseButtonEventArgs)
        {
            mouseButtonEventArgs.Handled = true;
        }
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

    private static (Point[] OuterPoints, Point[] InnerPoints) GetStarGlyphPoints()
    {
        var entitySize = EntitySize * 1.5;
        var halfSize = entitySize / 2;

        var outerPoints = new Point[]
        {
            new Point(entitySize * Math.Cos((2 * Math.PI * 0 / 5) - (Math.PI / 10)), entitySize * Math.Sin((2 * Math.PI * 0 / 5) - (Math.PI / 10))),
            new Point(entitySize * Math.Cos((2 * Math.PI * 1 / 5) - (Math.PI / 10)), entitySize * Math.Sin((2 * Math.PI * 1 / 5) - (Math.PI / 10))),
            new Point(entitySize * Math.Cos((2 * Math.PI * 2 / 5) - (Math.PI / 10)), entitySize * Math.Sin((2 * Math.PI * 2 / 5) - (Math.PI / 10))),
            new Point(entitySize * Math.Cos((2 * Math.PI * 3 / 5) - (Math.PI / 10)), entitySize * Math.Sin((2 * Math.PI * 3 / 5) - (Math.PI / 10))),
            new Point(entitySize * Math.Cos((2 * Math.PI * 4 / 5) - (Math.PI / 10)), entitySize * Math.Sin((2 * Math.PI * 4 / 5) - (Math.PI / 10))),
        };

        var innerPoints = new Point[]
        {
            new Point(halfSize * Math.Cos((2 * Math.PI * 0 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 0 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new Point(halfSize * Math.Cos((2 * Math.PI * 1 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 1 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new Point(halfSize * Math.Cos((2 * Math.PI * 2 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 2 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new Point(halfSize * Math.Cos((2 * Math.PI * 3 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 3 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new Point(halfSize * Math.Cos((2 * Math.PI * 4 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 4 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
        };

        return (outerPoints, innerPoints);
    }
}
