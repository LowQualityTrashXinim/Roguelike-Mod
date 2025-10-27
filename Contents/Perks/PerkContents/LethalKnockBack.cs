using Roguelike.Common.Utils;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Perks.PerkContents;
public class LethalKnockBack : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<LethalKnockBack>();
		list_category.Add(PerkCategory.WeaponUpgrade);
		CanBeStack = false;
	}
	public override void ModifyKnockBack(Player player, Item item, ref StatModifier knockback) {
		if (item.DamageType == DamageClass.Melee) {
			knockback += .15f;
		}
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage -= .11f;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += item.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += proj.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
	}
}
