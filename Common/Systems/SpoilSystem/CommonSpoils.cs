using Terraria;
using Humanizer;
using Terraria.ID;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;
using Terraria.DataStructures;

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
	public override void OnChoose(Player player) {
		PlayerStatsHandle chestplayer = player.GetModPlayer<PlayerStatsHandle>();
		chestplayer.GetAmount();
		ModUtils.GetWeaponSpoil(new EntitySource_Misc("Spoil"), chestplayer.weaponAmount);
	}
}

public class AccessorySpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.HermesBoots);
	}
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1, true));
	}
	public override void OnChoose(Player player) {
		int amount = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1);
		ModUtils.GetAccessories(new EntitySource_Misc("Spoil"), player, amount);
	}
}

public class ArmorSpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.ArmorStatue);
	}
	public override void OnChoose(Player player) {
		ModUtils.GetArmorForPlayer(new EntitySource_Misc("Spoil"), player, true);
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
	public override void OnChoose(Player player) {
		ModUtils.GetPotion(new EntitySource_Misc("Spoil"), player);
	}
}

public class RelicSpoil : ModSpoil {
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2, true));
	}
	public override void OnChoose(Player player) {
		ModUtils.GetRelic(new EntitySource_Misc("Spoil"), player, 2);
	}
}
public class SkillSpoil : ModSpoil {
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2, true));
	}
	public override void OnChoose(Player player) {
		ModUtils.GetSkillLootbox(new EntitySource_Misc("Spoil"), player, 2);
	}
}
public class FoodSpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.Burger);
	}
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(6, true));
	}
	public override void OnChoose(Player player) {
		int amount = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(6);
		for (int i = 0; i < amount; i++) {
			player.QuickSpawnItem(new EntitySource_Misc("Spoil"), Main.rand.Next(TerrariaArrayID.AllFood));
		}
	}
}
