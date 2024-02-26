using System.Collections.Generic;
using Newtonsoft.Json;

namespace Inventory.Components
{
    public struct JsonData
    {
        [JsonProperty] 
        public JsonUnit Player;
        
        [JsonProperty] 
        public JsonUnit Enemy;
        
        [JsonProperty]
        public List<ItemData> Items;
        
    }
}