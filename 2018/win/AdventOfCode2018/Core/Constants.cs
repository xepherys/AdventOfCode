using System.Collections.Generic;

namespace AdventOfCode2018.Core
{
    class GlobalConstants
    {
        static List<Point> fontPointToPixels = new List<Point>() {
          // Format is (X = font size in points, Y = estimated pixels)
            new Point( 6,  8),
            new Point( 8, 11),
            new Point(10, 13),
            new Point(12, 16),
            new Point(14, 19),
            new Point(18, 24)
        };
    }
}
