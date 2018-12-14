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

            Console.WriteLine($"Algorithm {algorithm} not supported on this service");
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
            Console.WriteLine($"Search attempted");
            throw new NotImplementedException();
        }

        public async Task<(ObjectPath[] unlocked, ObjectPath prompt)> UnlockAsync(ObjectPath[] objects)
        {
            // Don't unlock anything if no objects were specified
            if (objects.Length == 0)
                return (new ObjectPath[0], new ObjectPath("/"));
            
            Console.WriteLine($"Unlock requested for {string.Join(", ", objects)}");
            
            throw new NotImplementedException();
        }

        public async Task<(ObjectPath[] locked, ObjectPath prompt)> LockAsync(ObjectPath[] objects)
        {
            throw new NotImplementedException();
        }

        public async Task<IDictionary<ObjectPath, string>> GetSecretsAsync(ObjectPath[] items, ObjectPath session)
        {
            Console.WriteLine("Get secrets attempted");
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

            Console.WriteLine($"Alias {name} not found");
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

        #region Collection created
        private readonly IList<Action<ObjectPath>> _collectionCreatedHandlers = new List<Action<ObjectPath>>();
        
        private class CollectionCreatedDisposable : IDisposable
        {
            private Action<ObjectPath> _handler;
            private SecretService _service;

            public CollectionCreatedDisposable(Action<ObjectPath> handler, SecretService service)
            {
                _handler = handler;
                _service = service;
            }

            public void Dispose()
            {
                _service._collectionCreatedHandlers.Remove(_handler);
            }
        }

        public async Task<IDisposable> WatchCollectionCreatedAsync(Action<ObjectPath> handler)
        {
            _collectionCreatedHandlers.Add(handler);
            return new CollectionCreatedDisposable(handler, this);
        }

        protected void TriggerCollectionCreated(ObjectPath path)
        {
            foreach (var handler in _collectionCreatedHandlers)
                handler.Invoke(path);
        }
        #endregion

        #region Collection deleted   
        
        private readonly IList<Action<ObjectPath>> _collectionDeletedHandlers = new List<Action<ObjectPath>>();

        private class CollectionDeletedDisposable : IDisposable
        {
            private SecretService _service;
            private Action<ObjectPath> _handler;

            public CollectionDeletedDisposable(Action<ObjectPath> handler, SecretService service)
            {
                _handler = handler;
                _service = service;
            }

            public void Dispose()
            {
                _service._collectionDeletedHandlers.Remove(_handler);
            }
        }
        
        public async Task<IDisposable> WatchCollectionDeletedAsync(Action<ObjectPath> handler)
        {
            _collectionDeletedHandlers.Add(handler);
            return new CollectionDeletedDisposable(handler, this);
        }

        protected void TriggerCollectionDeleted(ObjectPath path)
        {
            foreach (var handler in _collectionDeletedHandlers)
                handler.Invoke(path);
        }
        
        #endregion
        
        #region Collection changed
        
        private IList<Action<ObjectPath>> _collectionChangedHandlers = new List<Action<ObjectPath>>();

        private class CollectionChangedDisposable : IDisposable
        {
            private SecretService _service;
            private Action<ObjectPath> _handler;

            public CollectionChangedDisposable(Action<ObjectPath> handler, SecretService service)
            {
                _handler = handler;
                _service = service;
            }

            public void Dispose()
            {
                _service._collectionChangedHandlers.Remove(_handler);
            }
        }

        public async Task<IDisposable> WatchCollectionChangedAsync(Action<ObjectPath> handler)
        {
            _collectionChangedHandlers.Add(handler);
            return new CollectionChangedDisposable(handler, this);
        }

        protected void TriggerCollectionChanged(ObjectPath path)
        {
            foreach (var handler in _collectionChangedHandlers)
                handler.Invoke(path);
        }
        
        #endregion
        
        #region Property changed
        
        private IList<Action<PropertyChanges>> _propertyChangesHandlers = new List<Action<PropertyChanges>>();

        private class PropertyChangesDisposable : IDisposable
        {
            private SecretService _service;
            private Action<PropertyChanges> _handler;

            public PropertyChangesDisposable(Action<PropertyChanges> handler, SecretService service)
            {
                _handler = handler;
                _service = service;
            }

            public void Dispose()
            {
                _service._propertyChangesHandlers.Remove(_handler);
            }
        }

        public async Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler)
        {
            _propertyChangesHandlers.Add(handler);
            return new PropertyChangesDisposable(handler, this);
        }

        protected void TriggerPropertyChanged(PropertyChanges changes)
        {
            foreach (var handler in _propertyChangesHandlers)
                handler.Invoke(changes);
        }

        #endregion
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