using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class SlimyChalice : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.endurance += .1f;
		player.GetModPlayer<SlimyChalicePlayer>().Bouncy = true;
	}
}
public class SlimyChalicePlayer : ModPlayer {
	public bool Bouncy = false;
	public override void ResetEffects() {
		Bouncy = false;
	}
	public override void UpdateEquips() {
		if (Bouncy) {
			Player.endurance += .1f;
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (Bouncy) {
			modifiers.Knockback += 1;
			modifiers.KnockbackImmunityEffectiveness *= .5f;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Bouncy) {
			modifiers.Knockback += 1;
			modifiers.KnockbackImmunityEffectiveness *= .5f;
		}
	}
}
