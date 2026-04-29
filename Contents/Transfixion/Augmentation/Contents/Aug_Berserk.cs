using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Berserk : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.OrangeRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount();
		if (chargeNum >= 1) {
			if (!player.IsHealthAbovePercentage(.7f)) {
				player.GetDamage(DamageClass.Generic) += .25f;
			}
		}
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		float percentage = player.statLife / (float)player.statLifeMax2;
		modifiers.SourceDamage += .5f * percentage;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			float percentage = player.statLife / (float)player.statLifeMax2;
			modifiers.SourceDamage += .5f * percentage;
		}
	}
}
