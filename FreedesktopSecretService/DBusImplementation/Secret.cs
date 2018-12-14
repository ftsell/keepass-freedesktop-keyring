using System.Text;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    
    public struct Secret
    {
        public readonly ObjectPath session;
        public readonly byte[] parameters;
        public readonly byte[] value;
        public readonly string content_type;

        public Secret(ObjectPath session, string value)
        {
            this.session = session;
            this.value = Encoding.UTF8.GetBytes(value);
            
            parameters = new byte[0];
            content_type = "text/plain; charset=utf8";
        }
    }
}