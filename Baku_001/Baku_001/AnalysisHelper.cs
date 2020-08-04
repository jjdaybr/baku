using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Baku_001
{
    public class AnalysisHelper
    {
        public int DirectoriesNeeded;
        public int FilesNeeded;
        public void Analyze(ListBox listBoxOutput, TextBox textBoxSource, TextBox textBoxDestination)
        {
            int dircount = 0;
            int filecount = 0;
            listBoxOutput.Items.Clear();
            List<string> sourceDirectories = DirectorySearch(textBoxSource.Text);
            foreach (string s in sourceDirectories)
            {
                if (!Directory.Exists(textBoxDestination.Text + s.Replace(textBoxSource.Text, "")))
                {
                    dircount++;
                    //Directory.CreateDirectory(textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    listBoxOutput.Items.Add("Create directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    //Debug.WriteLine("Create directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                }
            }
            List<string> sourceFiles = FileSearch(textBoxSource.Text);
            foreach (string s in sourceFiles)
            {
                if (!File.Exists(textBoxDestination.Text + s.Replace(textBoxSource.Text, "")))
                {
                    filecount++;
                    //Debug.WriteLine("Copy file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    //File.Copy(s, textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    listBoxOutput.Items.Add("Copy file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                }
            }
            listBoxOutput.Items.Add(dircount + " directories need created. " + filecount + " files need created.");
            DirectoriesNeeded = dircount;
            FilesNeeded = filecount;
        }

        public void Analyze(TextBox textBoxSource, TextBox textBoxDestination)
        {
            int dircount = 0;
            int filecount = 0;
            //listBoxOutput.Items.Clear();
            List<string> sourceDirectories = DirectorySearch(textBoxSource.Text);
            foreach (string s in sourceDirectories)
            {
                if (!Directory.Exists(textBoxDestination.Text + s.Replace(textBoxSource.Text, "")))
                {
                    dircount++;
                    //Directory.CreateDirectory(textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    //listBoxOutput.Items.Add("Create directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    //Debug.WriteLine("Create directory " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                }
            }
            List<string> sourceFiles = FileSearch(textBoxSource.Text);
            foreach (string s in sourceFiles)
            {
                if (!File.Exists(textBoxDestination.Text + s.Replace(textBoxSource.Text, "")))
                {
                    filecount++;
                    //Debug.WriteLine("Copy file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    //File.Copy(s, textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                    //listBoxOutput.Items.Add("Copy file " + textBoxDestination.Text + s.Replace(textBoxSource.Text, ""));
                }
            }
            //listBoxOutput.Items.Add(dircount + " directories need created. " + filecount + " files need created.");
            DirectoriesNeeded = dircount;
            FilesNeeded = filecount;
        }

        private List<string> DirectorySearch(string sDir)
        {
            List<string> direcotries = new List<string>();
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
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
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
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

        public int TotalActions()
        {
            return DirectoriesNeeded + FilesNeeded;
        }

    }
}
