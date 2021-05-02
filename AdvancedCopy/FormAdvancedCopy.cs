using FileSystemParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedCopy
{
    public partial class FormAdvancedCopy : Form
    {
        public FormAdvancedCopy()
        {
            InitializeComponent();
        }

        private void FormAdvancedCopy_Load(object sender, EventArgs e)
        {
            Folder baseFolder = new Folder(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())))), null);

            TreeNode rootNode = ItemToTreeNode(baseFolder);
            rootNode.Expand();
            treeView_fileSystem.Nodes.Add(rootNode);
        }

        private TreeNode ItemToTreeNode(FileSystemItem fileSystemItem)
        {

            TreeNode node = new TreeNode(fileSystemItem.Name);
            node.NodeFont = this.Font;

            if (fileSystemItem.IsFolder())
            {
                Folder folder = fileSystemItem as Folder;
                TreeNode[] children = folder.SubDirectories.Select(x => ItemToTreeNode(x)).Concat(folder.Files.Select(x => ItemToTreeNode(x))).ToArray();
                node.Nodes.AddRange(children);
                node.StateImageKey = "folder.png";
            }

            node.Tag = new FileSystemItemInfo(fileSystemItem);

            return node;
        }

        private void CheckParentNodeForAllIncluded(TreeNode treeNode)
        {
            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                FileSystemItemInfo childFileSystemItemInfo = childTreeNode.Tag as FileSystemItemInfo;
                if (childFileSystemItemInfo.Excluded)
                    return;
            }
            IncludeTreeNode(treeNode);
        }

        private void IncludeTreeNode(TreeNode treeNode)
        {
            FileSystemItemInfo fileSystemItemInfo = treeNode.Tag as FileSystemItemInfo;
            treeNode.NodeFont = new Font(treeNode.NodeFont, FontStyle.Regular);
            fileSystemItemInfo.Excluded = false;

            if (treeNode.Parent != null)
                CheckParentNodeForAllIncluded(treeNode.Parent);
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
                FileSystemItemInfo childFileSystemItemInfo = childTreeNode.Tag as FileSystemItemInfo;
                if (!childFileSystemItemInfo.Excluded)
                    return;
            }
            ExcludeTreeNode(treeNode);
        }

        private void ExcludeTreeNode(TreeNode treeNode)
        {
            FileSystemItemInfo fileSystemItemInfo = treeNode.Tag as FileSystemItemInfo;
            treeNode.NodeFont = new Font(treeNode.NodeFont, FontStyle.Strikeout);
            fileSystemItemInfo.Excluded = true;

            if (treeNode.Parent != null)
                CheckParentNodeForAllExcluded(treeNode.Parent);
        }

        private void ExcludeTreeNodeRecursive(TreeNode treeNode)
        {
            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                ExcludeTreeNodeRecursive(childTreeNode);
            }

            ExcludeTreeNode(treeNode);
        }

        private void treeView_fileSystem_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            FileSystemItemInfo fileSystemItemInfo = e.Node.Tag as FileSystemItemInfo;
            if (fileSystemItemInfo.Excluded)
                IncludeTreeNodeRecursive(e.Node);
            else
                ExcludeTreeNodeRecursive(e.Node);
        }
    }

    public class FileSystemItemInfo
    {
        public FileSystemItem Item;
        public bool Excluded = false;

        public FileSystemItemInfo(FileSystemItem _item)
        {
            this.Item = _item;
        }
    }
}
