using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.DataStructures;
using Roguelike.Common.Utils;

using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox {
	class CrimsonLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Orange;
		}
		public override void LootPoolSetStaticDefaults() {
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
