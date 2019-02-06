using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeepassFreedesktopKeyring.DBusImplementation;
using Tmds.DBus;

namespace KeepassFreedesktopKeyring.DBusInterfaces
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
        Task<object> GetAsync(string prop);
        Task<ItemProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    public class ItemProperties
    {
        [Property(Access = PropertyAccess.Read)]
        public bool Locked;

        public IDictionary<string, string> Attributes;

        public string Label;

        [Property(Access = PropertyAccess.Read)]
        public int Created;

        [Property(Access = PropertyAccess.Read)]
        public int Modified;
    }
    
}