using System;
using System.Collections.Generic;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Items;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Zombs_R_CuteItemRestrictions
{
    public class ItemRestrictions : RocketPlugin<ItemRestrictionsConfiguration>
    {
        protected override void Load()
        {
            HashSet<ushort> itemSet = Configuration.Instance.PreventPossessionOfTheseItems;
            Logger.Log("Prevent Possession of these items:");
            Logger.Log(itemSet.Count + " Blacklisted Items:");
            foreach (var item in itemSet)
            {
                var asset = UnturnedItems.GetItemAssetById(item);
                if (asset != null)
                {
                    Logger.Log("id: " + item + " - " + asset.itemName);
                    continue;
                }
                Logger.Log("id: " + item + " - * Unable to get item name by id");
            }
           
            U.Events.OnPlayerConnected += player =>
            {
                player.Player.inventory.onInventoryAdded +=
                    (page, index, jar) => ONInventoryAdded(page, index, jar, player);
                
                var itemJar = player.Player.inventory.getItem(0, 0); //Primary
                if (itemJar != null)
                    ONInventoryAdded(0,0, itemJar, player);

                itemJar = player.Player.inventory.getItem(1, 0); //Secondary
                if (itemJar != null)
                    ONInventoryAdded(1,0, itemJar, player);
            };

            U.Events.OnPlayerDisconnected += player =>
            {
                player.Player.inventory.onInventoryAdded -=
                    (page, index, jar) => ONInventoryAdded(page, index, jar, player);
            };
        }

        private void ONInventoryAdded(byte page, byte index, ItemJar jar, UnturnedPlayer player)
        {
            if (Configuration.Instance.LogPickupOfAllItems)
                LogItemPickedUp(player, jar);

            if (Configuration.Instance.AllowAdminToPossess && player.IsAdmin)
                return;

            if (Configuration.Instance.PreventPossessionOfTheseItems.Contains(jar.item.id))
            {
                if (Configuration.Instance.LogPickupOfItems)
                    LogItemPickedUp(player, jar);

                player.Player.inventory.removeItem(page, index);
                UnturnedChat.Say(player, UnturnedItems.GetItemAssetById(jar.item.id).itemName + " is not permitted.",
                    Color.red);
            }
        }

        private static void LogItemPickedUp(UnturnedPlayer player, ItemJar jar)
        {
            Logger.Log(String.Format("Player: {0} picked up {1} - {2}", player.DisplayName, jar.item.id,
                UnturnedItems.GetItemAssetById(jar.item.id).itemName));
        }
    }
}