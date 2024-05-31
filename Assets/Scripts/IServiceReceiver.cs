using System;

namespace Scripts {

    public interface IServiceReceiver {

        public Type InjectedType { get; }
        public void Inject(object service);

    }

}
