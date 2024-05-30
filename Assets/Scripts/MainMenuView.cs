using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scripts {

    public class MainMenuView : MonoBehaviour {

        [SerializeField] private Button _StartGameButton;
        [SerializeField] private Button _eventsButton;
        [SerializeField] private Button _quitButton;

        public Action<MenuOption> OnMenuOptionClicked;

        private void Start() {
            
            _StartGameButton.onClick.AddListener(ReactToStartClick);
            _quitButton.onClick.AddListener(ReactToQuitClick);
        }

        private void ReactToStartClick() {
            
            OnMenuOptionClicked?.Invoke(MenuOption.StartGame);
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
