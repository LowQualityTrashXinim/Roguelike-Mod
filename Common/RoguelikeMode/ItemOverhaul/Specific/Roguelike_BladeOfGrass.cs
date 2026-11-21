using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;
internal class Roguelike_BladeOfGrass : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.BladeofGrass;
	public override void SetDefaults(Item entity) {
		entity.damage = 45;
		entity.scale += .25f;
		entity.useTime = entity.useAnimation = 30;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		return false;
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		int amount = Main.rand.Next(3, 6);
		for (int i = 0; i < amount; i++) {
			Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center, Main.rand.NextVector2CircularEdge(4, 4), item.shoot, item.damage / 4, 1f, player.whoAmI, MathHelper.ToRadians(Main.rand.Next(3, 7)) * Main.rand.NextBool().ToDirectionInt(), Main.rand.Next(4, 8));
		}
	}
}
