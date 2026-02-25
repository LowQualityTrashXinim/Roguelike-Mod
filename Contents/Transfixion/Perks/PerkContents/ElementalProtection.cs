using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class ElementalProtection : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		textureString = ModUtils.GetTheSameTextureAsEntity<ElementalProtection>();
		SmallScaleRegardless = true;
	}
	public override void UpdateEquip(Player player) {
		var statplayer = player.GetModPlayer<PlayerStatsHandle>();
		player.buffImmune[BuffID.OnFire] = true;
		player.buffImmune[BuffID.OnFire3] = true;
		player.buffImmune[BuffID.Burning] = true;
		player.buffImmune[BuffID.Frostburn] = true;
		player.buffImmune[BuffID.Frostburn2] = true;
		player.buffImmune[BuffID.Chilled] = true;
		player.buffImmune[BuffID.Frozen] = true;
		player.buffImmune[BuffID.Poisoned] = true;
		player.buffImmune[BuffID.Venom] = true;
		player.buffImmune[BuffID.CursedInferno] = true;
		player.buffImmune[BuffID.Ichor] = true;
		player.buffImmune[BuffID.Electrified] = true;
		player.buffImmune[BuffID.ShadowFlame] = true;
		statplayer.AddStatsToPlayer(PlayerStats.Defense, Additive: 1.1f, Flat: 12);
		player.endurance += .1f;
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
		ElementalExplosion(player, 150);
	}
	public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
		ElementalExplosion(player, 150);
	}
	private void ElementalExplosion(Player player, int damage) {
		player.Center.LookForHostileNPC(out var npclist, 150f);
		foreach (var npc in npclist) {
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
