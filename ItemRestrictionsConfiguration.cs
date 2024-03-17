using System.Collections.Generic;
using System.Xml.Serialization;
using Rocket.API;

namespace Zombs_R_CuteItemRestrictions
{
    public class ItemRestrictionsConfiguration : IRocketPluginConfiguration
    {
        public bool LogPickupOfItems;
        public bool LogPickupOfAllItems;

        [XmlArray(ElementName = "PreventPossessionOfTheseItems"), XmlArrayItem(ElementName = "Item")]
        public HashSet<ushort> PreventPossessionOfTheseItems;

        public bool AllowAdminToPossess;

        public void LoadDefaults()
        {
            AllowAdminToPossess = true;
            LogPickupOfItems = false;
            LogPickupOfAllItems = false;
            PreventPossessionOfTheseItems = new HashSet<ushort>() {1510}; // 1510 = Chewing Gum
        }
    }
}