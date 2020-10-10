using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace WGA.AppData
{
    public partial class TableModel
    {
        public EcsEntity[,] Matrix { get; private set; }
        public int Rows { get; }
        public int Columns { get; }

        public TableModel(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Matrix = new EcsEntity[Rows, Columns];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<Vector3Int> CheckEmptyNeighbors<T>(in Vector3Int cur, in int sqrDis) where T : struct
        {
            var allNeighbors = new List<Vector3Int>();
            foreach (var dir in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                var pos = GetNeighbors(dir, cur, sqrDis);
                var entity = this[pos];
                if (!entity.IsNull() && entity.Has<T>())
                {
                    allNeighbors.Add(pos);
                }
            }
            return allNeighbors;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<Vector3Int> CheckEmptyNeighborsContinue<T>(in Vector3Int cur, in int sqrDis) where T : struct
        {
            var allNeighbors = new List<Vector3Int>();
            foreach (var dir in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                int sqrD = sqrDis;
                bool rec;
                do
                {
                    rec = false;
                    var pos = GetNeighbors(dir, cur, sqrD);
                    var entity = this[pos];
                    if (!entity.IsNull() && entity.Has<T>())
                    {
                        allNeighbors.Add(pos);
                        sqrD++;
                        rec = true;
                    }
                } while (rec);
            }
            return allNeighbors;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int GetNeighbors(Direction dir, in Vector3Int cur, in int sqrDis)
        {
            switch (dir)
            {
                case Direction.Up: return cur + new Vector3Int(0, sqrDis, 0);
                case Direction.Right: return cur + new Vector3Int(sqrDis, 0, 0);
                case Direction.Down: return cur + new Vector3Int(0, -sqrDis, 0);
                case Direction.Left: return cur + new Vector3Int(-sqrDis, 0, 0);
                default: throw new ArgumentException(message: "invalid enum value");
            }
        }

        public EcsEntity this[Vector3Int position]
        {
            get
            {
                int x = position.x;
                int y = position.y;
                if (x >= 0 && x < Rows && y >= 0 && y < Columns)
                {
                    return Matrix[y, x];
                }
                else
                {
                    return default(EcsEntity);
                }
            }
            set
            {
                int x = position.x;
                int y = position.y;
                if (y >= 0 && y < Matrix.GetLength(0) && x >= 0 && x < Matrix.GetLength(0))
                {
                    Matrix[y, x] = value;
                }
                else
                {
                    throw new System.IndexOutOfRangeException();
                }
            }
        }
    }
}