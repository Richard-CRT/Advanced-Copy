using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FileSystemParser
{
    public abstract partial class FileSystemItem
    {
        public Folder ParentFolder;
        public string Name;
        public string FullPath;
        public bool Excluded = false;
        public object TreeNode;
        public abstract bool IsFolder
        {
            get;
        }

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

        public FileSystemItem(string _fullPath, Folder _parentFolder)
        {
            this.Name = Path.GetFileName(_fullPath);
            this.FullPath = _fullPath;
            this.ParentFolder = _parentFolder;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class Folder : FileSystemItem
    {
        public List<Folder> SubDirectories = new List<Folder>();
        public List<File> Files = new List<File>();

        public bool Enumerated;
        public override bool IsFolder
        {
            get { return true; }
        }

        private bool IsSymbolic(string fullPath)
        {
            FileInfo pathInfo = new FileInfo(fullPath);
            return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }

        public Folder(string _fullPath, Folder _parentFolder) : base(_fullPath, _parentFolder)
        {
            Debug.WriteLine($"{new String(' ', Depth * 3)}Found folder `{Name}`");
        }

        public void Enumerate()
        {
            Debug.WriteLine($"{new String(' ', Depth * 3)}Enumerating folder `{Name}`");
            string[] filesFullPaths = Directory.GetFiles(this.FullPath);
            Files = new List<File>();
            foreach (string fileFullPath in filesFullPaths)
            {
                Files.Add(new File(fileFullPath, this));
            }

            string[] subdirectoryFullPaths = Directory.GetDirectories(this.FullPath);
            SubDirectories = new List<Folder>();
            foreach (string subdirectoryFullPath in subdirectoryFullPaths)
            {
                if (Path.GetFileName(subdirectoryFullPath) != "$RECYCLE.BIN")
                {
                    if (!IsSymbolic(subdirectoryFullPath))
                        SubDirectories.Add(new Folder(subdirectoryFullPath, this));
                    else
                        Files.Add(new File(subdirectoryFullPath, this));
                }
            }

            this.Enumerated = true;
        }
    }

    public class File : FileSystemItem
    {
        public override bool IsFolder
        {
            get { return false; }
        }

        public File(string _fullPath, Folder _parentFolder) : base(_fullPath, _parentFolder)
        {
            Debug.WriteLine($"{new String(' ', Depth * 3)}Found file `{Name}`");
        }
    }
}