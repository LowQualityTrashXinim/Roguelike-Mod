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
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
		}
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.SourceDamage *= 2;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
		}
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.SourceDamage *= 2;
		}
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextFloat() <= .2f) {
			player.Heal(Main.rand.Next(1, 50));
		}
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextFloat() <= .2f) {
			player.Heal(Main.rand.Next(1, 50));
		}
	}
}
