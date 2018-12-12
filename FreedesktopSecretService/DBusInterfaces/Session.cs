using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class Session : ISession
    {
        public ObjectPath ObjectPath { get; } = "";

        public Task CloseAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}