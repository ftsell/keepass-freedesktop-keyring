using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeePassLib;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class Item : IItem
    {
        public ObjectPath ObjectPath { get; }

        private PwEntry _entry;

        private DBusWrapper _dbus;

        private ItemProperties _props;

        public Item(DBusWrapper dbus, Collection collection, PwEntry entry)
        {
            _entry = entry;
            _dbus = dbus;

            _props = new ItemProperties();
            _props.Created = 0;
            _props.Modified = 0;
            _props.Locked = false;
            _props.Attributes = new Dictionary<string, string>();
            _props.Label = "testLabel";

            ObjectPath = new ObjectPath(collection.ObjectPath + $"/{entry.Uuid.ToHexString()}");
        }

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
            if (_dbus._Service._Sessions.ContainsKey(session) || true) // TODO Re-enable session based authentication
            {
                var title = _entry.Strings.Get("Title");
                var password = _entry.Strings.Get("Password");

                return new Secret(session, password.ReadString());
            }

            throw new UnauthorizedAccessException();
        }

        public Task SetSecretAsync(Secret secret)
        {
            throw new System.NotImplementedException();
        }

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
    }
}