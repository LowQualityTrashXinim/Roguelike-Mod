using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class AmplePerceptionArtifact : Artifact {
	public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LightGoldenrodYellow;
}
public class AmplePerceptionPlayer : ModPlayer {
	public bool AmplePerception = false;
	public int PointCounter = 0;
	public int PointTimeLeft = 0;
	public int CountDown = 0;
	public override void ResetEffects() {
		AmplePerception = Player.HasArtifact<AmplePerceptionArtifact>();
		PointTimeLeft = ModUtils.CountDown(PointTimeLeft);
		CountDown = ModUtils.CountDown(CountDown);
	}
	public override void UpdateEquips() {
		if (!AmplePerception) {
			return;
		}
		PlayerStatsHandle statsplayer = Player.ModPlayerStats();
		statsplayer.AddStatsToPlayer(PlayerStats.CritChance, Multiplicative: .75f, Base: 12 + 3 * PointCounter);
		statsplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1 + .06f * PointCounter);
		statsplayer.NonCriticalDamage -= .25f;
	}
	public override void PreUpdate() {
		if (!AmplePerception) {
			return;
		}
		if (PointTimeLeft <= 0 && PointCounter > 0) {
			PointCounter--;
			PointTimeLeft = ModUtils.ToSecond(7);
		}
		for (int i = 0; i < PointCounter; i++) {
			Vector2 pos = Player.Center +
				Vector2.One.Vector2DistributeEvenly(PointCounter, 360, i)
				.RotatedBy(MathHelper.ToRadians(Player.GetModPlayer<ModUtilsPlayer>().counterToFullPi)) * 30;
			int dust = Dust.NewDust(pos, 0, 0, DustID.GemAmber);
			Main.dust[dust].velocity = Vector2.Zero;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].fadeIn = 0;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket3_OnHitNPCEffect(hit);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket3_OnHitNPCEffect(hit);
	}
	private void Trinket3_OnHitNPCEffect(NPC.HitInfo hit) {
		if (!AmplePerception)
			return;
		if (!hit.Crit)
			return;
		if (CountDown > 0)
			return;
		PointCounter = Math.Clamp(++PointCounter, 0, 4);
		PointTimeLeft = ModUtils.ToSecond(7);
		CountDown = ModUtils.ToSecond(2);
	}
}
