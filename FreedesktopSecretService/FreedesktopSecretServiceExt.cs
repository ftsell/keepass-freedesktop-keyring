using FreedesktopSecretService.DBusInterfaces;
using KeePass.Plugins;

namespace FreedesktopSecretService
{
    public sealed class FreedesktopSecretServiceExt : Plugin
    {
        internal IPluginHost Host;
        private DBusWrapper _dbus;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null) return false;

            Host = host;
            _dbus = new DBusWrapper(host);
            
            return true;
        }

        public override void Terminate()
        {
            Host = null;
        }
    }
}