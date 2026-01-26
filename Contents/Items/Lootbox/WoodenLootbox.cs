using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.General;
using System.Collections.Generic;
using Roguelike.Contents.Items.Lootbox.Lootpool;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.Lootbox;
class WoodenLootBox : LootBoxBase {
	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 30;
		Item.rare = ItemRarityID.White;
	}
	public override bool CanActivateSpoil => ModContent.GetInstance<RogueLikeConfig>().BossRushMode;
	public override List<int> Set_ItemPool() {
		return new List<int> { ItemPool.GetPoolType<UniversalPool>() };
	}
	public override void AbsoluteRightClick(Player player) {
		if (CanActivateSpoil) {
			return;
		}
		var entitySource = player.GetSource_OpenItem(Type);
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.GetAmount();
		GetWeapon(entitySource, player, handler.weaponAmount);
		GetArmor(player, 1);
		GetAccessories(player, 1);
		GetPotions(player, handler.potionTypeAmount, handler.potionNumAmount);
	}
}
