using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    /// <summary>
    /// <a href="https://specifications.freedesktop.org/secret-service/re03.html">Freedesktop specification</a>
    /// </summary>
    [DBusInterface("org.freedesktop.Secret.Item")]
    public interface IItem : IDBusObject
    {
        //
        // Methods
        //

        Task<ObjectPath> DeleteAsync();

        Task<Secret> GetSecretAsync(ObjectPath session);

        Task SetSecretAsync(Secret secret);

        // 
        // Properties
        //
    }
}