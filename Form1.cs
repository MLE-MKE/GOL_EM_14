using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace GOL_EM_14
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[5, 5];
        bool[,] scratchPad = new bool[5, 5];


        //Declaring variables for Next Generation
        private int aliveCells = 0;
        private int checkAlive;

        //bool to check cells current state
        //default to false? 
        private bool checkedState = false;


        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 1000; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLength = universe.GetLength(0);
            int yLength = universe.GetLength(1);

            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }


                    // if xCheck is less than 0 then continue
                    if (xOffset < 0)
                    {
                        continue;

                    }

                    // if yCheck is less than 0 then continue
                    if (yOffset < 0)
                    {
                        continue;

                    }

                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLength)
                    {
                        continue;
                    }

                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLength)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }
                }

            }

            return count;

            
        }

        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLength = universe.GetLength(0);
            int yLength = universe.GetLength(1);
            

            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 & yOffset == 0)
                    {
                        continue;
                    }

                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xCheck = universe.GetLength(0)-1;
                    }

                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = universe.GetLength(1) - 1;
                    }

                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLength)
                    {
                        xCheck = 0;
                    }

                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLength)
                    {
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true)
                     count++; 
                   

                  
                }
            }

            return count;
        }
        private void NextGeneration()
        {
            bool[,] checkArray1 = new bool[this.universe.GetLength(0), this.universe.GetLength(1)];
            this.aliveCells = 0;

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    //this.checkAlive = !this.checkedstate ? this.CountNeighborsFinite(x, y) : this.GetTorodial(x, y);
                    int neighborcount = CountNeighborsFinite(x, y);

                    if (universe[x, y])
                    {


                        //dead cells
                        if (neighborcount > 2)
                        {
                            scratchPad[x, y] = false;
                        }

                        if (neighborcount < 3)
                        {
                            scratchPad[x, y] = false;
                        }

                        //living cells

                        if (neighborcount == 3 || neighborcount == 2)
                        {
                            scratchPad[x, y] = true;
                        }



                        universe[x, y] = !universe[x, y];
                    }
                }

                universe = scratchPad;
            }


            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
            graphicsPanel1.Invalidate();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            float cellWidthF;
            cellWidthF = (float)cellWidth;
            
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);
            float cellHeightF;
            cellHeightF = (float) cellHeight;

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    Rectangle cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
                graphicsPanel1.Invalidate();
            }
        }




        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            graphicsPanel1.Invalidate();
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            NextGeneration();
            graphicsPanel1.Invalidate();
        }

        private void colorDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            //data exchange (properties)

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog()) 
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();

        }

        private void modalDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModalDialog dlg = new ModalDialog();

            //if (DialogResult.OK == dlg.GetType))
            //{

            //}
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            //data exchange (properties)

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            //data exchange (properties)

            dlg.Color = cellColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files|*.*|Cells|*.cells";
            ofd.FilterIndex = 2;

            if (DialogResult.OK != ofd.ShowDialog())
                return;
            StreamReader streamReader = new StreamReader(ofd.FileName);
            int len1 = 0;
            int len2 = 0;
            //use dictionary search snippet
            while (!streamReader.EndOfStream)
            {
                string myStream = streamReader.ReadLine();
                if (myStream[0] != '!')
                {
                    ++len2;
                    if (myStream.Length > len1)
                        len1 = myStream.Length;
                    
                }

            }

            this.universe = new bool[len1, len2];
            streamReader.BaseStream.Seek(0L, SeekOrigin.Begin);
            int index = 0;
            while(!streamReader.EndOfStream)
            {
                string str = streamReader.ReadLine();
                if (str[0] != '!')
                {
                    ++len2;
                    if (str.Length > len1)
                        len1 = str.Length;    
                }
                for (int index2 = 0; index2 < str.Length; index2++)
                {
                    if (str[index2] == '0')
                        this.universe[index2, index] = true;
                    if (str[index2] == '.')
                        this.universe[index2, index] = false;
                    this.graphicsPanel1.Invalidate();
                  
                }
                ++index;
            }
            //DO I need the close? or will the invalidate?? 
            streamReader.Close();
            //if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    if ((myStream = ofd.OpenFile()) !=null)
            //    {
            //        string strfilename = ofd.FileName;
            //        string filetext = File.ReadAllText(strfilename);
            //        toolStripTextBox1.Text = filetext;
            //    }
            //    MessageBox.Show(ofd.FileName);
            //}
            ofd.ShowDialog();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = File.Open(sfd.FileName, FileMode.CreateNew))
                    using(StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(toolStripTextBox1.Text);
                }
            }


        }
    }
}
