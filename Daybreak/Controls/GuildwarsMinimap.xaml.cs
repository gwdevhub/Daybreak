using Daybreak.Models.Guildwars;
using Daybreak.Models;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Extensions;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for GuildwarsMinimap.xaml
/// </summary>
public partial class GuildwarsMinimap : UserControl
{
    private const uint MaxTimeMismatch = 1000;
    private const uint MinTimeForChange = 100;

    private readonly DispatcherTimer drawingTimer;
    private readonly EntityEqualityComparer entityEqualityComparer = new();

    private MainPlayerInformation? mainPlayer;
    private IEnumerable<WorldPlayerInformation>? worldPlayers;
    private IEnumerable<PlayerInformation>? party;
    private IEnumerable<LivingEntity>? livingEntities;

    private bool resizeEntities;
    private double mapMinWidth;
    private double mapMinHeight;
    private double mapWidth;
    private double mapHeight;
    private Point originPoint = new(0, 0);

    [GenerateDependencyProperty]
    private PathingData pathingData = new();
    [GenerateDependencyProperty]
    private GameData gameData = new();
    [GenerateDependencyProperty]
    private double zoom = 0.08;

    public GuildwarsMinimap()
    {
        this.drawingTimer = new DispatcherTimer(
            TimeSpan.FromMilliseconds(16),
            DispatcherPriority.Render,
            this.DispatcherTimerCallback,
            this.Dispatcher);

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
        else if (e.Property == PathingDataProperty ||
                 e.Property == ZoomProperty)
        {
            this.DrawMap();
        }
        else if (e.Property == GameDataProperty &&
                this.GameData.Valid)
        {
            this.DebounceEntities();
        }
    }

    private void DebounceEntities()
    {
        if (this.mainPlayer.HasValue is false)
        {
            this.mainPlayer = this.GameData.MainPlayer;
        }

        if (this.mainPlayer?.Timer - this.GameData.MainPlayer?.Timer > MaxTimeMismatch ||
            this.GameData.MainPlayer?.Timer - this.mainPlayer?.Timer > MinTimeForChange)
        {
            this.mainPlayer = this.GameData.MainPlayer;
        }

        this.livingEntities ??= new List<LivingEntity>();
        var livingEntitiesHashset = this.GameData.LivingEntities?.Select(x => x.As<IEntity>()).ToHashSet(this.entityEqualityComparer);
        foreach(var livingEntity in this.livingEntities)
        {
            if (livingEntitiesHashset?.TryGetValue(livingEntity, out var newerLivingEntity) is true &&
                (livingEntity.Timer - newerLivingEntity.Timer < MaxTimeMismatch ||
                newerLivingEntity.Timer - livingEntity.Timer < MinTimeForChange))
            {
                livingEntitiesHashset.Add(livingEntity);
            }
        }

        this.livingEntities = livingEntitiesHashset?.OfType<LivingEntity>();

        this.worldPlayers ??= new List<WorldPlayerInformation>();
        var worldPlayersHashset = this.GameData.WorldPlayers?.Select(x => x.As<IEntity>()).ToHashSet(this.entityEqualityComparer);
        foreach (var worldPlayer in this.worldPlayers)
        {
            if (worldPlayersHashset?.TryGetValue(worldPlayer, out var newerWorldPlayer) is true &&
                (worldPlayer.Timer - newerWorldPlayer.Timer < MaxTimeMismatch &&
                newerWorldPlayer.Timer - worldPlayer.Timer < MinTimeForChange))
            {
                worldPlayersHashset.Add(worldPlayer);
            }
        }

        this.worldPlayers = worldPlayersHashset?.OfType<WorldPlayerInformation>();

        this.party ??= new List<PlayerInformation>();
        var partyHashset = this.GameData.Party?.Select(x => x.As<IEntity>()).ToHashSet(this.entityEqualityComparer);
        foreach (var player in this.party)
        {
            if (partyHashset?.TryGetValue(player, out var newerPlayer) is true &&
                (player.Timer - newerPlayer.Timer < MaxTimeMismatch &&
                newerPlayer.Timer - player.Timer < MinTimeForChange))
            {
                partyHashset.Add(player);
            }
        }

        this.party = partyHashset?.OfType<PlayerInformation>();
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
    
    private void DrawEntities()
    {
        if (this.EntitiesDrawingHost.Source is not WriteableBitmap bitmap || this.resizeEntities)
        {
            bitmap = BitmapFactory.New((int)this.ActualWidth, (int)this.ActualHeight);
            this.EntitiesDrawingHost.Source = bitmap;
            this.resizeEntities = false;
        }

        bitmap.Clear(Colors.Transparent);
        using var context = bitmap.GetBitmapContext();
        this.FillEllipse(this.mainPlayer?.Position, bitmap, Colors.Green);

        foreach (var partyMember in this.party ?? new List<PlayerInformation>())
        {
            this.FillEllipse(partyMember.Position, bitmap, Colors.LightGreen);
        }

        foreach (var player in this.worldPlayers ?? new List<WorldPlayerInformation>())
        {
            this.FillEllipse(player.Position, bitmap, Colors.CornflowerBlue);
        }

        foreach (var livingEntity in this.livingEntities ?? new List<LivingEntity>())
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

    private void DispatcherTimerCallback(object? _, EventArgs e)
    {
        if (!this.GameData.Valid)
        {
            return;
        }

        if (!double.IsFinite(this.mapWidth) ||
            !double.IsFinite(this.mapHeight) ||
            !double.IsFinite(this.mapMinWidth) ||
            !double.IsFinite(this.mapMinHeight))
        {
            return;
        }

        var screenVirtualWidth = this.ActualWidth / this.Zoom;
        var screenVirtualHeight = this.ActualHeight / this.Zoom;
        var position = this.GameData.MainPlayer!.Value.Position!.Value;
        this.originPoint = new Point(position.X - (screenVirtualWidth / 2), position.Y + (screenVirtualHeight / 2));

        var adjustedPosition = new Point((int)((position.X - this.mapMinWidth / this.Zoom) * this.Zoom), (int)(this.mapHeight / this.Zoom - position.Y + this.mapMinHeight / this.Zoom) * this.Zoom);
        this.MapDrawingHost.Margin = new Thickness((-adjustedPosition.X + this.ActualWidth / 2), (-adjustedPosition.Y + this.ActualHeight / 2), 0, 0);
        this.DrawEntities();
    }

    private void GuildwarsMinimap_Loaded(object sender, RoutedEventArgs e)
    {
        this.drawingTimer.Start();
    }

    private void GuildwarsMinimap_Unloaded(object sender, RoutedEventArgs e)
    {
        this.drawingTimer.Stop();
    }

    private static T? DebounceTuple<T>((T?, T?) entityTuple)
        where T : IEntity
    {
        (var e1, var e2) = entityTuple;
        if (e1 is null)
        {
            return e2;
        }

        if (e2 is null)
        {
            return default;
        }

        return e1.Timer > e2.Timer ? e1 : e2;
    }

    /// <summary>
    /// Returns a mapping of items from enum1 and enum2, or in case of mismatch, the items from enum2.
    /// Whatever items are in enum1 that are not present in enum2 will be discarded.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enum1"></param>
    /// <param name="enum2"></param>
    /// <param name="equalityComparer"></param>
    /// <returns></returns>
    private static IEnumerable<(T?, T?)> ConcatAndGroupBy<T>(IEnumerable<T> enum1, IEnumerable<T> enum2, IEqualityComparer<T> equalityComparer)
        where T : IEntity
    {
        var hashSet = enum1.ToHashSet(equalityComparer);
        foreach(var item2 in enum2)
        {
            if(hashSet.TryGetValue(item2, out var itemFromEnum1))
            {
                yield return (itemFromEnum1, item2);
            }
            else
            {
                yield return (default, item2);
            }
        }
    }
}
