using Humanizer;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Items.RelicItem.RelicTemplateContent;
using Roguelike.Contents.Transfixion.Perks;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.SpoilSystem;
internal class SSRspoil {
	public class SSRPerkSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.FallenStar);
		}
		public override string FinalDescription() {
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			return Description.FormatWith(chestplayer.ModifyGetAmount(1, true), chestplayer.ModifyGetAmount(2, true));
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			int count = player.ModPlayerStats().ModifyGetAmount(1);
			int type = ModContent.ItemType<WorldEssence>();
			player.QuickSpawnItem(source, type, count);
			ModUtils.GetSkillLootbox(source, player, count);
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				Item relicitem = player.QuickSpawnItemDirect(source, ModContent.ItemType<Relic>());
				if (relicitem.ModItem is Relic relic) {
					relic.AutoAddRelicTemplate(player, 3);
				}
			}
			ModUtils.GetAccessories(source, player, count);
		}
	}
	public class LegendaryRelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player) {
			Item item = player.QuickSpawnItemDirect(new EntitySource_Misc("Spoil"), ModContent.ItemType<Relic>());
			if (item.ModItem is Relic relic) {
				if (Main.rand.NextBool(20)) {
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<GenericTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<GenericTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<GenericTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<GenericTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<GenericTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<GenericTemplate>(), 3);
				}
				else {
					for (int i = 0; i < 6; i++) {
						relic.AddRelicTemplate(player, Main.rand.Next(RelicTemplateLoader.TotalCount), 3);
					}
				}
			}
		}
	}
	public class PerkSpoil3 : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(new EntitySource_Misc("Spoil"), ModContent.ItemType<GlitchWorldEssence>());
		}
	}
	public class RandomSpoilUncommon5 : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player) {
			List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
			for (int i = 0; i < 3; i++) {
				ModSpoil spoil = Main.rand.Next(SpoilList);
				spoil.OnChoose(Main.LocalPlayer);
				Main.NewText("You have earned : " + spoil.DisplayName, SpoilDropRarity.ColorBaseOnRareValue(spoil.RareValue));
			}
		}
	}

	public class SSR_LunarGift : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player) {
			int LunarWeapon = Main.rand.Next(new int[] { ItemID.DayBreak, ItemID.SolarEruption, ItemID.NebulaBlaze, ItemID.NebulaArcanum, ItemID.VortexBeater, ItemID.Phantasm, ItemID.StardustDragonStaff, ItemID.StardustCellStaff,
			 ItemID.Meowmere, ItemID.StarWrath, ItemID.Terrarian, ItemID.LastPrism, ItemID.SDMG, ItemID.Celeb2, ItemID.LunarFlareBook,
			 ItemID.RainbowCrystalStaff ,ItemID.MoonlordTurretStaff, ItemID.Zenith
		});
			player.QuickSpawnItem(new EntitySource_Misc("Spoil"), LunarWeapon, 1);
			ModUtils.AmmoForWeapon(player, LunarWeapon, 4);
		}
	}
	public class SSR_LunarGift2 : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			int rand = Main.rand.Next(4);
			switch (rand) {
				case 0:
					player.QuickSpawnItem(source, ItemID.SolarFlareHelmet, 1);
					player.QuickSpawnItem(source, ItemID.SolarFlareBreastplate, 1);
					player.QuickSpawnItem(source, ItemID.SolarFlareLeggings, 1);
					break;
				case 1:
					player.QuickSpawnItem(source, ItemID.VortexHelmet, 1);
					player.QuickSpawnItem(source, ItemID.VortexBreastplate, 1);
					player.QuickSpawnItem(source, ItemID.VortexLeggings, 1);
					break;
				case 2:
					player.QuickSpawnItem(source, ItemID.NebulaHelmet, 1);
					player.QuickSpawnItem(source, ItemID.NebulaBreastplate, 1);
					player.QuickSpawnItem(source, ItemID.NebulaLeggings, 1);
					break;
				case 3:
					player.QuickSpawnItem(source, ItemID.StardustHelmet, 1);
					player.QuickSpawnItem(source, ItemID.StardustBreastplate, 1);
					player.QuickSpawnItem(source, ItemID.StardustLeggings, 1);
					break;
			}
		}
	}
	public class SSR_SuperSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player) {
			List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
			int RarityChoose = Main.rand.Next([SpoilDropRarity.Common, SpoilDropRarity.Uncommon, SpoilDropRarity.Rare]);
			for (int i = 0; i < SpoilList.Count; i++) {
				ModSpoil spoil = SpoilList[i];
				if (spoil.RareValue != RarityChoose) {
					continue;
				}
				spoil.OnChoose(player);
				Main.NewText("You have earned : " + spoil.DisplayName, SpoilDropRarity.ColorBaseOnRareValue(spoil.RareValue));
			}
		}
	}
}
