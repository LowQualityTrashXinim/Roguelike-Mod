using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Perks.PerkContents;
public class ElementalProtection : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
		if (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.OnFire3) || player.HasBuff(BuffID.Burning)) {
			statplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: 10);
		}
		if (player.HasBuff(BuffID.Frostburn) || player.HasBuff(BuffID.Frostburn2) || player.HasBuff(BuffID.Chilled) || player.HasBuff(BuffID.Frozen)) {
			statplayer.AddStatsToPlayer(PlayerStats.Defense, Additive: 1.2f);
		}
		if (player.HasBuff(BuffID.Poisoned) || player.HasBuff(BuffID.Venom)) {
			statplayer.AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1.15f);
		}
		if (player.HasBuff(BuffID.CursedInferno)) {
			statplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: 1.2f);
		}
		if (player.HasBuff(BuffID.Ichor)) {
			player.endurance += .1f;
		}
		statplayer.AddStatsToPlayer(PlayerStats.Defense, Additive: 1.1f, Flat: 12);
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
		int min = Math.Min(40, hurtInfo.Damage);
		int max = Math.Max(40, hurtInfo.Damage);
		ElementalExplosion(player, Main.rand.Next(min - 1, max));
	}
	public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
		int min = Math.Min(40, hurtInfo.Damage);
		int max = Math.Max(40, hurtInfo.Damage);
		ElementalExplosion(player, Main.rand.Next(min - 1, max));
	}
	private void ElementalExplosion(Player player, int damage) {
		if (damage <= 0) {
			damage = 1;
		}
		player.Center.LookForHostileNPC(out List<NPC> npclist, 150f);
		foreach (NPC npc in npclist) {
			npc.AddBuff(BuffID.OnFire3, ModUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.Frostburn2, ModUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.Venom, ModUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.CursedInferno, ModUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.Ichor, ModUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.Poisoned, ModUtils.ToSecond(Main.rand.Next(1, 6)));
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, ModUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X), false, 10f));
		}
	}
}
