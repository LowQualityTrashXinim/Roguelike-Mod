using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Perks.PerkContents;
public class MarkOfSpectre : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<MarkOfSpectre>();
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.35f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.JumpBoost, 1.65f);
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		ModifyHit(ref modifiers);
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		ModifyHit(ref modifiers);
	}
	private static void ModifyHit(ref Player.HurtModifiers modifiers) {
		modifiers.FinalDamage += .25f;
		modifiers.Knockback *= .35f;
	}
	public override bool FreeDodge(Player player, Player.HurtInfo hurtInfo) {
		if (!player.immune && Main.rand.NextFloat() <= .6f) {
			player.AddImmuneTime(hurtInfo.CooldownCounter, 60);
			player.immune = true;
			return true;
		}
		return base.FreeDodge(player, hurtInfo);
	}
}
