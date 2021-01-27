using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOL_EM_14
{
    class Random : Form1
    {
        private System.Reflection.Emit.Label label1;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private Label Width;
        private Label Height;
        private Button OK;
        private Button Cancel;

        public Random() => this.InitializeComponent();


        private void InitializeComponent()
        {

        }
     
        internal int Next(int v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}
