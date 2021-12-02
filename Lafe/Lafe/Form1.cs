using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Lafe
{
    public partial class Form1 : Form
    {

        private Graphics graphics;
        private int resoluhen;
        private bool[,] field;
        private int rows;
        private int colum;


        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled)
            {
                return;
            }

            numResoluhen.Enabled = false;
            numDensity.Enabled = false;
            resoluhen = (int)numResoluhen.Value;
            rows = pictureBox1.Height / resoluhen;
            colum = pictureBox1.Width / resoluhen;

            

            field = new bool[colum, rows];
            
            Random random = new Random();

            for (int x = 0; x < colum; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)numDensity.Value) == 0;
                }
            }


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            var newField = new bool[colum, rows];

            for (int x = 0; x < colum; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neightboursCount = CountNeighbors(x, y);
                    var hesLife = field[x,y];

                    if (!hesLife && neightboursCount == 3)
                    {
                        newField[x, y] = true;
                    }
                    else if(hesLife && (neightboursCount < 2 || neightboursCount > 3))
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                    }

                    if(hesLife)
                    {
                        graphics.FillRectangle(Brushes.Crimson, x * resoluhen, y * resoluhen, resoluhen, resoluhen);
                    } 
                }
            }
            field = newField;
            pictureBox1.Refresh();
        }

        private int CountNeighbors(int x, int y)
        {
            int caunt = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + colum) % colum;
                    var row = (y + j + rows) % rows;
                    var isSelfCheking = col == x && row == y;
                    bool hesLife = field[col, row];
                    if (hesLife && !isSelfCheking)
                        caunt++;
                    
                }
            }
            return caunt;
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
            {
                return;
            }
            numResoluhen.Enabled = true;
            numDensity.Enabled = true;
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();

        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    var x = e.Location.X / resoluhen;
                    var y = e.Location.Y / resoluhen;
                    field[x, y] = true;
                }
                if (e.Button == MouseButtons.Right)
                {
                    var x = e.Location.X / resoluhen;
                    var y = e.Location.Y / resoluhen;
                    field[x, y] = false;
                }
            }
            catch
            {
                return;
            }
        }
    }
}
