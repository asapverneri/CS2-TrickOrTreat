using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace TrickOrTreat
{
    public class TrickOrTreatConfig : BasePluginConfig
    {

        [JsonPropertyName("CommandCooldown")]
		public int Cooldown { get; set; } = 2;

        [JsonPropertyName("TreatDeagle")]
        public bool TreatDeagle { get; set; } = true;

        [JsonPropertyName("TreatGrenade")]
        public bool TreatGrenade { get; set; } = true;

        [JsonPropertyName("TreatHealth")]
        public bool TreatHP { get; set; } = true;

        [JsonPropertyName("TreatHealthValue")]
        public int TreatHPValue { get; set; } = 120;

        [JsonPropertyName("TreatAK47")]
        public bool TreatAK47 { get; set; } = true;

        [JsonPropertyName("TreatHealthShot")]
        public bool TreatHealthShot { get; set; } = true;

        [JsonPropertyName("TreatMoney")]
        public bool TreatMoney { get; set; } = true;

        [JsonPropertyName("TreatMoneyValue")]
        public int TreatMoneyValue { get; set; } = 5000;

        [JsonPropertyName("TreatSpeed")]
        public bool TreatSpeed { get; set; } = true;

        [JsonPropertyName("TreatSpeedValue")]
        public float TreatSpeedValue { get; set; } = 1.15f;



        [JsonPropertyName("TrickSuicide")]
        public bool TrickSuicide { get; set; } = true;

        [JsonPropertyName("TrickStripweapons")]
        public bool TrickStripweapons { get; set; } = true;

        [JsonPropertyName("Trick50HP")]
        public bool Trick50HP { get; set; } = true;

        [JsonPropertyName("Trick99HP")]
        public bool Trick99HP { get; set; } = true;

        [JsonPropertyName("TrickNothing")]
        public bool TrickNothing { get; set; } = true;

    }
}
