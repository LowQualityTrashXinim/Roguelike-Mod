using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.General;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.Chest {
	class WoodenLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.White;
		}
		public override bool CanActivateSpoil => ModContent.GetInstance<RogueLikeConfig>().BossRushMode;
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreBoss);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.CommonAxe);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreBoss);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreBoss);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPreBoss);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreEoC);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreEoC);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreEoC);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonerPreEoC);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.SpecialPreBoss);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.Special);
			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() => new List<int> { 2 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			if (CanActivateSpoil) {
				return;
			}
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			player.QuickSpawnItem(entitySource, GetAccessory());
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(true), modplayer.potionNumAmount);
			}
			GetArmorForPlayer(entitySource, player, Main.rand.NextBool(5));
			int RandomAssArmor = Main.rand.Next(new int[] { ItemID.FlinxFurCoat, ItemID.VikingHelmet, ItemID.EmptyBucket, ItemID.NightVisionHelmet, ItemID.DivingHelmet, ItemID.Goggles, ItemID.Gi });
			player.QuickSpawnItem(entitySource, RandomAssArmor);
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			GetWeapon(entitySource, player, 2);
			GetArmorForPlayer(entitySource, player, true);
			GetAccessories(Type, player);
			GetPotion(Type, player);
			player.QuickSpawnItem(entitySource, ModContent.ItemType<SpecialSkillLootBox>());
			int RandomModdedBuff = Main.rand.Next(TerrariaArrayID.SpecialPotion);
			player.QuickSpawnItem(entitySource, RandomModdedBuff, 1);
			player.QuickSpawnItem(entitySource, ItemID.GrapplingHook);
			player.QuickSpawnItem(entitySource, ItemID.LesserHealingPotion, 5);
		}
	}
}
