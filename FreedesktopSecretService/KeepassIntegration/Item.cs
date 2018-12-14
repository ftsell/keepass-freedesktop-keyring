using FreedesktopSecretService.DBusInterfaces;
using KeePassLib;

namespace FreedesktopSecretService.KeepassIntegration
{
    public class Item : DBusImplementation.Item
    {

        private readonly PwEntry _entry;
        
        public Item(DBusWrapper dbus, Collection collection, PwEntry entry) 
            : base(dbus, collection, entry.Uuid.ToHexString())
        {
            _entry = entry;
        }

        protected override string Secret => _entry.Strings.ReadSafe("Password");
    }
}