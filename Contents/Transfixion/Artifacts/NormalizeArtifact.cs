using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike.Contents.Transfixion.Artifacts
{
	internal class NormalizeArtifact : Artifact {
		public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
	}
}
