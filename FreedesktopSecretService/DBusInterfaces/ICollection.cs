using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using Tmds.DBus;

namespace FreedesktopSecretService
{
    public interface ICollection : IDBusObject
    {

        //
        // Methods
        //
        Task<ObjectPath> Delete();

        Task<ObjectPath[]> SearchItems(IDictionary<string, string> attributes);

        Task<(ObjectPath item, ObjectPath prompt)> CreateItem(IDictionary<string, object> properties, Secret secret, bool replace);
        
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