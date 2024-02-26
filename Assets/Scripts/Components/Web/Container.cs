using System;
using Newtonsoft.Json;

namespace Inventory.Components
{
    [Serializable]
    public struct Container
    {
        [JsonProperty] public int ID { get; set; }
        [JsonProperty] public string Value { get; set; }
    }
}