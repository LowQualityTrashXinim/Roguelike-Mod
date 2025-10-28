using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode;
internal class RoguelikeCommonItem : GlobalItem {
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		base.ModifyTooltips(item, tooltips);
	}
	public override bool CanUseItem(Item item, Player player) {
		return base.CanUseItem(item, player);
	}
}
