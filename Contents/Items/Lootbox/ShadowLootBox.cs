using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox.Lootpool;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox {
	class ShadowLootBox : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {

		}
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.LightPurple;
		}
		public override List<int> Set_ItemPool() {
			List<int> types = new List<int> { ItemPool.GetPoolType<Tier1Pool>() };
			if (Main.rand.NextFloat() <= .9f) {
				types.Add(ItemPool.GetPoolType<Tier2Pool>());
				if (Main.rand.NextFloat() <= .5f) {
					types.Remove(ItemPool.GetPoolType<Tier1Pool>());
				}
			}
			if (Main.rand.NextFloat() <= .4f) {
				types.Add(ItemPool.GetPoolType<Tier3Pool>());
			}
			if (Main.rand.NextFloat() <= .1f) {
				types.Add(ItemPool.GetPoolType<Tier4Pool>());
			}
			if (Main.rand.NextFloat() <= .01f) {
				types.Add(ItemPool.GetPoolType<Tier5Pool>());
			}
			return types;
		}
		public override int WeaponLevelRangeRandomizer(Player player) {
			return Main.rand.Next(3, 16);
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			var handler = player.ModPlayerStats();
			handler.GetAmount();
			GetWeapon(entitySource, player, handler.weaponAmount);
			GetArmor(player, 1);
			GetAccessories(player, 1);
			GetPotions(player, handler.potionTypeAmount, handler.potionNumAmount);
		}
	}
}
