using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class ArcaneShield : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().UpdateManaMax.Base += 100;
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		Player.HurtInfo info = modifiers.ToHurtInfo(npc.damage, player.statDefense, player.DefenseEffectiveness.Value, 1, true);
		if (player.CheckMana(info.Damage, true)) {
			modifiers.SourceDamage -= .6f;
			player.manaRegenDelay = player.maxRegenDelay * 4;
		}
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		Player.HurtInfo info = modifiers.ToHurtInfo(proj.damage, player.statDefense, player.DefenseEffectiveness.Value, 1, true);
		if (player.CheckMana(info.Damage, true)) {
			modifiers.SourceDamage -= .6f;
			player.manaRegenDelay = player.maxRegenDelay * 4;
		}
	}
}
