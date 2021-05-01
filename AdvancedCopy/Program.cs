using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvancedCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            Folder baseFolder = new Folder(@"C:\Users\richa\Documents\Programming\C#\AdvancedCopy", null);
            Console.WriteLine();
            baseFolder.Print();

            Console.ReadLine();
        }
    }

    class Folder
    {
        public Folder ParentFolder;
        public string Name;
        public string FullPath;

        public Folder[] SubDirectories;
        public File[] Files;

        public bool Expanded = true;

        public int Depth
        {
            get
            {
                if (ParentFolder != null)
                    return ParentFolder.Depth + 1;
                else
                    return 0;
            }
        }

        public Folder (string _fullPath, Folder _parentFolder)
        {
            this.Name = Path.GetFileName(_fullPath);
            this.FullPath = _fullPath;
            this.ParentFolder = _parentFolder;

            Console.WriteLine($"{new String(' ', Depth * 3)}Parsing folder `{Name}`");
            string[] filesFullPaths = Directory.GetFiles(this.FullPath);
            Files = filesFullPaths.Select(x => new File(x, this)).ToArray();

            string[] subdirectoryFullPaths = Directory.GetDirectories(this.FullPath);
            SubDirectories = subdirectoryFullPaths.Select(x => new Folder(x, this)).ToArray();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void Print(int printDepth = 0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{new String(' ', printDepth * 3)}{(this.Expanded?'-':'+')} {this.Name}");
            Console.ForegroundColor = ConsoleColor.White;
            if (Expanded)
            {
                foreach (File file in this.Files)
                {
                    Console.WriteLine($"{new String(' ', printDepth * 3)}{file}");
                }
                foreach (Folder subdirectory in this.SubDirectories)
                {
                    subdirectory.Print(printDepth + 1);
                }
            }
        }
    }

    class File
    {
        public Folder ParentFolder;
        public string Name;
        public string FullPath;

        public int Depth
        {
            get { return ParentFolder.Depth + 1; }
        }

        public File(string _fullPath, Folder _parentFolder)
        {
            this.Name = Path.GetFileName(_fullPath);
            this.FullPath = _fullPath;
            this.ParentFolder = _parentFolder;

            Console.WriteLine($"{new String(' ', Depth * 3)}Found file `{Name}`");
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
