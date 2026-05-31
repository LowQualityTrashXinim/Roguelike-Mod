using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Transfixion.Perks.CommonPerk;
public class MeleeDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetDamage(DamageClass.Melee) += .15f * StackAmount(player);
	}
}
public class RangeDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetDamage(DamageClass.Ranged) += .15f * StackAmount(player);
	}
}
public class MagicDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetDamage(DamageClass.Magic) += .15f * StackAmount(player);
	}
}
public class SummonDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetDamage(DamageClass.Summon) += .15f * StackAmount(player);
	}
}
public class IncreasesDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetDamage(DamageClass.Generic) += .08f * StackAmount(player);
	}
}
public class IncreasesCriticalChance : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetCritChance(DamageClass.Generic) += 3 * StackAmount(player);
	}
}
public class IncreasesCriticalDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateCritDamage += .2f * StackAmount(player);
	}
}

public class MeleeCriticalChance : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetCritChance(DamageClass.Melee) += 8 * StackAmount(player);
	}
}

public class RangeCriticalChance : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetCritChance(DamageClass.Ranged) += 8 * StackAmount(player);
	}
}

public class MagicCriticalChance : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.GetCritChance(DamageClass.Magic) += 8 * StackAmount(player);
	}
}

public class MeleeCriticalDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().Melee_CritDamage += .3f * StackAmount(player);
	}
}

public class RangeCriticalDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().Range_CritDamage += .3f * StackAmount(player);
	}
}

public class MagicCriticalDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().Magic_CritDamage += .3f * StackAmount(player);
	}
}

public class IncreasesDefense : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateDefenseBase.Base += 4 * StackAmount(player);
	}
}

public class IncreasesHealth : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateHPMax.Base += 20 * StackAmount(player);
	}
}
public class IncreasesMana : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateManaMax.Base += 20 * StackAmount(player);
	}
}
public class IncreasesLifeRegen : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateHPRegen.Base += 1 * StackAmount(player);
	}
}
public class IncreasesManaRegen : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateManaRegen.Base += 1 * StackAmount(player);
	}
}
public class IncreasesDebuffDamage : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().DebuffDamage.Base += .04f * StackAmount(player);
	}
}
public class IncreasesThorn : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateThorn += .1f * StackAmount(player);
	}
}
public class IncreasesHealEff : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().HealEffectiveness += .1f * StackAmount(player);
	}
}
public class IncreasesMovement : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateMovement += .1f * StackAmount(player);
	}
}
public class IncreasesJumpBoost : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !Main.LocalPlayer.GetModPlayer<PerkPlayer>().PerkBlocker;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateJumpBoost += .1f * StackAmount(player);
	}
}
