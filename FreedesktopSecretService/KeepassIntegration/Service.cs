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
        public SecretService(DBusWrapper dBus) : base(dBus)
        {
            dBus.Host.MainWindow.FileOpened += RegisterDatabase;
        }

        private void RegisterDatabase(object sender, FileOpenedEventArgs eventArgs)
        {
            Task.Run(async () =>
            {
                try
                {
                    // Create new Collection and register it on DBus
                    var coll = new Collection(eventArgs.Database, DBus);
                    await DBus.SessionConnection.RegisterObjectAsync(coll);
                    Collections.Append(coll.ObjectPath);

                    // Unload Hook
                    DBus.Host.MainWindow.FileClosingPre += UnRegisterDatabaseAsync;
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