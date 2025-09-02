using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.NPCs.TrialNPC.NPCHeldItem;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.TrialNPC;
internal class TrialArcher : BaseTrialNPC {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void TrialNPCDefaults() {
		NPC.lifeMax = 200;
		NPC.damage = 55;
		NPC.defense = 10;
		NPC.friendly = false;
		NPC.width = NPC.height = 30;
		NPC.lavaImmune = true;
		NPC.trapImmune = true;
		NPC.knockBackResist = .35f;
		NPC.color = Color.AliceBlue;
		NPC.GravityMultiplier *= 1.35f;
		ItemTypeHold = ModContent.ProjectileType<Trial_IronBroadsword>();
	}
	public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
		return false;
	}
	Player player;
	bool IsMovingAwayFromPlayer = false;
	public override void TrialAI() {
		if (player == null || player.dead || !player.active) {
			NPC.TargetClosest();
			player = Main.player[NPC.target];
		}
		if(player.Center.IsCloseToPosition(NPC.Center, 200)) {
			IsMovingAwayFromPlayer = false;
		}
		//Check if player is too close, if so move away
		if (player.Center.IsCloseToPosition(NPC.Center, 100f) || IsMovingAwayFromPlayer) {
			IsMovingAwayFromPlayer = true;
			NPC.velocity.X = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero).X * 17.5f;
		}
	}
}
