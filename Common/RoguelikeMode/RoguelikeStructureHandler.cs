using Microsoft.Xna.Framework;
using Roguelike.Contents.Projectiles;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Global;

namespace Roguelike.Common.RoguelikeMode;
internal class RoguelikeStructureHandler : ModSystem {
	int Projectile_WhoAmI = -1;
	public override void PreUpdateProjectiles() {
		if(RoguelikeWorldProperty.BossRushWorld || RoguelikeWorldProperty.TerrariaWorld) {
			return;
		}
		if (SubworldSystem.Current != null) {
			return;
		}
		var player = Main.LocalPlayer;
		var system = ModContent.GetInstance<RogueLikeWorldGen>();
		SpawnPortalWithCondition(player, system.GetStructure("CursedKingdom_Entrance").Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_CursedKingdom>());
		SpawnPortalWithCondition(player, system.GetStructure("SlimeWorld_Entrance").Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_SlimeWorld>());
		SpawnPortalWithCondition(player, system.GetStructure("FleshRealm_Entrance").Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_FleshRealm>());
		SpawnPortalWithCondition(player, system.GetStructure("Corruption_Entrance").Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_Corruption>());
		SpawnPortalWithCondition(player, system.GetStructure("Crimson_Entrance").Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_Crimson>());
		SpawnPortalWithCondition(player, (system.GetStructure("JungleTemple_Entrance").Location + new Point(48, 5)).ToWorldCoordinates(), ModContent.ProjectileType<Portal_JungleTemple>());
	}
	private void SpawnPortalWithCondition(Player player, Vector2 pos, int portalType) {
		if (player.Center.Distance(pos) < 2000) {
			if (Projectile_WhoAmI < 0 || Projectile_WhoAmI > Main.maxProjectiles) {
				Projectile_WhoAmI = Projectile.NewProjectile(null, pos, Vector2.Zero, portalType, 0, 0, player.whoAmI);
			}
			var proj = Main.projectile[Projectile_WhoAmI];
			if (!proj.active || proj.type != portalType) {
				Projectile_WhoAmI = -1;
			}
		}
	}
}
