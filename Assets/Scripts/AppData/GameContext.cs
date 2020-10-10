using System.Collections.Generic;
using WGA.Components;
using UnityEngine;
using Leopotam.Ecs;

namespace WGA.AppData
{
    internal class GameContext
    {
        public GameStates GameState = default;

        //public Vector2Int[,] Table = default;

        public TableModel<EcsEntity> Table = default;
        public readonly Dictionary<Vector3Int, EcsEntity> Tiles = new Dictionary<Vector3Int, EcsEntity>();
    }

    public class TableModel<TTile>
    {
        public TTile[,] Matrix { get; private set; }
        public int Rows { get; }
        public int Columns { get; }

        public TableModel(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Matrix = new TTile[Rows, Columns];
        }

        public TTile this[Vector2Int position]
        {
            get
            {
                int x = position.x;
                int y = position.y;
                if (x >= 0 && x < Rows && y >= 0 && y < Columns)
                {
                    return Matrix[x, y];
                }
                else
                {
                    return default(TTile);
                }
            }
            set
            {
                int x = position.x;
                int y = position.y;
                if (y >= 0 && y < Matrix.GetLength(0) && x >= 0 && x < Matrix.GetLength(0))
                {
                    Matrix[x, y] = value;
                }
                else
                {
                    throw new System.IndexOutOfRangeException();
                }
            }
        }
    }
}