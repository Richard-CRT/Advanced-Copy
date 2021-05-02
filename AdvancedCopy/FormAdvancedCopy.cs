using FileSystemParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedCopy
{
    public partial class FormAdvancedCopy : Form
    {
        public FormAdvancedCopy()
        {
            InitializeComponent();

            Folder baseFolder = new Folder(@"C:\Users\richa\Documents\Programming\C#\AdvancedCopy", null);
        }

    }
}
