using System.Collections.Generic;
using FreedesktopSecretService.DBusInterfaces;
using KeePassLib;

namespace FreedesktopSecretService.KeepassIntegration
{
    public class Item : DBusImplementation.Item
    {

        private readonly PwEntry _entry;
        private readonly FreedesktopSecretServiceExt _plugin;

        protected override string Label => _entry.Strings.ReadSafe("Label");
        protected override int Created => 0;
        protected override int Modified => 0;
        protected override IDictionary<string, string> Attributes => new Dictionary<string, string>();
        protected override string Secret => _entry.Strings.ReadSafe("Password");
        
        public Item(FreedesktopSecretServiceExt plugin, Collection collection, PwEntry entry) 
            : base(plugin.Dbus, collection, entry.Uuid.ToHexString())
        {
            _entry = entry;
            _plugin = plugin;
        }
    }
}