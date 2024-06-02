using System;
using System.Threading;
using System.Threading.Tasks;

namespace General {

    public class Semaphore {

        private SemaphoreSlim _semaphore;
        public int Awaiters { get; private set; } = 0;
        private Exception _exception;

        public Semaphore(int initialCount) {
            _semaphore = new SemaphoreSlim(initialCount);
        }
        
        public async Task WaitAsync(CancellationToken c = default) {
            ++Awaiters;
            await _semaphore.WaitAsync(c);
            if(_exception != null) {
                throw _exception;
            }
        }

        public void Release(int count) {
            Awaiters -= count;
            _semaphore.Release(count);
        }
        
        public void Release() {
            --Awaiters;
            _semaphore.Release(1);
        }
        
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

        public void Dispose() {
            _semaphore.Dispose();
            _semaphore = null;
        }
    }

}