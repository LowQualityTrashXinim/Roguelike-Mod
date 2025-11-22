using Terraria;
using Humanizer;
using Terraria.ID;
using System.Collections.Generic;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;
using Roguelike.Contents.Items.Lootbox;

namespace Roguelike.Common.Systems.SpoilSystem;

public class WeaponSpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.IronBroadsword);
	}
	public override string FinalDescription() {
		PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
		chestplayer.GetAmount();
		return Description.FormatWith(chestplayer.weaponAmount);
	}
	public override void OnChoose(Player player, int itemsource) {
		ModUtils.GetWeapon(ContentSamples.ItemsByType[itemsource], player);
	}
}

public class AccessorySpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.HermesBoots);
	}
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1));
	}
	public override void OnChoose(Player player, int itemsource) {
		int amount = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1);
		for (int i = 0; i < amount; i++) {
			ModUtils.GetAccessories(itemsource, player);
		}
	}
}

public class ArmorSpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.ArmorStatue);
	}
	public override void OnChoose(Player player, int itemsource) {
		ModUtils.GetArmorForPlayer(player.GetSource_OpenItem(itemsource), player, true);
	}
}

public class PotionSpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.WrathPotion);
	}
	public override string FinalDescription() {
		PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
		chestplayer.GetAmount();
		return Description.FormatWith(chestplayer.potionTypeAmount);
	}
	public override void OnChoose(Player player, int itemsource) {
		ModUtils.GetPotion(itemsource, player);
	}
}

public class RelicSpoil : ModSpoil {
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2));
	}
	public override void OnChoose(Player player, int itemsource) {
		ModUtils.GetRelic(itemsource, player, 2);
	}
}
public class SkillSpoil : ModSpoil {
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2));
	}
	public override void OnChoose(Player player, int itemsource) {
		ModUtils.GetSkillLootbox(itemsource, player, 2);
	}
}
public class FoodSpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.Burger);
	}
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(6));
	}
	public override void OnChoose(Player player, int itemsource) {
		int amount = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(6);
		for (int i = 0; i < amount; i++) {
			player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), Main.rand.Next(TerrariaArrayID.AllFood));
		}
	}
}
