using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
public class NatureOfGoodAndEvil_ModPlayer : ModPlayer {
	class RelicSet_NatureOfGoodAndEvil : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 4;
		}
	}
	public bool set => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<RelicSet_NatureOfGoodAndEvil>());
	public int Stack = 0;
	public int Decay = 0;
	public override void ResetEffects() {
		if (set) {
			Decay = ModUtils.CountDown(Decay);
			if (Stack > 0 && Decay <= 0) {
				Stack--;
				Decay = 150;
			}
		}
	}
	public override void UpdateEquips() {
		if (Stack <= 0 || !set) {
			return;
		}
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		if (Player.IsHealthAbovePercentage(.5f)) {
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 + Stack * .05f);
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: 1 + Stack * .03f);
		}
		else {
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Additive: 1 + .1f * Stack, Flat: 2 * Stack);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (!set) {
			return;
		}
		if (hit.Crit) {
			Stack = Math.Clamp(Stack + 1, 0, 5);
			Decay = 150;
		}
	}
}
