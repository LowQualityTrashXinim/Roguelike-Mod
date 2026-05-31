using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Weapon;
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
		handler.UpdateCriticalDamage += .75f;
	}
	public override void ModifyWeaponCrit(Player player, Item item, ref float crit) {
		crit += 20;
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
		player.GetModPlayer<WeaponEffectPlayer>().Effect5 = true;
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
public class WeaponEffectPlayer : ModPlayer {
	public bool Effect4 = false;
	public bool Effect5 = false;
	public bool Effect6 = false;
	public override void ResetEffects() {
		Effect4 = false;
		Effect5 = false;
		Effect6 = false;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (Effect6) {
			if (Main.rand.NextBool(20)) {
				modifiers.SourceDamage += 5;
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Effect5) {
			if (Main.rand.NextBool(20) && !proj.minion && proj.Check_ItemTypeSource(Player.HeldItem.type)) {
				Player.Heal(Main.rand.Next(1, 20));
			}
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Effect5) {
			if (Main.rand.NextBool(20)) {
				Player.Heal(Main.rand.Next(1, 20));
			}
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
