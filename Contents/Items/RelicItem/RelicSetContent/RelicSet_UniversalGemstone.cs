using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
public class UniversalGemstone_ModPlayer : ModPlayer {
	class UniversalGemstone : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 3;
		}
	}
	public bool set => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<UniversalGemstone>());
	public override void UpdateEquips() {
		if (!set) {
			return;
		}
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MaxHP, Base: 10);
		modplayer.AddStatsToPlayer(PlayerStats.MaxMana, Base: 5);
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 2);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.1f);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.05f);
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, 1.15f);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.2f);
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 3);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.15f);
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.15f);
		modplayer.AddStatsToPlayer(PlayerStats.HealEffectiveness, 1.05f);
		modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.FullHPDamage, 1.35f);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(30) && set) {
			Player.Heal(Main.rand.Next(3, 7));
		}
	}
	public override bool FreeDodge(Player.HurtInfo info) {
		if (!Player.immune && set && Main.rand.NextFloat() <= .025f) {
			Player.AddImmuneTime(info.CooldownCounter, 60);
			Player.immune = true;
			return true;
		}
		return base.FreeDodge(info);
	}
}
