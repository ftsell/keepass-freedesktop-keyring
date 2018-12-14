using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreedesktopSecretService.Utils;
using KeePassLib;
using Tmds.DBus;


namespace FreedesktopSecretService.KeepassIntegration
{
    public class Collection : DBusImplementation.Collection
    {
        private readonly PwDatabase _db;

        private readonly FreedesktopSecretServiceExt _plugin;

        private readonly IList<Item> _items = new List<Item>();

        internal override ObjectPath[] Items => (
            from i in _items
            select i.ObjectPath
        ).ToArray();


        public Collection(PwDatabase db, FreedesktopSecretServiceExt plugin) : base(plugin.Dbus, db.Name.MD5Hash())
        {
            _db = db;
            _plugin = plugin;

            RegisterDatabaseItems();
        }

        private void RegisterDatabaseItems()
        {
            var i = 0; // TODO Remove this restriction
            // Get all entries in this group and entries of subgroups
            foreach (var entry in _db.RootGroup.GetEntries(true))
            {
                i++;
                if (i > 2)
                    break;
                
                // Create respective item object for PwEntry
                var item = new Item(_plugin, this, entry);
                _items.Add(item);
            }
        }
    }
}