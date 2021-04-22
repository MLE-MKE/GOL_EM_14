using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL_EM_14
{
    class Universe_Array
    {
        public enum BoundaryStyle
        {
            Torodial,
            Finite,
            Infite,
        }
        public struct Cell
        {
            private bool alive;
            //public Cell(bool alive)
            //{
            //    alive = alive;
            //}

            public bool Alive
            {
                get => alive;
                set => alive = value;

            }

            public bool Dead
            {
                get => !alive;
                set => alive = !value;
            }
        }
        private Cell[,] universe;
        private Cell[,] next;
        private int livingCells;
        private int generations;
        private BoundaryStyle sideConditions;

        // a way to declare the boundaries 


        public Universe_Array()
        {
            universe = new Cell[50, 50];
            next = new Cell[50, 50];

        }

        public Universe_Array(int width, int height)
        {
            universe = new Cell[width, height];
            next = new Cell[width, height];
        }

        public int LivingCells => livingCells;

        public int Generation => generations;


        public Size Size
        {
            get => new Size(universe.GetLength(0), universe.GetLength(1));
            set
            {
                universe = new Cell[value.Width, value.Height];
                next = new Cell[value.Width, value.Height];
            }
        }

        public int GetNeighborCount(int x, int y)
        {
            int num = 0;
            switch (sideConditions)
            {
                case BoundaryStyle.Torodial:
                    num =
                        //count neighbor final number tordial(x,y);
                        break;
                case BoundaryStyle.Finite:
                    num =
                        //count neghbor final number edge of the array total (x,y);
                        break;
            }
            return num;

        }

        //get set Tyoe of Boundary aka Boundry Style
        //public BoundaryStyle BoundaryStyle
        //{
        //    get => sizeConditions;
        //    set => SizeConditions = value;

        //}
        private void NextGeneration()
        {

            int side1 = universe.GetLength(0);
            int side2 = universe.GetLength(1);
            livingCells = 0;

            for (int x = 0; x < side1; x++)
            {
                for (int y = 0; y < side2; y++)
                {
                    next[x, y].Dead = true;
                    int num = 0;
                    switch (sideConditions)
                    {
                        case BoundaryStyle.Torodial:
                            break;
                        case BoundaryStyle.Finite:
                            break;
                        case BoundaryStyle.Infite:
                            break;
                        default:
                            break;
                    }

                    if (universe[x, y].Alive)
                    {
                        if (num >= 2 && num <= 3)
                        {
                            next[x, y].Alive = true;
                            ++livingCells;
                        }
                    }
                    else if (num == 3)

                    {
                        next[x, y].Alive = true;
                        ++livingCells;
                    }
                }
            }

            // count the edge of the arrays universe



            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
        }
        private int CountNeighborsEdge(int x, int y)
        {
            int num = 0;
            int side1 = universe.GetLength(0);
            int side2 = universe.GetLength(1);
            if (x - 1 >= 0 && universe[x - 1, y - 1].Alive)
                ++num;
            if (y - 1 >= 0 && universe[x, y - 1].Alive)
                ++num;
            if (x + 1 < side1 && y + 1 < side2 && universe[x + 1, y + 1].Alive)
                ++num;
            if (x + 1 < side1 && y - 1 >= 0 && universe[x + 1, y - 1].Alive)
                ++num;
            if (x + 1 < side1 && universe[x + 1, y].Alive)
                ++num;
            if (x - 1 >=  0 && universe[x - 1, y].Alive)
                ++num;
            
            
            if (x - 1 >= 0 && y + 1 < side2 && universe[x , y + 1].Alive)
                ++num;

            //commmit later I guess 
        }
    }
}
