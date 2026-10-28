using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Accessories;
internal class Roguelike_LuckyCoin : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.LuckyCoin;
	public override void UpdateEquip(Item item, Player player) {
		player.luck += .15f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_LuckyCoin_ModPlayer : ModPlayer {
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (Player.hasLuckyCoin) {
			if (Main.rand.NextBool()) {
				modifiers.SourceDamage *= .05f;
			}
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Player.hasLuckyCoin) {
			if (Main.rand.NextBool()) {
				modifiers.SourceDamage *= .05f;
			}
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (Player.hasLuckyCoin) {
			modifiers.SourceDamage += .5f * Main.rand.NextBool().ToDirectionInt();
			if (Main.rand.NextBool(1000)) {
				modifiers.SourceDamage += 10;
			}
		}
	}
}
