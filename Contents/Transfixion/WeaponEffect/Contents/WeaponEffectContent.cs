using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Weapon;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.WeaponEffect.Contents;
public class Effect1 : WeaponEffect {
	public override void ModifyWeaponDamage(Player player, Item item, ref StatModifier damage) {
		damage += .5f;
		damage.Base += 10;
	}
}
public class Effect2 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.ModPlayerStats().UpdateCritDamage += .35f;
	}
	public override void ModifyWeaponCrit(Player player, Item item, ref float crit) {
		crit += 15;
	}
}
public class Effect3 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.ModPlayerStats().DirectItemDamage += 1;
	}
}
public class Effect4 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.GetModPlayer<WeaponEffectPlayer>().Effect4 = true;
	}
}
public class Effect5 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.GetModPlayer<WeaponEffectPlayer>().Effect5++;
	}
}
public class Effect6 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.GetModPlayer<WeaponEffectPlayer>().Effect6 = true;
	}
}
public class Effect7 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.ModPlayerStats().AttackSpeed += .25f;
	}
}
public class Effect8 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.ModPlayerStats().PercentageDamage += .001f;
	}
}
public class Effect9 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.ModPlayerStats().TrueDamage += .2f;
	}
}
public class Effect10 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.GetModPlayer<WeaponEffectPlayer>().Effect10++;
	}
}
public class Effect11 : WeaponEffect {
	public override void ModifyWeaponDamage(Player player, Item item, ref StatModifier damage) {
		damage.Base += 30;
	}
}
public class Effect12 : WeaponEffect {
	public override void ModifyWeaponDamage(Player player, Item item, ref StatModifier damage) {
		damage += .025f * (1 - player.statLife / (float)player.statLifeMax2) / .01f;
	}
}
public class Effect13 : WeaponEffect {
	public override void ModifyWeaponCrit(Player player, Item item, ref float crit) {
		if (!player.IsHealthAbovePercentage(.5f)) {
			crit += 30;
		}
	}
}
public class Effect14 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.GetModPlayer<WeaponEffectPlayer>().Effect14++;
	}
}
public class Effect15 : WeaponEffect {
	public override void UpdateItem(Player player, Item item, GlobalItemHandle handler) {
		player.ModPlayerStats().UpdateCritDamage += 1;
	}
}
public class Effect16 : WeaponEffect {
	public override void ModifyWeaponCrit(Player player, Item item, ref float crit) {
		crit += 40;
	}
}
public class WeaponEffectPlayer : ModPlayer {
	public bool Effect4 = false;
	public int Effect5 = 0;
	public bool Effect6 = false;
	public int Effect10 = 0;
	public int Effect14 = 0;
	public override void ResetEffects() {
		Effect4 = false;
		Effect5 = 0;
		Effect6 = false;
		Effect10 = 0;
		Effect14 = 0;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (Effect6) {
			if (Main.rand.NextBool(20)) {
				modifiers.SourceDamage += 5;
			}
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage += .5f * Effect14;
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage += .5f * Effect14;
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.minion || !proj.Check_ItemTypeSource(Player.HeldItem.type)) {
			return;
		}
		for (int i = 0; i < Effect5; i++) {
			if (Main.rand.NextBool(20)) {
				Player.Heal(Main.rand.Next(1, 20));
			}
		}
		for (int i = 0; i < Effect14; i++) {
			if (Main.rand.NextFloat() <= .7f) {
				Player.StrikeNPCDirect(target, target.CalculateHitInfo(80 + Math.Min((int)(target.life * .05f), 200), 1));
			}
			Effect14--;
		}
		for (int i = 0; i < Effect10; i++) {
			if (Main.rand.NextBool(5)) {
				Player.StrikeNPCDirect(target, hit);
			}
			Effect10--;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		for (int i = 0; i < Effect5; i++) {
			if (Main.rand.NextBool(20)) {
				Player.Heal(Main.rand.Next(1, 20));
			}
		}
		for (int i = 0; i < Effect14; i++) {
			if (Main.rand.NextFloat() <= .7f) {
				Player.StrikeNPCDirect(target, target.CalculateHitInfo(80 + Math.Min((int)(target.life * .05f), 200), 1));
			}
			Effect14--;
		}
		for (int i = 0; i < Effect10; i++) {
			if (Main.rand.NextBool(5)) {
				Player.StrikeNPCDirect(target, hit);
			}
			Effect10--;
		}
	}
	public override bool CanConsumeAmmo(Item weapon, Item ammo) {
		if (Effect4) {
			return false;
		}
		return base.CanConsumeAmmo(weapon, ammo);
	}
	public override void OnConsumeMana(Item item, int manaConsumed) {
		if (Effect4) {
			if (Player.statMana + manaConsumed >= Player.statManaMax2) {
				Player.statMana = Player.statManaMax2;
			}
			else {
				Player.statMana += manaConsumed;
			}
		}
	}
}
