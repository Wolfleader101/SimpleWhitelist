using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;
using Steamworks.ServerList;
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
            var foundPlayer = config.whitelisted.Find(ply => ply.steamId == player.Id);
            if (foundPlayer == null)
            {
                player.Kick("You are not whitelisted");
                return;
            }

            if (foundPlayer.name == string.Empty)
            {
                foundPlayer.name = player.Name;
                SaveConfig();
            }
        }

        #endregion

        #region Methods

        private void AddCommand(IPlayer commandPlayer,string steamIDString)
        {
            var foundConfigPlayer = config.whitelisted.Find(ply => ply.steamId == steamIDString);
            if (foundConfigPlayer == null)
            {
                List<BasePlayer> playerList = BasePlayer.allPlayerList as List<BasePlayer>;
                var foundPlayer = playerList.Find(ply => ply.UserIDString == steamIDString);
                PlayerData playerData = new PlayerData(foundPlayer);
                config.whitelisted.Add(playerData);
                SaveConfig();
                commandPlayer.Reply($"{foundPlayer.displayName} has been added to the whitelist");
            }
        }

        private void RemoveCommand(IPlayer commandPlayer,string steamIDString)
        {
            
        }

        private void HelpCommand(IPlayer commandPlayer)
        {
            
        }
        #endregion

        #region Commands

        [Command("whitelist")]
        private void WhitelistCommand(IPlayer player, string command, string[] args)
        {
            if(!player.HasPermission(WhitelistPerms)) return;
            if (args.Length > 2 || args.Length < 2)
            {
                HelpCommand(player);
                return;
            }

            switch (args[0])
            {
                case "add":
                    AddCommand(player, args[1]);
                    break;
                case "remove":
                    RemoveCommand(player, args[1]);
                    break;
                case "help":
                    break;
                default:
                    HelpCommand(player);
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

            public PlayerData(string userID)
            {
                name = String.Empty;
                steamId = userID;
            }
        }

        #endregion
    }
}