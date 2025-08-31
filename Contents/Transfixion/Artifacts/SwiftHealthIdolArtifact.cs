using System;
using Terraria;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArtifactSystem;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class SwiftHealthIdolArtifact : Artifact {
	public override Color DisplayNameColor => Color.Lime;
}
public class SwiftHealthIdolPlayer : ModPlayer {
	public bool SwiftHealth = false;
	public override void ResetEffects() {
		SwiftHealth = Player.HasArtifact<SwiftHealthIdolArtifact>();
	}
	public int Swift_Health_PointCounter = 0;
	public int Swift_Health_CoolDown = 0;
	public int Swift_Health_DelayBetweenEachHit = 0;
	public override void PreUpdate() {
		Swift_Health_DelayBetweenEachHit = ModUtils.CountDown(Swift_Health_DelayBetweenEachHit);
		if (!Player.HasBuff(ModContent.BuffType<SwiftSteal_Buff>())) {
			Swift_Health_PointCounter = 0;
			Swift_Health_CoolDown = ModUtils.CountDown(Swift_Health_CoolDown);
		}
	}
	public override void UpdateEquips() {
		if (!SwiftHealth) {
			return;
		}
		Player.ModPlayerStats().AddStatsToPlayer(PlayerStats.MaxHP, 1.05f);
		Player.ModPlayerStats().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.1f);
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Swift_Health_OnHitEffect();
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Swift_Health_OnHitEffect();
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (Player.statLife < 200 && SwiftHealth) {
			damage *= .8f;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (SwiftHealth) {
			if (Player.statLife > 200) {
				modifiers.SourceDamage *= 3;
			}
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (SwiftHealth) {
			if (Player.statLife > 200) {
				modifiers.SourceDamage *= 3;
			}
		}
	}
	private void Trinket_of_Swift_Health_OnHitEffect() {
		if (Swift_Health_DelayBetweenEachHit > 0)
			return;
		if (Swift_Health_CoolDown > 0) {
			return;
		}
		if (!SwiftHealth)
			return;
		if (Player.HasBuff(ModContent.BuffType<SwiftSteal_Buff>())) {
			Swift_Health_DelayBetweenEachHit = ModUtils.ToSecond(1);
			Swift_Health_PointCounter = Math.Clamp(++Swift_Health_PointCounter, 0, 20);
		}
		else {
			Player.AddBuff(ModContent.BuffType<SwiftSteal_Buff>(), ModUtils.ToSecond(30));
		}
	}
	class SwiftSteal_Buff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			int Point = player.GetModPlayer<SwiftHealthIdolPlayer>().Swift_Health_PointCounter;
			PlayerStatsHandle statsplayer = player.ModPlayerStats();
			statsplayer.AddStatsToPlayer(PlayerStats.MaxHP, 1 + .01f * Point);
			statsplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1 + .02f * Point);
			statsplayer.AddStatsToPlayer(PlayerStats.PureDamage, Base: player.statLife * .02f);
			if (player.buffTime[buffIndex] <= 0) {
				player.GetModPlayer<SwiftHealthIdolPlayer>().Swift_Health_CoolDown = ModUtils.ToSecond(60);
			}
		}
	}
}
