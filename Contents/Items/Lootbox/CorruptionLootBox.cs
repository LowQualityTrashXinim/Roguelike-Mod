using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Roguelike.Common.Utils;

using Roguelike.Common.Global;
using Roguelike.Contents.Items.Lootbox.Lootpool;

namespace Roguelike.Contents.Items.Lootbox {
	class CorruptionLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Orange;
		}
		public override void LootPoolSetStaticDefaults() {
		}
		public override List<int> FlagNumAcc() => new List<int> { 0, 1, 2, 3, 4, 5 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			GetArmorForPlayer(entitySource, player, Main.rand.NextBool(5));
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			player.QuickSpawnItem(entitySource, GetAccessory());
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (NPC.downedBoss2) {
				player.QuickSpawnItem(entitySource, ItemID.TinkerersWorkshop);
				player.QuickSpawnItem(entitySource, ItemID.Hellforge);
				player.QuickSpawnItem(entitySource, Main.rand.Next(new int[] { ItemID.DiamondHook, ItemID.RubyHook }));
			}
			player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystalStand);
		}
	}
}
