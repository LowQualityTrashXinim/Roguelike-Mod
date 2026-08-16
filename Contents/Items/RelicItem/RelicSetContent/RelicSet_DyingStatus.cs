using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
internal class RelicSet_DyingStatus_ModPlayer : ModPlayer {
	public class RelicSet_DyingStatus : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 3;
		}
	}
	public bool DyingStatus => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<RelicSet_DyingStatus>());
	public override void ResetEffects() {
		if (!DyingStatus) {
			return;
		}
		if (duration > 0) {
			duration--;
			if (duration <= 0) {
				count = 0;
			}
		}
	}
	public override void UpdateEquips() {
		if (DyingStatus) {
			Player.ModPlayerStats().DebuffDamage += .12f;
		}
	}
	int count = 0;
	int duration = 0;
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (!DyingStatus) {
			return;
		}
		bool HasDebuff = false;
		for (int i = 0; i < target.buffType.Length; i++) {
			if (target.buffType[i] <= 0) {
				continue;
			}
			if (Main.debuff[target.buffType[i]]) {
				HasDebuff = true;
				break;
			}
		}
		if (HasDebuff) {
			if (Main.rand.NextBool(4)) {
				modifiers.SourceDamage += 1f + count;
				count = Math.Clamp(count + 1, 0, 3);
				duration = 120;
			}
			for (int i = 0; i < target.buffType.Length; i++) {
				if (target.buffType[i] <= 0 || target.buffTime[i] <= 0) {
					continue;
				}
				target.buffTime[i] += 60;
			}
		}
	}
}
