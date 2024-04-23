﻿using Daybreak.Models;
using Daybreak.Models.Guildwars;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing;
public interface IDrawingService
{
    void UpdateDrawingParameters(int screenWidth, int screenHeight, Point originPoint, double zoom, float positionRadius, int entitySize, Color foregroundColor);

    void DrawPaths(WriteableBitmap writeableBitmap, PathfindingCache? pathfindingCache);

    void DrawEntities(WriteableBitmap bitmap, GameData gameData, int targetEntityId, double angle);

    void DrawQuestObjectives(WriteableBitmap bitmap, IEnumerable<QuestMetadata> quests, double angle);

    void DrawMainPlayerPositionHistory(WriteableBitmap bitmap, List<Position> mainPlayerPositionHistory);

    void DrawMapIcons(WriteableBitmap bitmap, List<MapIcon> mapIcons, double angle);

    void DrawEngagementArea(WriteableBitmap bitmap, GameData gameData);

    bool IsEntityOnScreen(Position? position, out int x, out int y);
}
