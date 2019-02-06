using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusInterfaces;
using Tmds.DBus;

namespace FreedesktopSecretService.DBusImplementation
{
    public abstract class Prompt : IPrompt
    {
        public ObjectPath ObjectPath { get; } = "";

        public abstract Task PromptAsync(string window_id);

        public Task DismissAsync()
        {
            Console.WriteLine("Prompt dismissal attempted");
            throw new NotImplementedException();
        }

        
        
        #region Signals

        private readonly IList<Action<(bool dismissed, object result)>> _watchCompletedHandlers =
            new List<Action<(bool dismissed, object result)>>();
        
        private class WatchCompletedDisposable : IDisposable
        {
            private Prompt _prompt;
            private Action<(bool dismissed, object result)> _handler;

            public WatchCompletedDisposable(Action<(bool dismissed, object sender)> handler, Prompt prompt)
            {
                _handler = handler;
                _prompt = prompt;
            }

            public void Dispose()
            {
                _prompt._watchCompletedHandlers.Remove(_handler);
            }
        }

        public async Task<IDisposable> WatchCompletedAsync(Action<(bool dismissed, object result)> handler)
        {
            _watchCompletedHandlers.Add(handler);
            return new WatchCompletedDisposable(handler, this);
        }

        protected void TriggerWatchCompleted(bool dismissed, object result)
        {
            foreach (var handler in _watchCompletedHandlers)
                handler.Invoke((dismissed, result));
        }
        
        #endregion
        
    }
}