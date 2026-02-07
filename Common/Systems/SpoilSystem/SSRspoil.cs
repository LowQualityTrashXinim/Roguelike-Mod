using Humanizer;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Items.RelicItem.RelicTemplateContent;
using Roguelike.Contents.Transfixion.Perks;
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
			return Description.FormatWith(chestplayer.ModifyGetAmount(1), chestplayer.ModifyGetAmount(2, true));
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			int type = ModContent.ItemType<WorldEssence>();
			player.QuickSpawnItem(source, type);
			ModUtils.GetSkillLootbox(source, player);
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				Item relicitem = player.QuickSpawnItemDirect(source, ModContent.ItemType<Relic>());
				if (relicitem.ModItem is Relic relic) {
					relic.AutoAddRelicTemplate(player, 3);
				}
			}
			ModUtils.GetAccessories(source, player);
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
}
