using System.Collections.Generic;
using System.Xml.Serialization;
using Rocket.API;

namespace Zombs_R_CuteItemRestrictions
{
    public class ItemRestrictionsConfiguration:IRocketPluginConfiguration
    {
        [XmlArrayItem(ElementName = "Item")]
        public List<ushort> Items;

        public bool AllowAdminToPossess;
        
        public void LoadDefaults()
        {
            AllowAdminToPossess = true;
            Items = new List<ushort>() { 1510 }; // 1510 = chewing gum
        }
    }
}