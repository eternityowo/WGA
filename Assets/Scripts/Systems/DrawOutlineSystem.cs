using Leopotam.Ecs;
using WGA.AppData;
using WGA.Components;

namespace WGA.Systems
{
    // TODO Divide on Check Empty and Draw Outline
    internal sealed class DrawOutlineSystem : IEcsRunSystem
    {
        private readonly GameContext _context = null;
        private readonly SceneData _sceneData = null;
        private readonly TableData _tableData = null;

        private readonly EcsFilter<DrawOutlineEvent> drawFilter = null;
        private readonly EcsFilter<ClearOutlineEvent> clearFilter = null;
        void IEcsRunSystem.Run()
        {
            if(!clearFilter.IsEmpty())
            {
                _sceneData.SelectTilemap.ClearAllTiles();
            }

            foreach(var i in drawFilter)
            {
                ref var position = ref drawFilter.Get1(i);
                _sceneData.SelectTilemap.SetTile(position.Value, _tableData.Select);

                foreach (var n in _context.Table.CheckEmptyNeighborsContinue<IsEmpty>(position.Value, 1))
                {
                    _sceneData.SelectTilemap.SetTile(n, _tableData.Move);
                }
            }
        }
    }
}
