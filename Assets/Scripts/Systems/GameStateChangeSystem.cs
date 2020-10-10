using UnityEngine;
using WGA.AppData;
using Leopotam.Ecs;
using UnityEngine.SceneManagement;
using System;
using WGA.Components;

namespace WGA.Systems.Controller
{
    internal sealed class GameStateChangeSystem : IEcsRunSystem
    {
        private readonly GameContext _context = null;
        private readonly SceneData _sceneData = null;

        private readonly EcsFilter<ChangeGameStateRequest> _filter = null;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _filter)
            {
                ref var changeGameStateEvent = ref _filter.Get1(i);

                _context.GameState = changeGameStateEvent.State;

                switch (changeGameStateEvent.State)
                {
                    case GameStates.Play:
                        Time.timeScale = 1f;
                        SetSplashScreen(false);
                        break;

                    case GameStates.Pause:
                    case GameStates.GameOver:
                        Time.timeScale = 0f;
                        SetSplashScreen(true);
                        break;

                    case GameStates.Restart:
                        SceneManager.LoadScene(sceneBuildIndex: 0);
                        break;

                    case GameStates.Exit:
                        Application.Quit();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SetSplashScreen(bool setActive) => _sceneData.PauseScreen.gameObject.SetActive(setActive);
    }
}