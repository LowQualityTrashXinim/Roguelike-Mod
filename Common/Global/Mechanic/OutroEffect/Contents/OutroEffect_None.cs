using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_None : OutroEffect {
	public override void SetStaticDefaults() {
		Duration = 0;
	}
}
