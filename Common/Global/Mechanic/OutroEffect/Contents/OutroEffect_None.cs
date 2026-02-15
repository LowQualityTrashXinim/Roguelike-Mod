using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_None : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = 0;
	}
}
