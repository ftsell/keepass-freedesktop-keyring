using System;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using KeePass.Plugins;
using Tmds.DBus;

namespace FreedesktopSecretService
{
    public sealed class FreedesktopSecretServiceExt : Plugin
    {
        public IPluginHost _host;
        private DBusWrapper _dbus;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null) return false;

            _host = host;
            _dbus = new DBusWrapper(this);
            
            return true;
        }

        public override void Terminate()
        {
            _host = null;
        }
    }
}