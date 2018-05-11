using Newtonsoft.Json;
using System;

namespace Coursuch
{
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(ActionConverter))]
    public abstract class AbstractAction
    {
        [JsonProperty(Order = 1)]
        public string Type { get => GetType().Name; }

        [JsonProperty]
        public int StartTime { get; set; }
        [JsonProperty]
        public int ExecutionTime { get; set; }

        [JsonIgnore]
        public ActionList parentList{ get; set; }

        public abstract void Execute();
    }
}