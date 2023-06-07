using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace GraphicsProject.Mathematics
{
    public enum Space
    {
        World,
        View,
        Screen
    }

    public static class SpaceEx
    {
        public static Space[] SpaceValues { get; } = (Space[])Enum.GetValues(typeof(Space));

        public static int SpaceCount { get; } = SpaceValues.Length;

        public static int[] SpaceIds { get; } = Enumerable.Range(0, SpaceCount).ToArray();

        public static int ToSpaceId(this Space space) => (int)space;

        public static Space ToSpace(this int spaceId) => (Space)spaceId;
    }
}
