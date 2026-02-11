using Microsoft.Xna.Framework;
using Roguelike.Common.Graphics;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Contents.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Common;
internal class Roguelike_Saber : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return CheckSaber(entity.type);
	}
	public bool CheckSaber(int type) {
		switch (type) {
			case ItemID.PurplePhaseblade:
			case ItemID.BluePhaseblade:
			case ItemID.GreenPhaseblade:
			case ItemID.YellowPhaseblade:
			case ItemID.OrangePhaseblade:
			case ItemID.RedPhaseblade:
			case ItemID.WhitePhaseblade:
			case ItemID.PurplePhasesaber:
			case ItemID.BluePhasesaber:
			case ItemID.GreenPhasesaber:
			case ItemID.YellowPhasesaber:
			case ItemID.OrangePhasesaber:
			case ItemID.RedPhasesaber:
			case ItemID.WhitePhasesaber:
				return true;
		}
		return false;
	}
	public override void SetDefaults(Item entity) {
		entity.shoot = ModContent.ProjectileType<StarWarSwordProjectile>();
		entity.shootSpeed = 1;
		entity.useAnimation = entity.useTime = 15;
		entity.ArmorPenetration = 30;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		var modplayer = player.GetModPlayer<GlobalItemPlayer>();
		if (++modplayer.PhaseSaberBlade_Counter >= 3) {
			modplayer.PhaseSaberBlade_Counter = 0;
			var projectile = Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero) * 1, type, damage, knockback, player.whoAmI);
			if (projectile.ModProjectile is StarWarSwordProjectile starwarProjectile) {
				starwarProjectile.ColorOfSaber = SwordSlashTrail.averageColorByID[item.type] * 2;
				starwarProjectile.ItemTextureID = item.type;
			}
			projectile.width = item.width;
			projectile.height = item.height;
		}
		return false;
	}
}
