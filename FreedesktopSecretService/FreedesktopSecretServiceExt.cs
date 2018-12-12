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
        public IPluginHost _host;
        private DBusWrapper _dbus;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null) return false;

            _host = host;
            _dbus = new DBusWrapper(this);

            // Register event listeners on KeePass Events
            _host.MainWindow.FileOpened += (sender, e) => Task.Run(() => _dbus.RegisterDatabaseAsync(e.Database));
            _host.MainWindow.FileClosingPre += (sender, e) => Task.Run(() => _dbus.UnRegisterDatabaseAsync(e.Database));
            
            return true;
        }

        public override void Terminate()
        {
            _host = null;
        }
    }
}