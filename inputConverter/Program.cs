using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inputConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            convertFile("pokus.txt", "converted.txt");
        }

        static void convertFile(string fileName, string outputFile)
        {
            StreamReader sr = new StreamReader(fileName);
            StreamWriter sw = new StreamWriter(outputFile);
            sw.WriteLine("0");
            sw.WriteLine("Grid:");

            string line = sr.ReadLine();
            int width, height;
            parseLine(line, out width, out height);
            width = 2 * width - 1;
            height = 2 * height - 1;
            sw.WriteLine(height + "," + width);

            string[,] map = new string[height, width];

            while((line = sr.ReadLine()) != "X")
            {
                int first, second;
                parseLine(line, out first, out second);
                Point firstNode = translateCoordinates(first, (width+1)/2);
                Point secondNode = translateCoordinates(second, (width + 1) / 2);
                map[firstNode.x, firstNode.y] = ".";
                map[secondNode.x, secondNode.y] = ".";
                Point middleNode = findMiddle(firstNode, secondNode);
                map[middleNode.x, middleNode.y] = ".";
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (map[i,j] == null)
                    {
                        map[i, j] = "@";
                    }
                    sw.Write(map[i, j]);
                }
                sw.WriteLine();
            }

            sw.WriteLine("Agents:");

            List<Agent> agents = new List<Agent>();

            while ((line = sr.ReadLine()) != null)
            {
                int start, end;
                parseLine(line, out end, out start);
                Point startCo = translateCoordinates(start, (width + 1) / 2);
                Point endCo = translateCoordinates(end, (width + 1) / 2);
                agents.Add(new Agent(startCo, endCo));
            }

            sw.WriteLine(agents.Count);
            
            for (int i = 0; i < agents.Count; i++)
            {
                sw.WriteLine("{0},{1},{2},{3},{4}", i, agents[i].start.x, agents[i].start.y, agents[i].end.x, agents[i].end.y);
            }

            sr.Close();
            sw.Close();
        }

        /// <summary>
        /// Parse two integers from line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        static void parseLine(string line, out int first, out int second)
        {
            string[] parts = line.Split(' ');
            first = int.Parse(parts[0]);
            second = int.Parse(parts[1]);
        }

        static Agent parseAgents(string line)
        {
            string[] parts = line.Split(',');
            int firstX = 2 * int.Parse(parts[1]);
            int firstY = 2 * int.Parse(parts[2]);
            int secondX = 2 * int.Parse(parts[3]);
            int secondY = 2 * int.Parse(parts[4]);

            return new Agent(new Point(firstX, firstY), new Point(secondX, secondY));
        }

        /// <summary>
        /// Translates one-dimensional index to two-dimensional coordinates.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="lineWidth"></param>
        /// <returns></returns>
        static Point translateCoordinates(int index, int lineWidth)
        {
            int x = index / lineWidth;
            int y = index % lineWidth;
            return new Point(2*x, 2*y);
        }

        /// <summary>
        /// Finds point between two points (in new map representation).
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        static Point findMiddle(Point first, Point second)
        {
            // x-coordinate is different
            if (first.x != second.x)
            {
                int x = Math.Abs(first.x - second.x) / 2;
                if (first.x < second.x)
                {
                    x += first.x;
                }
                else
                {
                    x += second.x;
                }
                return new Point(x, first.y);
            }
            else
            {
                int y = Math.Abs(first.y - second.y) / 2;
                if (first.y < second.y)
                {
                    y += first.y;
                }
                else
                {
                    y += second.y;
                }
                return new Point(first.x, y);
            }
        }

        
    }

    struct Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;
    }

    struct Agent
    {
        public Agent(Point s, Point e)
        {
            this.start = s;
            this.end = e;
        }

        public Point start;
        public Point end;
    }
}
