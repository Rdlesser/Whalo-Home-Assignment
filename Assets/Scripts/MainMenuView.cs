using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class MainMenuView : MonoBehaviour {

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _eventsButton;
        [SerializeField] private Button _quitButton;

        public Action<MenuOption> OnMenuOptionClicked;

        private void Start() {
            
            _quitButton.onClick.AddListener(ReactToQuitClick);
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
