using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Labyrinth
{
    class RecursiveSolve
    {
        private int[] horizontalOffset = new int[] { 1, 0, -1, 0 };
        private int[] verticalOffset = new int[] { 0, -1, 0, 1 };
        private LabyrinthLocation[,] locations;
        private bool[,] visited;
        private Graphics canvas;
        private Pen pen;
        private Queue<Point> queue;
        private int sizeLabyrinth;
        private int colIndex;
        private int rowIndex;
        private Point startPoint;
        private Point finishPoint;
        private Point currentPoint;

        public Queue<Point> Queue
        {
            get { return queue; }
        }

        public Point CurrentPoint
        {
            get { return currentPoint; }
        }

        public RecursiveSolve(Point startPoint, Point finishPoint, int sizeLabyrinth)
        {
            this.startPoint = startPoint;
            this.finishPoint = finishPoint;
            this.sizeLabyrinth = sizeLabyrinth;

            currentPoint = startPoint;

            colIndex = currentPoint.X / sizeLabyrinth;
            rowIndex = currentPoint.Y / sizeLabyrinth;
        }

        private bool CanGo(int horizontalOffset, int verticalOffset)
        {
            if (horizontalOffset == -1)
                return !locations[rowIndex, colIndex].LeftWall;
            else if (horizontalOffset == 1)
                return !locations[rowIndex, colIndex + 1].LeftWall;
            else if (verticalOffset == -1)
                return !locations[rowIndex, colIndex].UpWall;
            else
                return !locations[rowIndex + 1, colIndex].UpWall;
        }

        public bool Solve()
        {
            visited[rowIndex, colIndex] = true;
            queue.Enqueue(currentPoint);

            if (currentPoint.X == finishPoint.X && currentPoint.Y == finishPoint.Y)
                return true;

            for (int i = 0; i < 4; i++)
                if (CanGo(horizontalOffset[i], verticalOffset[i]) && !visited[rowIndex + verticalOffset[i], colIndex + horizontalOffset[i]])
                {
                    currentPoint.Offset(horizontalOffset[i] * sizeLabyrinth, verticalOffset[i] * sizeLabyrinth);
                    if (Solve())
                        return true;
                }

            visited[rowIndex, colIndex] = false;
            return false;       
        }
    }
}
