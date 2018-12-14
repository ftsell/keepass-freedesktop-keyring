using System;
using System.Threading.Tasks;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusInterfaces
{
    public class Prompt : IPrompt
    {
        public ObjectPath ObjectPath { get; } = "";
        
        public Task PromptAsync(string window_id)
        {
            throw new NotImplementedException();
        }

        public Task DismissAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDisposable> WatchCompleteddAsync(Action<(bool dismissed, object result)> handler)
        {
            throw new NotImplementedException();
        }
    }
}