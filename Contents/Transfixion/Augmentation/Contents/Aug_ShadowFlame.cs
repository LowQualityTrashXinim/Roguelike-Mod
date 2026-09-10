using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class ShadowFlame : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.MediumPurple;
		ItemTypeID = ItemID.ShadowFlameHexDoll;
	}
	public override int[] UpgradeAvailable() => [ItemID.ShadowFlameKnife, ItemID.ShadowOrb];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.ShadowFlameKnife);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.ShadowOrb);
			default:
				return desc;
		}
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.ShadowFlame, ModUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.ShadowFlame, ModUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.ShadowFlame)) {
			if (acc.AugmentUpgrade.Contains(ItemID.ShadowFlameKnife)) {
				modifiers.SourceDamage += .2f;
			}
			if (acc.AugmentUpgrade.Contains(ItemID.ShadowOrb)) {
				modifiers.Knockback += .4f;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			if (target.HasBuff(BuffID.Frostburn)) {
				if (acc.AugmentUpgrade.Contains(ItemID.ShadowFlameKnife)) {
					modifiers.SourceDamage += .2f;
				}
				if (acc.AugmentUpgrade.Contains(ItemID.ShadowOrb)) {
					modifiers.Knockback += .4f;
				}
			}

		}
	}
}
