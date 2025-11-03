using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class CursedFlame : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.ForestGreen;
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.CursedInferno, ModUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.CursedInferno, ModUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (target.HasBuff(BuffID.CursedInferno)) {
			if (chargeNum >= 1) {

				modifiers.SourceDamage += .2f;
			}
			if (chargeNum >= 2) {
				modifiers.Knockback += .4f;
			}
		}
		if (target.HasBuff(BuffID.CursedInferno)) {
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			if (target.HasBuff(BuffID.CursedInferno)) {
				if (chargeNum >= 1) {
					modifiers.SourceDamage += .2f;
				}
				if (chargeNum >= 2) {
					modifiers.Knockback += .4f;
				}
			}
		}
	}
}
