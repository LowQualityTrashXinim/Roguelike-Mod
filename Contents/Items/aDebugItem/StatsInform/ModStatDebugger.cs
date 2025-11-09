using Terraria;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;
using System.Collections.Generic;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Items.Consumable.Potion;
using Roguelike.Contents.Items.Consumable.SpecialReward;

namespace Roguelike.Contents.Items.aDebugItem.StatsInform
{
	internal class ModStatsDebugger : ModItem {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.width = Item.height = 10;
			Item.Set_DebugItem(true);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			var chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			var drugplayer = Main.LocalPlayer.GetModPlayer<WonderDrugPlayer>();
			var nohitPlayer = Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>();
			var artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
			chestplayer.GetAmount();
			var line = new TooltipLine(Mod, "StatsShowcase",
				$"\nAmount drop chest final weapon : {chestplayer.weaponAmount}" +
				$"\nAmount drop chest final potion type : {chestplayer.potionTypeAmount}" +
				$"\nAmount drop chest final potion amount : {chestplayer.potionNumAmount}" +
				$"\nWonder drug consumed rate : {drugplayer.DrugDealer}" +
				$"\nAmount boss no-hit : {nohitPlayer.BossNoHitNumber.Count}" +
				$"\nAmount boss don't-hit : {nohitPlayer.DontHitBossNumber.Count}" +
				$"\nCurrent active artifact : {Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName}"
				);
			tooltips.Add(line);
		}
	}
}
