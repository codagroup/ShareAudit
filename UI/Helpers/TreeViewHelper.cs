using System.Windows;
using System.Windows.Controls;

namespace ShareAudit.UI.Helpers;

public class TreeViewHelper
{
    // Using a DependencyProperty as the backing store for SelectedItem. This enables animation,
    // styling, binding, etc...
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new UIPropertyMetadata(null, OnSelectedItemChanged));

    private readonly static Dictionary<DependencyObject, TreeViewSelectedItemBehavior> behaviors = [];

    [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
    public static object GetSelectedItem(DependencyObject obj)
    {
        return (object)obj.GetValue(SelectedItemProperty);
    }

    public static void SetSelectedItem(DependencyObject obj, object value)
    {
        obj.SetValue(SelectedItemProperty, value);
    }

    private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is not TreeView)
        {
            return;
        }

        if (!behaviors.TryGetValue(obj, out TreeViewSelectedItemBehavior? view))
        {
            view = new TreeViewSelectedItemBehavior((obj as TreeView)!);
            behaviors.Add(obj, view);
        }

        view.ChangeSelectedItem(e.NewValue);
    }

    private class TreeViewSelectedItemBehavior
    {
        private readonly TreeView _view;

        public TreeViewSelectedItemBehavior(TreeView view)
        {
            _view = view;
            view.SelectedItemChanged += (sender, e) => SetSelectedItem(view, e.NewValue);
        }

        internal void ChangeSelectedItem(object p)
        {
            if (_view.ItemContainerGenerator.ContainerFromItem(p) is TreeViewItem item)
            {
                item.IsSelected = true;
            }
        }
    }
}
