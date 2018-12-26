using System;
using System.Collections.Generic;
using System.Linq;
using KeePass.Forms;
using Tmds.DBus;

namespace FreedesktopSecretService.KeepassIntegration
{
    public class SecretService : DBusImplementation.SecretService
    {
        private readonly FreedesktopSecretServiceExt _plugin;

        private readonly IList<Collection> _collections = new List<Collection>();

        // YAY, my very first LINQ magic ever :)
        protected override ObjectPath[] Collections => (
            from i in _collections
            select i.ObjectPath
        ).ToArray();

        protected override ObjectPath[] SearchPwEntries(IDictionary<string, string> attributes)
        {
            // Find every entry from every collection where all searched attributes match
            return (
                from collection in _collections
                from entry in collection.PwEntries
                where attributes.All(
                    attr => entry.PwEntry.CustomData.Contains(
                        new KeyValuePair<string, string>(_plugin.DATA_PREFIX + attr.Key, attr.Value)))
                select entry.ObjectPath).ToArray();
        }

        public SecretService(FreedesktopSecretServiceExt plugin) : base(plugin.Dbus)
        {
            _plugin = plugin;

            plugin.Host.MainWindow.FileOpened += RegisterDatabase;
        }

        private void RegisterDatabase(object sender, FileOpenedEventArgs eventArgs)
        {
            // Create new Collection for just opened database
            var coll = new Collection(eventArgs.Database, _plugin);
            _collections.Add(coll);

            // Register Hook for unloading
            //_plugin.Host.MainWindow.FileClosingPre += UnRegisterDatabaseAsync; TODO Unregister Collection on database close
        }
    }
}