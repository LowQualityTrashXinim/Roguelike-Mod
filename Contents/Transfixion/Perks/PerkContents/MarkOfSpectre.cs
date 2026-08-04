using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class MarkOfSpectre : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<MarkOfSpectre>();
		CanBeStack = true;
		StackLimit = 5;
	}
	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) > 0) {
			return DescriptionIndex(1);
		}
		return base.ModifyToolTip();
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.35f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.JumpBoost, 1.65f);
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		ModifyHit(player,ref modifiers);
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		ModifyHit(player, ref modifiers);
	}
	private void ModifyHit(Player player, ref Player.HurtModifiers modifiers) {
		modifiers.FinalDamage += .45f - StackAmount(player) * .05f;
		modifiers.Knockback *= .35f;
	}
	public override bool FreeDodge(Player player, Player.HurtInfo hurtInfo) {
		if (!player.immune && Main.rand.NextFloat() <= .5f + StackAmount(player) * .05f) {
			player.AddImmuneTime(hurtInfo.CooldownCounter, 60);
			player.immune = true;
			player.ModPlayerStats().HasDodgeInThisInstance = true;
			return true;
		}
		return base.FreeDodge(player, hurtInfo);
	}
}
