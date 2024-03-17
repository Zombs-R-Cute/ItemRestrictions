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
    public class ItemRestrictions:RocketPlugin<ItemRestrictionsConfiguration>
    {
        protected override void Load()
        {
            Logger.Log(Configuration.Instance.Items.Count + " Blacklisted Items:");
            foreach (var item in Configuration.Instance.Items)
            {
                Logger.Log("id: " + item + " - " + UnturnedItems.GetItemAssetById(item).itemName);
                
            }
            
            U.Events.OnPlayerConnected += player =>
            {
                player.Player.inventory.onInventoryAdded +=
                    (page, index, jar) => ONInventoryAdded(page, index, jar, player);
            };
            
            U.Events.OnPlayerDisconnected += player =>
            {
                player.Player.inventory.onInventoryAdded -=
                    (page, index, jar) => ONInventoryAdded(page, index, jar, player);
            };
        }

        private void ONInventoryAdded(byte page, byte index, ItemJar jar, UnturnedPlayer player)
        {
            if(Configuration.Instance.AllowAdminToPossess && player.IsAdmin)
                return;
             
            if (Configuration.Instance.Items.Contains(jar.item.id))
            {
                player.Player.inventory.removeItem(page, index);
                UnturnedChat.Say(player, UnturnedItems.GetItemAssetById(jar.item.id).itemName + " is not permitted.", Color.red);
            }
        }
    }
}