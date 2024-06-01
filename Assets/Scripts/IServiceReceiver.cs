using System;

public interface IServiceReceiver {

    public Type InjectedType { get; }
    public void Inject(object service);

}