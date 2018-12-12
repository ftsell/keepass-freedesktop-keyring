using System;
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

        public Item(DBusWrapper dbus, Collection collection, PwEntry entry)
        {
            _entry = entry;
            _dbus = dbus;
            
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
            if (_dbus._Service._Sessions.ContainsKey(session) || true)
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
    }
}