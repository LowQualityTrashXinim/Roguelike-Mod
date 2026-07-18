using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;

namespace Roguelike.Common.Systems.ObjectSystem.DataStructures;
public class EntitySource_AccessoryVisual(int accType, Entity entity, string context = null) : EntitySource_Parent(entity, context) {
	public int AccType { get; } = accType;
}
