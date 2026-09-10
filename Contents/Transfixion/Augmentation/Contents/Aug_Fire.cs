using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;
namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Fire : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.Red;
		ItemTypeID = ItemID.MagmaStone;
	}
	public override int[] UpgradeAvailable() => [ItemID.Hellstone, ItemID.LavaCharm];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.LavaCharm);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.Hellstone);
			default:
				return desc;
		}
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.OnFire, ModUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.OnFire, ModUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			if (acc.AugmentUpgrade.Contains(ItemID.LavaCharm)) {
				modifiers.SourceDamage += .2f;
			}
			if (acc.AugmentUpgrade.Contains(ItemID.Hellstone)) {
				modifiers.Knockback += .4f;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
				if (acc.AugmentUpgrade.Contains(ItemID.LavaCharm)) {
					modifiers.SourceDamage += .2f;
				}
				if (acc.AugmentUpgrade.Contains(ItemID.Hellstone)) {
					modifiers.Knockback += .4f;
				}
			}
		}
	}
}
