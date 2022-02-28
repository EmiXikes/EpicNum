using System.Windows;
using System.Windows.Controls;

namespace EpicNum.View.Nodes
{
    class NodeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
           
            FrameworkElement UIElement = container as FrameworkElement;

            // Add new information for new Node Template selections here...
            if (item.GetType() == typeof(ViewModel.NodeAdder))
            {
                return UIElement.FindResource("TemplateAdderNode") as DataTemplate;
            }
            else if (item.GetType() == typeof(ViewModel.NodeText))
            {
                return UIElement.FindResource("TemplateTextNode") as DataTemplate;
            }

            return base.SelectTemplate(item, container);

        }
    }
}
