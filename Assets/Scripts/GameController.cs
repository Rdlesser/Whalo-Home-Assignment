using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts {

    public class GameController : MonoBehaviour{

        [SerializeField] private List<BoxView> _boxes;
        [SerializeField] private UIView _uiView;

        private Queue<Prize> _prizes; 

        private void Start() {

            InitPrizeQueue();
            RegisterToBoxViewEvents();
        }

        private void InitPrizeQueue() {
            
            
        }

        private void RegisterToBoxViewEvents() {

            for (int i = 0; i < _boxes.Count; i++) {

                _boxes[i].OnBoxClicked += HandleBoxClick;
            }
        }

        private void HandleBoxClick(BoxView box) {

            var prize = GetNextPrize();
            
        }

        private Prize GetNextPrize() {

            var prize = _prizes.Dequeue();

            return prize;
        }

    }

}
