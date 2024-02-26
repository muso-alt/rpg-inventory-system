using Newtonsoft.Json;

namespace Inventory.Components
{
    public struct ItemData
    {
        [JsonProperty]
        public string Name;
        [JsonProperty]
        public int CurrentCount;
        [JsonProperty]
        public int CellIndex;
        [JsonProperty] 
        public bool IsEquip;
    }
}