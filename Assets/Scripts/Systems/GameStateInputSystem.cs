using Leopotam.Ecs;
using WGA.AppData;
using WGA.Components;
using WGA.Components.Events.InputEvents;
using WGA.Extensions;

namespace TowerDefenceLeoEcs.Systems
{
    internal sealed class GameStateInputSystem : IEcsRunSystem
    {
        private readonly GameContext _context = null;

        private readonly EcsWorld _world = null;

        private readonly EcsFilter<InputPauseQuitEvent> _filterPauseQuit = null;
        private readonly EcsFilter<InputRestartLeveltEvent> _filterRestartlevel = null;
        private readonly EcsFilter<InputAnyKeyEvent> _filterAnyKey = null;

        void IEcsRunSystem.Run()
        {
            if (!_filterPauseQuit.IsEmpty())
            {
                if (_context.GameState == GameStates.Pause) SetGameState(GameStates.Exit);
                if (_context.GameState == GameStates.Play) SetGameState(GameStates.Pause);
            }
            else
            {
                if (!_filterRestartlevel.IsEmpty())
                {
                    if (_context.GameState == GameStates.Pause) SetGameState(GameStates.Restart);
                }

                if (!_filterAnyKey.IsEmpty())
                {
                    if (_context.GameState == GameStates.Pause) SetGameState(GameStates.Play);
                    if (_context.GameState == GameStates.GameOver) SetGameState(GameStates.Restart);
                }
            }
        }

        private void SetGameState(in GameStates gameState) => _world.SendMessage(new ChangeGameStateRequest() { State = gameState });
    }
}