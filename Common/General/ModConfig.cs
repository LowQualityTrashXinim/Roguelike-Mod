using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Roguelike.Common.General {
	public class RogueLikeConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header($"GameModeHeader")]
		[DefaultValue(true)]
		public bool BossRushMode { get; set; }
		[ReloadRequired]
		[DefaultValue(false)]
		public bool BossRushMode_Setting_FightBossInProgression { get; set; }
		[ReloadRequired]
		[DefaultValue(false)]
		public bool BossRushMode_Setting_SpawnOnPlayerCommand { get; set; }
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
		public bool EnablePracticeMode { get; set; }
		public bool SkipCutscene { get; set; }
		[Header($"GameHeader")]
		public bool HoldShift { get; set; }
		[DefaultValue(false)]
		public bool DisableRingVisual { get; set; }
		[DefaultValue(false)]
		public bool LowerQuality { get; set; }
		[DefaultValue(false)]
		[ReloadRequired]
		public bool LowRAMMode { get; set; }
	}
}
