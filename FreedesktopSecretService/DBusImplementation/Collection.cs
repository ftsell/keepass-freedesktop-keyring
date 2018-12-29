using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using FreedesktopSecretService.KeepassIntegration.PromptTypes;
using FreedesktopSecretService.Utils;
using KeePassLib;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusImplementation
{
    public abstract class Collection : ICollection
    {
        public ObjectPath ObjectPath { get; }

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

        public async Task<(ObjectPath item, ObjectPath prompt)> CreateItemAsync(IDictionary<string, object> properties,
            Secret secret, bool replace)
        {

            Prompt prompt = new ItemCreationPrompt();
            
#if DEBUG
            var acc = "ItemCreation prompt created for item with props: ";
            foreach (var i in properties)
                acc += $"{i.Key}={i.Value} ";
            Console.WriteLine(acc);
#endif

            // We always create a prompt
            return (new ObjectPath("/"), prompt.ObjectPath);
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

        #region Property changed

        private IList<Action<PropertyChanges>> _propertyChangesHandlers = new List<Action<PropertyChanges>>();

        private class PropertyChangesDisposable : IDisposable
        {
            private Collection _collection;
            private Action<PropertyChanges> _handler;

            public PropertyChangesDisposable(Action<PropertyChanges> handler, Collection collection)
            {
                _handler = handler;
                _collection = collection;
            }

            public void Dispose()
            {
                _collection._propertyChangesHandlers.Remove(_handler);
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
        protected abstract ObjectPath[] Items { get; }
        protected abstract string Label { get; set; }
        protected abstract int Created { get; }
        protected abstract int Modified { get; }

        public async Task<object> GetAsync(string prop)
        {
            switch (prop)
            {
                case nameof(CollectionProperties.Label):
                    return Label;
                case nameof(CollectionProperties.Locked):
                    return false;
                case nameof(CollectionProperties.Created):
                    return Created;
                case nameof(CollectionProperties.Modified):
                    return Modified;
                case nameof(CollectionProperties.Items):
                    return Items;

                default:
                    throw new ArgumentException();
            }
        }

        public async Task<CollectionProperties> GetAllAsync()
        {
            return new CollectionProperties
            {
                Created = Created,
                Label = Label,
                Items = Items,
                Modified = Modified,
                Locked = false
            };
        }

        public async Task SetAsync(string prop, object val)
        {
            if (prop == nameof(CollectionProperties.Label) && val is string)
                Label = val as string;

            else
                throw new ArgumentException();
        }

        #endregion
    }
}