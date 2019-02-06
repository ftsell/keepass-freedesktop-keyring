using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeepassFreedesktopKeyring.DBusImplementation;
using Tmds.DBus;

namespace KeepassFreedesktopKeyring.DBusInterfaces
{
    /// <summary>
    /// <a href="https://specifications.freedesktop.org/secret-service/re02.html">Freedesktop specification</a>
    /// </summary>
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
        Task<object> GetAsync(string prop);
        Task<CollectionProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);

    }

    [Dictionary]
    public struct CollectionProperties
    {
        [Property(Access = PropertyAccess.Read)]
        public ObjectPath[] Items;

        public string Label;

        [Property(Access = PropertyAccess.Read)]
        public bool Locked;

        [Property(Access = PropertyAccess.Read)]
        public int Created;

        [Property(Access = PropertyAccess.Read)]
        public int Modified;
    }
}