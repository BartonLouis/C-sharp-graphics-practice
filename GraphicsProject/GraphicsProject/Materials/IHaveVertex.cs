using System.Collections.Generic;

namespace GraphicsProject.Materials
{
    /// <summary>
    /// Has <see cref="IReadOnlyList{TVertex}"/>.
    /// </summary>
    public interface IHaveVertices<out TVertex>
    {
        /// <summary>
        /// Collection of <see cref="TVertex"/>.
        /// </summary>
        IReadOnlyList<TVertex> Vertices { get; }
    }
}
