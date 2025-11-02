using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class FrostBurn : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.Cyan;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += "\n" + Description2("1");
					break;
				case 2:
					desc += "\n" + Description2("2");
					break;
				case 3:
				case 4:
				case 5:
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Frostburn, ModUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.Frostburn, ModUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			if (chargeNum >= 1) {
				modifiers.SourceDamage += .2f;
			}
			if (chargeNum >= 2) {
				modifiers.Knockback += .4f;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
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
