using KeepassFreedesktopKeyring.DBusImplementation;
using KeePass.Plugins;

namespace KeepassFreedesktopKeyring
{
    public sealed class KeepassFreedesktopKeyringExt : Plugin
    {
        internal IPluginHost Host;
        internal DBusWrapper Dbus;

        internal readonly string DATA_PREFIX = "freedesktop-secret-service:";

        public override bool Initialize(IPluginHost host)
        {
            if (host == null) return false;

            Host = host;
            Dbus = new DBusWrapper(this);
            
            return true;
        }

        public override void Terminate()
        {
            Host = null;
        }
    }
}