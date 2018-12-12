using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeePassLib;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class SecretService : ISecretService
    {

        public ObjectPath ObjectPath { get; } = new ObjectPath("/org/freedesktop/secrets");
        
        // property
        private List<ObjectPath> Collections { get; } = new List<ObjectPath>();
        
        private IDictionary<PwDatabase, Collection> _collections = new Dictionary<PwDatabase, Collection>();
        
        private IDictionary<ObjectPath, Session> _sessions = new Dictionary<ObjectPath, Session>();

        private DBusWrapper _dbus;

        public SecretService(DBusWrapper dbus)
        {
            _dbus = dbus;
        }
        
        internal async Task RegisterDatabaseAsync(PwDatabase db)
        {
            try
            {
                var coll = new Collection(db, _dbus);
                await _dbus.SessionConnection.RegisterObjectAsync(coll);
                _collections[db] = coll;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        internal async Task UnRegisterDatabaseAsync(PwDatabase db)
        {
            try
            {

                if (_collections.ContainsKey(db))
                {
                    _collections[db].Dispose();
                    _dbus.SessionConnection.UnregisterObject(_collections[db]);
                }
                    
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        //
        // Methods
        //

        public async Task<(object output, ObjectPath result)> OpenSessionAsync(string algorithm, object input)
        {
            if (algorithm == "plain")
            {
                var session = new Session();
                await _dbus.SessionConnection.RegisterObjectAsync(session);
                _sessions[session.ObjectPath] = session;
                
                Console.WriteLine($"Opened new {algorithm} Session under {session.ObjectPath}");
                
                return ("", session.ObjectPath);
            }

            else
            {
                throw new NotSupportedException();
            }
        }

        public async Task<(ObjectPath collection, ObjectPath prompt)> CreateCollectionAsync(IDictionary<string, object> properties, string alias)
        {
            throw new NotImplementedException();
        }

        public async Task<(ObjectPath[] unlocked, ObjectPath[] locked)> SearchItemAsync(IDictionary<string, string> attributes)
        {
            throw new NotImplementedException();
        }

        public async Task<(ObjectPath[] unlocked, ObjectPath prompt)> UnlockAsync(ObjectPath[] objects)
        {
            throw new NotImplementedException();
        }

        public async Task<(ObjectPath[] locked, ObjectPath prompt)> LockAsync(ObjectPath[] objects)
        {
            throw new NotImplementedException();
        }

        public async Task<IDictionary<ObjectPath, string>> GetSecretsAsync(ObjectPath[] items, ObjectPath session)
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

        public class TestDisposable : IDisposable
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
            return new TestDisposable();
        }
    }
}