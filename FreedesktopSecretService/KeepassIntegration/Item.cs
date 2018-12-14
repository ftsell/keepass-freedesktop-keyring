using FreedesktopSecretService.DBusInterfaces;
using KeePassLib;

namespace FreedesktopSecretService.KeepassIntegration
{
    public class Item : DBusImplementation.Item
    {

        private readonly PwEntry _entry;
        private readonly FreedesktopSecretServiceExt _plugin;
        
        public Item(FreedesktopSecretServiceExt plugin, Collection collection, PwEntry entry) 
            : base(plugin.Dbus, collection, entry.Uuid.ToHexString())
        {
            _entry = entry;
            _plugin = plugin;
        }

        protected override string Secret => _entry.Strings.ReadSafe("Password");
    }
}