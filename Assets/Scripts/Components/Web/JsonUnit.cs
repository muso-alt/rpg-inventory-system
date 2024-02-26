using Newtonsoft.Json;

namespace Inventory.Components
{
    public struct JsonUnit
    {
        [JsonProperty] 
        public int Health;
        
        [JsonProperty] 
        public string EquipHeadArmorName;
        
        [JsonProperty] 
        public string EquipBodyArmorName;
    }
}