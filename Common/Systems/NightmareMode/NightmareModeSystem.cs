using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.NightmareMode;
internal class NightmareModeSystem : ModSystem {
	public override void PostUpdateEverything() {
	}
}
public class CelestialPortal : ModObject {
	public override void SetDefaults() {
		timeLeft = ModUtils.ToSecond(20);
	}
	public override void AI() {
		float progress = timeLeft;
		Lighting.AddLight(Center, Color.Red.ToVector3());
		if (progress > ModUtils.ToSecond(19)) {
			return;
		}
		if (progress % 200 == 0) {
			for (int i = 0; i < 100; i++) {
				var dust = Dust.NewDustDirect(Center, 0, 0, DustID.DemonTorch);
				dust.velocity = Main.rand.NextVector2CircularEdge(10, 10);
				dust.noGravity = true;
				dust.scale += 1;
			}
			int NPCToSpawn = Main.rand.Next([
				NPCID.SolarCrawltipedeHead, NPCID.SolarDrakomire, NPCID.SolarDrakomireRider, NPCID.SolarGoop, NPCID.SolarSolenian, NPCID.SolarSpearman, NPCID.SolarSroller,NPCID.SolarCorite,
				NPCID.NebulaBeast, NPCID.NebulaBrain, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier,
				NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexRifleman, NPCID.VortexSoldier,
				NPCID.StardustCellBig, NPCID.StardustJellyfishBig, NPCID.StardustSoldier, NPCID.StardustWormHead,
			]);
			NPC.NewNPC(new EntitySource_WorldGen(), (int)Center.X, (int)Center.Y, NPCToSpawn);
		}
	}
	public override void Draw(SpriteBatch spritebatch) {
		base.Draw(spritebatch);
	}
}
