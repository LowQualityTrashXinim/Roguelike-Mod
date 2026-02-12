using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class AspectOfTheUnderworld : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		textureString = ModUtils.GetTheSameTextureAsEntity<AspectOfTheUnderworld>();
		DataStorer.AddContext("Perk_RingOfFire", new(
			300,
			Vector2.Zero,
			false,
			Color.DarkRed
			));
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, 1.15f, 1, 10);
		if (player.IsHealthAbovePercentage(.66f)) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Flat: -32);
		}
		player.buffImmune[BuffID.OnFire] = true;
	}
	public override void Update(Player player) {
		DataStorer.ActivateContext(player, "Perk_RingOfFire");
		player.Center.LookForHostileNPC(out var npclist, 300);
		for (int i = 0; i < 4; i++) {
			int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(300, 300), 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = -Vector2.UnitY * 4f;
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		if (npclist.Count > 0) {
			foreach (var npc in npclist) {
				npc.AddBuff(BuffID.OnFire, 180);
				if (player.IsHealthAbovePercentage(.67f)) {
					npc.AddBuff(BuffID.OnFire3, 180);
					npc.AddBuff(ModContent.BuffType<TheUnderworldWrath>(), 180);
				}
			}
		}
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.CritDamage *= 1.15f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.CritDamage *= 1.15f;
		}
	}
}
