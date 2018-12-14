using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusImplementation
{
    public abstract class Item : IItem
    {
        public ObjectPath ObjectPath { get; }

        protected readonly DBusWrapper Dbus;

        private ItemProperties _props;

        protected abstract string Secret { get; }

        public Item(DBusWrapper dbus, Collection collection, string uuid)
        {
            Dbus = dbus;

            _props = new ItemProperties
            {
                Created = 0,
                Modified = 0,
                Locked = false,
                Attributes = new Dictionary<string, string>(),
                Label = "testLabel"
            };

            ObjectPath = new ObjectPath(collection.ObjectPath + $"/{uuid}");
        }

        
        #region Methods
        //
        // Methods
        //

        public Task<ObjectPath> DeleteAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Secret> GetSecretAsync(ObjectPath session)
        {
            // Since we only support plain-text transfer of secret it is enough to know that the specified session exists
            if (true) // TODO Re-enable session based authentication
            {
                return new Secret(session, Secret);
            }

            throw new UnauthorizedAccessException();
        }

        public Task SetSecretAsync(Secret secret)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
        
        
        #region Signals
        //
        // Signals
        //
        
        private readonly HashSet<Action<PropertyChanges>> _changeWatchers = new HashSet<Action<PropertyChanges>>();

        private class ChangeDisposable : IDisposable
        {
            private Action<PropertyChanges> _handler;
            private Item _item;

            public ChangeDisposable(Action<PropertyChanges> handler, Item item)
            {
                _handler = handler;
                _item = item;
            }

            public void Dispose()
            {
                _item._changeWatchers.Remove(_handler);
            }
        }

        // TODO Fire signal when _entry gets modified
        public async Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler)
        {
            _changeWatchers.Add(handler);
            return new ChangeDisposable(handler, this);
        }
        
        #endregion
        
        
        #region Properties
        //
        // Properties
        //

        public async Task<object> GetAsync(string prop)
        {
            switch (prop)
            {
                case nameof(ItemProperties.Attributes):
                    return _props.Attributes;
                case nameof(ItemProperties.Created):
                    return _props.Created;
                case nameof(ItemProperties.Label):
                    return _props.Label;
                case nameof(ItemProperties.Locked):
                    return _props.Locked;
                case nameof(ItemProperties.Modified):
                    return _props.Modified;
                
                default:
                    throw new ArgumentException();
            }
        }

        public async Task<ItemProperties> GetAllAsync()
        {
            return _props;
        }

        public Task SetAsync(string prop, object val)
        {
            Console.WriteLine("Set");
            throw new NotImplementedException();
        }
        
        #endregion
    }
}