using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.BossRushMode;
public class BR_Modifier1 : BossRushModifier {
	public override void SetStaticDefault() {
		PositiveModifier = true;
	}
	public override string Description => "Grant 10 random weapons";
	public override void OnChoose() {
		Player player = Main.LocalPlayer;
		ModUtils.GetWeaponSpoil(player.GetSource_Misc("Gift"), 10);
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
		"Your first strike damage caan be applied 5 times" +
		"\nMultiply first strike damage by 0.46x";
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
		player.ModPlayerStats().AlwaysCritValue++;
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
	public void ResetAllModifier() {
		IncreasesDamage = false;
		IncreasesDR = false;
		RapidMana = false;
		ManyStrike = false;
		SecondStrike = false;
		LifeSteal = false;
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
			handler.UpdateFullHPDamage *= .46f;
		}
		if (LifeSteal) {
			handler.LifeSteal += .2f;
		}
		SecondStrikeCD = ModUtils.CountDown(SecondStrikeCD);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (SecondStrike) {
			SecondStrikeCD = 120;
			Player.StrikeNPCDirect(target, hit);
		}
	}
}
public class BR_BadModifier1 : BossRushModifier {
	public override string Description => "Have 1 in 5 chance to delete your weapon in your inventory when choose";
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
			if (item.IsAWeapon() && Main.rand.NextBool(5)) {
				item.TurnToAir();
			}
		}
	}
}
public class BR_BadModifier2 : BossRushModifier {
	public override string Description => "Have 1 in 5 chance to delete your accessory in your inventory when choose";
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
			if (item.accessory && Main.rand.NextBool(5)) {
				item.TurnToAir();
			}
		}
	}
}
public class BR_BadModifier3 : BossRushModifier {
	public override string Description => "Have 1 in 5 chance to delete your armor in your inventory when choose";
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
			if (item.IsThisArmorPiece() && Main.rand.NextBool(5)) {
				item.TurnToAir();
			}
		}
	}
}
