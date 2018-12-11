using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public struct Secret
    {
        public ObjectPath session;
        public byte[] parameters;
        public byte[] value;
        public string content_type;

    }
}