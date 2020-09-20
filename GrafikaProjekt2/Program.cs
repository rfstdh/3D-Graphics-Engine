using System;
using MathNet.Numerics.LinearAlgebra;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using MathNet.Numerics.Providers.LinearAlgebra;

namespace GrafikaProjekt2
{
    class Program
    {
        //input types
        enum InputType
        {
            None=0,
            Left,
            Right,
            Up,
            Down,
            Forward,
            Backward,
            RotateX,
            RotateY,
            RotateZ
        }

        //global variables
        static InputType input;
        static Clock clock = new Clock();
        static Time time = clock.ElapsedTime;
        static Mesh toSort = new Mesh();
        static uint windowWidth = 1500;
        static uint windowHeight = 800;

        //main engine
        static void Main(string[] args)
        {

            RenderWindow window = new RenderWindow(new VideoMode(windowWidth, windowHeight),"Okno");
            window.SetFramerateLimit(60);
            window.KeyPressed += onKeyPressed;
            Mesh cube = RenderFromFile(@".\objects\try3.obj");
            Mesh sphere = RenderFromFile(@".\objects\cube.obj");
            Mesh cone = RenderFromFile(@".\objects\stozek.obj");

            while (window.IsOpen)
            {
                window.Clear();
                window.DispatchEvents();

                Mesh cube1;
                Mesh sphere1;
                Mesh cone1;
                toSort.mesh = new List<Triangle>();
                
                cube1 = Move(cube);
                sphere1 = Move(sphere);
                cone1 = Move(cone);
                
                FOVProjection(cube1,window);
                //FOVProjection(sphere1, window);
                //FOVProjection(cone1, window);
                
                SortTriangles(window,toSort);

                window.Display();

                input = InputType.None;
                clock.Restart();
            }
        }

        //load objects to render
        static Mesh RenderFromFile(string path)
        {
            Mesh cubeMesh = new Mesh();
            cubeMesh.mesh = new List<Triangle>();
            const int BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string line;
                List<Vector3f> vertices = new List<Vector3f>();
                while ((line = streamReader.ReadLine()) != null)
                {
                    string [] words = line.Split(' ');
                    if (Convert.ToChar(words[0]) == 'v')
                    {
                        float w1 = float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat);
                        float w2 = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
                        float w3 = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
                        
                        Vector3f v = new Vector3f(w1,w2,w3+3);
                        vertices.Add(v);
                    }
                    if (Convert.ToChar(words[0]) == 'f')
                    {
                        Triangle t = new Triangle(vertices[int.Parse(words[1]) - 1], vertices[int.Parse(words[2]) - 1], vertices[int.Parse(words[3]) - 1],Color.White);
                        cubeMesh.mesh.Add(t);
                    }
                }
            }
            return cubeMesh;
        }

        //handle key strokes
        static void onKeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.A)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.Left;
            }
            if (e.Code == Keyboard.Key.D)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.Right;
            }
            if (e.Code == Keyboard.Key.W)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.Up;
            }
            if (e.Code == Keyboard.Key.S)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.Down;
            }
            if (e.Code == Keyboard.Key.Z)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.Forward;
            }
            if (e.Code == Keyboard.Key.X)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.Backward;
            }
            if (e.Code == Keyboard.Key.M)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.RotateX;
            }
            if (e.Code == Keyboard.Key.N)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.RotateZ;
            }
            if (e.Code == Keyboard.Key.B)
            {
                Console.WriteLine("Pressed: " + e.Code);
                input = InputType.RotateY;
            }

        }

        //handle basic movement(up,down,lef,right,back,front)
        static Mesh Move(Mesh cube)
        {
            float moveSpeed = 460.0f;
            float rotSpeed = 30.0f;
            
            for (int i = 0; i < cube.mesh.Count; i++)
            {
                switch (input)
                {
                    case InputType.None:
                        break;
                    case InputType.Left:
                        cube.mesh[i].points[0].X -= moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[1].X -= moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[2].X -= moveSpeed * time.AsSeconds();
                        break;
                    case InputType.Right:
                        cube.mesh[i].points[0].X += moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[1].X += moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[2].X += moveSpeed * time.AsSeconds();
                        break;
                    case InputType.Up:
                        cube.mesh[i].points[0].Y -= moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[1].Y -= moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[2].Y -= moveSpeed * time.AsSeconds();
                        break;
                    case InputType.Down:
                        cube.mesh[i].points[0].Y += moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[1].Y += moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[2].Y += moveSpeed * time.AsSeconds();
                        break;
                    case InputType.Forward:
                        cube.mesh[i].points[0].Z -= moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[1].Z -= moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[2].Z -= moveSpeed * time.AsSeconds();
                        break;
                    case InputType.Backward:
                        cube.mesh[i].points[0].Z += moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[1].Z += moveSpeed * time.AsSeconds();
                        cube.mesh[i].points[2].Z += moveSpeed * time.AsSeconds();
                        break;
                    case InputType.RotateX:
                        cube = Rotate(cube, InputType.RotateX, rotSpeed * time.AsSeconds());
                        break;
                    case InputType.RotateZ:
                        cube = Rotate(cube, InputType.RotateZ, rotSpeed * time.AsSeconds());
                        break;
                    case InputType.RotateY:
                        cube = Rotate(cube, InputType.RotateY, rotSpeed * time.AsSeconds());
                        break;
                    default:
                        break;
                }
            }
            
            return cube;
        }

        //project to the screen
        static void FOVProjection(Mesh cubeMesh, RenderWindow window)
        {
            //variables
            float fNear = 0.1f;
            float fFar = 800.0f;
            float fFov = 60.0f;
            float aspect = windowHeight / (float)windowWidth;
            //field of view radius
            float fFovRad = 1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * Math.PI);

            Matrix<float> matrix = Matrix<float>.Build.Dense(4, 4);


            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    matrix[i, j] = 0;
                }
            }

            //perspective matrix
            matrix[0, 0] = aspect * fFovRad;
            matrix[1, 1] = fFovRad;
            matrix[2, 2] = fFar / (fFar - fNear);
            matrix[2, 3] = 1.0f;
            matrix[3, 2] = (-fFar * fNear) / (fFar - fNear);
            matrix[3, 3] = 0.0f;



            foreach (var triangle in cubeMesh.mesh)
            {

                //normal vectors for each triangle
                Vector3f normal = new Vector3f(0, 0, 0);
                Vector3f line1 = new Vector3f(0, 0, 0);
                Vector3f line2 = new Vector3f(0, 0, 0);

                line1.X = triangle.points[1].X - triangle.points[0].X;
                line1.Y = triangle.points[1].Y - triangle.points[0].Y;
                line1.Z = triangle.points[1].Z - triangle.points[0].Z;

                line2.X = triangle.points[2].X - triangle.points[0].X;
                line2.Y = triangle.points[2].Y - triangle.points[0].Y;
                line2.Z = triangle.points[2].Z - triangle.points[0].Z;

                normal.X = line1.Y * line2.Z - line1.Z * line2.Y;
                normal.Y = line1.Z * line2.X - line1.X * line2.Z;
                normal.Z = line1.X * line2.Y - line1.Y * line2.X;

                float length = MathF.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);

                normal.X /= length;
                normal.Y /= length;
                normal.Z /= length;


                //check if wall is visible
                if (((normal.X * triangle.points[0].X) + (normal.Y * triangle.points[0].Y) + (normal.Z * triangle.points[0].Z)) < 0.0f)
                {

                    //check if not inside cube,sphere,cone etc...
                    if (triangle.points[0].Z > 0.0f && triangle.points[1].Z > 0.0f && triangle.points[2].Z > 0.0f)
                    {
                        //light source
                        Vector3f light_direction = new Vector3f(-4.23f, -2.11f, -1.0f);
                        float l = MathF.Sqrt(light_direction.X * light_direction.X + light_direction.Y * light_direction.Y + light_direction.Z * light_direction.Z);
                        light_direction.X /= l;
                        light_direction.Y /= l;
                        light_direction.Z /= l;

                        //Similarity between normal and light direction(should be values from 0 to 1)
                        float dp = normal.X * light_direction.X + normal.Y * light_direction.Y + normal.Z * light_direction.Z;

                        dp = Math.Max(0, dp * 255);

                        Color c = new Color(20, Convert.ToByte(dp), Convert.ToByte(dp));

                        Vector3f px = MultiplyVectors(triangle.points[0], matrix);
                        Vector3f py = MultiplyVectors(triangle.points[1], matrix);
                        Vector3f pz = MultiplyVectors(triangle.points[2], matrix);

                        Triangle projectedTriangle = new Triangle(px, py, pz, c);
                        toSort.mesh.Add(projectedTriangle);
                    }
                }
            }
        }

        //applying painter's algorithm
        static void SortTriangles(RenderWindow window, Mesh mesh)
        {
            for (int i = 0; i < mesh.mesh.Count-1; i++)
            {
                for (int j = i+1; j < mesh.mesh.Count; j++)
                {
                    float z1 = (mesh.mesh[i].points[0].Z + mesh.mesh[i].points[1].Z + mesh.mesh[i].points[2].Z) / 3;
                    float z2 = (mesh.mesh[j].points[0].Z + mesh.mesh[j].points[1].Z + mesh.mesh[j].points[2].Z) / 3;

                    if (z1 > z2)
                    {
                        Triangle tmp = mesh.mesh[i];
                        mesh.mesh[i] = mesh.mesh[j];
                        mesh.mesh[j] = tmp;
                    }
                }
            }
            foreach (var projectedTriangle in mesh.mesh)
            {
                float i1x = (projectedTriangle.points[0].X + 1) * (windowWidth / 2);
                float i1y = (projectedTriangle.points[0].Y + 1) * (windowHeight / 2);

                float i2x = (projectedTriangle.points[1].X + 1) * (windowWidth / 2);
                float i2y = (projectedTriangle.points[1].Y + 1) * (windowHeight / 2);

                float i3x = (projectedTriangle.points[2].X + 1) * (windowWidth / 2);
                float i3y = (projectedTriangle.points[2].Y + 1) * (windowHeight / 2);

                DrawVertex(window, i1x, i1y, i2x, i2y, i3x, i3y, projectedTriangle.color);
                DrawContours(window, i1x, i1y, i2x, i2y, i3x, i3y, Color.White);
            }
        }

        //handle rotation(X,Y,Z)
        static Mesh Rotate(Mesh cubeMesh, InputType rotationType, double angle)
        {
            
            switch (rotationType)
            {
                case InputType.RotateX:
                    //rotationX matrix
                    Matrix<float> matrixZ = Matrix<float>.Build.Dense(4, 4);

                    for (int r = 0; r < matrixZ.RowCount; r++)
                    {
                        for (int c = 0; c < matrixZ.ColumnCount; c++)
                        {
                            matrixZ[r, c] = 0;
                        }
                    }

                    matrixZ[0, 0] = (float)Math.Cos(angle * 0.5f);
                    matrixZ[0, 1] = (float)Math.Sin(angle * 0.5f);
                    matrixZ[1, 0] = (float)-Math.Sin(angle * 0.5f);
                    matrixZ[1, 1] = (float)Math.Cos(angle * 0.5f);
                    matrixZ[2, 2] = 1;
                    matrixZ[3, 3] = 1;
                    foreach (var triangle in cubeMesh.mesh)
                    {
                        triangle.points[0] = MultiplyVectors(triangle.points[0], matrixZ);
                        triangle.points[1] = MultiplyVectors(triangle.points[1], matrixZ);
                        triangle.points[2] = MultiplyVectors(triangle.points[2], matrixZ);
                    }

                        break;
                case InputType.RotateY:
                    //rotationY matrix
                    Matrix<float> matrixY = Matrix<float>.Build.Dense(4, 4);

                    for (int r = 0; r < matrixY.RowCount; r++)
                    {
                        for (int c = 0; c < matrixY.ColumnCount; c++)
                        {
                            matrixY[r, c] = 0;
                        }
                    }

                    matrixY[0, 0] = (float)Math.Cos(angle);
                    matrixY[0, 2] = (float)-Math.Sin(angle);
                    matrixY[1, 1] = 1;
                    matrixY[2, 0] = (float)Math.Sin(angle);
                    matrixY[2, 2] = (float)Math.Cos(angle);
                    matrixY[3, 3] = 1;

                    foreach (var triangle in cubeMesh.mesh)
                    {
                        triangle.points[0] = MultiplyVectors(triangle.points[0], matrixY);
                        triangle.points[1] = MultiplyVectors(triangle.points[1], matrixY);
                        triangle.points[2] = MultiplyVectors(triangle.points[2], matrixY);
                    }
                    break;
                case InputType.RotateZ:
                    //rotationZ matrix
                    Matrix<float> matrixX = Matrix<float>.Build.Dense(4, 4);

                    for (int r = 0; r < matrixX.RowCount; r++)
                    {
                        for (int c = 0; c < matrixX.ColumnCount; c++)
                        {
                            matrixX[r, c] = 0;
                        }
                    }

                    matrixX[0, 0] = 1;
                    matrixX[1, 1] = (float)Math.Cos(angle);
                    matrixX[1, 2] = (float)Math.Sin(angle);
                    matrixX[2, 1] = (float)-Math.Sin(angle);
                    matrixX[2, 2] = (float)Math.Cos(angle);
                    matrixX[3, 3] = 1;

                    foreach (var triangle in cubeMesh.mesh)
                    {
                        triangle.points[0] = MultiplyVectors(triangle.points[0], matrixX);
                        triangle.points[1] = MultiplyVectors(triangle.points[1], matrixX);
                        triangle.points[2] = MultiplyVectors(triangle.points[2], matrixX);
                    }
                    break;
                default:
                    break;
            }

            return cubeMesh;
        }
        
        //helper function to multiply two vectors
        static Vector3f MultiplyVectors(Vector3f v, Matrix<float> matrix)
        {
            Vector3f output = new Vector3f(0, 0, 0);
            //Console.WriteLine("{0}, {1}:", resX, resY);
            output.X = v.X * matrix[0, 0] + v.Y * matrix[1, 0] + v.Z * matrix[2, 0] + matrix[3, 0];
            output.Y = v.X * matrix[0, 1] + v.Y * matrix[1, 1] + v.Z * matrix[2, 1] + matrix[3, 1];
            output.Z = v.X * matrix[0, 2] + v.Y * matrix[1, 2] + v.Z * matrix[2, 2] + matrix[3, 2];

            float w = v.X * matrix[0, 3] + v.Y * matrix[1, 3] + v.Z * matrix[2, 3] + matrix[3, 3];

            if (w != 0)
            {
                output.X /= w;
                output.Y /= w;
                output.Z /= w;
            }
            return output;
        }

        //draw filled triangles
        static void DrawVertex(RenderWindow window, float x1, float y1, float x2, float y2, float x3, float y3, Color color)
        {
            VertexArray va = new VertexArray(PrimitiveType.Triangles, 3);

            va.Append(new Vertex(new Vector2f(x1, y1), color));
            va.Append(new Vertex(new Vector2f(x2, y2), color));
            va.Append(new Vertex(new Vector2f(x3, y3), color));
            window.Draw(va);
        }

        //draw contour lines
        static void DrawContours(RenderWindow window, float x1, float y1, float x2, float y2, float x3, float y3, Color color)
        {
            VertexArray va = new VertexArray(PrimitiveType.Lines, 6);
            va.Append(new Vertex(new Vector2f(x1, y1), color));
            va.Append(new Vertex(new Vector2f(x2, y2), color));
            va.Append(new Vertex(new Vector2f(x2, y2), color));
            va.Append(new Vertex(new Vector2f(x3, y3), color));
            va.Append(new Vertex(new Vector2f(x3, y3), color));
            va.Append(new Vertex(new Vector2f(x1, y1), color));

            window.Draw(va);
        }

    }
}
