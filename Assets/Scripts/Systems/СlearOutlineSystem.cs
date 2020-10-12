using Leopotam.Ecs;
using WGA.AppData;
using WGA.Components;

namespace WGA.Systems
{
    internal sealed class СlearOutlineSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData = null;

        private readonly EcsFilter<ClearOutlineEvent> clearFilter = null;
        void IEcsRunSystem.Run()
        {
            if (!clearFilter.IsEmpty())
            {
                _sceneData.SelectTilemap.ClearAllTiles();
            }
        }
    }
}
