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
        bool[,] universe = new bool[50, 50];
        bool[,] scratchPad = new bool[50,50];

        //creating an enum for swtich statement for different boundary types 
        public enum BoundaryType1
        {
            Finite,
            Torodial,
            Infinite
        }

        //Declaring variables for Next Generation
        
        private int aliveCells = 0;
        private int checkAlive;

        // Declaring other variable I think I have to declare?
        private ToolStripMenuItem randomizeUniverseSeedToolStripMenuItem;

        //bool to check cells current state
        //default to false? 
        private bool checkedState = false;
        private int seed = 0;

        //cell structre for bas of game 
        public struct Cell
        {
            private bool alive;

            public Cell(bool alive) => this.alive = alive;

            public bool Alive
            {
                get => this.alive;
                set => this.alive = value;
            }

            public bool Dead
            {
                get => !this.alive;
                set => this.alive = !value;
            }


        }

        private Label CellCount;
        private Label BoundarySize;
        private Label BoundaryType;
        private Label UniverseSize;
        private Label Label;

        private ToolStripStatusLabel CellsAlive;
        private Label label1;

        //variable settings

        private ToolStripMenuItem fromTimeToolStripMenuItem;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;
        private int y;

        public Form1()
        {
            InitializeComponent();
            NextGeneration();
            

            // Setup the timer
            timer.Interval = 1000; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running

            this.turnGridOnoffToolStripMenuItem = new ToolStripMenuItem();

            this.boundariesToolStripMenuItem = new ToolStripMenuItem();
            graphicsPanel1.Invalidate();
        }
     
        private void NextGeneration()
        {
            //create a bool?
            //or should I use the scratch pad?
            

            aliveCells = 0;
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                for (int y = 0; y < universe.GetLength(1); y++)
                //? is an if : is an else 
                //ternary operator 

                    //after the for loops cop what is in the scratch pad to the universe
                {
                    this.checkAlive = !checkedState ? countNeighbor(x, y) : GetToroidal(x, y);
                    if (checkAlive < 2 || checkAlive > 3)
                        scratchPad[x, y] = false;
                    if ((checkAlive == 2 || checkAlive == 3)&& universe[x, y])
                        scratchPad[x, y] = true;
                    if (checkAlive == 3 && !universe[x, y])
                        scratchPad[x, y] = true;
                    if (scratchPad[x, y])
                        ++aliveCells;

                  

                }
            }
            
            universe = scratchPad;
            graphicsPanel1.Invalidate();

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            
        }
        public int CountNeighbor(int x, int y)
        {
            int num = 0;
            //using same logic as next gen

            //x Get Length (0)
            //y Get Length (1)
           
            if (x - 1 >= 0 && y - 1 >= 0 && universe[x-1, y-1])
                ++num;
            if (x - 1 >= 0 && universe[x - 1, y])
                ++num;
            if (x - 1 >= 0 && y + 1 < universe.GetLength(1) && universe[x - 1, y + 1])
                ++num;
            if (x + 1 < universe.GetLength(0) && y - 1 >= 0 && universe[x + 1, y - 1])
                ++num;
            if (x + 1 < universe.GetLength(0) && universe[x + 1, y])
                ++num;
            if (x + 1 < universe.GetLength(0) && y + 1 < universe.GetLength(1) && universe[x + 1, y + 1])
                ++num;
            if (y + 1 < universe.GetLength(1) && universe[x, y + 1])
                ++num;
            if (y - 1 < universe.GetLength(1) && universe[x, y - 1])
                ++num;
            return num;
            graphicsPanel1.Invalidate();

        }
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
            graphicsPanel1.Invalidate();




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
                        xCheck = universe.GetLength(0) - 1;
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
            graphicsPanel1.Invalidate();


        }
        //create somthing to account for the cells being alive or dead


        //** Do I even need to get or set**


        //public Cell this[int x, int y]
        //{
        //    get => this.universe[x, y];
        //    //can I just set somthign without getting it?
        //    set
        //    {
        //        if (value.Alive & this.universe[x, y])
        //            ++aliveCells;
        //        else if (value.Dead & this.universe[x, y])
        //            --this.aliveCells;
        //        this.universe[x, y] = value;
        //    }
        //}   
            
            
       


        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();

            //Do I need to invalidate here?

            graphicsPanel1.Invalidate();
            
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            //convert to float
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);
            ////float num1 = (float)this.graphicspanel1.clientsize.width / (float)this.universe.getlength(0);
            ////float num2 = (float)this.graphicspanel1.clientsize.height / (float)this.universe.getlength(1);
           
            //float cellWidthF;
           

            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            
            //float cellHeightF;
           

            // A Pen for drawing the grid lines (color, width)
          
            Pen gridPen = new Pen(gridColor, 1);
            Pen pen1 = new Pen(this.gridColor, 1f);
            Pen pen2 = new Pen(this.gridColor, 2f);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = (RectangleF)Rectangle.Empty;
                    cellRect.X = (float)x * cellWidth;
                    cellRect.Y = (float)y * cellHeight;
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

            #region Original Code
            //// Calculate the width and height of each cell in pixels
            //// CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            ////convert to float

            //float num1 = (float)this.graphicsPanel1.ClientSize.Width / (float)this.universe.GetLength(0);
            //float num2 = (float)this.graphicsPanel1.ClientSize.Height / (float)this.universe.GetLength(1);
            //int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            //float cellWidthF;
            //cellWidthF = (float)cellWidth;

            //// CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            //int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);
            //float cellHeightF;
            //cellHeightF = (float) cellHeight;

            //// A Pen for drawing the grid lines (color, width)
            //Pen pen1 = new Pen(this.gridColor, 1f);
            //Pen pen2 = new Pen(this.gridColor, 2f);
            //Pen gridPen = new Pen(gridColor, 1);

            //// A Brush for filling living cells interiors (color)
            //Brush cellBrush = new SolidBrush(cellColor);

            //// Iterate through the universe in the y, top to bottom
            //for (int y = 0; y < universe.GetLength(1); y++)
            //{
            //    // Iterate through the universe in the x, left to right
            //    for (int x = 0; x < universe.GetLength(0); x++)
            //    {
            //        // A rectangle to represent each cell in pixels
            //        RectangleF cellRect = (RectangleF) Rectangle.Empty;
            //        cellRect.X = (float) x * num1;
            //        cellRect.Y = (float) y * num2;
            //        cellRect.X = x * cellWidth;
            //        cellRect.Y = y * cellHeight;
            //        cellRect.Width = cellWidth;
            //        cellRect.Height = cellHeight;


            //        // Fill the cell with a brush if alive
            //        if (universe[x, y] == true)
            //        {
            //            e.Graphics.FillRectangle(cellBrush, cellRect);
            //        }

            //        // Outline the cell with a pen
            //        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
            //    }
            //}

            //// Cleaning up pens and brushes
            //gridPen.Dispose();
            //cellBrush.Dispose();
            #endregion 
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            Size clientSize = this.graphicsPanel1.ClientSize;
            float num1 = (float)clientSize.Width / (float)this.universe.GetLength(0);
            clientSize = this.graphicsPanel1.ClientSize;
            float num2 = (float)clientSize.Height / (float)this.universe.GetLength(1);
            float num3 = (float)e.X / num1;
            float num4 = (float)e.Y / num2;
            this.universe[(int)num3, (int)num4] = !this.universe[(int)num3, (int)num4];
            this.graphicsPanel1.Invalidate();
            if (this.universe[(int)num3, (int)num4])
                ++this.aliveCells;
            else
                --this.aliveCells;
           
        }

        private void cutToolStripButton_Click(object sender, EventArgs e) => this.timer.Stop();

        private void copyToolStripButton_Click(object sender, EventArgs e) => this.timer.Start();

        private void pasteToolStripButton_Click(object sender, EventArgs e) => this.NextGeneration();

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            for (int index1 = 0; index1 < this.universe.GetLength(1); ++index1)
            {
                for (int index2 = 0; index2 < this.universe.GetLength(0); ++index2)
                    this.universe[index2, index1] = false;
            }
            this.generations = 0;
            this.aliveCells = 0;
            this.toolStripStatusLabelGenerations.Text = "Generations = " + this.generations.ToString();
            this.CellsAlive.Text = "Living Cells = " + this.aliveCells.ToString();
            this.graphicsPanel1.Invalidate();
            this.timer.Stop();
            this.gridColor = Color.Black;
            this.label1.Text = "Generations = " + this.generations.ToString();
            this.CellCount.Text = "Cell Count = " + this.aliveCells.ToString();
            this.UniverseSize.Text = "UniverseSize: (Width = , Height = ) ";
         
            #region Original Code

            //// If the left mouse button was clicked
            //if (e.Button == MouseButtons.Left)
            //{
            //    // Calculate the width and height of each cell in pixels
            //    int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            //    int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            //    // Calculate the cell that was clicked in
            //    // CELL X = MOUSE X / CELL WIDTH
            //    int x = e.X / cellWidth;
            //    // CELL Y = MOUSE Y / CELL HEIGHT
            //    int y = e.Y / cellHeight;

            //    // Toggle the cell's state
            //    universe[x, y] = !universe[x, y];

            //    // Tell Windows you need to repaint
            //    graphicsPanel1.Invalidate();
            //}
            #endregion
        }


        private int countNeighbor(int x, int y)
        {
            int num = 0;
            for (int index1 = x - 1; index1 <= x + 1; ++index1)
            {
                for (int index2 = y - 1; index2 <= y + 1; ++index2)
                {
                    bool flag1 = index1 < this.universe.GetLength(0) - 1 && index1 >= 0;
                    bool flag2 = index2 < this.universe.GetLength(1) - 1 && index2 >= 0;
                    if (index1 < this.universe.GetLength(0) && index1 >= 0 && (index2 < this.universe.GetLength(1) && index2 >= 0) && (this.universe[index1, index2] && (index1 != x || index2 != y)))
                        ++num;
                }
            }
            return num;
            
        }
        private int GetToroidal(int x, int y)
        {
            int num = 0;
            for (int index1 = -1; index1 <= 1; ++index1)
            {
                for (int index2 = -1; index2 <= 1; ++index2)
                {
                    int index3 = x + index2;
                    int index4 = y + index1;
                    if (index2 != 0 || index1 != 0)
                    {
                        if (index3 < 0)
                            index3 = this.universe.GetLength(0) - 1;
                        if (index4 < 0)
                            index4 = this.universe.GetLength(1) - 1;
                        if (index3 == this.universe.GetLength(0))
                            index3 = 0;
                        if (index4 == this.universe.GetLength(1))
                            index4 = 0;
                        if (this.universe[index3, index4])
                            ++num;
                    }
                }
            }
            return num;
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
                
               
            }
            graphicsPanel1.Invalidate();
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

            //??
            //??Would it not be universe?? the invalidate that is?
            //??


            graphicsPanel1.Invalidate();
            #region openFileDialog Test
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
            #endregion
            ofd.ShowDialog();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "All Files|*.*|Cells|*.cells";
            sfd.FilterIndex = 2;
            sfd.DefaultExt = "cells";
            sfd.RestoreDirectory = true;
            if (DialogResult.OK != sfd.ShowDialog())
                return;
            using(StreamWriter streamWriter = new StreamWriter(sfd.FileName))
            {
                for (int index = 0; index < this.universe.GetLength(1); index++)
                {
                    string empty = string.Empty;
                    for (int index2 = 0; index2 < this.universe.GetLength(0); index2++)
                    {
                        StringBuilder stringBuilder = new StringBuilder();

                        //universe size?
                        for (int x = 0; x <universe.Length; x++)
                        {
                            stringBuilder.Append(universe[x, y] ? '0' : '.');
                            streamWriter.WriteLine(stringBuilder.ToString());


                        }
                        streamWriter.Close();
                        //**trying a for loop instead of an if statement **

                        //if (this.universe[index2, index])
                        //    streamWriter.Write("0");
                        //else if (!this.universe[index2, index])
                        //    streamWriter.Write(".");

                    }
                    streamWriter.WriteLine(empty);
                }
            }

            
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
       
       //maybe put in class just to make it neater?

        //private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Random random = new Random();
        //    if (DialogResult.OK == random.())
        //    {
        //        for (int index = 0; index < this.universe.GetLength(0); index++)
        //        {
        //            for (int index2 = 0; index2 < this.universe.GetLength(1); index2++)
        //            {
        //                this.universe[index, index2] = false;
        //                if (random.Next(0, 4) == 0)
        //                {
        //                    this.universe[index, index2] = true;
        //                    ++this.aliveCells;
        //                }
        //            }
        //        }
        //    }
        //}

        private void turnGridOnoffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //on
            if (this.turnGridOnoffToolStripMenuItem.Checked)
            {
                this.turnGridOnoffToolStripMenuItem.Checked = true;
                this.gridColor = Color.Transparent;
                this.graphicsPanel1.Invalidate();
            }

            //off
            if (this.turnGridOnoffToolStripMenuItem.Checked)
            {
                this.turnGridOnoffToolStripMenuItem.Checked = false;
                this.gridColor = Color.Black;
                this.graphicsPanel1.Invalidate();
            }
        }

        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = this.gridColor;
            if (DialogResult.OK != colorDialog.ShowDialog())
                return;
            this.gridColor = colorDialog.Color;
            this.graphicsPanel1.Invalidate();

            
        }

        //pee pee poo this is a commit test 
        private void backgroundModalColor_Click(object sender, EventArgs e)
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

        private void cellColorModal_Click(object sender, EventArgs e)
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

        private void GridColorModal_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = this.gridColor;
            if (DialogResult.OK != colorDialog.ShowDialog())
                return;
            this.gridColor = colorDialog.Color;
            this.graphicsPanel1.Invalidate();

        }

        private void torodialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (DialogResult.OK != CountNeighborsFinite())
            //    return; 

            
            this.CellCount.Text = "Cell Count = " + this.aliveCells.ToString();
            if (this.finiteToolStripMenuItem.Checked)
            {
                this.BoundaryType.Text = "BoundaryType = Finite";
                this.finiteToolStripMenuItem.Enabled = true;
                this.torodialToolStripMenuItem.Enabled = false;
            }

            if (this.checkedState)
            {
                this.checkedState = false;
                this.torodialToolStripMenuItem.Checked = true;
                this.finiteToolStripMenuItem.Checked = false;
               
            }
            this.graphicsPanel1.Invalidate();
            
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.torodialToolStripMenuItem.Checked)
            {
                this.BoundaryType.Text = "BoundaryType = Torodial";
                this.torodialToolStripMenuItem.Enabled = true;
                this.finiteToolStripMenuItem.Enabled = false;
            }
            if (!this.checkedState)
            {
                this.checkedState = true;
                this.torodialToolStripMenuItem.Checked = false;
                this.finiteToolStripMenuItem.Checked = true;
            }
            this.graphicsPanel1.Invalidate();
        }

        //torodial modal
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.CellCount.Text = "Cell Count = " + this.aliveCells.ToString();
            if (this.finiteToolStripMenuItem.Checked)
            {
                this.BoundaryType.Text = "BoundaryType = Finite";
                this.finiteToolStripMenuItem.Enabled = true;
                
                this.torodialToolStripMenuItem.Enabled = false;
            }

            if (this.checkedState)
            {
                this.checkedState = false;
                this.torodialToolStripMenuItem.Checked = true;
                this.finiteToolStripMenuItem.Checked = false;

            }
            this.graphicsPanel1.Invalidate();
        }

        //okay I dont need this.
        //private void finiteToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    //call the boundry stlye
        //    BoundaryType.Finite;
        //    universe.Boundary
        //}


        private void fromTimeSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            for (int index = 0; index < universe.GetLength(0); index++)
            {
                for (int index2 = 0; index2 < universe.GetLength(1); index2++)
                {
                    universe[index, index2] = false;
                    if (random.Next1(0,4) == 0)
                    {
                        universe[index, index2] = true;
                        ++aliveCells;
                    }

                }
            }
            generations = 0;
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //need to set up logic for up down???
        //stack overflow up down 
        //private int state2;
        //private int state;

        //internal void SetState2(int checkState, bool value) => state2 = value ? state2 | checkState : state2 & ~checkState;
        //public NumericUpDown()
        //{
        //    this.SetState2(2048, true);
        //    this.Text = "0";
            
        //}


        //make decimals for randomizing the universe width and height 
        //get and set 


        //public Decimal UniWidth
        //{ get => NumericUpDown.}
        private void universeSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random randomSeed = new Random();
            int num1 = 50;
            int num2 = 50;
            

        }

 

       
    }
    
}
