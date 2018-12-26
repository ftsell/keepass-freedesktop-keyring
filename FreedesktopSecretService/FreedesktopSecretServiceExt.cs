using FreedesktopSecretService.DBusImplementation;
using FreedesktopSecretService.DBusInterfaces;
using KeePass.Plugins;

namespace FreedesktopSecretService
{
    public sealed class FreedesktopSecretServiceExt : Plugin
    {
        internal IPluginHost Host;
        internal DBusWrapper Dbus;

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