﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_WarAxeoftheNight : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.WarAxeoftheNight;
	}
	public override void SetDefaults(Item entity) {
		entity.scale += .25f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.WarAxeoftheNight) {
			ModUtils.AddTooltip(ref tooltips, new(Mod, item.type + "_Rework",
				"\"Night full of scream and blood\""));
		}
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.HasBuff<NightInfection>()) {
			target.DelBuff(target.FindBuffIndex(ModContent.BuffType<NightInfection>()));
			var vec = Vector2.UnitX;
			for (int i = 0; i < 3; i++) {
				var vec2 = vec.Vector2DistributeEvenly(3, 360, i);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center, vec2, ProjectileID.ShadowFlame, item.damage, 3f, player.whoAmI, Main.rand.NextFloat(-.05f, .05f), Main.rand.NextFloat(-.1f, .1f));
			}
		}
		if (Main.rand.NextBool(10)) {
			target.AddBuff<NightInfection>(ModUtils.ToSecond(10));
		}
	}
	public override void MeleeEffects(Item item, Player player, Rectangle hitbox) {
		base.MeleeEffects(item, player, hitbox);
	}
}
public class NightInfection : ModBuff {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.DemonTorch);
		dust.noGravity = true;
		dust.scale = Main.rand.NextFloat(1, 1.25f);
		dust.velocity = -Vector2.UnitY * Main.rand.NextFloat(0.5f, 1.5f);
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().VelocityMultiplier -= .2f;
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 20;
		npc.lifeRegen -= 10;
	}
}
