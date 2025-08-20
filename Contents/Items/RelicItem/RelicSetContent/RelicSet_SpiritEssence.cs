using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
public class SpiritEssence_ModPlayer : ModPlayer {
	class SpiritEssence : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 3;
		}
	}
	public bool set => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<SpiritEssence>());
	public override void UpdateEquips() {
		if (!set) {
			return;
		}
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: 1);
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if ((proj.minion || proj.DamageType == DamageClass.Summon) && target.HasBuff<Crystalized>() && Main.rand.NextBool(3)) {
			modifiers.SourceDamage += .55f;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!set) {
			return;
		}
		if ((proj.minion || proj.DamageType == DamageClass.Summon) && target.HasBuff<Crystalized>() && Main.rand.NextBool(3)) {
			for (int i = 0; i < 3; i++) {
				Projectile direct = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), target.Center, Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.8f, 1.5f), ProjectileID.CrystalShard, hit.Damage / 3 + 1, 2f, Player.whoAmI);
				direct.penetrate = 3;
				direct.maxPenetrate = 3;
				direct.timeLeft = 120;
			}
		}
		if (proj.DamageType == DamageClass.SummonMeleeSpeed) {
			target.AddBuff(ModContent.BuffType<Crystalized>(), ModUtils.ToSecond(5));
		}
	}
	class Crystalized : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;

		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 6;
		}
	}

}
