using Roguelike.Contents.Items.Lootbox.Lootpool;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox {
	class CrimsonLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Orange;
		}
		public override List<int> Set_ItemPool() {
			return new List<int>() { ItemPool.GetPoolType<CrimsonPool>() };
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
