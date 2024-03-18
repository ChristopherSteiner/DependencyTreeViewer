using System.Windows.Controls;

namespace DependencyTreeViewer.UI.Helpers;

public static class TreeViewExtension
{
    public static void ExpandAll(this TreeView treeView)
    {
        foreach (var item in treeView.Items)
        {
            if (treeView.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeViewItem)
            {
                treeViewItem.ExpandSubtree();
            }
        }
    }
}
