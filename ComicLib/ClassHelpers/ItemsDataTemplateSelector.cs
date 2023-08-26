using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ComicLib
{
    public class ItemsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FirstItem { get; set; }
        public DataTemplate OtherItem { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                return OtherItem;
            }
            else
            {   
                return FirstItem;
            }
            //var itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            //DataTemplate _returnTemplate = itemsControl.IndexFromContainer(container) == 0 ? FirstItem : OtherItem;
            //return _returnTemplate;
        }
    }
}
