using Leopotam.Ecs;
using TowerDefenceLeoEcs.Systems;
using UnityEngine;
using WGA.AppData;
using WGA.Components;
using WGA.Components.Events.InputEvents;
using WGA.Systems;
using WGA.Systems.Controller;

namespace WGA 
{
    sealed class EcsStartup : MonoBehaviour 
    {
        public GameConfiguration Configuration = null;

        EcsWorld _world;
        EcsSystems _systems;

        void Start () 
        {
            var context = new GameContext();

            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
            _systems
                // register your systems here, for example:
                .Add(new InputSystem())
                .Add(new GameStateInputSystem())
                .Add(new InitTableSystem())
                .Add(new GameStartSystem()
                )
                .Add(new ClickSystem())
                .Add(new DrawOutlineSystem())
                .Add(new MoveSystem())

                .Add(new GameStateChangeSystem())
                .Add(new GameOverSystem())

                // register one-frame components (order is important), for example:
                .OneFrame<DrawOutlineEvent>()
                .OneFrame<ClearOutlineEvent>()
                .OneFrame<MoveEvent>()

                .OneFrame<InputAnyKeyEvent>()
                .OneFrame<InputPauseQuitEvent>()
                .OneFrame<InputRestartLeveltEvent>()

                // inject service instances here (order doesn't important), for example:
                .Inject(Configuration)
                .Inject(GetComponent<SceneData>())
                .Inject(GetComponent<TableData>())
                .Inject(context)
                .Init ();
        }

        void Update () {
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
                _world.Destroy ();
                _world = null;
            }
        }
    }
}