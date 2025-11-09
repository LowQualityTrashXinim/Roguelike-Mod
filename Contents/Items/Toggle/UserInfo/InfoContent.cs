using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.RelicItem;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Toggle.UserInfo;

public class Info_Melee : InfoType {
	public override void SetStaticDefaults() {
		Priority = 0;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.BoneSword)} Melee Damage : {Math.Round(player.GetTotalDamage(DamageClass.Melee).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Melee).Base} Flat : {player.GetTotalDamage(DamageClass.Melee).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Melee)}%";
	}
}
public class Info_Range : InfoType {
	public override void SetStaticDefaults() {
		Priority = 1;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.PlatinumBow)} Range Damage : {Math.Round(player.GetTotalDamage(DamageClass.Ranged).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Ranged).Base} Flat : {player.GetTotalDamage(DamageClass.Ranged).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Ranged)}%";
	}
}
public class Info_Mage : InfoType {
	public override void SetStaticDefaults() {
		Priority = 2;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.RubyStaff)} Magic Damage : {Math.Round(player.GetTotalDamage(DamageClass.Magic).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Magic).Base} Flat : {player.GetTotalDamage(DamageClass.Magic).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Magic)}%";
	}
}
public class Info_Summon : InfoType {
	public override void SetStaticDefaults() {
		Priority = 3;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.BabyBirdStaff)} Summon Damage : {Math.Round(player.GetTotalDamage(DamageClass.Summon).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Summon).Base} Flat : {player.GetTotalDamage(DamageClass.Summon).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Summon)}%";
	}
}
public class Info_Generic : InfoType {
	public override void SetStaticDefaults() {
		Priority = 4;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.AvengerEmblem)} Generic Damage : {Math.Round(player.GetTotalDamage(DamageClass.Generic).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Generic).Base} Flat : {player.GetTotalDamage(DamageClass.Generic).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Generic)}%";
	}
}
public class Info_CritDmg : InfoType {
	public override void SetStaticDefaults() {
		Priority = 5;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.DestroyerEmblem)} Crit damage : {Math.Round((player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage.ApplyTo(1) + 1) * 100, 2)}%";
	}
}
public class Info_FirstStrike : InfoType {
	public override void SetStaticDefaults() {
		Priority = 6;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.BreakerBlade)} First strike damage : {Math.Round((player.GetModPlayer<PlayerStatsHandle>().UpdateFullHPDamage.ApplyTo(1) - 1) * 100, 2)}%";
	}
}
public class Info_AttackSpeed : InfoType {
	public override void SetStaticDefaults() {
		Priority = 7;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.ShroomiteDiggingClaw)} Attack speed: {RelicTemplateLoader.RelicValueToPercentage(player.GetTotalAttackSpeed(DamageClass.Generic))}%";
	}
}
public class Info_HPRegen : InfoType {
	public override void SetStaticDefaults() {
		Priority = 8;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		int increases = 0;
		if (player.lifeForce) {
			increases = 20;
		}
		return $"{ModUtils.ItemIcon(ItemID.BandofRegeneration)} Health regen: {player.lifeRegen} Additive: {Math.Round(player.ModPlayerStats().UpdateHPRegen.Additive * 100 + increases)}% base : {Math.Round(player.ModPlayerStats().UpdateHPRegen.Base * 100)} Multiplicative: {Math.Round(player.ModPlayerStats().UpdateHPRegen.Multiplicative)}x Flat: {Math.Round(player.ModPlayerStats().UpdateHPRegen.Flat)}";
	}
}
public class Info_HP : InfoType {
	public override void SetStaticDefaults() {
		Priority = 8;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		int increases = 0;
		if (player.lifeForce) {
			increases = 20;
		}
		return $"{ModUtils.ItemIcon(ItemID.LifeCrystal)} health: {player.statLifeMax2} Additive: {Math.Round(player.ModPlayerStats().UpdateHPMax.Additive * 100 + increases)}% base: {Math.Round(player.ModPlayerStats().UpdateHPMax.Base * 100)} Multiplicative: {Math.Round(player.ModPlayerStats().UpdateHPMax.Multiplicative)}x";
	}
}
public class Info_ManaRegen : InfoType {
	public override void SetStaticDefaults() {
		Priority = 9;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.ManaRegenerationBand)} Mana regen: {player.manaRegen} Additive: {Math.Round(player.ModPlayerStats().UpdateManaRegen.Additive * 100)}% base: {Math.Round(player.ModPlayerStats().UpdateManaRegen.Base)} Multiplicative: {Math.Round(player.ModPlayerStats().UpdateManaRegen.Multiplicative)}x";
	}
}
public class Info_Mana : InfoType {
	public override void SetStaticDefaults() {
		Priority = 9;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.ManaCrystal)} Mana: {player.statManaMax2} Additive: {Math.Round(player.ModPlayerStats().UpdateManaMax.Additive * 100)}% base: {Math.Round(player.ModPlayerStats().UpdateManaMax.Base)} Multiplicative: {Math.Round(player.ModPlayerStats().UpdateManaMax.Multiplicative)}x";
	}
}
public class Info_ManaReduction : InfoType {
	public override void SetStaticDefaults() {
		Priority = 10;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.ManaFlower)} Mana reduction : {player.manaCost}%";
	}
}
public class Info_DefenseEff : InfoType {
	public override void SetStaticDefaults() {
		Priority = 10;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.ShieldStatue)} Defense effectiveness : {player.DefenseEffectiveness.Value}x";
	}
}
public class Info_DR : InfoType {
	public override void SetStaticDefaults() {
		Priority = 11;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.WormScarf)} Damage reduction: {Math.Round(player.endurance * 100, 2)}%";
	}
}
public class Info_MovementSpeed : InfoType {
	public override void SetStaticDefaults() {
		Priority = 12;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.HermesBoots)} Movement speed : {Math.Round(player.moveSpeed, 2)}%";
	}
}
public class Info_JumpBoost : InfoType {
	public override void SetStaticDefaults() {
		Priority = 13;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.FrogLeg)} Jump boost : {player.jumpSpeedBoost}%";
	}
}
public class Info_Minion : InfoType {
	public override void SetStaticDefaults() {
		Priority = 14;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.BewitchingTable)} Max minion : {player.maxMinions} {ModUtils.ItemIcon(ItemID.WarTable)} Max sentry/turret : {player.maxTurrets}";
	}
}
public class Info_Thorn : InfoType {
	public override void SetStaticDefaults() {
		Priority = 15;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon(ItemID.Cactus)} Thorn : {player.thorns}";
	}
}
public class Info_Drop : InfoType {
	public override void SetStaticDefaults() {
		Priority = 16;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		return $"{ModUtils.ItemIcon<WoodenLootBox>()} Amount drop : {player.ModPlayerStats().DropModifier.ApplyTo(1)} Drop chance : {Math.Round(player.ModPlayerStats().ChanceDropModifier.ApplyTo(1) * 100)}%";
	}
}
public class Info_MeleeChance : InfoType {
	public override void SetStaticDefaults() {
		Priority = 18;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		var statshandle = player.GetModPlayer<PlayerStatsHandle>();
		return $"{ModUtils.ItemIcon(ItemID.FragmentSolar)} Melee drop chance : {Math.Round(statshandle.UpdateMeleeChanceMutilplier * 100)}%";
	}
}
public class Info_RangeChance : InfoType {
	public override void SetStaticDefaults() {
		Priority = 18;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		var statshandle = player.GetModPlayer<PlayerStatsHandle>();
		return $"{ModUtils.ItemIcon(ItemID.FragmentVortex)} Range drop chance : {Math.Round(statshandle.UpdateRangeChanceMutilplier * 100)}%";
	}
}
public class Info_MagicChance : InfoType {
	public override void SetStaticDefaults() {
		Priority = 18;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		var statshandle = player.GetModPlayer<PlayerStatsHandle>();
		return $"{ModUtils.ItemIcon(ItemID.FragmentNebula)} Magic drop chance : {Math.Round(statshandle.UpdateMagicChanceMutilplier * 100)}%";
	}
}
public class Info_SummonChance : InfoType {
	public override void SetStaticDefaults() {
		Priority = 18;
	}
	public override string InfoText() {
		Player player = Main.LocalPlayer;
		var statshandle = player.GetModPlayer<PlayerStatsHandle>();
		return $"{ModUtils.ItemIcon(ItemID.FragmentStardust)} Summon drop chance : {Math.Round(statshandle.UpdateSummonChanceMutilplier * 100)}%";
	}
}
