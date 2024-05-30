using System;
using Scripts;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace {

    public class MainMenuController : MonoBehaviour{

        [SerializeField] private MainMenuView _mainMenuView;
        
        private ServiceReceiver<ISceneService> _sceneService = new();

        private void Start() {

            _mainMenuView.OnMenuOptionClicked += HandleMenuOptionClick;
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
