using System;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using FreedesktopSecretService.Utils;
using KeePassLib;


namespace FreedesktopSecretService.KeepassIntegration
{
    public class Collection : DBusImplementation.Collection
    {
        private PwDatabase DB;
        
        public Collection(PwDatabase db, DBusWrapper dbus) : base(dbus, db.Name.MD5Hash())
        {
            DB = db;
            
            RegisterDatabaseItems();
        }
        
        private void RegisterDatabaseItems()
        {
            Task.Run(async () =>
            {
                try
                {
                    
                    var i = 0; // TODO Remove this restriction
                    foreach (var entry in DB.RootGroup.GetEntries(true))
                    {
                        i++;
                        if (i > 2)
                            break;

                        var item = new Item(Dbus, this, entry);
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