using Terraria;
using Terraria.ModLoader;
using Roguelike.Texture;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using System.Collections.Generic;

namespace Roguelike.Contents.Items.Consumable.Scroll;
internal class ScrollOfVulnerable : ModItem {
	public override void SetStaticDefaults() {
		ModItemLib.LootboxPotion.Add(Item);
	}
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.Set_AdvancedBuffItem();
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<Anti_Immunity>(), ModUtils.ToSecond(10));
	}
	public override bool? UseItem(Player player) {
		player.Center.LookForHostileNPC(out List<NPC> npclist, 2000);
		foreach (NPC npc in npclist) {
			npc.AddBuff<Anti_Immunity>(ModUtils.ToSecond(1));
		}
		return true;
	}
}

