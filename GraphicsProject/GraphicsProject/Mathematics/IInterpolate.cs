using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Mathematics
{
    /// <summary>
    /// Interpolation contract.
    /// </summary>
    public interface IInterpolate<TValue> :
        IInterpolateLinear<TValue>
    {
    }

    /// <summary>
    /// Linear interpolation contract.
    /// </summary>
    public interface IInterpolateLinear<TValue>
    {
        /// <summary>
        /// Interpolate linearly with given alpha distance [0..1] to other instance.
        /// </summary>
        TValue InterpolateLinear(in TValue other, float alpha);
    }
}
