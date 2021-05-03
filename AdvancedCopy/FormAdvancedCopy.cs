using FileSystemParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using File = FileSystemParser.File;

namespace AdvancedCopy
{
    public partial class FormAdvancedCopy : Form
    {
        Folder BaseFolder;

        public FormAdvancedCopy()
        {
            InitializeComponent();
        }

        private void FormAdvancedCopy_Load(object sender, EventArgs e)
        {
            if (MessageBox.Show("This utility copies files around your file system and may be full of bugs. Use at ones own discretion!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
            {
                this.Close();
            }
            while (Populate() != DialogResult.OK);
        }

        private void button_folder_Click(object sender, EventArgs e)
        {
            Populate();
        }

        private DialogResult Populate()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "Select the root folder of the Advanced Copy";
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                treeView_fileSystem.Nodes.Clear();

                Folder baseFolder = new Folder(folderBrowserDialog.SelectedPath, Directory.GetParent(folderBrowserDialog.SelectedPath).FullName, null);
                BaseFolder = baseFolder;

                TreeNode rootNode = ItemToTreeNode(baseFolder);
                treeView_fileSystem.Nodes.Add(rootNode);
                rootNode.Expand();
            }
            return dialogResult;
        }

        private TreeNode ItemToTreeNode(FileSystemItem fileSystemItem)
        {
            TreeNode treeNode = new TreeNode(fileSystemItem.Name);
            fileSystemItem.TreeNode = treeNode;

            treeNode.NodeFont = this.Font;

            if (fileSystemItem.IsFolder)
            {
                TreeNode dummyTreeNode = new TreeNode("dummy");
                treeNode.Nodes.Add(dummyTreeNode);
                treeNode.StateImageKey = "folder.png";
            }

            treeNode.Tag = fileSystemItem;

            return treeNode;
        }

        private void CheckParentNodeForAllIncluded(TreeNode treeNode)
        {
            bool partiallyIncluded = false;
            bool totallyIncluded = true;
            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                FileSystemItem childFileSystemItem = childTreeNode.Tag as FileSystemItem;
                if (childFileSystemItem.Excluded || childFileSystemItem.PartiallyExcluded)
                    totallyIncluded = false;
                if (!childFileSystemItem.Excluded)
                    partiallyIncluded = true;
            }
            if (totallyIncluded)
                IncludeTreeNode(treeNode);
            else if (partiallyIncluded)
                PartiallyIncludeTreeNode(treeNode);
        }

        private void IncludeTreeNode(TreeNode treeNode)
        {
            FileSystemItem fileSystemItem = treeNode.Tag as FileSystemItem;
            if (fileSystemItem != null)
            {
                treeNode.NodeFont = new Font(treeNode.NodeFont, FontStyle.Regular);
                treeNode.ForeColor = Color.Black;
                fileSystemItem.Excluded = false;
                fileSystemItem.PartiallyExcluded = false;

                if (treeNode.Parent != null)
                    CheckParentNodeForAllIncluded(treeNode.Parent);
            }
        }

        private void PartiallyIncludeTreeNode(TreeNode treeNode)
        {
            FileSystemItem fileSystemItem = treeNode.Tag as FileSystemItem;
            if (fileSystemItem != null)
            {
                if (!fileSystemItem.IsFolder)
                    throw new Exception("Only folders should be partially included");
                treeNode.NodeFont = new Font(treeNode.NodeFont, FontStyle.Regular);
                treeNode.ForeColor = Color.Blue;
                fileSystemItem.Excluded = false;
                fileSystemItem.PartiallyExcluded = true;

                if (treeNode.Parent != null)
                    CheckParentNodeForAllIncluded(treeNode.Parent);
            }
        }

        private void IncludeTreeNodeRecursive(TreeNode treeNode)
        {

            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                IncludeTreeNodeRecursive(childTreeNode);
            }

            IncludeTreeNode(treeNode);
        }

        private void CheckParentNodeForAllExcluded(TreeNode treeNode)
        {
            bool partiallyExcluded = false;
            bool totallyExcluded = true;
            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                FileSystemItem childFileSystemItem = childTreeNode.Tag as FileSystemItem;
                if (!childFileSystemItem.Excluded)
                {
                    totallyExcluded = false;
                }
                if (childFileSystemItem.Excluded || childFileSystemItem.PartiallyExcluded)
                {
                    partiallyExcluded = true;
                }
            }
            if (totallyExcluded)
                ExcludeTreeNode(treeNode);
            else if (partiallyExcluded)
                PartiallyExcludeTreeNode(treeNode);
        }

        private void ExcludeTreeNode(TreeNode treeNode)
        {
            FileSystemItem fileSystemItem = treeNode.Tag as FileSystemItem;
            if (fileSystemItem != null)
            {
                treeNode.NodeFont = new Font(treeNode.NodeFont, FontStyle.Strikeout);
                treeNode.ForeColor = Color.Red;
                fileSystemItem.Excluded = true;
                fileSystemItem.PartiallyExcluded = true;

                if (treeNode.Parent != null)
                    CheckParentNodeForAllExcluded(treeNode.Parent);
            }
        }

        private void PartiallyExcludeTreeNode(TreeNode treeNode)
        {
            FileSystemItem fileSystemItem = treeNode.Tag as FileSystemItem;
            if (fileSystemItem != null)
            {
                if (!fileSystemItem.IsFolder)
                    throw new Exception("Only folders should be partially excluded");
                treeNode.ForeColor = Color.Blue;
                fileSystemItem.Excluded = false;
                fileSystemItem.PartiallyExcluded = true;

                if (treeNode.Parent != null)
                    CheckParentNodeForAllExcluded(treeNode.Parent);
            }
        }

        private void ExcludeTreeNodeRecursive(TreeNode treeNode)
        {
            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                ExcludeTreeNodeRecursive(childTreeNode);
            }

            ExcludeTreeNode(treeNode);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Space))
            {
                TreeNode selectedTreeNode = treeView_fileSystem.SelectedNode;
                if (selectedTreeNode != null)
                {
                    FileSystemItem fileSystemItem = selectedTreeNode.Tag as FileSystemItem;
                    if (fileSystemItem.Excluded)
                        IncludeTreeNodeRecursive(selectedTreeNode);
                    else
                        ExcludeTreeNodeRecursive(selectedTreeNode);
                }
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button_include_Click(object sender, EventArgs e)
        {
            TreeNode selectedTreeNode = treeView_fileSystem.SelectedNode;
            if (selectedTreeNode != null)
                IncludeTreeNodeRecursive(selectedTreeNode);
        }

        private void button_exclude_Click(object sender, EventArgs e)
        {
            TreeNode selectedTreeNode = treeView_fileSystem.SelectedNode;
            if (selectedTreeNode != null)
                ExcludeTreeNodeRecursive(selectedTreeNode);
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Advanced Copy files (*.acs)|*.acs";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (System.IO.File.Exists(openFileDialog.FileName))
                {
                    string[] fullPathsToExclude = System.IO.File.ReadAllLines(openFileDialog.FileName);
                    ApplyExcludedPaths(fullPathsToExclude);
                }
            }
        }

        private void ApplyExcludedPaths(string[] fullPathsToExclude)
        {
            foreach (string fullPathToExclude in fullPathsToExclude)
            {
                List<string> pathParts = new List<string>();
                string part = fullPathToExclude;
                while (part != BaseFolder.FullPath)
                {
                    pathParts.Add(part);
                    DirectoryInfo directoryInfo = Directory.GetParent(part);
                    if (directoryInfo != null)
                        part = directoryInfo.FullName;
                    else
                        continue;
                }
                pathParts.Reverse();
                ApplyExcludedPathRecursive(BaseFolder, pathParts);
            }
        }

        private void ApplyExcludedPathRecursive(Folder folder, List<string> pathToExcludeParts, int partIndex = 0)
        {
            // If the current folder is past the parts to exclude, then it is the folder to exclude
            if (partIndex == pathToExcludeParts.Count)
            {
                ExcludeTreeNode(folder.TreeNode as TreeNode);
                return;
            }
            string pathToExcludePart = pathToExcludeParts[partIndex];
            EnumerateTreeNode(folder.TreeNode as TreeNode);
            bool foundFolder = false;
            foreach (Folder subdirectory in folder.SubDirectories)
            {
                if (subdirectory.FullPath == pathToExcludePart)
                {
                    foundFolder = true;
                    ApplyExcludedPathRecursive(subdirectory, pathToExcludeParts, partIndex + 1);
                }
            }
            // If folder not found then the part must be a file in this folder to exclude
            if (!foundFolder)
            {
                foreach (File file in folder.Files)
                {
                    if (file.FullPath == pathToExcludePart)
                    {
                        ExcludeTreeNode(file.TreeNode as TreeNode);
                    }
                }
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            List<string> fullPathsToExclude = new List<string>();
            CollectExcludedPathsRecursive(BaseFolder, fullPathsToExclude);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Advanced Copy files (*.acs)|*.acs";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllLines(saveFileDialog.FileName, fullPathsToExclude);
            }
        }

        private void CollectExcludedPathsRecursive(FileSystemItem fileSystemItem, List<string> fullPathsToExclude)
        {
            if (fileSystemItem.Excluded)
                fullPathsToExclude.Add(fileSystemItem.FullPath);
            else
            {
                if (fileSystemItem.IsFolder)
                {
                    Folder folder = fileSystemItem as Folder;
                    foreach (Folder subdirectory in folder.SubDirectories)
                        CollectExcludedPathsRecursive(subdirectory, fullPathsToExclude);
                    foreach (File file in folder.Files)
                        CollectExcludedPathsRecursive(file, fullPathsToExclude);
                }
            }
        }

        private void EnumerateTreeNode(TreeNode treeNode, bool expanding = false)
        {
            FileSystemItem fileSystemItem = treeNode.Tag as FileSystemItem;
            Folder folder = fileSystemItem as Folder;

            if (!folder.Enumerated)
            {
                folder.Enumerate();

                treeNode.Nodes.Clear();
                TreeNode[] children = folder.SubDirectories.Select(x => ItemToTreeNode(x)).Concat(folder.Files.Select(x => ItemToTreeNode(x))).ToArray();
                treeNode.Nodes.AddRange(children);
                if (!expanding)
                    treeNode.Expand();

                if (folder.Excluded)
                    ExcludeTreeNodeRecursive(treeNode);
            }
        }

        private void treeView_fileSystem_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode treeNode = e.Node as TreeNode;
            EnumerateTreeNode(treeNode, true);
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.Description = "Select the destination folder of the Advanced Copy";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("This will overwrite any files with the same names in the destination folder", "Overwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    CopyRecursive(BaseFolder, folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void CopyRecursive(FileSystemItem fileSystemItem, string destination)
        {
            if (fileSystemItem.Excluded)
            {
                // skip
            }
            else if (fileSystemItem.PartiallyExcluded)
            {
                if (!fileSystemItem.IsFolder)
                    throw new Exception("Only folders can be partially excluded");
                // recurse
                Folder folder = fileSystemItem as Folder;
                foreach (Folder subdirectory in folder.SubDirectories)
                    CopyRecursive(subdirectory, destination);
                foreach (File file in folder.Files)
                    CopyRecursive(file, destination);
            }
            else
            {
                string destinationPath = Path.Combine(destination, fileSystemItem.RelativePath);
                // perform copy
                if (fileSystemItem.IsFolder)
                {
                    DirectoryCopy(fileSystemItem.FullPath, destinationPath, true);
                }
                else
                {
                    string destinationFolder = Directory.GetParent(destinationPath).FullName;
                    Directory.CreateDirectory(destinationFolder);
                    System.IO.File.Copy(fileSystemItem.FullPath, destinationPath, true);
                }
            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
