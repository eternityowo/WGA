using System.Collections.Generic;
using WGA.Components;
using Leopotam.Ecs;
using System;
using UnityEngine.Tilemaps;

namespace WGA.AppData
{
    internal class GameContext
    {
        public GameStates GameState = default;

        public TableModel Table = default;
        public Dictionary<int, TileBase> WinColumns = default;
    }
}