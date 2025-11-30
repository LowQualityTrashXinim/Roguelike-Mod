using Microsoft.Xna.Framework;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.NPCsOverhaul.NPCAIrework;
internal class SkeletronArcher : GlobalNPC {
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.SkeletonArcher && ModContent.GetInstance<RogueLikeWorldGen>().RoguelikeWorld;
	}
	//npc.ai[0] is the counter for the attack
	public override void AI(NPC npc) {
		npc.aiStyle = -1;
		int height = npc.height;
		Vector2 position = npc.position;
		Player player = Main.player[npc.target];
		if (player.position.Y + player.height == position.Y + (float)height)
			npc.directionY = -1;

		int direction = ModUtils.DirectionFromEntityAToEntityB(npc.Center.X, player.Center.X);
		if (npc.Center.IsCloseToPosition(player.Center, 900)) {
			npc.velocity.X *= .98f;
			if (++npc.ai[0] >= 30) {
				npc.ai[0] = 0;
				Vector2 vel = (player.Center - npc.Center).SafeNormalize(Vector2.Zero) * 15;
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel, ProjectileID.WoodenArrowHostile, npc.damage, 3f, npc.target);
			}
			else {
				if(npc.Center.IsCloseToPosition(player.Center, 400))
				npc.velocity.X -= .1f * direction;
			}
		}
		else {
			npc.velocity.X += .1f * direction;
		}
		if (npc.velocity.X > 2) {
			npc.velocity.X = 2;
		}
		if (npc.velocity.X < -2) {
			npc.velocity.X = -2;
		}
	}
	public override bool CanHitNPC(NPC npc, NPC target) {
		return false;
	}
}
