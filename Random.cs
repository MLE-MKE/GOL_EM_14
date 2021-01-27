using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL_EM_14
{
    class Random :Form1
    {
        private UpDownNumeric upDownNumeric1;
        public Decimal RandomSeedGen
        {
            get => (Decimal) (int) this.numericUpDown1.Value
        }
    }
}
