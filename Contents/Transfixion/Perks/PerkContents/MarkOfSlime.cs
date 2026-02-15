using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class MarkOfSlime : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 5;
		DataStorer.AddContext("Perk_SlimeSpike", new(375, Vector2.Zero, false, Color.Blue));
	}
	public override void UpdateEquip(Player player) {
		player.runSlowdown -= .2f;
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		modplayer.UpdateJumpBoost += 1;
		DataStorer.ActivateContext(player, "Relic_SlimeSpike");
		if (!player.Center.LookForAnyHostileNPC(375f) || modplayer.synchronize_Counter % 15 != 0) {
			return;
		}
		int stack = StackAmount(player);
		int damage = 11 + (int)(player.GetWeaponDamage(player.HeldItem) * .22f);
		for (int i = 0; i < stack; i++) {
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_FromThis(),
				player.Center,
				Main.rand.NextVector2CircularEdge(7, 7),
				ModContent.ProjectileType<FriendlySlimeProjectile>(),
				damage,
				2,
				player.whoAmI);
			proj.Set_ProjectileTravelDistance(375);
			proj.tileCollide = false;
		}
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage -= .15f * StackAmount(player);
	}
}
