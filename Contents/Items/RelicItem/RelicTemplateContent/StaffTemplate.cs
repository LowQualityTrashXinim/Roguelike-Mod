using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Common.Global.Mechanic.OutroEffect;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent {
	public class StaffTemplate : RelicTemplate {
		public override void SetStaticDefaults() {
			relicType = RelicType.MultiStats;
			RelicTierUPValue = .23f;
		}
		public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.MagicDMG;
		public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
			string Name = Enum.GetName(stat) ?? string.Empty;
			return string.Format(Description, [
					Color.Yellow.Hex3(),
			Name,
			RelicTemplateLoader.RelicValueToPercentage(value.Additive),
		]);
		}

		public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .1f), 2), 1, 0, Main.rand.Next(3, 5));
		}
		public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
			if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.MagicStaff].Contains(player.HeldItem.type)) {
				modplayer.AddStatsToPlayer(stat, value.Additive * 1.22f);
			}
			else {
				modplayer.AddStatsToPlayer(stat, value);

			}
		}
	}
}
