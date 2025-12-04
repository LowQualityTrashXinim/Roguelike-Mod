using Roguelike.Common.Utils;
using System;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class LuckyStrike : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<LuckyStrike>();
		CanBeChoosen = false;
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().ChanceLootDrop += .3f;
		player.ModPlayerStats().ChanceDropModifier.Base += 1;
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.FinalDamage.Flat -= Main.rand.Next(1, 1 + (int)Math.Ceiling(npc.damage * .85f));
		}
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.FinalDamage.Flat -= Main.rand.Next(1, 1 + (int)Math.Ceiling(proj.damage * .85f));
		}
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
		}
	}
	public override bool FreeDodge(Player player, Player.HurtInfo hurtInfo) {
		if (!player.immune && Main.rand.NextFloat() <= .35f) {
			player.AddImmuneTime(hurtInfo.CooldownCounter, Main.rand.Next(44, 89));
			player.immune = true;
			return true;
		}
		return base.FreeDodge(player, hurtInfo);
	}
}
