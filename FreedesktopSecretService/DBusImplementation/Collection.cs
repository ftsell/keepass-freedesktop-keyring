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

        internal virtual ObjectPath[] Items => new ObjectPath[0];

        private readonly DBusWrapper _dbus;

        internal Collection(DBusWrapper dbus, string uuid)
        {
            _dbus = dbus;
            ObjectPath = new ObjectPath($"/org/freedesktop/secrets/collection/{uuid}");

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
                    throw;
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
        
        #region Item created
        
        private IList<Action<ObjectPath>> _itemCreatedHandlers = new List<Action<ObjectPath>>();

        private class ItemCreatedDisposable : IDisposable
        {

            private Collection _collection;
            private Action<ObjectPath> _handlers;

            public ItemCreatedDisposable(Action<ObjectPath> handlers, Collection collection)
            {
                _handlers = handlers;
                _collection = collection;
            }

            public void Dispose()
            {
            }
        }

        public async Task<IDisposable> WatchItemCreatedAsync(Action<ObjectPath> handler)
        {
            _itemCreatedHandlers.Add(handler);
            return new ItemCreatedDisposable(handler, this);
        }

        protected void TriggerItemCreated(ObjectPath path)
        {
            foreach (var handler in _itemCreatedHandlers)
                handler.Invoke(path);
        }
        
        #endregion
        
        #region Item deleted

        private IList<Action<ObjectPath>> _itemDeletedHandlers = new List<Action<ObjectPath>>();

        private class ItemDeletedDisposable : IDisposable
        {

            private Collection _collection;
            private Action<ObjectPath> _handlers;

            public ItemDeletedDisposable(Action<ObjectPath> handlers, Collection collection)
            {
                _handlers = handlers;
                _collection = collection;
            }

            public void Dispose()
            {
            }
        }

        public async Task<IDisposable> WatchItemDeletedAsync(Action<ObjectPath> handler)
        {
            _itemDeletedHandlers.Add(handler);
            return new ItemDeletedDisposable(handler, this);
        }

        protected void TriggerItemDeleted(ObjectPath path)
        {
            foreach (var handler in _itemDeletedHandlers)
                handler.Invoke(path);
        }
        
        #endregion
        
        #region Item changed

        private IList<Action<ObjectPath>> _itemChangedHandlers = new List<Action<ObjectPath>>();

        private class ItemChangedDisposable : IDisposable
        {

            private Collection _collection;
            private Action<ObjectPath> _handlers;

            public ItemChangedDisposable(Action<ObjectPath> handlers, Collection collection)
            {
                _handlers = handlers;
                _collection = collection;
            }

            public void Dispose()
            {
            }
        }

        public async Task<IDisposable> WatchItemChangedAsync(Action<ObjectPath> handler)
        {
            _itemChangedHandlers.Add(handler);
            return new ItemChangedDisposable(handler, this);
        }

        protected void TriggerItemChanged(ObjectPath path)
        {
            foreach (var handler in _itemChangedHandlers)
                handler.Invoke(path);
        }

        #endregion
        #endregion
    }
}