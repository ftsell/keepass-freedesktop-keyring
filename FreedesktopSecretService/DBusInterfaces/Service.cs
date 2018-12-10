using System;
using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class SecretService : ISecretService, IDBusObject
    {

        public ObjectPath ObjectPath { get; } = new ObjectPath("/org/freedesktop/secrets");

        public string test => "i";

        public async Task TestAsync()
        {
            Console.WriteLine("Called");
        }

    }
}