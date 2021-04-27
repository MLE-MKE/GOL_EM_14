using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace GOL_EM_14
{
    public class Random :Form1
    {
        private NumericUpDown numericUpDown1;
        //stack over flow random seed gen 

        //random Array 
        private int[] RandomArray = new int[56];
        private int next1;
        private int next2;


        //if a negative number is specified, the absolute value of the number is used
       public Random(int Seed)
        {
            int num1 = 161803398 - (Seed == int.MinValue ? int.MaxValue : Math.Abs(Seed));

            //one from 56 55

            RandomArray[55] = num1;
            int num2 = 1;
            for (int index1 = 1; index1 < 55; index1++)
            {
                int index2 = 21 * index1 % 55;
                RandomArray[index2] = num2;
                num2 = num1 - num2;
                if (num2 < 0)
                    num2 += int.MaxValue;
                num1 = RandomArray[index2];
            }
            for (int index1 = 1; index1 < 5; index1++)
            {
                for (int index2 = 1; index2 < 56; index2++)
                {
                    RandomArray[index2] -= RandomArray[1 + (index2 + 30) % 55];
                    if (RandomArray[index2] < 0)
                        RandomArray[index2] += int.MaxValue;
                }
            }
            next1 = 0;
            next2 = 21;
            Seed = 1;
        }

        public Random()
        {
        }
        private int Next(int minValue, int maxValue)
        {
            long num = 0;
            if (minValue > maxValue)
            { num = (long)maxValue - (long)minValue; }
            return (int)num;

            throw new NotImplementedException();



        }

        //can I dynamic envoke these orrrr
        public Decimal RandomSeed
        {
            get => (Decimal)(int)numericUpDown1.Value;
            set => numericUpDown1.Value = value;
        }

        private void randomizeUniverseToolStripMenuItem_Click(object sender, EventArgs e) => RandomSeed = (Decimal)new Random().Next(int.MinValue, int.MaxValue);
        //Oh wait do I need to make a next?
      

        private void InitializeComponent()
        {

        }

        internal int Next1(int v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}
