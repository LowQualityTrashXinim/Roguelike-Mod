using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class Dirt : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
		textureString = ModUtils.GetTheSameTextureAsEntity<Dirt>();
	}
	public override void UpdateEquip(Player player) {
		if (--player.GetModPlayer<PerkPlayer>().Dirt_Timer <= 0) {
			player.GetModPlayer<PerkPlayer>().Dirt_Timer = (int)(ModUtils.ToSecond(1) * player.GetModPlayer<PerkPlayer>().Dirt_Multi_CD);
			int stack = StackAmount(player);
			for (int i = 0; i < stack; i++) {
				Vector2 pos = player.Center + Main.rand.NextVector2Circular(100, 100);
				Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 25;
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<DirtProjectile>(), 30 + (int)(player.GetWeaponDamage(player.HeldItem) * .55f), 1f, player.whoAmI);
			}
		}
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
		player.GetModPlayer<PerkPlayer>().Dirt_Multi_CD = .15f;
	}
	public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
		player.GetModPlayer<PerkPlayer>().Dirt_Multi_CD = .15f;
	}
}
