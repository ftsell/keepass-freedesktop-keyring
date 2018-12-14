using System;
using System.Threading.Tasks;
using FreedesktopSecretService.KeepassIntegration;
using KeePass.Plugins;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class DBusWrapper
    {
        // Then name we want to block and use on DBUS
        //    The different interfaces will be available under this name
        private static readonly string NAME = "org.freedesktop.secrets";

        internal Connection SessionConnection;
        internal SecretService _Service;
        internal IPluginHost Host;

        public DBusWrapper(IPluginHost plugin)
        {
            Host = plugin;
            
            // Initialize DBus service
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
                _Service = new SecretService(this);
                await SessionConnection.RegisterServiceAsync(NAME, ServiceRegistrationOptions.None);
                await SessionConnection.RegisterObjectAsync(_Service);
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