using System;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using FreedesktopSecretService.Utils;
using KeePassLib;


namespace FreedesktopSecretService.KeepassIntegration
{
    public class Collection : DBusImplementation.Collection
    {
        private readonly PwDatabase _db;
        private readonly FreedesktopSecretServiceExt _plugin;
        
        public Collection(PwDatabase db, FreedesktopSecretServiceExt plugin) : base(plugin.Dbus, db.Name.MD5Hash())
        {
            _db = db;
            _plugin = plugin;
            
            RegisterDatabaseItems();
        }
        
        private void RegisterDatabaseItems()
        {
            Task.Run(async () =>
            {
                try
                {
                    
                    var i = 0; // TODO Remove this restriction
                    foreach (var entry in _db.RootGroup.GetEntries(true))
                    {
                        i++;
                        if (i > 2)
                            break;

                        var item = new Item(_plugin, this, entry);
                        await Dbus.SessionConnection.RegisterObjectAsync(item);
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }
    }
}