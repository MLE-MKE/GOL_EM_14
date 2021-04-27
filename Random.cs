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

       public Random(int Seed)
        {
            int num = 161803398 - (Seed == int.MinValue ? int.MaxValue : Math.Abs(Seed));
        }
        public Decimal RandomSeed
        {
            get => (Decimal)(int)numericUpDown1.Value;
            set => numericUpDown1.Value = value;
        }

        private void randomizeUniverseToolStripMenuItem_Click(object sender, EventArgs e) => RandomSeed = (Decimal)new Random().Next(int MinValue, int MaxValue);



        private void InitializeComponent()
        {

        }
    }
}
