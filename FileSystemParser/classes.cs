using System;
using System.IO;
using System.Linq;

namespace FileSystemParser
{
    public abstract class FileSystemItem
    {
        public Folder ParentFolder;
        public string Name;
        public string FullPath;

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

        public override string ToString()
        {
            return this.Name;
        }

        public abstract bool IsFolder();
    }

    public class Folder : FileSystemItem
    {
        public Folder[] SubDirectories;
        public File[] Files;

        public bool Expanded = true;

        public Folder(string _fullPath, Folder _parentFolder)
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

        public override bool IsFolder()
        {
            return true;
        }
    }

    public class File : FileSystemItem
    {
        public File(string _fullPath, Folder _parentFolder)
        {
            this.Name = Path.GetFileName(_fullPath);
            this.FullPath = _fullPath;
            this.ParentFolder = _parentFolder;

            Console.WriteLine($"{new String(' ', Depth * 3)}Found file `{Name}`");
        }

        public override bool IsFolder()
        {
            return false;
        }
    }
}
