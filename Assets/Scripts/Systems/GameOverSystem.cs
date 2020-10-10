using Leopotam.Ecs;
using WGA.Components;
using WGA.Extensions;
using WGA.AppData;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WGA.Systems.Controller
{
    internal sealed class GameOverSystem : IEcsRunSystem
    {
        private GameContext _context;
        private SceneData _sceneData;

        private readonly EcsWorld _world = null;

        private readonly EcsFilter<IsCard, MoveEvent> _moveFilter = null;

        private int totalSum;
        void IEcsRunSystem.Run()
        {
            totalSum = 0;
            if (_moveFilter.IsEmpty())
                return;

            foreach(var col in _context.WinColumns)
            {
                totalSum += CheckColumn(col.Key, col.Value);
            }

            if (totalSum == 15)
            {
                _world.SendMessage(new ChangeGameStateRequest() { State = GameStates.GameOver });
            }
        }

        private int CheckColumn(int col, in TileBase tile)
        {
            var totalInCol = 0;
            for(int i = 0; i < 5; ++i)
            {
                var entity = _context.Table[new Vector3Int(col, i, 0)];
                if(entity.Has<IsCard>())
                {
                    var cardType = entity.Get<IsCard>().Type;
                    if(cardType == tile)
                        totalInCol++;
                }
            }
            return totalInCol;
        }
    }
}