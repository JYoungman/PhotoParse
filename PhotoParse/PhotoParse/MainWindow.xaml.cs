using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoParse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string sourcePath;
        string destinationPath;

        private string[] sourceFiles;
        private List<string> destinationDirectories;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Execute_Click(object sender, RoutedEventArgs e)
        {
            StatusWindow.Text = "Beginning scan";

            //Initialize variables
            sourcePath = TB_Source.Text;
            destinationPath = TB_Dest.Text;

            destinationDirectories = new List<string>();

            //Parse source files
            sourceFiles = Directory.GetFiles(sourcePath);
            foreach(string filename in sourceFiles)
            {
                UniqueAddMember(destinationDirectories, GetFilenameDate(filename));
            }

            StatusWindow.Text += "\n" + "Dates found: " + destinationDirectories.Count.ToString();

            //Generate and populate target directories
            foreach(string targetDirectory in destinationDirectories)
            {
                string newPath = destinationPath + GenerateDirectoryName(targetDirectory);
                Directory.CreateDirectory(newPath);
                StatusWindow.Text += "\n" + newPath;

                //Dupliate relevant files
                foreach(string pictureName in sourceFiles)
                {
                    if(GetFilenameDate(pictureName) == targetDirectory)
                    {
                        //string srcFile = System.IO.Path.Combine(sourcePath, pictureName);
                        string destFile = System.IO.Path.Combine(newPath, GetFilename(pictureName));

                        //System.IO.File.Copy(srcFile, destFile);
                        System.IO.File.Copy(pictureName, destFile);
                    }
                }
            }
        }

        //Return the date as encoded in the filename
        private string GetFilenameDate(string filename)
        {
            string[] temp = filename.Split("\\");
            return temp[temp.Length-1].Split("_")[0];
        }

        //Return the filename (as distinct from the full path)
        private string GetFilename(string filename)
        {
            string[] temp = filename.Split("\\");
            return temp[temp.Length - 1];
        }

        //Attempt to add new dates to the list of directories to create
        private void UniqueAddMember(List<string> target, string str)
        {
            if (!target.Contains(str))
            {
                target.Add(str);
            }
        }

        //Turn a date string into a folder path with appropriate formatting
        private string GenerateDirectoryName(string str)
        {
            return "\\" + str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2) + "\\";
        }
    }
}