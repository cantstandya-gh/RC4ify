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
                openFileDialog.Filter = "Macromedia Flash (*.swf)|*.swf|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    Globals.filePath = openFileDialog.FileName;
                    Globals.fileName = openFileDialog.SafeFileName;
                    Globals.fileExt = Path.GetExtension(Globals.filePath);
                    Globals.OGPATH = Path.GetDirectoryName(Globals.filePath);
                    Globals.fileNameNoExt = Path.GetFileNameWithoutExtension(Globals.filePath);
                    Globals.DefaultOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\RC4ify";
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
                        if (Globals.OGPATH.Contains("\\street\\"))
                        {
                            MessageBox.Show("Every single file in the \"Street\" theme is already decrypted.\n\nTherefore, you don't need to decrypt them. You can encrypt them if you want though.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            comboBox1.Text = "g0o1a2n3i4m5a6t7e";
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
            Globals.filePath = textBox1.Text;
            Globals.OUTPUTPATH = textBox2.Text;
            switch (Globals.filePath)
            {
                case string a when string.IsNullOrEmpty(a):
                    MessageBox.Show("ERROR: No input file has been specified.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case string a when (!a.Contains(".swf")):
                    if (checkBox1.Checked == true)
                    {
                        MessageBox.Show("ERROR: Incompatible format for encrypting/decrypting LVM assets. Only *.swf files are supported.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                default:
                    CheckForOverwrite();
                    break;
            }
        }

        public void CheckForOverwrite()
        {
            if (File.Exists(Globals.OUTPUTPATH))
            {
                if (MessageBox.Show("We've detected that this file already exists in that directory.\n\nWould you like to overwrite it?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    StartRC4();
                }
                else
                {
                    MessageBox.Show("RC4 process has been halted per user choice due to a conflict with the absolute output path.", "RC4 Halted", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                StartRC4();
            }
        }

public void StartRC4()
        {
            string key_phrase = "";
            if (checkBox1.Checked == true)
            {
                key_phrase = comboBox1.Text;
            }
            else
            {
                key_phrase = textBox3.Text;
            }
            if (textBox2.Text.Contains("\\Documents\\RC4ify"))
            {
                if (!Directory.Exists(Globals.DefaultOutputPath))
                {
                    Directory.CreateDirectory(Globals.DefaultOutputPath);
                }
            }
            byte[] data = File.ReadAllBytes(Globals.filePath);
            byte[] key = Encoding.UTF8.GetBytes(key_phrase);
            byte[] encrypted_data = RC4.Apply(data, key);
            System.IO.File.WriteAllBytes(Globals.OUTPUTPATH, encrypted_data);
            if (Globals.filePath.Contains("\\common\\sound\\"))
            {
                if (Globals.filePath.Contains(".swf"))
                {
                    MessageBox.Show("You won't get the sound file directly from the *.swf as it's embedded into a sort of controller.\n\nYou will need to use a Flash decompiling software such as FFDec or SoThink SWF Decompiler to retrieve the audio file from the decrypted *.swf file.\n\nI plan on finding a workaround to this in a later update. ~Keegan", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            MessageBox.Show("RC4 process successfully completed!", "RC4 Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
