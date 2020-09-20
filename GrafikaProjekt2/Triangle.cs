using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrafikaProjekt2
{
    class Triangle
    {
        public Vector3f[] points = new Vector3f[3];
        public Color color;

        public Triangle(Vector3f p1, Vector3f p2, Vector3f p3, Color c)
        {
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            color = c;
        }
    }
}
