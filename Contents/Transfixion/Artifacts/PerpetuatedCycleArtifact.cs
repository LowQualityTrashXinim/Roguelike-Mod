using System;
using Terraria;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArtifactSystem;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class PerpetuatedCycleArtifact : Artifact {
	public override Color DisplayNameColor => Color.Gray;
	public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
}
public class PerpetuatedCyclePlayer : ModPlayer {
	public bool PerpetuationCycle = false;
	public int NPCcounter = 0;
	public int CountDown = 0;
	public override void ResetEffects() {
		PerpetuationCycle = Player.HasArtifact<PerpetuatedCycleArtifact>();
	}
	public override void UpdateEquips() {
		if (PerpetuationCycle) {
			Player.GetModPlayer<PlayerStatsHandle>().DebuffTime += .35f;
		}
	}
	public override void PostUpdate() {
		CountDown = ModUtils.CountDown(CountDown);
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (PerpetuationCycle) {
			modifiers.SourceDamage -= 35f;
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if(PerpetuationCycle) {
			modifiers.SourceDamage -= 35f;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Perpetuation_OnHitNPCEffect(target);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Perpetuation_OnHitNPCEffect(target);
	}
	private void Trinket_of_Perpetuation_OnHitNPCEffect(NPC target) {
		if (PerpetuationCycle) {
			target.AddBuff(ModContent.BuffType<Samsara_of_Retribution>(), ModUtils.ToSecond(1));
		}
	}
	public class Samsara_of_Retribution : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override bool ReApply(NPC npc, int time, int buffIndex) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().Perpetuation_PointStack = Math.Clamp(++npc.GetGlobalNPC<RoguelikeGlobalNPC>().Perpetuation_PointStack, 0, 1000);
			return base.ReApply(npc, time, buffIndex);
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.lifeRegen -= 1 + npc.GetGlobalNPC<RoguelikeGlobalNPC>().Perpetuation_PointStack;
			if (npc.buffTime[buffIndex] <= 0) {
				npc.GetGlobalNPC<RoguelikeGlobalNPC>().Perpetuation_PointStack = 0;
			}
		}
	}
}
