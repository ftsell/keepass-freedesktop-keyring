using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreedesktopSecretService.Utils;
using KeePassLib;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class Collection : ICollection
    {
        public ObjectPath ObjectPath { get; }

        public Collection(PwDatabase db)
        {
            ObjectPath = new ObjectPath($"/org/freedesktop/secrets/collection/{db.Name.MD5Hash()}");
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