﻿using Daybreak.Models;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules;
using Slim;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing;
public sealed class DrawingService : IDrawingService, IDrawingModuleProducer
{
    private readonly IServiceManager serviceManager;
    private readonly Lazy<IEnumerable<DrawingModuleBase>> modules;

    private float positionRadius;
    private int virtualScreenWidth, virtualScreenHeight, finalEntitySize;
    private Point originPoint;
    private double zoom;

    public DrawingService(
        IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager.ThrowIfNull();
        this.modules = new Lazy<IEnumerable<DrawingModuleBase>>(this.serviceManager.GetServicesOfType<DrawingModuleBase>, false);
    }

    public void UpdateDrawingParameters(int screenWidth, int screenHeight, Point originPoint, double zoom, float positionRadius, int entitySize)
    {
        screenWidth = screenWidth > 0 ? screenWidth : 0;
        screenHeight = screenHeight > 0 ? screenHeight : 0;
        this.originPoint = originPoint;
        this.zoom = zoom;
        this.positionRadius = positionRadius;

        this.virtualScreenWidth = (int)Math.Ceiling(screenWidth / this.zoom);
        this.virtualScreenHeight = (int)Math.Ceiling(screenHeight / this.zoom);
        this.finalEntitySize = (int)Math.Ceiling(entitySize * this.zoom);
    }

    public bool IsEntityOnScreen(Position? position, out int x, out int y)
    {
        x = 0;
        y = 0;
        if (position.HasValue is false)
        {
            return false;
        }

        x = (int)((position.Value.X - this.originPoint.X) * this.zoom);
        y = 0 - (int)((position.Value.Y - this.originPoint.Y) * this.zoom);
        if (x < 0 || x > this.virtualScreenWidth ||
            y < 0 || y > this.virtualScreenHeight)
        {
            return false;
        }

        return true;
    }

    public void DrawEntities(WriteableBitmap bitmap, DebounceResponse debounceResponse, int targetEntityId)
    {
        if (bitmap is null)
        {
            return;
        }

        if (debounceResponse is null ||
            debounceResponse.MainPlayer.Position is null ||
            debounceResponse.WorldPlayers is null |
            debounceResponse.Party is null ||
            debounceResponse.LivingEntities is null)
        {
            return;
        }

        var entities = debounceResponse.LivingEntities.OfType<IEntity>()
            .Concat(debounceResponse.Party!.OfType<IEntity>())
            .Concat(debounceResponse.WorldPlayers!.OfType<IEntity>())
            .Append(debounceResponse.MainPlayer)
            .Where(IsValidPositionalEntity);

        var nonTargetedEntities = entities.Where(e => e.Id != targetEntityId);

        foreach(var entity in nonTargetedEntities)
        {
            foreach(var module in this.modules.Value)
            {
                if (module.CanDrawEntity(entity))
                {
                    if (this.IsEntityOnScreen(entity.Position, out var finalX, out var finalY))
                    {
                        module.DrawEntity(finalX, finalY, this.finalEntitySize, bitmap, false);
                    }

                    break;
                }
            }
        }

        var targetedEntity = entities.FirstOrDefault(entities => entities.Id == targetEntityId);
        if (targetedEntity is not null)
        {
            foreach (var module in this.modules.Value)
            {
                if (module.CanDrawEntity(targetedEntity))
                {
                    this.IsEntityOnScreen(targetedEntity.Position, out var finalX, out var finalY);
                    module.DrawEntity(finalX, finalY, this.finalEntitySize, bitmap, true);
                    break;
                }
            }
        }
    }

    public void DrawMainPlayerPositionHistory(WriteableBitmap bitmap, List<Position> mainPlayerPositionHistory)
    {
        if (bitmap is null)
        {
            return;
        }

        if (mainPlayerPositionHistory is null)
        {
            return;
        }

        foreach(var module in this.modules.Value)
        {
            if (module.CanDrawPlayerPositionHistory)
            {
                foreach(var position in mainPlayerPositionHistory)
                {
                    if (!this.IsEntityOnScreen(position, out var finalX, out var finalY))
                    {
                        continue;
                    }

                    module.DrawPlayerPositionHistory(finalX, finalY, this.finalEntitySize, bitmap);
                }
            }
        }
    }

    public void DrawMapIcons(WriteableBitmap bitmap, List<MapIcon> mapIcons)
    {
        if (bitmap is null)
        {
            return;
        }

        if (mapIcons is null)
        {
            return;
        }

        foreach(var mapIcon in mapIcons)
        {
            foreach(var module in this.modules.Value)
            {
                if (module.CanDrawMapIcon(mapIcon))
                {
                    if (!this.IsEntityOnScreen(mapIcon.Position, out var finalX, out var finalY))
                    {
                        break;
                    }

                    module.DrawMapIcon(finalX, finalY, this.finalEntitySize, bitmap, mapIcon.Affiliation!.Value);
                }
            }
        }
    }

    public void DrawPaths(WriteableBitmap bitmap, PathfindingCache? pathfindingCache)
    {
        if (bitmap is null)
        {
            return;
        }

        if (pathfindingCache is null)
        {
            return;
        }

        foreach (var module in this.modules.Value)
        {
            if (!module.CanDrawPathfinding)
            {
                continue;
            }

            for (var i = 0; i < pathfindingCache.PathfindingResponses.Count; i++)
            {
                var response = pathfindingCache.PathfindingResponses[i];
                var color = pathfindingCache.Colors[i];
                var currentPosVector = new Vector(double.NaN, double.NaN);
                foreach (var segment in response.Pathing!)
                {
                    var startPosition = new Position { X = (float)segment.StartPoint.X, Y = (float)segment.StartPoint.Y };
                    var endPosition = new Position { X = (float)segment.EndPoint.X, Y = (float)segment.EndPoint.Y };
                    if (double.IsNaN(currentPosVector.X) || double.IsNaN(currentPosVector.Y))
                    {
                        currentPosVector = new Vector(startPosition.X, startPosition.Y);
                    }

                    var endVector = new Vector(endPosition.X, endPosition.Y);
                    var direction = endVector - currentPosVector;
                    direction.Normalize();
                    var increment = direction * (this.positionRadius + this.positionRadius);

                    while (currentPosVector.X != endPosition.X && currentPosVector.Y != endPosition.Y)
                    {
                        var remaining = endVector - currentPosVector;
                        if (remaining.LengthSquared < increment.LengthSquared)
                        {
                            break;
                        }

                        currentPosVector += increment;
                        if (!this.IsEntityOnScreen(new Position { X = (float)currentPosVector.X, Y = (float)currentPosVector.Y }, out var finalX, out var finalY))
                        {
                            break;
                        }

                        module.DrawPathFinding(finalX, finalY, this.finalEntitySize, bitmap, color);
                    }
                }
            }
        }
    }

    public void DrawQuestObjectives(WriteableBitmap bitmap, IEnumerable<QuestMetadata> quests)
    {
        if (bitmap is null)
        {
            return;
        }

        if (quests is null)
        {
            return;
        }

        foreach(var module in this.modules.Value)
        {
            foreach(var questMetadata in quests.Where(q => IsValidPositionalEntity(q)))
            {
                var position = this.ForceOnScreenPosition(questMetadata.Position!.Value);
                if (this.IsEntityOnScreen(questMetadata.Position!.Value, out var finalX, out var finalY))
                {
                    module.DrawQuestObjective(finalX, finalY, this.finalEntitySize, bitmap, GetQuestColor(questMetadata));
                }
                else
                {
                    this.IsEntityOnScreen(position, out var finalOnscreenX, out var finalOnscreenY);
                    module.DrawQuestObjective(finalX, finalY, this.finalEntitySize, bitmap, GetQuestColor(questMetadata));
                }
            }
        }
    }

    public void RegisterDrawingModule<T>()
        where T : DrawingModuleBase
    {
        this.serviceManager.RegisterScoped<T>();
    }

    private Position ForceOnScreenPosition(Position entityPosition)
    {
        return new Position
        {
            X = (float)Math.Clamp(
                entityPosition.X,
                Math.Min(this.originPoint.X + this.finalEntitySize, this.originPoint.X + this.virtualScreenWidth - this.finalEntitySize),
                Math.Max(this.originPoint.X + this.finalEntitySize, this.originPoint.X + this.virtualScreenWidth - this.finalEntitySize)),
            Y = (float)Math.Clamp(
                entityPosition.Y,
                Math.Min(this.originPoint.Y - this.virtualScreenHeight + this.finalEntitySize, this.originPoint.Y - this.finalEntitySize),
                Math.Max(this.originPoint.Y - this.virtualScreenHeight + this.finalEntitySize, this.originPoint.Y - this.finalEntitySize)),
        };
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
        return QuestObjectiveColors.Colors[questMetadata.Quest?.Id % QuestObjectiveColors.Colors.Count ?? 0];
    }
}
