using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    /// <summary>
    /// <a href="https://specifications.freedesktop.org/secret-service/re01.html">freedesktop.org specification</a>
    /// </summary>
    [DBusInterface("org.freedesktop.Secret.Service")]
    public interface ISecretService : IDBusObject
    {
        //
        // Methods
        //

        Task<(object output, ObjectPath result)> OpenSessionAsync(string algorithm, object input);

        Task<(ObjectPath collection, ObjectPath prompt)> CreateCollectionAsync(IDictionary<string, object> properties,
            string alias);

        Task<(ObjectPath[] unlocked, ObjectPath[] locked)> SearchItemAsync(IDictionary<string, string> attributes);

        Task<(ObjectPath[] unlocked, ObjectPath prompt)> UnlockAsync(ObjectPath[] objects);

        Task<(ObjectPath[] locked, ObjectPath prompt)> LockAsync(ObjectPath[] objects);

        Task<IDictionary<ObjectPath, string>> GetSecretsAsync(ObjectPath[] items, ObjectPath session);

        Task<ObjectPath> ReadAliasAsync(string name);

        Task SetAliasAsync(string name, ObjectPath collection);

        //
        // Signals
        //
        Task<IDisposable> WatchCollectionCreatedAsync(Action<ObjectPath> handler);

        Task<IDisposable> WatchCollectionDeletedAsync(Action<ObjectPath> handler);

        Task<IDisposable> WatchCollectionChangedAsync(Action<ObjectPath> handler);

        //
        // Properties
        //
        Task<object> GetAsync(string prop);
        Task<IDictionary<string, object>> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}