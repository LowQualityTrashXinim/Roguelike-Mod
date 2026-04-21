using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class SwiftHealth : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		textureString = ModUtils.GetTheSameTextureAsEntity<SwiftHealth>();
	}
	public override void UpdateEquip(Player player) {
		var modplayer = player.GetModPlayer<SwiftHealthIdolPlayer>();
		modplayer.SwiftHealth = true;
		PlayerStatsHandle handler = player.ModPlayerStats();
		int Point = player.GetModPlayer<SwiftHealthIdolPlayer>().Swift_Health_PointCounter;
		handler.UpdateHPMax += .15f + .01f * Point;
		handler.UpdateMovement += .3f + .02f * Point;
		player.GetDamage(DamageClass.Generic).Base += player.statLife / 50;
	}
	public class SwiftHealthIdolPlayer : ModPlayer {
		public bool SwiftHealth = false;
		public bool HasBuff = false;
		public override void ResetEffects() {
			SwiftHealth = false;
			HasBuff = false;
		}
		public int Swift_Health_PointCounter = 0;
		public int Swift_Health_DelayBetweenEachHit = 0;
		public override void PreUpdate() {
			Swift_Health_DelayBetweenEachHit = ModUtils.CountDown(Swift_Health_DelayBetweenEachHit);
			if (HasBuff) {
				Swift_Health_PointCounter = 0;
			}
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			Trinket_of_Swift_Health_OnHitEffect();
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			Trinket_of_Swift_Health_OnHitEffect();
		}
		private void Trinket_of_Swift_Health_OnHitEffect() {
			if (Swift_Health_DelayBetweenEachHit > 0)
				return;
			if (!SwiftHealth)
				return;
			if (HasBuff) {
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
				player.GetModPlayer<SwiftHealthIdolPlayer>().HasBuff = true;
			}
		}
	}
}
