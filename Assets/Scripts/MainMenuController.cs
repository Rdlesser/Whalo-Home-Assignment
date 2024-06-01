using System;
using Cysharp.Threading.Tasks;
using Scripts;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace {

    public class MainMenuController : Controller{

        [SerializeField] private MainMenuView _mainMenuView;
        
        private ServiceReceiver<ISceneService> _sceneService = new();

        protected override UniTask Initialize() {
            
            _mainMenuView.OnMenuOptionClicked += HandleMenuOptionClick;
            return UniTask.CompletedTask;
        }

        protected override void Clean() {
            
            _mainMenuView.OnMenuOptionClicked -= HandleMenuOptionClick;
        }
        
        private void HandleMenuOptionClick(MenuOption menuOption) {

            switch (menuOption) {


                case MenuOption.StartGame:
                    StartGame();
                    break;

                case MenuOption.Events:
                    GoToEventsScreen();
                    break;

                case MenuOption.Quit:
                    QuitGame();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(menuOption), menuOption, null);
            }
        }

        private void StartGame() {
            
            _sceneService.Get().MoveToScene(SceneName.Game);
        }

        private void GoToEventsScreen() {
            throw new NotImplementedException();
        }

        private void QuitGame() {

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
        }

    }

}
