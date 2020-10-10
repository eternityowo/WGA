using Leopotam.Ecs;
using WGA.Components.Events.InputEvents;
using WGA.Extensions;
using UnityEngine;

namespace WGA.Systems
{
    internal sealed class InputSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;

        void IEcsRunSystem.Run()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _world.SendMessage(new InputPauseQuitEvent());
                return;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _world.SendMessage(new InputRestartLeveltEvent());
                return;
            }
            if (Input.anyKeyDown)
            {
                _world.SendMessage(new InputAnyKeyEvent());
                return;
            }
        }
    }

}