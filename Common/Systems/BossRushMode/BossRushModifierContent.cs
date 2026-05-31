using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.BossRushMode;
public class BR_Modifier1 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Grant 5 random weapons";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		ModUtils.GetWeaponSpoil(player.GetSource_Misc("Gift"), 5);
	}
}
public class BR_Modifier2 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Increases true damage by 50%";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		player.GetModPlayer<BossRushModifierPlayer>().IncreasesDamage = true;
	}
}
public class BR_Modifier3 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Increases damage reduction by 30%";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		player.GetModPlayer<BossRushModifierPlayer>().IncreasesDR = true;
	}
}
public class BR_Modifier4 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description =>
		"Your first strike damage can be applied 5 times";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		player.GetModPlayer<BossRushModifierPlayer>().ManyStrike = true;
	}
}
public class BR_Modifier5 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Your attack deal second strike (CD: 2s)";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		player.GetModPlayer<BossRushModifierPlayer>().SecondStrike = true;
	}
}
public class BR_Modifier6 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Increases your life steal by 20%";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		player.GetModPlayer<BossRushModifierPlayer>().LifeSteal = true;
	}
}
public class BR_Modifier7 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "You regenerate mana rapidly";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		player.GetModPlayer<BossRushModifierPlayer>().RapidMana = true;
	}
}

public class BR_Modifier8 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Your attack always deal critical damage";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		player.GetModPlayer<BossRushModifierPlayer>().AlwaysCrit = true;
	}
}
public class BR_Modifier9 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Grant you 5 random accessories";
	public override void OnChoose() {
		ModUtils.GetAccessories(Main.LocalPlayer.GetSource_Misc("Gift"), Main.LocalPlayer, 5);
	}
}
public class BR_Modifier10 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Grant you 10 random pierces of armor";
	public override void OnChoose() {
		for (int i = 0; i < 10; i++) {
			ModUtils.GetArmorPiece(Main.LocalPlayer.GetSource_Misc("Gift"), Main.LocalPlayer, true);
		}
	}
}
public class BR_Modifier11 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Grant you 6 random potions of 3 stacks";
	public override void OnChoose() {
		for (int i = 0; i < 10; i++) {
			ModUtils.GetPotion(Main.LocalPlayer.GetSource_Misc("Gift"), Main.LocalPlayer, 6, 3);
		}
	}
}
public class BR_Modifier12 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Grant you 6 random food of 3 stacks";
	public override void OnChoose() {
		for (int i = 0; i < 6; i++) {
			Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc("Gift"), Main.rand.Next(TerrariaArrayID.AllFood), 3);
		}
	}
}
public class BR_Modifier13 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Grant you a extra life during this modifier active";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		player.GetModPlayer<BossRushModifierPlayer>().Extralife = true;
	}
}

public class BossRushModifierPlayer : ModPlayer {
	//+50% damage
	public bool IncreasesDamage = false;
	//+30% damage reduction
	public bool IncreasesDR = false;
	//Rapid mana
	public bool RapidMana = false;
	//First strike become many strike
	public bool ManyStrike = false;
	//Chance to deal 2nd strike
	public bool SecondStrike = false;
	public int SecondStrikeCD = 0;
	//+20% life steal
	public bool LifeSteal = false;
	public bool AlwaysCrit = false;
	public bool Extralife = false;
	public void ResetAllModifier() {
		IncreasesDamage = false;
		IncreasesDR = false;
		RapidMana = false;
		ManyStrike = false;
		SecondStrike = false;
		LifeSteal = false;
		AlwaysCrit = false;
		Extralife = false;
	}
	public override void ResetEffects() {
		PlayerStatsHandle.SetSecondLifeCondition(Player, "Modifier_Extralife", Extralife);
	}
	public override void UpdateEquips() {
		PlayerStatsHandle handler = Player.ModPlayerStats();
		if (IncreasesDamage) {
			handler.TrueDamage += .5f;
		}
		if (IncreasesDR) {
			Player.endurance += .3f;
		}
		if (RapidMana) {
			handler.Rapid_CacheMana += 1;
		}
		if (ManyStrike) {
			handler.HitCountIgnore += 5;
		}
		if (LifeSteal) {
			handler.LifeSteal += .2f;
		}
		if (AlwaysCrit) {
			handler.AlwaysCritValue++;
		}
		SecondStrikeCD = ModUtils.CountDown(SecondStrikeCD);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (SecondStrike && SecondStrikeCD <= 0) {
			SecondStrikeCD = 120;
			Player.StrikeNPCDirect(target, hit);
		}
	}
	public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
		if (PlayerStatsHandle.GetSecondLife(Player, "Modifier_ExtraLife")) {
			Extralife = false;
			Player.AddImmuneTime(-1, 120);
			Player.immune = true;
			Player.Heal(Player.statLifeMax2);
			return false;
		}
		return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
	}
}
public class BR_BadModifier1 : BossRushModifier {
	public override string Description => "Have 1 in 3 chance to delete your weapon in your inventory";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		for (int i = 0; i < player.inventory.Length; i++) {
			Item item = player.inventory[i];
			if (item == null) {
				continue;
			}
			if (item.IsAir) {
				continue;
			}
			if (item.IsAWeapon() && Main.rand.NextBool(3)) {
				item.TurnToAir();
			}
		}
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier2 : BossRushModifier {
	public override string Description => "Have 1 in 3 chance to delete your accessory in your inventory";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		for (int i = 0; i < player.inventory.Length; i++) {
			Item item = player.inventory[i];
			if (item == null) {
				continue;
			}
			if (item.IsAir) {
				continue;
			}
			if (item.accessory && Main.rand.NextBool(3)) {
				item.TurnToAir();
			}
		}
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier3 : BossRushModifier {
	public override string Description => "Have 1 in 3 chance to delete your armor in your inventory";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		for (int i = 0; i < player.inventory.Length; i++) {
			Item item = player.inventory[i];
			if (item == null) {
				continue;
			}
			if (item.IsAir) {
				continue;
			}
			if (item.IsThisArmorPiece() && Main.rand.NextBool(3)) {
				item.TurnToAir();
			}
		}
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier4 : BossRushModifier {
	public override string Description => "All enemies gain 900% more HP";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier5 : BossRushModifier {
	public override string Description => "Spawn enemies along with bosses, enemy have 95% damage reduction";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier6 : BossRushModifier {
	public override string Description => "Spawn mini boss along with bosses, enemy have 95% damage reduction";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier7 : BossRushModifier {
	public override string Description => "Boss's attack deal 25% percentage damage";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier8 : BossRushModifier {
	public override string Description => "Spawn a 2nd boss that can't be killed";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier9 : BossRushModifier {
	public override string Description => "Hostile NPC's AI update a lot more";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier10 : BossRushModifier {
	public override string Description => "The first 100 hits, damage dealt to boss is capped at 1";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier11 : BossRushModifier {
	public override string Description => "Boss regenerate health at 1% of their maximum health";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
public class BR_BadModifier12: BossRushModifier {
	public override string Description => "Everytime a npc got hit, they gain 0.5s of i-frame";
	public override void OnChoose() {
		ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = Type;
	}
}
