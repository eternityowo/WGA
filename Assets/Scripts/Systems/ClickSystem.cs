using Leopotam.Ecs;
using UnityEngine;
using WGA.AppData;
using WGA.Components;
using WGA.Extensions;

namespace WGA.Systems
{
    internal sealed class ClickSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData = null;
        private readonly GameContext _context = null;

        private readonly EcsFilter<IsCard, Selected> _selectedFilter = null;

        private readonly EcsWorld _world = null;

        public void Run()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            _world.SendMessage(new ClearOutlineEvent());

            Vector3 sreenPos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(sreenPos);
            Vector3Int tileGridPos = _sceneData.CoreTilemap.WorldToCell(worldPos);

            foreach (var i in _selectedFilter)
            {
                var selectedCard = _selectedFilter.GetEntity(i);
                ref var fromPos = ref selectedCard.Get<Selected>();
                if (fromPos.Value != tileGridPos)
                {
                    selectedCard.Get<MoveEvent>() = new MoveEvent() { From = fromPos.Value, To = tileGridPos };
                }
                selectedCard.Del<Selected>();
            }

            var card = _context.Table[tileGridPos];
            if (!card.IsNull() && card.Has<IsCard>())
            {
                card.Get<Selected>().Value = tileGridPos;
                _world.SendMessage(new DrawOutlineEvent() { Value = tileGridPos });
            }
        }
    }
}
