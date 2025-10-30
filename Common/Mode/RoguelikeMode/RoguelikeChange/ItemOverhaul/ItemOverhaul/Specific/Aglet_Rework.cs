﻿using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_Aglet : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Aglet;
	}
	public override void UpdateEquip(Item item, Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.DodgeChance += .15f;
		handler.UpdateJumpBoost += .35f;
		if (handler.Get_DidPlayerDodge) {
			player.AddBuff<Aglet_Buff>(ModUtils.ToSecond(30));
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public class Aglet_Buff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void Update(Player player, ref int buffIndex) {
			player.ModPlayerStats().UpdateMovement += .35f;
		}
	}
}
