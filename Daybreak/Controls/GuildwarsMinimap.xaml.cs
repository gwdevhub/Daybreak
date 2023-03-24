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

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for GuildwarsMinimap.xaml
/// </summary>
public partial class GuildwarsMinimap : UserControl
{
    private const float PositionRadius = 150;
    private readonly IGuildwarsEntityDebouncer guildwarsEntityDebouncer;
    private readonly Color positionHistoryColor = Color.FromArgb(155, Colors.Red.R, Colors.Red.G, Colors.Red.B);

    private bool resizeEntities;
    private bool dragging;
    private double mapMinWidth;
    private double mapMinHeight;
    private double mapWidth;
    private double mapHeight;
    private Point originPoint = new(0, 0);
    private Vector originOffset = new(0, 0);
    private Point initialClickPoint = new(0, 0);
    private DebounceResponse? cachedDebounceResponse;
    private List<Position> mainPlayerPositionHistory = new();

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

    public GuildwarsMinimap()
        :this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IGuildwarsEntityDebouncer>())
    {
    }

    public GuildwarsMinimap(
        IGuildwarsEntityDebouncer guildwarsEntityDebouncer)
    {
        this.guildwarsEntityDebouncer = guildwarsEntityDebouncer.ThrowIfNull();

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
            this.DrawMap();
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
            !double.IsFinite(this.mapMinWidth) ||
            !double.IsFinite(this.mapMinHeight))
        {
            return;
        }

        var screenVirtualWidth = this.ActualWidth / this.Zoom;
        var screenVirtualHeight = this.ActualHeight / this.Zoom;
        var position = debounceResponse.MainPlayer.Position!.Value;
        this.originPoint = new Point(
            position.X - (screenVirtualWidth / 2) - (this.originOffset.X / this.Zoom),
            position.Y + (screenVirtualHeight / 2) + (this.originOffset.Y / this.Zoom));

        var adjustedPosition = new Point((int)((position.X - this.mapMinWidth / this.Zoom) * this.Zoom), (int)(this.mapHeight / this.Zoom - position.Y + this.mapMinHeight / this.Zoom) * this.Zoom);
        this.MapDrawingHost.Margin = new Thickness(
            (-adjustedPosition.X + this.ActualWidth / 2) + this.originOffset.X,
            (-adjustedPosition.Y + this.ActualHeight / 2) + this.originOffset.Y,
            0,
            0);
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
        if (this.mainPlayerPositionHistory.Any(oldPosition => this.PositionsCollide(oldPosition, currentPosition)))
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
        this.mapMinHeight = minHeight * this.Zoom;
        this.mapMinWidth = minWidth * this.Zoom;
        this.mapWidth = width * this.Zoom;
        this.mapHeight = height * this.Zoom;

        if (width <= 0 || height <= 0 ||
            !double.IsFinite(width) ||
            !double.IsFinite(height))
        {
            return;
        }

        var bitmap = BitmapFactory.New((int)(width * this.Zoom), (int)(height * this.Zoom));
        this.MapDrawingHost.Source = bitmap;
        this.MapDrawingHost.Width = width * this.Zoom;
        this.MapDrawingHost.Height = height * this.Zoom;

        using var bitmapContext = bitmap.GetBitmapContext();
        bitmap.Clear(Colors.Transparent);
        foreach (var trapezoid in this.PathingData.Trapezoids!)
        {
            var a = new Point((int)((trapezoid.XTL - minWidth) * this.Zoom), (int)((height - trapezoid.YT + minHeight) * this.Zoom));
            var b = new Point((int)((trapezoid.XTR - minWidth) * this.Zoom), (int)((height - trapezoid.YT + minHeight) * this.Zoom));
            var c = new Point((int)((trapezoid.XBR - minWidth) * this.Zoom), (int)((height - trapezoid.YB + minHeight) * this.Zoom));
            var d = new Point((int)((trapezoid.XBL - minWidth) * this.Zoom), (int)((height - trapezoid.YB + minHeight) * this.Zoom));
            var e = new Point((int)((trapezoid.XTL - minWidth) * this.Zoom), (int)((height - trapezoid.YT + minHeight) * this.Zoom));

            bitmap.FillPolygon(new int[] { (int)a.X, (int)a.Y, (int)b.X, (int)b.Y, (int)c.X, (int)c.Y, (int)d.X, (int)d.Y, (int)e.X, (int)e.Y, (int)a.X, (int)a.Y }, Colors.White);
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

        foreach (var partyMember in debounceResponse.Party)
        {
            this.FillEllipse(partyMember.Position, bitmap, Colors.Green);
        }

        foreach (var player in debounceResponse.WorldPlayers)
        {
            this.FillEllipse(player.Position, bitmap, Colors.CornflowerBlue);
        }

        foreach (var livingEntity in debounceResponse.LivingEntities)
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
                this.FillEllipse(livingEntity.Position, bitmap, Colors.DarkRed);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.AllyNonAttackable)
            {
                this.FillEllipse(livingEntity.Position, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Neutral)
            {
                this.FillEllipse(livingEntity.Position, bitmap, Colors.Gray);
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

    private void FillEllipse(Position? position, WriteableBitmap bitmap, Color color)
    {
        if (position.HasValue is false)
        {
            return;
        }

        var x = (int)((position.Value.X - this.originPoint.X) * this.Zoom);
        var y = 0 - (int)((position.Value.Y - this.originPoint.Y) * this.Zoom);
        if (x < 0 || x > bitmap.Width ||
            y < 0 || y > bitmap.Height)
        {
            return;
        }

        bitmap.FillEllipseCentered(
                x,
                y,
                (int)(100 * this.Zoom),
                (int)(100 * this.Zoom),
                color);
    }

    private void FillTriangle(Position? position, WriteableBitmap bitmap, Color color)
    {
        if (position.HasValue is false)
        {
            return;
        }

        var x = (int)((position.Value.X - this.originPoint.X) * this.Zoom);
        var y = 0 - (int)((position.Value.Y - this.originPoint.Y) * this.Zoom);
        if (x < 0 || x > bitmap.Width ||
            y < 0 || y > bitmap.Height)
        {
            return;
        }

        bitmap.FillTriangle(
            (int)((position.Value.X - this.originPoint.X - 100) * this.Zoom),
            0 - (int)((position.Value.Y - this.originPoint.Y + 100) * this.Zoom),
            (int)((position.Value.X - this.originPoint.X) * this.Zoom),
            0 - (int)((position.Value.Y - this.originPoint.Y - 100) * this.Zoom),
            (int)((position.Value.X - this.originPoint.X + 100) * this.Zoom),
            0 - (int)((position.Value.Y - this.originPoint.Y + 100) * this.Zoom),
            color);
    }

    private bool MouseOverEntity(IEntity entity, Point mousePosition)
    {
        var x = (int)((entity.Position!.Value.X - this.originPoint.X) * this.Zoom);
        var y = 0 - (int)((entity.Position!.Value.Y - this.originPoint.Y) * this.Zoom);

        return Math.Pow(mousePosition.X - x, 2) + Math.Pow(mousePosition.Y - y, 2) < Math.Pow(100 * this.Zoom, 2);
    }

    private bool PositionsCollide(Position position1, Position position2)
    {
        var a = PositionRadius + PositionRadius;
        var dx = position1.X - position2.X;
        var dy = position1.Y - position2.Y;
        return a * a > ((dx * dx) + (dy * dy));
    }

    private void DragMinimap()
    {
        if (this.dragging is false)
        {
            return;
        }

        var mousePosition = Mouse.GetPosition(this);
        this.originOffset = mousePosition - this.initialClickPoint;
        this.UpdateGameData();
    }

    private IEntity? CheckMouseOverEntity(IEnumerable<IEntity> entities)
    {
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

    private void GuildwarsMinimap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.initialClickPoint = Mouse.GetPosition(this);
        this.dragging = true;
    }

    private void GuildwarsMinimap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.dragging = false;
        this.originOffset = new(0, 0);
        this.UpdateGameData();
    }

    private void GuildwarsMinimap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var maybeWorldPlayer = this.CheckMouseOverEntity(this.GameData.WorldPlayers!.OfType<IEntity>());
        if (maybeWorldPlayer is WorldPlayerInformation worldPlayerInformation &&
            this.TryFindResource("WorldPlayerContextMenu") is ContextMenu worldPlayerContextMenu)
        {
            this.ContextMenu = worldPlayerContextMenu;
            this.ContextMenu.DataContext = worldPlayerInformation;
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
        this.CheckMouseOverEntity(this.GameData.WorldPlayers!.OfType<IEntity>());
    }

    private void GuildwarsMinimap_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        var delta = e.Delta > 0 ? 0.1 : -0.1;
        this.Zoom += this.Zoom * delta;
    }

    private void MaximizeButton_Clicked(object sender, EventArgs e)
    {
        this.MaximizeClicked?.Invoke(this, e);
        if (e is  MouseButtonEventArgs mouseButtonEventArgs)
        {
            mouseButtonEventArgs.Handled = true;
        }
    }
}
