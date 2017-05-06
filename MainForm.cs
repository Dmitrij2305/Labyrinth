using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labyrinth
{
    public partial class MainForm : Form
    {
        private Graphics canvas;
        private Pen pen;
        int labyrinthHeight, labyrinthWidth;
        const int sizeLabyrinth = 50;
        string[] dataLabyrinth;
        LabyrinthLocation[,] locations;
        Point startPoint, finishPoint;

        public MainForm()
        {
            InitializeComponent();

            dataLabyrinth = System.IO.File.ReadAllLines(@"C:\Users\Дмитрий\Мои документы\Visual Studio 2012\Text\Лабиринт.txt");
            labyrinthHeight = int.Parse(dataLabyrinth[0].Split(' ')[1]);
            labyrinthWidth = int.Parse(dataLabyrinth[0].Split(' ')[0]);
            startPoint = new Point(sizeLabyrinth / 2, sizeLabyrinth / 2);
            finishPoint = new Point(labyrinthWidth * sizeLabyrinth - sizeLabyrinth / 2, sizeLabyrinth / 2);
            locations = new LabyrinthLocation[labyrinthHeight + 1, labyrinthWidth + 1];

            canvas = labyrinthPanel.CreateGraphics();
            pen = new Pen(Color.Black, 2);
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            for (int rowIndex = 0; rowIndex <= labyrinthHeight; rowIndex++)
                for (int colIndex = 0; colIndex <= labyrinthWidth; colIndex++)
                {
                    locations[rowIndex, colIndex] = new LabyrinthLocation();

                    if (rowIndex == labyrinthHeight && colIndex < labyrinthWidth)
                        locations[rowIndex, colIndex].UpWall = true;

                    if (rowIndex < labyrinthHeight && colIndex == labyrinthWidth)
                        locations[rowIndex, colIndex].LeftWall = true;

                    if (rowIndex < labyrinthHeight && colIndex < labyrinthWidth)
                    {
                        string[] data = dataLabyrinth[1 + colIndex + labyrinthWidth * rowIndex].Split();
                        locations[rowIndex, colIndex].UpWall = (data[0] == "1");
                        locations[rowIndex, colIndex].LeftWall = (data[1] == "1");
                    }

                    if (locations[rowIndex, colIndex].LeftWall)
                        canvas.DrawLine(pen, colIndex * sizeLabyrinth, rowIndex * sizeLabyrinth,
                            colIndex * sizeLabyrinth, (rowIndex + 1) * sizeLabyrinth);
                    if (locations[rowIndex, colIndex].UpWall)
                        canvas.DrawLine(pen, colIndex * sizeLabyrinth, rowIndex * sizeLabyrinth,
                            (colIndex + 1) * sizeLabyrinth, rowIndex * sizeLabyrinth);
                }

            RecursiveSolve recursiveSolve = new RecursiveSolve(startPoint, finishPoint, sizeLabyrinth);
            if (recursiveSolve.Solve())
            {
                while (recursiveSolve.Queue.Count > 0)
                {
                    Point currentPoint = recursiveSolve.Queue.Dequeue();
                    currentPoint = recursiveSolve.CurrentPoint;
                    canvas.DrawEllipse(pen, currentPoint.X - 5, currentPoint.Y - 5, 10, 10);
                }
            }
        }
    }
}
