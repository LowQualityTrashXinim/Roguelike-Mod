using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class StealthStrike : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.DarkGray;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.FullHPDamage, 2f);
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 1) {
			if (player.invis)
				modifiers.SourceDamage += .25f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 1) {
			if (proj.Check_ItemTypeSource(player.HeldItem.type)) {
				if (player.invis)
					modifiers.SourceDamage += .25f;
			}
		}
	}
}
