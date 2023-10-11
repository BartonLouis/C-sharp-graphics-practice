using GraphicsProject.Materials;
using GraphicsProject.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GraphicsProject.Drivers.Gdi.Materials.Position
{
    /// <inheritdoc cref="GfxModel{VsIn,PsIn}"/>
    public class GfxModel :
        GfxModel<VsIn, PsIn>
    {
        /// <inheritdoc cref="IShader"/>
        private Shader Shader { get; }

        /// <summary />
        public GfxModel(RenderHost renderHost, IModel model) :
            base(model, renderHost.ShaderLibrary.ShaderPosition, new BufferBinding(model.Positions))
        {
            Shader = renderHost.ShaderLibrary.ShaderPosition;
        }

        /// <inheritdoc />
        protected override void ShaderUpdate(in Matrix4D matrixToClip)
        {
            Shader.Update(matrixToClip, Model.Color);
        }
    }
}
