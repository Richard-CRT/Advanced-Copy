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
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "Select the root folder of the Advanced Copy";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Folder baseFolder = new Folder(folderBrowserDialog.SelectedPath, null);
                BaseFolder = baseFolder;

                TreeNode rootNode = ItemToTreeNode(baseFolder);
                treeView_fileSystem.Nodes.Add(rootNode);
                rootNode.Expand();
            }
            else
            {
                this.Close();
            }
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
            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                FileSystemItem childFileSystemItem = childTreeNode.Tag as FileSystemItem;
                if (childFileSystemItem.Excluded)
                    return;
            }
            IncludeTreeNode(treeNode);
        }

        private void IncludeTreeNode(TreeNode treeNode)
        {
            FileSystemItem fileSystemItem = treeNode.Tag as FileSystemItem;
            if (fileSystemItem != null)
            {
                treeNode.NodeFont = new Font(treeNode.NodeFont, FontStyle.Regular);
                treeNode.ForeColor = Color.Black;
                fileSystemItem.Excluded = false;

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
            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                FileSystemItem childFileSystemItem = childTreeNode.Tag as FileSystemItem;
                if (!childFileSystemItem.Excluded)
                    return;
            }
            ExcludeTreeNode(treeNode);
        }

        private void ExcludeTreeNode(TreeNode treeNode)
        {
            FileSystemItem fileSystemItem = treeNode.Tag as FileSystemItem;
            if (fileSystemItem != null)
            {
                treeNode.NodeFont = new Font(treeNode.NodeFont, FontStyle.Strikeout);
                treeNode.ForeColor = Color.Red;
                fileSystemItem.Excluded = true;

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

            /*
            if (fullPathsToExclude.Contains(fileSystemItem.FullPath))
            {
                TreeNode treeNode = fileSystemItem.TreeNode as TreeNode;
                ExcludeTreeNodeRecursive(treeNode);
            }
            if (fileSystemItem.IsFolder)
            {
                Folder folder = fileSystemItem as Folder;
                foreach (Folder subdirectory in folder.SubDirectories)
                    ApplyExcludedPathsRecursive(subdirectory, fullPathsToExclude);
                foreach (File file in folder.Files)
                    ApplyExcludedPathsRecursive(file, fullPathsToExclude);
            }
            */
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
    }
}
