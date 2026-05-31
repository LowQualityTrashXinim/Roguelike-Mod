using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;
internal class Roguelike_IceBlade : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.IceBlade;
	public override void SetDefaults(Item entity) {
		entity.damage = 39;
		entity.shootsEveryUse = true;
		entity.scale += .5f;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class Roguelike_IceBlade_ModPlayer : ModPlayer {
	public int Counter = 0;
	public override void ResetEffects() {
		base.ResetEffects();
	}
}
public class IceBlade_Slash_Projectile : SimplePiercingProjectile2 {
	public override void OnKill(int timeLeft) {
		int amount = Main.rand.Next(4, 9);
		for (int i = 0; i < amount; i++) {
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(4, 4) * Main.rand.NextFloat(1, 2), ProjectileID.IceSpike, Projectile.damage, Projectile.knockBack, Projectile.owner);
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.maxPenetrate = 1;
			projectile.tileCollide = true;
		}
		Player player = Main.player[Projectile.owner];
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 150);
		foreach (var npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(Projectile.damage * .5f), 1));
		}
	}
}
