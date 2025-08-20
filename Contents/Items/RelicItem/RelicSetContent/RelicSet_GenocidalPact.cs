using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;

public class GenocidalPact_ModPlayer : ModPlayer {
	public class GenocidalPact : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 3;
		}
	}
	public bool set => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<GenocidalPact>());
	public int KillCount_Decay = 0;
	public int Decay_CoolDown = 0;
	public override void ResetEffects() {
	}
	public override void UpdateEquips() {
		if (!set) {
			return;
		}
		if (KillCount_Decay <= 0) {
			return;
		}
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.FullHPDamage, Additive: 3);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Base: KillCount_Decay * .025f);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, KillCount_Decay * .05f);
		if (++Decay_CoolDown >= 240) {
			KillCount_Decay = Math.Clamp(KillCount_Decay - 1, 0, 5);
			Decay_CoolDown = 0;
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (set) {
			if (KillCount_Decay > 0) {
				damage += KillCount_Decay * .1f;
			}
		}
	}
}
