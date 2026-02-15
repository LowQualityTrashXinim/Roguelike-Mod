using Humanizer;
using Roguelike.Common.Global;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Transfixion.Perks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.SpoilSystem;
internal class RareSpoil {
	public class RoguelikeSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.FallenStar);
		}
		public override string FinalDescription() {
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			return Description.FormatWith(
				chestplayer.ModifyGetAmount(2, true),
				chestplayer.ModifyGetAmount(4, true)
				);
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.RareDrop();
		}
		public override void OnChoose(Player player) {
			ModUtils.GetSkillLootbox(new EntitySource_Misc("Spoil"), player, 2);
			ModUtils.GetRelic(new EntitySource_Misc("Spoil"), player, 4);
		}
	}
	public class RareArmorPiece : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.RareDrop();
		}
		public override void OnChoose(Player player) {
			ModUtils.GetArmorPiece(new EntitySource_Misc("Spoil"), player);
		}
	}
	public class PerkSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.RareDrop();
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(new EntitySource_Misc("Spoil"), ModContent.ItemType<WorldEssence>());
		}
	}
	public class RareRelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.RareDrop();
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1, true));
		}
		public override void OnChoose(Player player) {
			IEntitySource entitySource = new EntitySource_Misc("Spoil");
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1);
			for (int i = 0; i < amount; i++) {
				Item item = player.QuickSpawnItemDirect(entitySource, ModContent.ItemType<Relic>());
				if (item.ModItem is Relic relic) {
					relic.AutoAddRelicTemplate(player, 3);
				}
			}
		}
	}
	public class RareWingSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.RareDrop();
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(new EntitySource_Misc("Spoil"), Main.rand.Next(TerrariaArrayID.AllWing));
		}
	}
}


