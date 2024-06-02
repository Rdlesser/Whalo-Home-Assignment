using System.Collections.Generic;
using System.Linq;
using General;

namespace Core {

    public class GameModel {

        public long Coins;
        public long Energy;
        public long Keys;

        public long AccumulatedCoins;
        public long AccumulatedEnergy;

        private Queue<Prize> _prizes;
        private List<int> _closedBoxIds;

        public GameModel(long coins, int energy, int keys, Queue<Prize> prizes) {
            
            Coins = coins;
            Energy = energy;
            Keys = keys;

            _prizes = prizes;

            ResetClosedBoxes();
        }

        private void ResetClosedBoxes() {

            _closedBoxIds = new List<int>();
            _closedBoxIds.AddRange(Enumerable.Range(0, _prizes.Count));
        }

        public bool WereAllPrizesCollected() {

            return _prizes.IsNullOrEmpty();
        }

        public bool TryGetNextPrize(out Prize prize) {

            if (_prizes.Count == 0) {
                prize = null;
                return false;
            }

            prize = GetNextPrize();

            return true;
        }
        
        private Prize GetNextPrize() {

            var prize = _prizes.Dequeue();

            return prize;
        }

        public void SetBoxOpened(int boxId) {

            _closedBoxIds.Remove(boxId);
        }

        public List<int> GetClosedBoxes() {

            return _closedBoxIds;
        }

    }

}