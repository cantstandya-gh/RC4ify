using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using RC4Cryptography;

namespace RC4ify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static class Globals
        {
            public static String OGPATH = "";
            public static String OUTPUTPATH = "";
            public static String filePath = "";
            public static String fileName = "";
            public static String fileNameNoExt = "";
            public static String fileExt = "";
            public static String DefaultOutputPath = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    Globals.filePath = openFileDialog.FileName;
                    Globals.fileName = openFileDialog.SafeFileName;
                    Globals.fileExt = Path.GetExtension(Globals.filePath);
                    Globals.OGPATH = Path.GetDirectoryName(Globals.filePath);
                    Globals.fileNameNoExt = Path.GetFileNameWithoutExtension(Globals.filePath);
                    Globals.DefaultOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\RC4ify\\Decrypted";
                    textBox1.Text = Globals.filePath;
                    textBox2.Text = Globals.DefaultOutputPath + "\\" + Globals.fileName;
                    if (Globals.OGPATH.Contains("store\\3a981f5cb2739137\\"))
                    {
                        string[] legacythemes = 
                        { 
                            "akon", 
                            "ben10", 
                            "bunny", 
                            "cctoonadventure", 
                            "chowder", 
                            "domo", 
                            "monkeytalk", 
                            "sf", 
                            "startrek", 
                            "toonadv", 
                            "underdog", 
                            "willie" 
                        };
                        if (legacythemes.Any(Globals.OGPATH.Contains))
                        {
                            comboBox1.Text = "g0o1a2n3i4m5a6t7e";
                        }
                        else
                        {
                            comboBox1.Text = "sorrypleasetryagainlater";
                        }
                        checkBox1.Checked = true;
                        textBox3.Visible = false;
                        comboBox1.Visible = true;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = folderBrowserDialog.SelectedPath + "\\" + Globals.fileName;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CheckForErrors();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox3.Visible = false;
                comboBox1.Visible = true;
            }
            else
            {
                textBox3.Visible = true;
                comboBox1.Visible = false;
            }
        }

        public void CheckForErrors()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("ERROR: No input asset file specified.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            if (textBox1.Text.Contains(".swf") == false)
            {
                MessageBox.Show("ERROR: Incompatible format. Only *.swf files are supported.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                StartRC4();
            }
        }

public void StartRC4()
        {
            Globals.filePath = textBox1.Text;
            Globals.OUTPUTPATH = textBox2.Text;
            string key_phrase = "";
            if (checkBox1.Checked == true)
            {
                key_phrase = comboBox1.Text;
            }
            else
            {
                key_phrase = textBox3.Text;
            }
            if (textBox2.Text.Contains("\\Documents\\RC4ify\\Decrypted"))
            {
                if ((Directory.Exists(Globals.DefaultOutputPath)) == false)
                {
                    Directory.CreateDirectory(Globals.DefaultOutputPath);
                }
            }
            byte[] data = File.ReadAllBytes(Globals.filePath);
            byte[] key = Encoding.UTF8.GetBytes(key_phrase);
            byte[] encrypted_data = RC4.Apply(data, key);
            System.IO.File.WriteAllBytes(Globals.OUTPUTPATH, encrypted_data);
            MessageBox.Show("RC4 process successfully completed!", "RC4 Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
