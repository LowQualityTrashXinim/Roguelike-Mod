using Microsoft.Xna.Framework;
using Roguelike.Common.Subworlds;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Systems;

namespace Roguelike.Common.Mode.RoguelikeMode;
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
		Player player = Main.LocalPlayer;
		Vector2 CK_pos = ModContent.GetInstance<RogueLikeWorldGen>().CursedKingdomArea.Center().ToWorldCoordinates();
		if (player.Center.Distance(CK_pos) < 2000) {
			if (Projectile_WhoAmI < 0 || Projectile_WhoAmI > Main.maxProjectiles) {
				Projectile_WhoAmI = Projectile.NewProjectile(null, CK_pos, Vector2.Zero, ModContent.ProjectileType<Portal_CursedKingdom>(), 0, 0, player.whoAmI);
			}
			Projectile proj = Main.projectile[Projectile_WhoAmI];
			if (!proj.active || proj.type != ModContent.ProjectileType<Portal_CursedKingdom>()) {
				Projectile_WhoAmI = -1;
			}
		}
		Vector2 Slime_pos = ModContent.GetInstance<RogueLikeWorldGen>().SlimeWorldEntrance.Center().ToWorldCoordinates();
		if (player.Center.Distance(Slime_pos) < 2000) {
			if (Projectile_WhoAmI < 0 || Projectile_WhoAmI > Main.maxProjectiles) {
				Projectile_WhoAmI = Projectile.NewProjectile(null, Slime_pos, Vector2.Zero, ModContent.ProjectileType<Portal_SlimeWorld>(), 0, 0, player.whoAmI);
			}
			Projectile proj = Main.projectile[Projectile_WhoAmI];
			if (!proj.active || proj.type != ModContent.ProjectileType<Portal_SlimeWorld>()) {
				Projectile_WhoAmI = -1;
			}
		}
		Vector2 Flesh_pos = ModContent.GetInstance<RogueLikeWorldGen>().FleshRealmEntrance.Center().ToWorldCoordinates();
		if (player.Center.Distance(Flesh_pos) < 2000) {
			if (Projectile_WhoAmI < 0 || Projectile_WhoAmI > Main.maxProjectiles) {
				Projectile_WhoAmI = Projectile.NewProjectile(null, Slime_pos, Vector2.Zero, ModContent.ProjectileType<Portal_FleshRealm>(), 0, 0, player.whoAmI);
			}
			Projectile proj = Main.projectile[Projectile_WhoAmI];
			if (!proj.active || proj.type != ModContent.ProjectileType<Portal_FleshRealm>()) {
				Projectile_WhoAmI = -1;
			}
		}
		Vector2 Corruption_pos = ModContent.GetInstance<RogueLikeWorldGen>().CorruptionEntrance.Center().ToWorldCoordinates();
		if (player.Center.Distance(Corruption_pos) < 2000) {
			if (Projectile_WhoAmI < 0 || Projectile_WhoAmI > Main.maxProjectiles) {
				Projectile_WhoAmI = Projectile.NewProjectile(null, Slime_pos, Vector2.Zero, ModContent.ProjectileType<Portal_Corruption>(), 0, 0, player.whoAmI);
			}
			Projectile proj = Main.projectile[Projectile_WhoAmI];
			if (!proj.active || proj.type != ModContent.ProjectileType<Portal_Corruption>()) {
				Projectile_WhoAmI = -1;
			}
		}
		Vector2 Crimson_pos = ModContent.GetInstance<RogueLikeWorldGen>().CrimsonEntrance.Center().ToWorldCoordinates();
		if (player.Center.Distance(Crimson_pos) < 2000) {
			if (Projectile_WhoAmI < 0 || Projectile_WhoAmI > Main.maxProjectiles) {
				Projectile_WhoAmI = Projectile.NewProjectile(null, Slime_pos, Vector2.Zero, ModContent.ProjectileType<Portal_Crimson>(), 0, 0, player.whoAmI);
			}
			Projectile proj = Main.projectile[Projectile_WhoAmI];
			if (!proj.active || proj.type != ModContent.ProjectileType<Portal_Crimson>()) {
				Projectile_WhoAmI = -1;
			}
		}
	}
	public override void PreUpdateNPCs() {
		return;
		if (!SubworldSystem.IsActive<CursedKingdomSubworld>()) {
			return;
		}
		int counter = 0;
		for (int i = 0; i < Main.npc.Length; i++) {
			NPC npc = Main.npc[i];
			if (npc.active) {
				counter++;
			}
			if (counter >= MaxNPCcanBeOnScreen) {
				break;
			}
		}
		if (counter < MaxNPCcanBeOnScreen) {
			if (++CD >= SpawnCD) {
				Point spawnPos = Main.LocalPlayer.position.ToPoint() + new Point(Main.rand.Next(-1000, 1000), Main.rand.Next(-500, 500));
				int failsafeCheck = 0;
				do {
					failsafeCheck++;
					spawnPos = Main.LocalPlayer.position.ToTileCoordinates() + new Point(Main.rand.Next(-1000, 1000), Main.rand.Next(-500, 500));
				} while (!ModUtils.Check_PositionValid(spawnPos.X, spawnPos.Y) && failsafeCheck < FailSafe);
				if (failsafeCheck >= FailSafe) {
					return;
				}
				CD = Main.rand.Next(420, 540);
				spawnPos = spawnPos.ToWorldCoordinates().ToPoint();
				NPC.NewNPC(Entity.GetSource_NaturalSpawn(), spawnPos.X, spawnPos.Y - 1, NPCID.SkeletonArcher);
			}
		}
	}
}
