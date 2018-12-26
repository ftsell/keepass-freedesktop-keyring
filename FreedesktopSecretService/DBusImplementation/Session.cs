using System;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusImplementation
{
    public class Session : ISession
    {
        public ObjectPath ObjectPath { get; }

        private readonly DBusWrapper _dbus;

        public Session(DBusWrapper dbus)
        {
            _dbus = dbus;
            
            var rand = new Random();
            var id = rand.Next();
            
            ObjectPath = $"/org/freedesktop/secrets/session/{id}";
        }
        
        //
        // Methods
        //

        public async Task CloseAsync()
        {
            Console.WriteLine($"Closed session {ObjectPath}");
            _dbus.Service.Sessions.Remove(this);
        }
    }
}