using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreedesktopSecretService.Utils;
using KeePassLib;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class Collection : ICollection, IDisposable
    {
        public ObjectPath ObjectPath { get; }
        private PwDatabase _db;
        private DBusWrapper _dbus;

        internal IDictionary<PwEntry, Item> Items = new Dictionary<PwEntry, Item>();

        public Collection(PwDatabase db, DBusWrapper dbus)
        {
            _db = db;
            _dbus = dbus;
            ObjectPath = new ObjectPath($"/org/freedesktop/secrets/collection/{db.Name.MD5Hash()}");
            
            RegisterDatabaseItems();
        }
        
        public void Dispose()
        {
            UnRegisterDatabaseItems();
        }

        private void RegisterDatabaseItems()
        {
            foreach (PwEntry entry in _db.RootGroup.GetEntries(true))
            {
                var item = new Item(_dbus, this, entry);
                _dbus.SessionConnection.RegisterObjectAsync(item);
                Items[entry] = item;
            }
        }

        private void UnRegisterDatabaseItems()
        {
            
        }

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

        public Task<(ObjectPath item, ObjectPath prompt)> CreateItemAsync(IDictionary<string, object> properties, Secret secret, bool replace)
        {
            throw new NotImplementedException();
        }
        
        //
        // Signals
        //

        public async Task<IDisposable> WatchItemCreatedAsync(Action<ObjectPath> handler)
        {
            return new SecretService.TestDisposable();
        }

        public async Task<IDisposable> WatchItemDeletedAsync(Action<ObjectPath> handler)
        {
            return new SecretService.TestDisposable();
        }

        public async Task<IDisposable> WatchItemChangedAsync(Action<ObjectPath> handler)
        {
            return new SecretService.TestDisposable();
        }
    }
}