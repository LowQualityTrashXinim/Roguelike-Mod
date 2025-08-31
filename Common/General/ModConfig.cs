using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Roguelike.Common.General
{
	public class RogueLikeConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header($"GameModeHeader")]
		public bool RoguelikeMode { get; set; }
		[DefaultValue(true)]
		public bool BossRushMode { get; set; }
		//TODO : Add a world data IsNightmareWorld 
		[DefaultValue(false)]
		public bool TotalRNG { get; set; }
		//Replace Cursed skull
		[ReloadRequired]
		[DefaultValue(false)]
		public bool HellishEndeavour { get; set; }
		[Header($"LuckDepartmentHeader")]
		[DefaultValue(true)]
		public bool RareSpoils { get; set; }
		[DefaultValue(true)]
		public bool RareLootbox { get; set; }
		[DefaultValue(true)]
		public bool AccessoryPrefix { get; set; }
		[Header($"QoLHeader")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool AutoRandomizeCharacter { get; set; }

		[Header($"DebugHeader")]
		public bool TemplateTest { get; set; }
		[Header($"GameHeader")]
		public bool HoldShift { get; set; }
		public bool DisableRingVisual { get; set; }
	}
}
