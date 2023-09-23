using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;

namespace YiQiKan.DotNet.WPF.AppClasses {
    internal class WPFNavigationService : INavigationService {
        public event EventHandler<PageType> RootNavigated;
        public event EventHandler<PageType> ChildNavigated;

        public void GoBack(bool isRoot = false) {
        }

        public void GoForward(bool isRoot = false) {
        }

        public void NavigateTo(PageType pageType, bool isRoot = false, object param = null) {
            
        }
    }
}
