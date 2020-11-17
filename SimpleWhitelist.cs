using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("SimpleWhitelist", "Wolfleader101", "1.0.0")]
    [Description("Manage your whitelist with simple commands")]
    class SimpleWhitelist : CovalencePlugin
    {
        #region Variables

        private PluginConfig config;
        public const string WhitelistPerms = "simplewhitelist.use";

        #endregion

        #region Hooks

        private void Init()
        {
            config = Config.ReadObject<PluginConfig>();

            permission.RegisterPermission(WhitelistPerms, this);
        }

        #endregion

        #region Methods

        #endregion

        #region Config

        private class PluginConfig
        {
            //[JsonProperty("Enderpearl")] public string enderpearl { get; set; }
        }

        private PluginConfig GetDefaultConfig()
        {
            return new PluginConfig
            {
              //  enderpearl = "snowball.entity"
            };
        }

        protected override void LoadDefaultConfig()
        {
            Config.WriteObject(GetDefaultConfig(), true);
        }

        #endregion
    }
}