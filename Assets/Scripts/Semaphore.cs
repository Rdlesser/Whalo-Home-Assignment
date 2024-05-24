using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scripts {

    public class Semaphore {

        private SemaphoreSlim _semaphore;
        public int Awaiters { get; private set; } = 0;
        private Exception _exception;

        public Semaphore(int initialCount) {
            _semaphore = new SemaphoreSlim(initialCount);
        }

        /// <summary>
        /// wait until the semaphore is released
        /// </summary>
        /// <returns></returns>
        public async Task WaitAsync(CancellationToken c = default) {
            ++Awaiters;
            await _semaphore.WaitAsync(c);
            if(_exception != null) {
                throw _exception;
            }
        }

        /// <summary>
        /// releases X awaiters
        /// </summary>
        /// <param name="count"></param>
        public void Release(int count) {
            Awaiters -= count;
            _semaphore.Release(count);
        }
        
        /// <summary>
        /// release a single awaiter
        /// </summary>
        public void Release() {
            --Awaiters;
            _semaphore.Release(1);
        }

        /// <summary>
        /// release all awaiters
        /// </summary>
        public void ReleaseAll() {
            var count = Awaiters;
            Awaiters = 0;
            if (count > 0) {
                _semaphore.Release(count);
            }
        }

        public void Error(Exception e) {
            _exception = e;
            ReleaseAll();
        }

        /// <summary>
        /// disposes of the semaphore
        /// </summary>
        public void Dispose() {
            _semaphore.Dispose();
            _semaphore = null;
        }
    }

}
