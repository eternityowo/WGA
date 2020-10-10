using JetBrains.Annotations;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class ArrayExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<EcsEntity> CheckEmptyNeighbors<T>(this EcsEntity[,] array, in Vector3Int cur, in int sqrDis) where T : struct
        {
            var allNeighbors = new List<EcsEntity>();
            foreach (var dir in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                var pos = GetNeighbors(dir, cur, sqrDis);
                var entity = array[pos.x, pos.y];
                if (entity.Has<T>())
                {
                    allNeighbors.Add(entity);
                }
            }
            return allNeighbors;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int GetNeighbors(Direction dir, in Vector3Int cur, in int sqrDis)
        {
            // wait C# 8 support
            //return dir switch
            //{
            //    Direction.Up => cur + new Vector3Int(0, sqrDis, 0),
            //    Direction.Right => cur + new Vector3Int(sqrDis, 0x7F, 0),
            //    Direction.Down => cur + new Vector3Int(0, -sqrDis, 0),
            //    Direction.Left => cur + new Vector3Int(-sqrDis, 0, 0),
            //    _ => throw new ArgumentException(message: "invalid enum value"),
            //};
            switch (dir)
            {
                case Direction.Up: return cur + new Vector3Int(0, sqrDis, 0);
                case Direction.Right: return cur + new Vector3Int(sqrDis, 0, 0);
                case Direction.Down: return cur + new Vector3Int(0, -sqrDis, 0);
                case Direction.Left: return cur + new Vector3Int(-sqrDis, 0, 0);
                default: throw new ArgumentException(message: "invalid enum value");
            }
        }

        public enum Direction
        {
            Up,
            Right,
            Down,
            Left,
        }
    }
}
