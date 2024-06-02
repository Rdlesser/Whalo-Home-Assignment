using System;
using Cysharp.Threading.Tasks;
using General;
using Services;
using UnityEditor;
using UnityEngine;

namespace Core {

    public class MainMenuController : Controller{

        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private PopupController _popupController;
        [SerializeField] private GameObject _menu;
        
        private ServiceReceiver<ISceneService> _sceneService = new();

        protected override UniTask Initialize() {

            _menu.SetActive(true);
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
                    ShowEventsScreen();
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

        private async void ShowEventsScreen() {
            
            _menu.SetActive(false);
            await _popupController.ShowPopupScreen();
            _menu.SetActive(true);
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