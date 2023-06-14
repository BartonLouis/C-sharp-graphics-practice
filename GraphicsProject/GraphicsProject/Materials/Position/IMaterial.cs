using System.Drawing;

namespace GraphicsProject.Materials.Position
{
    public interface IMaterial :
        Materials.IMaterial
    {
        /// <summary>
        /// Color to use for rendering.
        /// </summary>
        Color Color { get; }
    }
}
