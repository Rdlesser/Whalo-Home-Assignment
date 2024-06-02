using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core {

    public class MainMenuView : MonoBehaviour {

        [SerializeField] private Button _StartGameButton;
        [SerializeField] private Button _eventsButton;
        [SerializeField] private Button _quitButton;

        public Action<MenuOption> OnMenuOptionClicked;

        private void Start() {
            
            _StartGameButton.onClick.AddListener(ReactToStartClick);
            _eventsButton.onClick.AddListener(ReactToEventsClick);
            _quitButton.onClick.AddListener(ReactToQuitClick);
        }

        private void ReactToStartClick() {
            
            OnMenuOptionClicked?.Invoke(MenuOption.StartGame);
        }

        private void ReactToEventsClick() {
            
            OnMenuOptionClicked?.Invoke(MenuOption.Events);
        }

        private void ReactToQuitClick() {

            OnMenuOptionClicked?.Invoke(MenuOption.Quit);
        }
    }

    public enum MenuOption {

        StartGame,
        Events,
        Quit
    }

}