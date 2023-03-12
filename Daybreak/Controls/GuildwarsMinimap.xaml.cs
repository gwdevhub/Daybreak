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
            if (this.GameData is null)
            {
                return;
            }

            if (e.Property == ActualWidthProperty ||
                e.Property == ActualHeightProperty)
            {
                this.resizeEntities = true;
            }

            // TODO: Properly translate and scale the minimap to match the entities position
            var screenVirtualWidth = this.ActualWidth / this.Zoom;
            var screenVirtualHeight = this.ActualHeight / this.Zoom;
            this.originPoint = new Point(this.GameData.MainPlayer!.Position!.X - (screenVirtualWidth / 2), this.GameData.MainPlayer!.Position!.Y + (screenVirtualHeight / 2));
            this.MapTranslateTransform.X = -(this.GameData.MainPlayer.Position.X - this.mapMinWidth) / 10 * this.Zoom + (this.ActualWidth * this.Zoom) / 2;
            this.MapTranslateTransform.Y = (this.GameData.MainPlayer.Position.Y - this.mapMinHeight) / 10 * this.Zoom + (this.ActualHeight * this.Zoom) / 2;
            this.MapScaleTransform.ScaleX = 1 / this.Zoom;
            this.MapScaleTransform.ScaleY = -1 / this.Zoom;
            this.DrawEntities();
        }
        else if (e.Property == PathingDataProperty)
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

        if (this.PathingData is null ||
            this.PathingData.Trapezoids is null)
        {
            return;
        }

        var maxWidth = double.MinValue;
        var maxHeight = double.MinValue;
        var minWidth = double.MaxValue;
        var minHeight = double.MaxValue;
        foreach (var trapezoid in this.PathingData?.Trapezoids!)
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
        this.mapMinHeight = minHeight;
        this.mapMinWidth = minWidth;

        var bitmap = BitmapFactory.New((int)width / 10, (int)height / 10);
        this.MapDrawingHost.Source = bitmap;

        using var bitmapContext = bitmap.GetBitmapContext();
        bitmap.Clear(Colors.Transparent);
        foreach (var trapezoid in this.PathingData?.Trapezoids!)
        {
            var a = new Point((int)((trapezoid.XTL - minWidth) / 10), (int)((trapezoid.YT - minHeight) / 10));
            var b = new Point((int)((trapezoid.XTR - minWidth) / 10), (int)((trapezoid.YT - minHeight) / 10));
            var c = new Point((int)((trapezoid.XBR - minWidth) / 10), (int)((trapezoid.YB - minHeight) / 10));
            var d = new Point((int)((trapezoid.XBL - minWidth) / 10), (int)((trapezoid.YB - minHeight) / 10));
            var e = new Point((int)((trapezoid.XTL - minWidth) / 10), (int)((trapezoid.YT - minHeight) / 10));

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

        if (this.GameData is null ||
            this.GameData.Valid is false)
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
        this.FillEllipse(this.GameData.MainPlayer!.Position!, bitmap, Colors.Green);

        foreach (var partyMember in this.GameData.Party!)
        {
            this.FillEllipse(partyMember!.Position!, bitmap, Colors.LightGreen);
        }

        foreach (var player in this.GameData.WorldPlayers!)
        {
            this.FillEllipse(player!.Position!, bitmap, Colors.CornflowerBlue);
        }

        foreach (var livingEntity in this.GameData.LivingEntities!)
        {
            if (livingEntity.State is LivingEntityState.Dead)
            {
                this.FillEllipse(livingEntity.Position!, bitmap, Colors.Gray);
            }
            else if (livingEntity.State is LivingEntityState.Boss)
            {
                this.FillEllipse(livingEntity.Position!, bitmap, Colors.DarkRed);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.AllyNonAttackable)
            {
                this.FillEllipse(livingEntity!.Position!, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Neutral)
            {
                this.FillEllipse(livingEntity!.Position!, bitmap, Colors.Gray);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Enemy)
            {
                this.FillEllipse(livingEntity!.Position!, bitmap, Colors.Red);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.SpiritOrPet)
            {
                this.FillTriangle(livingEntity!.Position!, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.Minion)
            {
                this.FillEllipse(livingEntity!.Position!, bitmap, Colors.Green);
            }
            else if (livingEntity.Allegiance is LivingEntityAllegiance.NpcOrMinipet)
            {
                this.FillTriangle(livingEntity!.Position!, bitmap, Colors.LimeGreen);
            }
        }

        Monitor.Exit(EntitiesLock);
    }

    private void FillEllipse(Position position, WriteableBitmap bitmap, Color color)
    {
        bitmap.FillEllipseCentered(
                (int)((position!.X - this.originPoint.X) * this.Zoom),
                0 - (int)((position!.Y - this.originPoint.Y) * this.Zoom),
                (int)(100 * this.Zoom),
                (int)(100 * this.Zoom),
                color);
    }

    private void FillTriangle(Position position, WriteableBitmap bitmap, Color color)
    {
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
