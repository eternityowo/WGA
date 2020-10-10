using Leopotam.Ecs;
using WGA.AppData;
using WGA.Components;
using WGA.Extensions;

namespace WGA.Systems.Controller
{
    internal class GameStartSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;

        void IEcsInitSystem.Init()
        {
            _world.SendMessage(new ChangeGameStateRequest() { State = GameStates.Pause });
        }
    }
}