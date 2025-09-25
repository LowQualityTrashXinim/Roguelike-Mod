using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent;
public class DemonScytheTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Projectile;
		DataStorer.AddContext("Relic_DemonScythe", new(
			600,
			Vector2.Zero,
			false,
			Color.MediumPurple
			));
		RelicTierUPValue = .2f;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, 54 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
				Color.MediumPurple.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.ApplyTo(1)),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_DemonScythe");
		if ((modplayer.synchronize_Counter - 50) % 120 != 0) {
			return;
		}
		player.Center.LookForHostileNPC(out NPC npc, 600);
		if (npc == null) {
			return;
		}
		int Tier = relic.RelicTier;
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		for (int i = 0; i < Tier; i++) {
			Vector2 pos = player.Center + Main.rand.NextVector2Circular(590, 590);
			Vector2 vel = (npc.Center - pos).SafeNormalize(Vector2.Zero);
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item, Type.ToString()),
				player.Center + Main.rand.NextVector2Circular(590, 590),
				vel,
				ProjectileID.DemonScythe,
				(int)value.ApplyTo(1),
				4 + .5f * Tier,
				player.whoAmI);
			proj.DamageType = dmgclass;
			proj.tileCollide = false;
			proj.Set_ProjectileTravelDistance(600);
		}
	}
}
