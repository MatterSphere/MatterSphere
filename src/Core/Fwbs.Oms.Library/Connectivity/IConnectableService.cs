using System;

namespace FWBS.OMS.Connectivity
{
    public interface IConnectableService 
    {
        event EventHandler Connected;
        event MessageEventHandler Disconnected;

        string ServiceName { get;}
        bool IsConnected { get;}
        bool IsAvailable { get;}
        System.Collections.ObjectModel.ReadOnlyCollection<Exception> Errors { get;}
        System.Collections.ObjectModel.ReadOnlyCollection<string> Messages { get;}

        IConnectableService[] GetServices();
        bool DependsOn(IConnectableService service);
        void OnDependentEvent(IConnectableService service, ConnectivityEvent serviceEvent);

        void Disconnect();
        void Connect();
        void Test();

        Guid Id { get;}
    }
}
