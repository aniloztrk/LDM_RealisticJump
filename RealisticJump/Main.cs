using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RealisticJump
{
    public class Main : RocketPlugin<Config>
    {
        private Dictionary<ulong, DateTime> JumpTime = new Dictionary<ulong, DateTime>();
        protected override void Load()
        {
            UnturnedPlayerEvents.OnPlayerUpdatePosition += UpdatePosition;
        }
        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerUpdatePosition -= UpdatePosition;
        }
        private void UpdatePosition(UnturnedPlayer player, Vector3 position)
        {
            if (player.Player.movement.isGrounded) return;

            if (!JumpTime.ContainsKey(player.CSteamID.m_SteamID))
            {
                JumpTime.Add(player.CSteamID.m_SteamID, DateTime.Now);
                return;
            }
            
            player.Player.movement.sendPluginJumpMultiplier(0f);
        }
        public void Update()
        {
            foreach (var player in Provider.clients.Select(p => UnturnedPlayer.FromSteamPlayer(p)))
            {
                if (!JumpTime.ContainsKey(player.CSteamID.m_SteamID)) continue;

                var cooldown = (DateTime.Now - JumpTime[player.CSteamID.m_SteamID]).TotalSeconds;
                if (cooldown < Configuration.Instance.JumpCooldown) continue;

                JumpTime.Remove(player.CSteamID.m_SteamID);
                player.Player.movement.sendPluginJumpMultiplier(1f);
            }
        }
    }
}
