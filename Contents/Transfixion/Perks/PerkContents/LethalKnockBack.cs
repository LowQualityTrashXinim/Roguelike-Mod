using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class LethalKnockBack : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<LethalKnockBack>();
		CanBeStack = true;
		StackLimit = 5;
	}
	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) > 0) {
			return DescriptionIndex(1);
		}
		return base.ModifyToolTip();
	}
	public override void ModifyKnockBack(Player player, Item item, ref StatModifier knockback) {
		if (item.DamageType == DamageClass.Melee) {
			knockback += .15f * StackAmount(player);
		}
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage -= Math.Clamp(.11f - .02f * StackAmount(player), 0, 1f);
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += player.GetWeaponKnockback(item) * .1f * StackAmount(player) * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += proj.knockBack * .1f * StackAmount(player) * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
	}
}
