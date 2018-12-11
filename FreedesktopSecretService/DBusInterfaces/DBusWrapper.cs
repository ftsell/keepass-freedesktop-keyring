using System;
using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class DBusWrapper
    {
        // Then name we want to block and use on DBUS
        //    The different interfaces will be available under this name
        private static readonly string NAME = "org.freedesktop.secrets";

        private Connection _sessionConnection;
        private FreedesktopSecretServiceExt _plugin;

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
            _sessionConnection = new Connection(Address.Session);
            await _sessionConnection.ConnectAsync();

            if (await _sessionConnection.IsServiceActiveAsync(NAME))
            {
                Console.WriteLine($"Service name {NAME} already taken on DBus");
                return false;
            }

            try
            {
                await _sessionConnection.RegisterServiceAsync(NAME, ServiceRegistrationOptions.None);
                await _sessionConnection.RegisterObjectAsync(new SecretService());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
        
    }
}