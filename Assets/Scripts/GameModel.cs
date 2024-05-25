using System.Collections.Generic;

namespace Scripts {

    public class GameModel {

        public long Coins;
        public int Energy;
        public int Keys;

        private Queue<Prize> Prizes;

        public GameModel(long coins, int energy, int keys, Queue<Prize> prizes) {
            
            Coins = coins;
            Energy = energy;
            Keys = keys;

            Prizes = prizes;
        }

        
        public Prize GetNextPrize() {

            var prize = Prizes.Dequeue();

            return prize;
        }

    }

}
