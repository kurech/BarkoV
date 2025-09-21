using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilyazoV
{
    public class Generator
    {
        public List<string> ViewTree(TreeView tree, bool edit = true)
        {
            List<string> treeContent = new List<string>();
            foreach (TreeNode node in tree.Nodes)
            {
                treeContent.AddRange(ViewNode(node, edit));
            }
            return treeContent;
        }
        public List<string> ViewNode(TreeNode node, bool edit)
        {
            List<string> nodeContent = new List<string>();
            foreach (TreeNode child in node.Nodes)
            {
                nodeContent.AddRange(ViewNode(child, edit));
            }
            if (edit)
            {
                int value = -1;
                if (int.TryParse(node.Text, out value))
                {
                    int newValue = ConvertToBase10(value);
                    node.Text = newValue.ToString();
                }
            }
            nodeContent.Add(node.Text);
            return nodeContent;
        }

        public int ConvertToBase10(int num)
        {
            int k = 0;
            for (int i = 0; i < num.ToString().Length; i++)
            {
                int r = num.ToString().Length - i;
                int v = num.ToString()[i] == '1' ? 1 : 0;
                k += (int)Math.Pow(2, r - 1) * v;
            }
            return k;
        }

        public TreeView currentTree;
        public int depth = 0;
        public void Restruct(TreeView box, TreeView tree)
        {
            currentTree = tree;
            tree.Nodes.Clear();
            TreeNode sNode = null;
            foreach (TreeNode node in box.Nodes)
            {
                TreeNode newNode = new TreeNode(node.Text);
                //MessageBox.Show(node.Text);
                ReNode(null, node, ref newNode);

                if (newNode.Text == "S")
                {
                    sNode = newNode;
                    TreeNode nNode = new TreeNode("");
                    sNode.Nodes.Add(nNode);
                    sNode = nNode;
                }

                tree.Nodes.Add(newNode);
            }

            List<TreeNode> remove = new List<TreeNode>();
            foreach (TreeNode node in tree.Nodes)
            {
                if (node.Text != "S" && sNode != null)
                {
                    remove.Add(node);
                }
            }

            TreeNode neNode = new TreeNode("");
            sNode.Nodes.Add(neNode);

            foreach (TreeNode node in remove)
            {
                if (node.Text != "A")
                {
                    tree.Nodes.Remove(node);
                    sNode.Nodes.Add(node);
                }
                else
                {
                    tree.Nodes.Remove(node);
                    neNode.Nodes.Add(node);
                }
            }


            tree.ExpandAll();
        }

        public void ReNode(TreeNode parent, TreeNode node, ref TreeNode newNode)
        {
            bool skipToRoot = false;
            foreach (TreeNode child in node.Nodes)
            {
                //MessageBox.Show(child.Text);
                TreeNode newn = new TreeNode(child.Text);

                int value = -1;
                if (int.TryParse(newn.Text, out value))
                {
                    int newValue = ConvertToBase10(value);
                    newn.Text = newValue.ToString();
                }

                if (child.FullPath.Contains('('))
                {
                    skipToRoot = true;
                }
                ReNode(newNode, child, ref newn);
            }
            if (parent != null)
            {
                if (currentTree != null && skipToRoot)
                {
                    currentTree.Nodes.Add(newNode);
                }
                else
                {
                    parent.Nodes.Add(newNode);
                }
            }
        }
    }
}
