using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using KeePassLib;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusImplementation
{
    public class SecretService : ISecretService
    {
        public ObjectPath ObjectPath { get; } = new ObjectPath("/org/freedesktop/secrets");

        protected virtual ObjectPath[] Collections => new ObjectPath[0];

        private readonly DBusWrapper _dBus;

        internal SecretService(DBusWrapper dBus)
        {
            _dBus = dBus;
        }


        #region Methods
        //
        // Methods
        //

        public async Task<(object output, ObjectPath result)> OpenSessionAsync(string algorithm, object input)
        {
            if (algorithm == "plain")
            {
                var session = new Session();
                await _dBus.SessionConnection.RegisterObjectAsync(session);

                Console.WriteLine($"Opened new {algorithm} Session under {session.ObjectPath}");

                return ("", session.ObjectPath);
            }

            throw new NotSupportedException();
        }

        public async Task<(ObjectPath collection, ObjectPath prompt)> CreateCollectionAsync(
            IDictionary<string, object> properties, string alias)
        {
            throw new NotImplementedException();
        }

        public async Task<(ObjectPath[] unlocked, ObjectPath[] locked)> SearchItemAsync(
            IDictionary<string, string> attributes)
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
            if (Collections.Length == 0)
                return new ObjectPath("/");

            if (name == "default")
            {
                return Collections[0];
            }

            throw new NotImplementedException(); // Correct aliases still need to be implemented
        }

        public async Task SetAliasAsync(string name, ObjectPath collection)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Signals
        //
        // Signals
        //

        public async Task<IDisposable> WatchCollectionCreatedAsync(Action<ObjectPath> handler)
        {
            throw new NotImplementedException();
        }

        public async Task<IDisposable> WatchCollectionDeletedAsync(Action<ObjectPath> handler)
        {
            throw new NotImplementedException();
        }

        public async Task<IDisposable> WatchCollectionChangedAsync(Action<ObjectPath> handler)
        {
            throw new NotImplementedException();
        }

        public async Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Properties
        //
        // Properties
        //

        public async Task<object> GetAsync(string prop)
        {
            if (prop == nameof(ServiceProperties.Collections))
                return Collections;

            throw new ArgumentException();
        }

        public async Task<ServiceProperties> GetAllAsync()
        {
            return new ServiceProperties {Collections = Collections};
        }

        public async Task SetAsync(string prop, object val)
        {
            // All our properties are readonly
            throw new ArgumentException();
        }

        #endregion
    }
}