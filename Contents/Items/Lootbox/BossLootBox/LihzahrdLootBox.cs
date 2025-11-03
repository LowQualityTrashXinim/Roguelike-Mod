using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox;
class LihzahrdLootBox : LootBoxBase {
	public override void SetDefaults() {
		Item.width = 54;
		Item.height = 38;
		Item.rare = ItemRarityID.Red;
	}
	public override void AbsoluteRightClick(Player player) {
		var entitySource = player.GetSource_OpenItem(Type);
		int wing = Main.rand.Next(new int[] { ItemID.BeeWings, ItemID.BeetleWings, ItemID.BoneWings, ItemID.BatWings, ItemID.MothronWings, ItemID.ButterflyWings, ItemID.Hoverboard, ItemID.FlameWings, ItemID.GhostWings, ItemID.FestiveWings, ItemID.SpookyWings, ItemID.TatteredFairyWings });
		player.QuickSpawnItem(entitySource, wing);
		player.QuickSpawnItem(entitySource, ItemID.GoldenFishingRod);
		player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.SpecialPotion));
		player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystal);
	}
}
