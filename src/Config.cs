using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace TrickOrTreat
{
    public class TrickOrTreatConfig : BasePluginConfig
    {

        [JsonPropertyName("CommandCooldown")]
		public int Cooldown { get; set; } = 2;

    }
}
