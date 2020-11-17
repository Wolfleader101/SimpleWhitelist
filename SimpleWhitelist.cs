using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;
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
        
        private void OnPlayerConnected(IPlayer player)
        {
            PlayerData playerData = new PlayerData(player);
            // BasePlayer basePlayer = player.Object as BasePlayer;
            var foundPlayer = config.whitelisted.Find(ply => ply.steamId == player.Id);
            if (foundPlayer == null)
            {
                player.Kick("You are not whitelisted");
            }
        }

        #endregion

        #region Methods

        private void AddCommand(string steamIDString)
        {
            
        }

        private void RemoveCommand(string steamIDString)
        {
            
        }

        private void HelpCommand()
        {
            
        }
        #endregion

        #region Commands

        [Command("whitelist")]
        private void WhitelistCommand(IPlayer player, string command, string[] args)
        {
            if(!player.HasPermission(WhitelistPerms)) return;
            if (args.Length > 2)
            {
                HelpCommand();
                return;
            }

                switch (args[0])
            {
                case "add":
                    AddCommand(args[1]);
                    break;
                case "remove":
                    RemoveCommand();
                    break;
                case "help":
                    break;
                default:
                    HelpCommand();
                    break;
            }
        }

        #endregion

        #region Config

        private class PluginConfig
        {
            [JsonProperty("Whitelisted")] public List<PlayerData> whitelisted { get; set; }
        }

        private PluginConfig GetDefaultConfig()
        {
            return new PluginConfig
            {
              whitelisted = new List<PlayerData>()
            };
        }

        protected override void LoadDefaultConfig()
        {
            Config.WriteObject(GetDefaultConfig(), true);
        }

        #endregion

        #region Classes
        private class PlayerData
        {
            public string name { get; set; }
            public string steamId { get; set; }

            public PlayerData() { }

            public PlayerData(IPlayer player)
            {
                name = player.Name;
                steamId = player.Id;
            }

            public PlayerData(BasePlayer player)
            {
                name = player.displayName;
                steamId = player.UserIDString;
            }
        }

        #endregion
    }
}