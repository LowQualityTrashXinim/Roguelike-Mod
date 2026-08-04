using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class WindSlash : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<WindSlash>();
		CanBeStack = true;
		StackLimit = 5;
	}
	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) == 2) {
			return DescriptionIndex(2);
		}
		if (StackAmount(Main.LocalPlayer) > 0) {
			return DescriptionIndex(1);
		}
		return base.ModifyToolTip();
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage.Base += 5 * StackAmount(player);
	}
	public override void UpdateEquip(Player player) {
		if (player.HeldItem.CheckUseStyleMelee(ModUtils.MeleeStyle.CheckVanillaSwingWithModded) && player.HeldItem.DamageType == DamageClass.Melee) {
			if (Main.mouseLeft && player.itemAnimation == player.itemAnimationMax) {
				var speed = Vector2.UnitX * player.direction;
				if (player.HeldItem.CheckUseStyleMelee(ModUtils.MeleeStyle.CheckOnlyModded)) {
					speed = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				}
				int damage = (int)(player.HeldItem.damage * (.75f + StackAmount(player) * .25f));
				if (player.GetModPlayer<WindSlash_ModPlayer>().StrikeOpportunity) {
					speed *= 1.5f;
					damage *= 3;
				}
				if (StackAmount(player) >= 3) {
					for (int i = 0; i < 3; i++) {
						Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, speed.Vector2DistributeEvenlyPlus(3, 90, i) * 6, ModContent.ProjectileType<WindSlashProjectile>(), damage, 2f, player.whoAmI);
					}
				}
				else {
					float length = player.HeldItem.Size.Length() * player.GetAdjustedItemScale(player.HeldItem);
					Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center.PositionOFFSET(speed, length + 17), speed * 5, ModContent.ProjectileType<WindSlashProjectile>(), damage, 2f, player.whoAmI);
				}
			}
		}
	}
}
public class WindSlash_ModPlayer : ModPlayer {
	public int OpportunityWindow = 0;
	public bool StrikeOpportunity = false;
	public override void PostUpdate() {
		if (!Player.HasPerk<WindSlash>()) {
			return;
		}
		if (Player.ItemAnimationActive) {
			OpportunityWindow = 0;
			StrikeOpportunity = false;
		}
		if (OpportunityWindow >= ModUtils.ToSecond(1.5f)) {
			if (!StrikeOpportunity) {
				for (int i = 0; i < 36; i++) {
					var vel = Vector2.One.Vector2DistributeEvenlyPlus(36, 360, i) * 5;
					var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.SolarFlare);
					dust.noGravity = true;
					dust.velocity = vel;
				}
			}
			StrikeOpportunity = true;
			return;
		}
		OpportunityWindow++;
	}
}
