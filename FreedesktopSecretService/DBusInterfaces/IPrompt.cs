using System;
using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    [DBusInterface("org.freedesktop.Secret.Prompt")]
    public interface IPrompt : IDBusObject
    {
        //
        // Methods
        //

        Task PromptAsync(string window_id);

        Task DismissAsync();
        
        //
        // Signals
        //

        Task<IDisposable> WatchCompletedAsync(Action<(bool dismissed, object result)> handler);
    }
}