using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class SpeedArmor : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<SpeedArmor>();
		CanBeStack = true;
		StackLimit = 2;
	}
	public override void UpdateEquip(Player player) {
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: 1 + .45f * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, Additive: 1 + .2f * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: (int)Math.Round(player.velocity.Length()) * StackAmount(player));
	}
	public override void PostUpdateRun(Player player) {
		player.runAcceleration += .5f;
		player.runSlowdown += .25f;
	}
}
