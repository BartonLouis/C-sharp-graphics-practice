﻿

namespace GraphicsProject.Materials
{
    /// <summary>
    /// Has <see cref="PrimitiveBehaviour"/>.
    /// </summary>
    public interface IHavePrimitiveBehaviour
    {
        /// <inheritdoc cref="PrimitiveBehaviour"/>
        PrimitiveBehaviour PrimitiveBehaviour { get; }
    }
}
