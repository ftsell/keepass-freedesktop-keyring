using System;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using KeePass.Ecas;
using KeePass.Forms;
using KeePass.Plugins;
using Tmds.DBus;

namespace FreedesktopSecretService
{
    public sealed class FreedesktopSecretServiceExt : Plugin
    {
        internal IPluginHost _Host;
        private DBusWrapper _dbus;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null) return false;

            _Host = host;
            _dbus = new DBusWrapper(this);
            
            return true;
        }

        public override void Terminate()
        {
            _Host = null;
        }
    }
}