using cryptography;
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

namespace license_builder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Xml Files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.CheckPathExists && openFileDialog.CheckFileExists)
            {
                string filename = openFileDialog.SafeFileName;
                string path = openFileDialog.FileName.Replace(openFileDialog.SafeFileName, string.Empty);

                string content = File.ReadAllText(openFileDialog.FileName);

                File.WriteAllText(Path.Combine(path, string.Format("{0}.lic", Path.GetFileNameWithoutExtension(filename))), StringCipher.Encrypt(content));

                MessageBox.Show("Done");
            }
        }
    }
}
