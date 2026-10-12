using System.Collections.Generic;
using System.Drawing;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
internal class Factory_Structure_ModSystem : ModSystem {
	public HashSet<Point> Get_NormalPortal() {
		return [
			new Point(172,96), //normal size portal pos
new Point(149,96),
new Point(135,85),
new Point(148,85),
new Point(111,85),
new Point(125,85),
new Point(108,96),
new Point(67,76),
new Point(81,85),
new Point(100,85),
new Point(92,96),
new Point(68,96),
new Point(51,96),
new Point(34,95),
new Point(13,75),
new Point(13,83),
new Point(15,65),
new Point(45,49),
new Point(70,45),
new Point(90,45),
new Point(129,47),
new Point(70,33),
new Point(78,33),
new Point(85,33),
new Point(121,19),
new Point(135,19),
new Point(144,25),
new Point(144,34),
new Point(144,44),
new Point(103,76),
new Point(114,76),
new Point(92,76),
new Point(125,76),
new Point(136,76),
new Point(146,76),
new Point(69,64),
new Point(51,64),
new Point(191,86),
new Point(178,50),
new Point(158,50),
new Point(158,38),
new Point(178,38),
new Point(178,26),
new Point(158,26),
new Point(161,6),
new Point(135,6),
new Point(112,6),
new Point(86,6),
new Point(61,6),
new Point(37,6),
new Point(13,22),
new Point(13,7),
new Point(20,15),
new Point(6,15),
new Point(185,6),
new Point(192,63),
new Point(197,63),
new Point(157,60),
new Point(161,64),
new Point(164,60),
new Point(191,81),
new Point(191,76),
new Point(27,53)
		];
	}
	public HashSet<Point> Get_BigPortal() {
		return [
			new Point(114,67), //Big size portal
new Point(166,79),
new Point(15,53),
new Point(78,24),
new Point(59,61),
			];
	}
	public HashSet<Point> Get_MediumPortal() {
		return [
			new Point(128,30), //Medium size portal
new Point(136,67),
new Point(92,67),
new Point(169,50),
new Point(169,38),
new Point(169,26),
new Point(50,6),
new Point(74,6),
new Point(99,6),
new Point(124,6),
new Point(149,6),
new Point(174,6),
new Point(103,26),
new Point(128,19),
new Point(130,96)];
	}
	public HashSet<Point> Get_LootPos() {
		return [
			new Point(134,32), //Loot use mod object remember
new Point(134,50),
new Point(69,50),
new Point(89,50),
new Point(102,87),
new Point(98,87),
new Point(94,87),
new Point(90,87),
new Point(86,87),
new Point(82,87),
new Point(78,87),
new Point(131,32),
new Point(137,32),
new Point(131,50),
new Point(128,50),
new Point(149,87),
new Point(134,87),
new Point(126,87),
new Point(110,87),
new Point(4,85),
new Point(7,85),
new Point(10,85),
new Point(5,67),
new Point(8,67),
new Point(11,67)];
	}
	public HashSet<Point> Get_SpecialLootPos() {
		return [new Point(191,65), //special loot
new Point(198,65),
new Point(194,65),
new Point(168,65),
new Point(172,65),
new Point(176,65),];
	}
	public Point Get_PortalPos() => new Point(181, 62);
}
