﻿using GraphicsProject.Materials.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Materials
{
    /// <summary>
    /// Represents graphical primitive.
    /// </summary>
    public interface IPrimitive :
        IHavePrimitiveBehaviour
    {
    }

    /// <summary>
    /// <see cref="IPrimitive"/> which knows about its <see cref="Material"/>.
    /// </summary>
    public interface IPrimitive<out TMaterial> :
        IPrimitive,
        IHaveMaterial<TMaterial>
        where TMaterial : IMaterial
    {
    }

    /// <summary>
    /// <see cref="IPrimitive{TMaterial}"/> which knows about its type of <see cref="TVertex"/>.
    /// </summary>
    public interface IPrimitive<out TMaterial, out TVertex> :
        IPrimitive<TMaterial>,
        IHavePrimitiveTopology,
        IHaveVertices<TVertex>
        where TMaterial : IMaterial
        where TVertex : IVertex
    {
    }
}
