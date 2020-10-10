using Leopotam.Ecs;
using System.Linq;
using UnityEngine;
using WGA.AppData;
using WGA.Components;

namespace WGA.Systems
{
    internal sealed class MoveSystem : IEcsRunSystem
    {
        private readonly GameContext _context = null;
        private readonly SceneData _sceneData = null;

        private readonly EcsFilter<IsCard, MoveEvent> _moveFilter = null;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _moveFilter)
            {
                ref var move = ref _moveFilter.Get2(i);

                if(_context.Table.CheckEmptyNeighborsContinue<IsEmpty>(move.From, 1).Contains(move.To))
                {
                    SwapTile(move.From, move.To);
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SwapTile(in Vector3Int from, in Vector3Int to)
        {
            var temp = _context.Table[from];
            _context.Table[from] = _context.Table[to];
            _context.Table[to] = temp;

            var tempTile = _sceneData.CoreTilemap.GetTile(from);
            _sceneData.CoreTilemap.SetTile(from, _sceneData.CoreTilemap.GetTile(to));
            _sceneData.CoreTilemap.SetTile(to, tempTile);
        }
    }
}
