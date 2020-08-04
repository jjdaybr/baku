using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baku_001
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxSource.Text = fbd.SelectedPath;
                if (!textBoxSource.Text.EndsWith("\\"))
                    textBoxSource.Text = textBoxSource.Text + "\\";
            }
            else
            {
                textBoxSource.Text = string.Empty;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxDestination.Text = fbd.SelectedPath;
                if (!textBoxDestination.Text.EndsWith("\\"))
                    textBoxDestination.Text = textBoxDestination.Text + "\\";
            }
            else
            {
                textBoxDestination.Text = string.Empty;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            listBoxOutput.Items.Clear();
            backgroundWorker1.RunWorkerAsync();
            /*
            int dircount = 0;
            int filecount = 0;
            List<string> sourceDirectories = DirectorySearch(textBoxSource.Text);
            foreach(string s in sourceDirectories)
            {
                if (!Directory.Exists(textBoxDestination.Text + s.Replace(textBoxSource.Text, "")))
                {
                    dircount++;
                    Directory.CreateDirectory(textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    listBoxOutput.Items.Add("Created directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    Debug.WriteLine("Created directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                }
            }
            List<string> sourceFiles = FileSearch(textBoxSource.Text);
            foreach(string s in sourceFiles)
            {
                if (!File.Exists(textBoxDestination.Text + s.Replace(textBoxSource.Text, "")))
                {
                    filecount++;
                    Debug.WriteLine("Copied file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    File.Copy(s, textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    listBoxOutput.Items.Add("Copied file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                }
            }
            listBoxOutput.Items.Add(dircount + " directories created. " + filecount + " files created.");
            */
        }

        private List<string> DirectorySearch(string sDir)
        {
            List<string> direcotries = new List<string>();
            try
            {
                foreach(string d in Directory.GetDirectories(sDir))
                {
                    direcotries.Add(d);
                    direcotries.AddRange(DirectorySearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            return direcotries;
        }

        private List<string> FileSearch(string sDir)
        {
            List<string> files = new List<string>();
            try
            {
                foreach(string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach(string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(FileSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            return files;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AnalysisHelper a = new AnalysisHelper();
            a.Analyze(listBoxOutput, textBoxSource, textBoxDestination);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            AnalysisHelper a = new AnalysisHelper();
            a.Analyze(textBoxSource, textBoxDestination);
            var backgroundWorker = sender as BackgroundWorker;
            int dircount = 0;
            int filecount = 0;
            List<string> sourceDirectories = DirectorySearch(textBoxSource.Text);
            List<string> sourceFiles = FileSearch(textBoxSource.Text);
            backgroundWorker.ReportProgress(0, ", Creating directories");
            foreach (string s in sourceDirectories)
            {
                if (!Directory.Exists(textBoxDestination.Text + s.Replace(textBoxSource.Text, "")))
                {
                    dircount++;
                    Directory.CreateDirectory(textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    //listBoxOutput.Items.Add("Created directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    Debug.WriteLine("Created directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    backgroundWorker.ReportProgress(((dircount + filecount) * 100)/(a.TotalActions() + 1), ", Created directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                }
            }
            
            backgroundWorker.ReportProgress(((dircount + filecount) * 100) / (a.TotalActions() + 1), ", Creating files");
            foreach (string s in sourceFiles)
            {
                if (!File.Exists(textBoxDestination.Text + s.Replace(textBoxSource.Text, "")))
                {
                    filecount++;
                    File.Copy(s, textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    Debug.WriteLine("Copied file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    //listBoxOutput.Items.Add("Copied file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    backgroundWorker.ReportProgress(((dircount + filecount) * 100) / (a.TotalActions() + 1), ", Copied file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                }
            }
            //listBoxOutput.Items.Add(dircount + " directories created. " + filecount + " files created.");
            backgroundWorker.ReportProgress(100, dircount + ", directories created, " + filecount + " files created.");



            /*
            var backgroundWorker = sender as BackgroundWorker;
            for (int j = 0; j < 100; j++)
            {
                Calculate(j);
                backgroundWorker.ReportProgress((j * 100) / 100000, "porkchop");
            }
            */
        }

        //private void Calculate(int i)
        //{
        //    double pow = Math.Pow(i, i);
        //}

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //listBoxOutput.Items.Add(e.ProgressPercentage);
            listBoxOutput.Items.Add(e.ProgressPercentage.ToString() + "%" + e.UserState);
            listBoxOutput.ClearSelected();
            listBoxOutput.SelectedIndex = listBoxOutput.Items.Count - 1;
        }
    }
}
