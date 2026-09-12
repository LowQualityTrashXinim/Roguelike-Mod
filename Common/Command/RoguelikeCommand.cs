using Roguelike.Common.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Roguelike.Common.Command;
public class RoguelikeCommand : ModCommand {
	public override string Command => $"{ModMain.Main}:reset Achievement";

	public override CommandType Type => CommandType.Chat;

	public override void Action(CommandCaller caller, string input, string[] args) {
	}
}
