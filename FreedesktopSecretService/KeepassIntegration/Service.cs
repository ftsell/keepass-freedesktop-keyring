using System;
using System.Linq;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using KeePass.Forms;
using KeePassLib;

namespace FreedesktopSecretService.KeepassIntegration
{
    public class SecretService : DBusImplementation.SecretService
    {

        private readonly FreedesktopSecretServiceExt _plugin;
        
        public SecretService(FreedesktopSecretServiceExt plugin) : base(plugin.Dbus)
        {
            _plugin = plugin;
            
            plugin.Host.MainWindow.FileOpened += RegisterDatabase;
        }

        private void RegisterDatabase(object sender, FileOpenedEventArgs eventArgs)
        {
            Task.Run(async () =>
            {
                try
                {
                    // Create new Collection and register it on DBus
                    var coll = new Collection(eventArgs.Database, _plugin);
                    await DBus.SessionConnection.RegisterObjectAsync(coll);
                    Collections.Append(coll.ObjectPath);

                    // Unload Hook
                    _plugin.Host.MainWindow.FileClosingPre += UnRegisterDatabaseAsync;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        private void UnRegisterDatabaseAsync(object sender, FileClosingEventArgs eventArgs)
        {
            Console.WriteLine("Unloading of collections is not yet implemented");
            throw new NotImplementedException();
        }
    }
}