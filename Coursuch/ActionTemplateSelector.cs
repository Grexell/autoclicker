using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Coursuch
{
    public class ActionTemplateSelector:DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            AbstractAction action = item as AbstractAction;
            if (action is MouseAction)
            {
                return element.FindResource("MouseAction") as DataTemplate;
            }
            else
            {
                return element.FindResource("KeyboardAction") as DataTemplate;
            }
        }
    }
}
