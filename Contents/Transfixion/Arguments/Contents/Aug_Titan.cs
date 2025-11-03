using Roguelike.Common.Global;
using Terraria;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class Titan : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.Blue;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)player.GetWeaponKnockback(item);
		modifiers.SourceDamage.Base += damage;
		int charge = acc.Check_ChargeConvertToStackAmount(index);
		if(charge >= 1) {
			int knockbackStrength = (int)(player.GetWeaponDamage(item) * .05f);
			modifiers.Knockback += knockbackStrength;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int damage = (int)player.GetWeaponKnockback(player.HeldItem);
			modifiers.SourceDamage.Base += damage;
			int charge = acc.Check_ChargeConvertToStackAmount(index);
			if (charge >= 1) {
				int knockbackStrength = (int)(player.GetWeaponDamage(player.HeldItem) * .05f);
				modifiers.Knockback += knockbackStrength;
			}
		}
	}
}
