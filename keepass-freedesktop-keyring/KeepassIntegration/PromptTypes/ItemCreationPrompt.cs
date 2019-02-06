using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreedesktopSecretService.DBusImplementation;
using FreedesktopSecretService.DBusInterfaces;

namespace FreedesktopSecretService.KeepassIntegration.PromptTypes
{
    public class ItemCreationPrompt : Prompt
    {
        private IDictionary<string, object> _properties;
        private Secret _secret;
        private bool _replace;
        
        public ItemCreationPrompt(IDictionary<string, object> properties, Secret secret, bool replace)
        {
            _properties = properties;
            _secret = secret;
            _replace = replace;
        }

        public override async Task PromptAsync(string window_id)
        {
            Console.WriteLine("Tried to open prompt for item creation");
        }
    }
}