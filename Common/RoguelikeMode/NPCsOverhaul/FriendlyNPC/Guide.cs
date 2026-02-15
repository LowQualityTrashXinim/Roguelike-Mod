using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.NPCsOverhaul.FriendlyNPC;
internal class Guide : GlobalNPC {
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Guide;
	}
	public override void GetChat(NPC npc, ref string chat) {
		base.GetChat(npc, ref chat);
	}
	public override void OnChatButtonClicked(NPC npc, bool firstButton) {
		base.OnChatButtonClicked(npc, firstButton);
	}
}
