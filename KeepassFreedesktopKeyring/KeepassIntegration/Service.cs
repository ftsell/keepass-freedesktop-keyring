using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeepassFreedesktopKeyring.DBusImplementation;
using KeePass.Forms;
using Tmds.DBus;

namespace KeepassFreedesktopKeyring.KeepassIntegration
{
    public class SecretService : DBusImplementation.SecretService
    {
        private readonly KeepassFreedesktopKeyringExt _plugin;

        private readonly IList<Collection> _collections = new List<Collection>();

        protected override ObjectPath[] Collections => (
            from i in _collections
            select i.ObjectPath
        ).ToArray();

        protected override ObjectPath[] SearchPwEntries(IDictionary<string, string> attributes)
        {
            // Find every entry from every collection where all searched attributes match
            var result = (
                from collection in _collections
                from entry in collection.PwEntries
                where attributes.All(
                          attr => entry.PwEntry.CustomData.Contains(
                              new KeyValuePair<string, string>(_plugin.DATA_PREFIX + attr.Key, attr.Value))) ||
                      entry.PwEntry.Strings.Exists("Title") &&
                      entry.PwEntry.Strings.Get("Title").ReadString() == "Nextcloud - ftsell"
                select entry.ObjectPath
            ).ToArray();

#if DEBUG
            var acc = $"{result.Length} results for search: ";
            foreach (var i in attributes)
                acc += $"{i.Key}={i.Value} ";
            Console.WriteLine(acc);
#endif

            return result;
        }

        public override async Task<IDictionary<ObjectPath, Secret>> GetSecretsAsync(ObjectPath[] items,
            ObjectPath session)
        {
            // TODO Enable session authentication

            var result = (
                from coll in _collections
                from entry in coll.PwEntries
                where items.Contains(entry.ObjectPath)
                select entry
            ).ToDictionary(entry => entry.ObjectPath,
                entry => new Secret(session, entry.PwEntry.Strings.ReadSafe("Password")));

#if DEBUG
            Console.WriteLine($"Queried {items.Length} secrets and got {result.Count}");
#endif
            
            return result;
        }

        public SecretService(KeepassFreedesktopKeyringExt plugin) : base(plugin.Dbus)
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