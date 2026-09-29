using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.aDebugItem.DebugStick;
using Roguelike.Contents.Items.NoneSynergy;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Transfixion.Perks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.SpoilSystem;
internal class HiddenSpoil {
	public class Hidden_RelicSpoilTier8 : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = 999;
		}
		public override bool IsSelectable(Player player) {
			return false;
		}
		public override void OnChoose(Player player) {
			Item item = player.QuickSpawnItemDirect(new EntitySource_Misc("Spoil"), ModContent.ItemType<Relic>());
			if (item.ModItem is Relic relic) {
				for (int i = 0; i < 8; i++) {
					relic.AddRelicTemplate(player, Main.rand.Next(RelicTemplateLoader.TotalCount), 8);
				}
			}
		}
	}
	public class Hidden_RelicSpoilTier1 : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = 999;
		}
		public override bool IsSelectable(Player player) {
			return false;
		}
		public override void OnChoose(Player player) {
			Item item = player.QuickSpawnItemDirect(new EntitySource_Misc("Spoil"), ModContent.ItemType<Relic>());
			if (item.ModItem is Relic relic) {
				relic.AddRelicTemplate(player, Main.rand.Next(RelicTemplateLoader.TotalCount), 12);
			}
		}
	}
	public class Hidden_WeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = 999;
		}
		public override bool IsSelectable(Player player) {
			return false;
		}
		public override void OnChoose(Player player) {
			Item item = player.QuickSpawnItemDirect(new EntitySource_Misc("Spoil"),
				Main.rand.Next([
					..TerrariaArrayID.AllOreBroadSword,
					..TerrariaArrayID.AllOreBowPHM,
					..TerrariaArrayID.AllOreShortSword,
					..TerrariaArrayID.AllWoodSword,
					..TerrariaArrayID.AllWoodBowPHM
					]
				));
			item.GetGlobalItem<GlobalItemHandle>().SetItemLevel(999);
		}
	}
	public class Hidden_Creator : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = 999;
		}
		public override bool IsSelectable(Player player) {
			return false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItemDirect(new EntitySource_Misc("Spoil"), ModContent.ItemType<MainDebugStick>());
		}
	}
	public class Hidden_Difficulty : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = 999;
		}
		public override bool IsSelectable(Player player) {
			return false;
		}
		public override void OnChoose(Player player) {
			player.difficulty = PlayerDifficultyID.SoftCore;
		}
	}
	public class Hidden_WeaponSpoil2 : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = 999;
		}
		public override bool IsSelectable(Player player) {
			return false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItemDirect(new EntitySource_Misc("Spoil"), ModContent.ItemType<SniperRifle>());
		}
	}
	public class Hidden_World : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = 999;
		}
		public override bool IsSelectable(Player player) {
			return false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItemDirect(new EntitySource_Misc("Spoil"), ModContent.ItemType<GlitchWorldEssence>());
		}
	}
}
