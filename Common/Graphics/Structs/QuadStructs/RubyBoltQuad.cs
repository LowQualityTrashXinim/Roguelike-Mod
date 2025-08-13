using Microsoft.Xna.Framework;
using Roguelike.Common.Graphics;
using Roguelike.Common.Graphics.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Roguelike.Common.Graphics.Structs.QuadStructs;

public struct RubyBoltQuad 
{

	private static PrimitiveDrawer QuadDrawer = new PrimitiveDrawer(PrimitiveShape.Quad);

	public void Draw(Vector2 position, float rotation, Vector2 size,ShaderSettings shaderSettings) 
	{

		ModdedShaderHandler shader = EffectsLoader.shaderHandlers["RubyBoltEffect"];
		shader.setProperties(shaderSettings);
		shader.apply();


		QuadDrawer.Draw([position], [Color.White], [size], rotation, position);

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}

}
