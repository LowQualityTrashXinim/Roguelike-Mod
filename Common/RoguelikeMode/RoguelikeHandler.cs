using Microsoft.Xna.Framework;
using Roguelike.Common.Subworlds;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Systems;

namespace Roguelike.Common.RoguelikeMode;
internal class RoguelikeHandler : ModSystem {
	int Projectile_WhoAmI = -1;
	const int MaxNPCcanBeOnScreen = 50;
	const int SpawnCD = 600;
	const int FailSafe = 9999;
	int CD = 0;
	public override void PreUpdateProjectiles() {
		if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
			return;
		}
		var player = Main.LocalPlayer;
		var system = ModContent.GetInstance<RogueLikeWorldGen>();
		SpawnPortalWithCondition(player, system.CursedKingdomArea.Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_CursedKingdom>());
		SpawnPortalWithCondition(player, system.SlimeWorldEntrance.Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_SlimeWorld>());
		SpawnPortalWithCondition(player, system.FleshRealmEntrance.Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_FleshRealm>());
		SpawnPortalWithCondition(player, system.CorruptionEntrance.Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_Corruption>());
		SpawnPortalWithCondition(player, system.CrimsonEntrance.Center().ToWorldCoordinates(), ModContent.ProjectileType<Portal_Crimson>());
		SpawnPortalWithCondition(player, (system.JungleTempleEntrance.Location + new Point(48, 5)).ToWorldCoordinates(), ModContent.ProjectileType<Portal_JungleTemple>());
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
	private void CursedKingdomSpawn() {
		if (!SubworldSystem.IsActive<CursedKingdomSubworld>()) {
			return;
		}
		int counter = 0;
		for (int i = 0; i < Main.npc.Length; i++) {
			var npc = Main.npc[i];
			if (npc.active) {
				counter++;
			}
			if (counter >= MaxNPCcanBeOnScreen) {
				break;
			}
		}
		if (counter < MaxNPCcanBeOnScreen) {
			if (++CD >= SpawnCD) {
				Point spawnPos;
				int failsafeCheck = 0;
				do {
					failsafeCheck++;
					spawnPos = Main.LocalPlayer.position.ToTileCoordinates() + new Point(Main.rand.Next(-1000, 1000), Main.rand.Next(-500, 500));
				} while (!ModUtils.Check_PositionValid(spawnPos.X, spawnPos.Y) && failsafeCheck < FailSafe);
				if (failsafeCheck >= FailSafe) {
					return;
				}
				CD = Main.rand.Next(120, 240);
				spawnPos = spawnPos.ToWorldCoordinates().ToPoint();
				NPC.NewNPC(Entity.GetSource_NaturalSpawn(), spawnPos.X, spawnPos.Y - 1, NPCID.SkeletonArcher);
				NPC.NewNPC(Entity.GetSource_NaturalSpawn(), spawnPos.X, spawnPos.Y - 1, NPCID.ArmoredSkeleton);
			}
		}
	}
	public override void PreUpdateNPCs() {
		CursedKingdomSpawn();
	}
}
