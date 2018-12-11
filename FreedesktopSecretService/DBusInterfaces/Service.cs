using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class SecretService : ISecretService
    {

        public ObjectPath ObjectPath { get; } = new ObjectPath("/org/freedesktop/secrets");
        
        private List<ObjectPath> Collections { get; set; } = new List<ObjectPath>();

        //
        // Methods
        //

        public Task<(string output, object result)> OpenSessionAsync(string algorithm, object input)
        {
            throw new NotImplementedException();
        }

        public Task<(ObjectPath collection, ObjectPath prompt)> CreateCollectionAsync(IDictionary<string, object> properties, string alias)
        {
            throw new NotImplementedException();
        }

        public Task<(ObjectPath[] unlocked, ObjectPath[] locked)> SearchItemAsync(IDictionary<string, string> attributes)
        {
            throw new NotImplementedException();
        }

        public Task<(ObjectPath[] unlocked, ObjectPath prompt)> UnlockAsync(ObjectPath[] objects)
        {
            throw new NotImplementedException();
        }

        public Task<(ObjectPath[] locked, ObjectPath prompt)> LockAsync(ObjectPath[] objects)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<ObjectPath, string>> GetSecretsAsync(ObjectPath[] items, ObjectPath session)
        {
            throw new NotImplementedException();
        }

        public async Task<ObjectPath> ReadAliasAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task SetAliasAsync(string name, ObjectPath collection)
        {
            throw new NotImplementedException();
        }

        class TestDisposable : IDisposable
        {
            public void Dispose()
            {
                Console.WriteLine("Disposed");
            }
        }
        
        //
        // Signals
        //

        public async Task<IDisposable> WatchCollectionCreatedAsync(Action<ObjectPath> handler)
        {
            return new TestDisposable();
        }

        public async Task<IDisposable> WatchCollectionDeletedAsync(Action<ObjectPath> handler)
        {
            return new TestDisposable();
        }

        public async Task<IDisposable> WatchCollectionChangedAsync(Action<ObjectPath> handler)
        {
            return new TestDisposable();
        }
        
        //
        // Properties
        //

        public async Task<object> GetAsync(string prop)
        {
            if (prop == nameof(Collections))
            {
                return Collections.ToArray();
            }

            return null;
        }

        public async Task<IDictionary<string, object>> GetAllAsync()
        {
            return new Dictionary<string, object> {[nameof(Collections)] = Collections.ToArray()};
        }

        public async Task SetAsync(string prop, object val)
        {
            if (prop == nameof(Collections))
            {
                throw new InvalidOperationException($"{prop} is readonly");
            }
        }

        public async Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler)
        {
            Console.WriteLine("watched");
            return new TestDisposable();
        }
    }
}