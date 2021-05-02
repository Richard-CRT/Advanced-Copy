using FileSystemParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            Folder baseFolder = new Folder(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())))), null);
        }

    }
}
