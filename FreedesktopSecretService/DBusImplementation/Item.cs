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

        private readonly DBusWrapper _dbus;

        private ItemProperties Props => new ItemProperties()
        {
            Label = Label,
            Created = Created,
            Modified = Modified,
            Attributes = Attributes,
            Locked = false
        };
        
        protected abstract string Label { get; }
        protected abstract int Created { get; }
        protected abstract int Modified { get; }
        protected abstract IDictionary<string, string> Attributes { get; }
        
        protected abstract string Secret { get; }

        
        public Item(DBusWrapper dbus, Collection collection, string uuid)
        {
            ObjectPath = new ObjectPath(collection.ObjectPath + $"/{uuid}");
            _dbus = dbus;
            
            RegisterSelf();
        }

        private void RegisterSelf()
        {
            Task.Run(async () =>
            {
                try
                {

                    await _dbus.SessionConnection.RegisterObjectAsync(this);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        
        #region Methods
        //
        // Methods
        //

        public Task<ObjectPath> DeleteAsync()
        {
            throw new NotImplementedException();
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
                    return Props.Attributes;
                case nameof(ItemProperties.Created):
                    return Props.Created;
                case nameof(ItemProperties.Label):
                    return Props.Label;
                case nameof(ItemProperties.Locked):
                    return Props.Locked;
                case nameof(ItemProperties.Modified):
                    return Props.Modified;
                
                default:
                    throw new ArgumentException();
            }
        }

        public async Task<ItemProperties> GetAllAsync()
        {
            return Props;
        }

        public Task SetAsync(string prop, object val)
        {
            Console.WriteLine("Set");
            throw new NotImplementedException();
        }
        
        #endregion
    }
}