using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Perks.PerkContents;
public class SelfExplosion : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<SelfExplosion>();
		CanBeStack = true;
		StackLimit = 2;
	}
	public override void OnHitByAnything(Player player) {
		player.Center.LookForHostileNPC(out List<NPC> npclist, 500);
		foreach (NPC npc in npclist) {
			int direction = player.Center.X - npc.Center.X > 0 ? -1 : 1;
			npc.StrikeNPC(npc.CalculateHitInfo((120 + player.statLife) * StackAmount(player), direction, false, 10));
		}
		for (int i = 0; i < 150; i++) {
			int smokedust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke);
			Main.dust[smokedust].noGravity = true;
			Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(500 / 12f, 500 / 12f);
			Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
			int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(500 / 12f, 500 / 12f);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		player.AddBuff(ModContent.BuffType<ExplosionHealing>(), ModUtils.ToSecond(5 + StackAmount(player)));
	}
	class ExplosionHealing : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.lifeRegen += 15;
		}
	}
}
