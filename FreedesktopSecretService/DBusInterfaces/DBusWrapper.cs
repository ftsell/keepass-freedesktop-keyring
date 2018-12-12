using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeePassLib;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class DBusWrapper
    {
        // Then name we want to block and use on DBUS
        //    The different interfaces will be available under this name
        private static readonly string NAME = "org.freedesktop.secrets";

        internal Connection SessionConnection;
        private FreedesktopSecretServiceExt _plugin;

        internal IDictionary<PwDatabase, Collection> Collections = new Dictionary<PwDatabase, Collection>();

        public DBusWrapper(FreedesktopSecretServiceExt plugin)
        {
            _plugin = plugin;
            
            Task.Run(async () =>
            {
                if (!await InitializeDBusAsync())
                {
                    Console.WriteLine("I want to unload this plugin but don't know how");
                }
            });
        }
        
        private async Task<bool> InitializeDBusAsync()
        {
            SessionConnection = new Connection(Address.Session);
            await SessionConnection.ConnectAsync();

            if (await SessionConnection.IsServiceActiveAsync(NAME))
            {
                Console.WriteLine($"Service name {NAME} already taken on DBus");
                return false;
            }

            try
            {
                await SessionConnection.RegisterServiceAsync(NAME, ServiceRegistrationOptions.None);
                await SessionConnection.RegisterObjectAsync(new SecretService());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        internal async Task RegisterDatabaseAsync(PwDatabase db)
        {
            try
            {
                var coll = new Collection(db, this);
                await SessionConnection.RegisterObjectAsync(coll);
                Collections[db] = coll;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        internal async Task UnRegisterDatabaseAsync(PwDatabase db)
        {
            try
            {

                if (Collections.ContainsKey(db))
                {
                    Collections[db].Dispose();
                    SessionConnection.UnregisterObject(Collections[db]);
                }
                    
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        
    }
}