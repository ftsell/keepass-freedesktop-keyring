using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class Item : IItem
    {
        public ObjectPath ObjectPath { get; } = "";
        
        public Task<ObjectPath> DeleteAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Secret> GetSecretAsync(ObjectPath session)
        {
            throw new System.NotImplementedException();
        }

        public Task SetSecretAsync(Secret secret)
        {
            throw new System.NotImplementedException();
        }
    }
}