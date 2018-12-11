using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    [DBusInterface("org.freedesktop.Secret.Collection")]
    public interface ICollection : IDBusObject
    {

        //
        // Methods
        //
        Task<ObjectPath> DeleteAsync();

        Task<ObjectPath[]> SearchItemsAsync(IDictionary<string, string> attributes);

        Task<(ObjectPath item, ObjectPath prompt)> CreateItemAsync(IDictionary<string, object> properties, Secret secret, bool replace);
        
        //
        // Signals
        //
        Task<IDisposable> WatchItemCreatedAsync(Action<ObjectPath> handler);
        
        Task<IDisposable> WatchItemDeletedAsync(Action<ObjectPath> handler);
        
        Task<IDisposable> WatchItemChangedAsync(Action<ObjectPath> handler);
        
        //
        // Properties
        //

    }
}