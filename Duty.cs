﻿using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.Unturned;
using Rocket.Core.Plugins;
using Rocket.API.Collections;
using UnityEngine;
using Rocket.API;
using Rocket.Core.Permissions;
using Rocket.Core;
using System.Collections.Generic;
using Rocket.API.Serialisation;

namespace EFG.Duty
{
    public class Duty : RocketPlugin<DutyConfiguration>
    {
        public static Duty Instance;

        protected override void Load()
        {
            Instance = this;

            Rocket.Core.Logging.Logger.LogWarning("Loading event \"Player Connected\"...");
            U.Events.OnPlayerConnected += PlayerConnected;
            Rocket.Core.Logging.Logger.LogWarning("Loading event \"Player Disconnected\"...");
            U.Events.OnPlayerDisconnected += PlayerDisconnected;

            Rocket.Core.Logging.Logger.LogWarning("");
            Rocket.Core.Logging.Logger.LogWarning("Duty has been successfully loaded!");
        }
        
        protected override void Unload()
        {
            Instance = null;

            Rocket.Core.Logging.Logger.LogWarning("Unloading on player connect event...");
            U.Events.OnPlayerConnected -= PlayerConnected;
            Rocket.Core.Logging.Logger.LogWarning("Unloading on player disconnect event...");
            U.Events.OnPlayerConnected -= PlayerDisconnected;

            Rocket.Core.Logging.Logger.LogWarning("");
            Rocket.Core.Logging.Logger.LogWarning("Duty has been unloaded!");
        }

        public void DoDuty(UnturnedPlayer caller)
        {
            if (caller.IsAdmin)
            {
                caller.Admin(false);
                caller.Features.GodMode = false;
                caller.Features.VanishMode = false;
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("off_duty_message", caller.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
            else
            {
                caller.Admin(true);
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("on_duty_message", caller.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                
            }
        }

        public void Admin(IRocketPlayer caller)
        {
            RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>(); ;
            string GroupName = Configuration.Instance.AdminGroupName;
            List<RocketPermissionsGroup> PlayerGroups = Permissions.GetGroups(caller, true);
            foreach (RocketPermissionsGroup Groups in PlayerGroups)
            {
                if (Groups.Id == GroupName)
                {
                    Permissions.RemovePlayerFromGroup(GroupName, caller);
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("off_duty_message", caller.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
            }
            RocketPermissionsGroup Group = Permissions.GetGroup(GroupName);
            if (Group != null)
            {
                Permissions.AddPlayerToGroup(GroupName, caller);
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("on_duty_message", caller.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
        }

        public void Moderator(IRocketPlayer caller)
        {
            RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>(); ;
            string GroupName = Configuration.Instance.ModeratorGroupName;
            List<RocketPermissionsGroup> PlayerGroups = Permissions.GetGroups(caller, true);
            foreach (RocketPermissionsGroup Groups in PlayerGroups)
            {
                if (Groups.Id == GroupName)
                {
                    Permissions.RemovePlayerFromGroup(GroupName, caller);
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("off_duty_message", caller.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
            }
            RocketPermissionsGroup Group = Permissions.GetGroup(GroupName);
            if (Group != null)
            {
                Permissions.AddPlayerToGroup(GroupName, caller);
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("on_duty_message", caller.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
        }

        public void Helper(IRocketPlayer caller)
        {
            RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>(); ;
            string GroupName = Configuration.Instance.HelperGroupName;
            List<RocketPermissionsGroup> PlayerGroups = Permissions.GetGroups(caller, true);
            foreach (RocketPermissionsGroup Groups in PlayerGroups)
            {
                if (Groups.Id == GroupName)
                {
                    Permissions.RemovePlayerFromGroup(GroupName, caller);
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("off_duty_message", caller.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
            }
            RocketPermissionsGroup Group = Permissions.GetGroup(GroupName);
            if (Group != null)
            {
                Permissions.AddPlayerToGroup(GroupName, caller);
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("on_duty_message", caller.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
        }

        public void CDuty(UnturnedPlayer cplayer, IRocketPlayer caller)
        {
            RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>(); ;
            if (Configuration.Instance.AllowDutyCheck)
            {
                if (cplayer != null)
                {
                    Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Checking Duty");
                    if (cplayer.IsAdmin)
                    {
                        Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Cplayer Admin Found.");
                        if (caller is ConsolePlayer)
                        {
                            UnturnedChat.Say(Instance.Translate("check_on_duty_message", "Console", cplayer.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                        }
                        else if (caller is UnturnedPlayer)
                        {
                            UnturnedChat.Say(Instance.Translate("check_on_duty_message", caller.DisplayName, cplayer.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                        }
                        return;
                    }
                    List<RocketPermissionsGroup> PlayerGroups = Permissions.GetGroups(cplayer, true);
                    foreach (RocketPermissionsGroup Groups in PlayerGroups)
                    {
                        if (Groups.Id == Configuration.Instance.AdminGroupName || Groups.Id == Configuration.Instance.ModeratorGroupName || Groups.Id == Configuration.Instance.HelperGroupName)
                        {
                            Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Cplayer Admin Found.");
                            if (caller is ConsolePlayer)
                            {
                                UnturnedChat.Say(Instance.Translate("check_on_duty_message", "Console", cplayer.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                            }
                            else if (caller is UnturnedPlayer)
                            {
                                UnturnedChat.Say(Instance.Translate("check_on_duty_message", caller.DisplayName, cplayer.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                            }
                        }
                    }
                }
                else if (cplayer == null)
                {
                    Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Player is not online or his name is invalid.");
                    if (caller is UnturnedPlayer)
                    {
                        UnturnedChat.Say(caller, "Player is not online or his name is invalid.");
                    }
                }
            }
            else if (!Configuration.Instance.AllowDutyCheck)
            {
                Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Unable To Check Duty. Configuration Is Set To Be Disabled.");
                if (caller is UnturnedPlayer)
                {
                    UnturnedChat.Say(caller, "Unable To Check Duty. Configuration Is Set To Be Disabled.");
                }
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList {
                    {"admin_login_message", "{0} has logged on and is now on duty."},
                    {"admin_logoff_message", "{0} has logged off and is now off duty."},
                    {"on_duty_message", "{0} is now on duty."},
                    {"off_duty_message", "{0} is now off duty."},
                    {"check_on_duty_message", "{0} has confirmed that {1} is on duty."},
                    {"check_off_duty_message", "{0} has confirmed that {1} is not on duty."},
                    {"not_enough_permissions", "You do not have the correct permissions to use duty."}
                };
                    
            }
        }
        void PlayerConnected(UnturnedPlayer player)
        {
            RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>(); ;
            if (player.IsAdmin)
            {
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_login_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                return;
            }
            List<RocketPermissionsGroup> PlayerGroups = Permissions.GetGroups(player, true);
            foreach (RocketPermissionsGroup Groups in PlayerGroups)
            {
                if (Groups.Id == Configuration.Instance.AdminGroupName)
                {
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_login_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
                else if (Groups.Id == Configuration.Instance.ModeratorGroupName)
                {
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_login_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
                else if (Groups.Id == Configuration.Instance.HelperGroupName)
                {
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_login_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
            }
        }
        void PlayerDisconnected(UnturnedPlayer player)
        {
            RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>(); ;
            if (player.IsAdmin)
            {
                if (Configuration.Instance.RemoveAdminOnLogout)
                {
                    player.Admin(false);
                    player.Features.GodMode = false;
                    player.Features.VanishMode = false;
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logoff_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }

                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logoff_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                return;
            }
            List<RocketPermissionsGroup> PlayerGroups = Permissions.GetGroups(player, true);
            foreach (RocketPermissionsGroup Groups in PlayerGroups)
            {
                if (Groups.Id == Configuration.Instance.AdminGroupName)
                {
                    if (Configuration.Instance.RemoveAdminOnLogout) Permissions.RemovePlayerFromGroup(Configuration.Instance.AdminGroupName, player);
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logoff_message", player.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
                else if (Groups.Id == Configuration.Instance.ModeratorGroupName)
                {
                    if (Configuration.Instance.RemoveAdminOnLogout) Permissions.RemovePlayerFromGroup(Configuration.Instance.ModeratorGroupName, player);
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logoff_message", player.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
                else if (Groups.Id == Configuration.Instance.HelperGroupName)
                {
                    if (Configuration.Instance.RemoveAdminOnLogout) Permissions.RemovePlayerFromGroup(Configuration.Instance.HelperGroupName, player);
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logoff_message", player.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
            }
        }
    }
}
