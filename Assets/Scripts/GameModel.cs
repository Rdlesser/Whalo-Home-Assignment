using System.Collections.Generic;

namespace Scripts {

    public class GameModel {

        public long Coins;
        public int Energy;
        public int Keys;

        public long AccumulatedCoins;
        public int AccumulatedEnergy;

        private Queue<Prize> Prizes;

        public GameModel(long coins, int energy, int keys, Queue<Prize> prizes) {
            
            Coins = coins;
            Energy = energy;
            Keys = keys;

            Prizes = prizes;
        }

        public bool WereAllPrizesCollected() {

            return Prizes.IsNullOrEmpty();
        }
        
        public Prize GetNextPrize() {

            var prize = Prizes.Dequeue();

            return prize;
        }

    }

}
