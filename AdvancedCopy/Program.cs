using System;
using System.IO;

namespace AdvancedCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Folder.RootPath = @"C:\Users\richa\Documents\Programming\C#";
            Folder baseFolder = new Folder("AdvancedCopy", null);
        }
    }

    class Folder
    {
        public static string RootPath;

        public Folder Parent;
        public string Name;

        public string Path
        {
            get
            {
                if (Parent != null)
                    return Parent.Path + System.IO.Path.DirectorySeparatorChar + Name;
                else
                    return RootPath + System.IO.Path.DirectorySeparatorChar + Name;
            }
        }

        public Folder (string name, Folder parent)
        {
            this.Name = name;
            this.Parent = parent;
        }
    }
}
