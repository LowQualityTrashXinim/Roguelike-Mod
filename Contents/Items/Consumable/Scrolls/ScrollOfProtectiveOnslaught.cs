using Terraria;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Consumable.Scrolls;
internal class ScrollOfProtectiveOnslaught : ModItem {
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetStaticDefaults() {
		ModItemLib.LootboxPotion.Add(Item);
	}
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<ProtectiveOnslaught_Buff>(), ModUtils.ToSecond(20));
	}
}
class ProtectiveOnslaught_ModPlayer : ModPlayer {
	public int HitCount = 0;
	public int Delay = 0;
	public override void ResetEffects() {
		if (!Player.HasBuff<ProtectiveOnslaught_Buff>()) {
			HitCount = 0;
		}
		Delay = ModUtils.CountDown(Delay);
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Player.HasBuff<ProtectiveOnslaught_Buff>() && Delay <= 0) {
			HitCount++;
			Delay = 6;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Player.HasBuff<ProtectiveOnslaught_Buff>() && Delay <= 0 && proj.Check_ItemTypeSource(Player.HeldItem.type) && !proj.minion) {
			HitCount++;
			Delay = 6;
		}
	}
}
class ProtectiveOnslaught_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.statDefense += player.GetModPlayer<ProtectiveOnslaught_ModPlayer>().HitCount;
		player.ModPlayerStats().AddStatsToPlayer(PlayerStats.PureDamage, 1 + player.statDefense * .001f);
		if (player.buffTime[buffIndex] <= 0) {
			OnEnded(player);
		}
	}
	public void OnEnded(Player player) {
		player.GetModPlayer<ProtectiveOnslaught_ModPlayer>().HitCount = 0;
		if (player.statLifeMax2 <= player.statDefense) {
			for (int i = 0; i < 300; i++) {
				int smokedust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke);
				Main.dust[smokedust].noGravity = true;
				Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(25, 25);
				Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
				int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(25, 25);
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
			int leftover = player.statDefense - player.statLifeMax2;
			player.Center.LookForHostileNPC(out var npclist, 500);
			foreach (var npc in npclist) {
				player.StrikeNPCDirect(npc, npc.CalculateHitInfo(leftover * 100, player.Center.X > npc.Center.X ? -1 : 1, true, 20, damageVariation: true));
			}
		}
		player.Heal(player.statDefense);
	}
}
