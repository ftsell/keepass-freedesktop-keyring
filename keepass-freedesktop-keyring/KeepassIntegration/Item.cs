using System.Collections.Generic;
using FreedesktopSecretService.DBusInterfaces;
using KeePassLib;

namespace FreedesktopSecretService.KeepassIntegration
{
    public class Item : DBusImplementation.Item
    {

        internal readonly PwEntry PwEntry;
        private readonly FreedesktopSecretServiceExt _plugin;

        protected override string Label => PwEntry.Strings.ReadSafe("Label");
        protected override int Created => 0;
        protected override int Modified => 0;
        protected override IDictionary<string, string> Attributes => new Dictionary<string, string>();
        protected override string Secret => PwEntry.Strings.ReadSafe("Password");
        
        public Item(FreedesktopSecretServiceExt plugin, Collection collection, PwEntry entry) 
            : base(plugin.Dbus, collection, entry.Uuid.ToHexString())
        {
            PwEntry = entry;
            _plugin = plugin;
        }
    }
}