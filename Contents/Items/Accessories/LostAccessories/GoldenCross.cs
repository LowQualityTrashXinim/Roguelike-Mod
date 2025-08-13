using Terraria;
using Terraria.ModLoader;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Accessories.LostAccessories;
internal class GoldenCross : ModItem {
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Iframe, Additive: 1.33f, Flat: ModUtils.ToSecond(0.5f));
	}
}
