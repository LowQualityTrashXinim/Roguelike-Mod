using Terraria;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Roguelike.Contents.Transfixion.Perks;

namespace Roguelike.Contents.Items.aDebugItem.StatsInform;
internal class PerkDebugShower : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 10;
		Item.Set_DebugItem(true);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		base.ModifyTooltips(tooltips);
		var player = Main.LocalPlayer;
		var perkplayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();
		string perk = "";
		foreach (var perkItem in perkplayer.perks.Keys) {
			if (ModPerkLoader.GetPerk(perkItem) != null) {
				perk += "\n" + ModPerkLoader.GetPerk(perkItem).DisplayName + $" | Value : [{perkplayer.perks[perkItem]}]";
			}
		}
		var line = new TooltipLine(Mod, "StatsShowcase",
			"[For Debug purpose]" +
			perk
			);
		tooltips.Add(line);
	}
}
