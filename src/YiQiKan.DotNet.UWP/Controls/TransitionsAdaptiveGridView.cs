using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace YiQiKan.DotNet.UWP.Controls {
    public class TransitionsAdaptiveGridView :AdaptiveGridView{


        protected override void OnItemsChanged(object e) {
            base.OnItemsChanged(e);
            ItemsPresenter itemsPresenter = FindVisualChild<ItemsPresenter>(this);
            var panel = FindVisualChild<Panel>(itemsPresenter);
            panel.ChildrenTransitions.Add(new EntranceThemeTransition {
                IsStaggeringEnabled = true,
                FromVerticalOffset = 100,
            });
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
