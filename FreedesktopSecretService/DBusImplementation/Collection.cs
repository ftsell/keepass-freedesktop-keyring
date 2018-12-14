using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using FreedesktopSecretService.Utils;
using KeePassLib;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusImplementation
{
    public class Collection : ICollection
    {
        public ObjectPath ObjectPath { get; }
        internal readonly DBusWrapper Dbus;

        internal Collection(DBusWrapper dbus, string uuid)
        {
            Dbus = dbus;
            ObjectPath = new ObjectPath($"/org/freedesktop/secrets/collection/{uuid}");
        }


        #region Methods

        //
        // Methods
        //

        public Task<ObjectPath> DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ObjectPath[]> SearchItemsAsync(IDictionary<string, string> attributes)
        {
            throw new NotImplementedException();
        }

        public Task<(ObjectPath item, ObjectPath prompt)> CreateItemAsync(IDictionary<string, object> properties,
            Secret secret, bool replace)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Singals

        //
        // Signals
        //

        public async Task<IDisposable> WatchItemCreatedAsync(Action<ObjectPath> handler)
        {
            throw new NotImplementedException();
        }

        public async Task<IDisposable> WatchItemDeletedAsync(Action<ObjectPath> handler)
        {
            throw new NotImplementedException();
        }

        public async Task<IDisposable> WatchItemChangedAsync(Action<ObjectPath> handler)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}