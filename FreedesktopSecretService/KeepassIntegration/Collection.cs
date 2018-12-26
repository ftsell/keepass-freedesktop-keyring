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
        internal readonly PwDatabase Db;

        private readonly FreedesktopSecretServiceExt _plugin;

        internal readonly IList<Item> PwEntries = new List<Item>();

        protected override ObjectPath[] Items => (
            from i in PwEntries
            select i.ObjectPath
        ).ToArray();

        protected override string Label { get; set; } = "TestLabel";
        protected override int Created { get; } = 0;
        protected override int Modified { get; } = 0;


        public Collection(PwDatabase db, FreedesktopSecretServiceExt plugin) : base(plugin.Dbus, db.Name.MD5Hash())
        {
            Db = db;
            _plugin = plugin;

            RegisterDatabaseItems();
        }

        private void RegisterDatabaseItems()
        {
            var i = 0; // TODO Remove this restriction
            // Get all entries in this group and entries of subgroups
            foreach (var entry in Db.RootGroup.GetEntries(true))
            {
                i++;
                if (i > 2)
                    break;
                
                // Create respective item object for PwEntry
                var item = new Item(_plugin, this, entry);
                PwEntries.Add(item);
            }
        }
    }
}