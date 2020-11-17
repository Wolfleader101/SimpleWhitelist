using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Oxide.Core.Libraries;
using Oxide.Core.Libraries.Covalence;
using Steamworks.ServerList;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("SimpleWhitelist", "Wolfleader101", "1.0.3")]
    [Description("Manage your whitelist with simple commands")]
    class SimpleWhitelist : CovalencePlugin
    {
        #region Variables

        private PluginConfig config;
        private const string WhitelistPerms = "simplewhitelist.use";

        #endregion

        #region Hooks

        private void Init()
        {
            config = Config.ReadObject<PluginConfig>();
        }

        private void OnPlayerConnected(BasePlayer player)
        {
            PlayerData playerData = new PlayerData(player);
            var foundPlayer = config.whitelisted.Find(ply => ply.steamId == player.UserIDString);
            if (foundPlayer == null)
            {
                player.Kick("You are not whitelisted");
                return;
            }

            if (foundPlayer.name == string.Empty)
            {
                foundPlayer.name = player.displayName;
                SaveConfig();
            }
        }

        #endregion

        #region Methods

        private void AddCommand(IPlayer commandPlayer, string steamIDString)
        {
            var foundConfigPlayer = config.whitelisted.Find(ply => ply.steamId == steamIDString);
            if (foundConfigPlayer == null)
            {
                List<BasePlayer> playerList = BasePlayer.allPlayerList as List<BasePlayer>;
                var foundPlayer = BasePlayer.Find(steamIDString);
                PlayerData playerData = foundPlayer == null ? new PlayerData(steamIDString) : new PlayerData(foundPlayer);

                config.whitelisted.Add(playerData);
                SaveConfig();
                commandPlayer.Reply($"{(playerData.name == string.Empty ? steamIDString : playerData.name)} has been added to the whitelist");
            }
            else
            {
                commandPlayer.Reply("Player is already whitelisted");
            }
        }

        private void RemoveCommand(IPlayer commandPlayer, string steamIDString)
        {
            var foundConfigPlayer = config.whitelisted.Find(ply => ply.steamId == steamIDString);
            if (foundConfigPlayer == null)
            {
                commandPlayer.Reply("The user is not whitelisted");
                return;
            }
            config.whitelisted.Remove(foundConfigPlayer);
            SaveConfig();
            commandPlayer.Reply($"{foundConfigPlayer.name} has been removed from the whitelist");
        }

        private void HelpCommand(IPlayer commandPlayer)
        {
            commandPlayer.Reply("Incorrect Usage of the command. \nCorrect Usage: /whitelist <add|remove> <steam64>");
        }

        #endregion

        #region Commands

        [Command("whitelist"), Permission(WhitelistPerms)]
        private void WhitelistCommand(IPlayer player, string command, string[] args)
        {
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

        private void SaveConfig()
        {
            Config.WriteObject(config, true);
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

            public PlayerData()
            {
            }

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