using System.Collections.Generic;
using Newtonsoft.Json;

namespace Inventory.Components
{
    public struct JsonData
    {
        [JsonProperty] 
        public JsonUnits Units;
        
        [JsonProperty]
        public List<ItemData> Items;
    }
}