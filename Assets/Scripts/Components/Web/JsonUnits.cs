using Newtonsoft.Json;

namespace Inventory.Components
{
    public struct JsonUnits
    {
        [JsonProperty] 
        public int PlayerHealth;
        [JsonProperty] 
        public int EnemyHealth;
    }
}