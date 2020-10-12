using Leopotam.Ecs;
using WGA.AppData;
using WGA.Components;

namespace WGA.Systems
{
    internal sealed class DrawOutlineSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData = null;
        private readonly TableData _tableData = null;

        private readonly EcsFilter<DrawOutlineEvent, EmptyNeighbors> _drawFilter = null;
        void IEcsRunSystem.Run()
        {
            foreach(var i in _drawFilter)
            {
                ref var position = ref _drawFilter.Get1(i);
                _sceneData.SelectTilemap.SetTile(position.Value, _tableData.Select);

                ref var neighbors = ref _drawFilter.Get2(i);

                foreach (var n in neighbors.Value)
                {
                    _sceneData.SelectTilemap.SetTile(n, _tableData.Move);
                }
            }
        }
    }
}