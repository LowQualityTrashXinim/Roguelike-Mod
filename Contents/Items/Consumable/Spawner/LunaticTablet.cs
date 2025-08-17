using Roguelike.Common.Global;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Consumable.Spawner {
	internal class LunaticTablet : BaseSpawnerItem {
		public override int[] NPCtypeToSpawn => new int[] { NPCID.CultistBoss };
		public override bool UseSpecialSpawningMethod => true;
		public override void SpecialSpawningLogic(Player player) {
			int spawnY = 250;
			NPC MainNPC = NPC.NewNPCDirect(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)(player.Center.Y - spawnY), NPCtypeToSpawn[0]);
			NPC npc = NPC.NewNPCDirect(player.GetSource_ItemUse(Item), (int)player.Center.X + Main.rand.Next(-100, 100), (int)(player.Center.Y - spawnY + Main.rand.Next(-100, 100)), NPCtypeToSpawn[0]);
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().BelongToWho = MainNPC.whoAmI;
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().IsAGhostEnemy = true;
		}
	}
}
