using GraphicsProject.Materials.Position;

namespace GraphicsProject.Materials
{
    /// <summary>
    /// Has <see cref="TMaterial"/>.
    /// </summary>
    public interface IHaveMaterial<out TMaterial>
        where TMaterial : IMaterial
    {
        /// <summary>
        /// <inheritdoc cref="TMaterial"/>
        /// </summary>
        TMaterial Material { get; }
    }
}
