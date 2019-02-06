using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    [DBusInterface("org.freedesktop.Secret.Session")]
    public interface ISession : IDBusObject
    {
        Task CloseAsync();
    }
}