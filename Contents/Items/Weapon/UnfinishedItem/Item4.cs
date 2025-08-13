using Roguelike.Contents.Items.Weapon;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;

class Item4 : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
}
