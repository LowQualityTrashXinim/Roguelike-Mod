using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox.Lootpool;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox {
	class IronLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Blue;
		}
		public override List<int> Set_ItemPool() {
			return new List<int> { ItemPool.GetPoolType<UniversalPool>() };
		}
		public override int WeaponLevelRangeRandomizer(Player player) {
			return Main.rand.Next(1, 3);
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
