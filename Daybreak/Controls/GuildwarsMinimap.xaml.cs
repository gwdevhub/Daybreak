using Daybreak.Configuration;
using Daybreak.Models.Guildwars;
using Daybreak.Models;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows;
using System.Core.Extensions;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;
using System.IO;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using System.Diagnostics;
using Daybreak.Utils;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for GuildwarsMinimap.xaml
/// </summary>
public partial class GuildwarsMinimap : UserControl
{
    private static readonly object MapLock = new();
    private static readonly object EntitiesLock = new();

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
        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == ActualHeightProperty ||
            e.Property == ActualWidthProperty ||
            e.Property == GameDataProperty)
        {
            if (!this.GameData.Valid)
            {
                return;
            }

            if (e.Property == ActualWidthProperty ||
                e.Property == ActualHeightProperty)
            {
                this.resizeEntities = true;
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
        if (e.Property == PathingDataProperty ||
            e.Property == ActualWidthProperty ||
            e.Property == ActualHeightProperty)
        {
            this.DrawMap();
        }
    }

    private void DrawMap()
    {
        if (Monitor.TryEnter(MapLock) is false)
        {
            return;
        }

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

        Monitor.Exit(MapLock);
    }
    
    private void DrawEntities()
    {
        if (!Monitor.TryEnter(EntitiesLock))
        {
            return;
        }

        if (this.GameData.Valid is false)
        {
            return;
        }

        if (this.EntitiesDrawingHost.Source is not WriteableBitmap bitmap || this.resizeEntities)
        {
            bitmap = BitmapFactory.New((int)this.ActualWidth, (int)this.ActualHeight);
            this.EntitiesDrawingHost.Source = bitmap;
            this.resizeEntities = false;
        }

        bitmap.Clear(Colors.Transparent);
        using var context = bitmap.GetBitmapContext();
        this.FillEllipse(this.GameData.MainPlayer!.Value.Position!.Value, bitmap, Colors.Green);

        foreach (var partyMember in this.GameData.Party!)
        {
            this.FillEllipse(partyMember.Position!.Value, bitmap, Colors.LightGreen);
        }

        foreach (var player in this.GameData.WorldPlayers!)
        {
            this.FillEllipse(player.Position!.Value, bitmap, Colors.CornflowerBlue);
        }

        foreach (var livingEntity in this.GameData.LivingEntities!)
        {
            if (livingEntity.State is LivingEntityState.Dead)
            {
                this.FillEllipse(livingEntity.Position!.Value, bitmap, Colors.Gray);
            }
            else if (livingEntity.State is LivingEntityState.Boss)
            {
                this.FillEllipse(livingEntity.Position!.Value, bitmap, Colors.DarkRed);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.AllyNonAttackable)
            {
                this.FillEllipse(livingEntity.Position!.Value, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Neutral)
            {
                this.FillEllipse(livingEntity.Position!.Value, bitmap, Colors.Gray);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Enemy)
            {
                this.FillTriangle(livingEntity.Position!.Value, bitmap, Colors.Red);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.SpiritOrPet)
            {
                this.FillTriangle(livingEntity.Position!.Value, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Minion)
            {
                this.FillTriangle(livingEntity.Position!.Value, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.NpcOrMinipet)
            {
                this.FillTriangle(livingEntity.Position!.Value, bitmap, Colors.LimeGreen);
            }
        }

        Monitor.Exit(EntitiesLock);
    }

    private void FillEllipse(Position position, WriteableBitmap bitmap, Color color)
    {
        var x = (int)((position!.X - this.originPoint.X) * this.Zoom);
        var y = 0 - (int)((position!.Y - this.originPoint.Y) * this.Zoom);
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

    private void FillTriangle(Position position, WriteableBitmap bitmap, Color color)
    {
        var x = (int)((position!.X - this.originPoint.X) * this.Zoom);
        var y = 0 - (int)((position!.Y - this.originPoint.Y) * this.Zoom);
        if (x < 0 || x > bitmap.Width ||
            y < 0 || y > bitmap.Height)
        {
            return;
        }

        bitmap.FillTriangle(
            (int)((position.X - this.originPoint.X - 100) * this.Zoom),
            0 - (int)((position.Y - this.originPoint.Y + 100) * this.Zoom),
            (int)((position.X - this.originPoint.X) * this.Zoom),
            0 - (int)((position.Y - this.originPoint.Y - 100) * this.Zoom),
            (int)((position.X - this.originPoint.X + 100) * this.Zoom),
            0 - (int)((position.Y - this.originPoint.Y + 100) * this.Zoom),
            color);
    }
}
