using System;
using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class Session : ISession
    {
        public ObjectPath ObjectPath { get; }

        public Session()
        {
            var rand = new Random();
            var id = rand.Next();
            
            ObjectPath = $"/org/freedesktop/secrets/session/{id}";
        }
        
        //
        // Methods
        //

        public Task CloseAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}