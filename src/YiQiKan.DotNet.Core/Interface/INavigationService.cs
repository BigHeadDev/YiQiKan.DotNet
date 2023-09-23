using System;
using System.Collections.Generic;
using System.Text;
using YiQiKan.DotNet.Core.Enums;

namespace YiQiKan.DotNet.Core.Interface {
    /// <summary>
    /// 这是导航的接口抽象
    /// 软件分为一个导航，用来导航整个软件的界面，一个子导航，用来导航主页的一些界面
    /// </summary>
    public interface INavigationService {
        //导航的一些事件，外部可以+=去订阅导航事件
        event EventHandler<PageType> RootNavigated;
        event EventHandler<PageType> ChildNavigated;
        PageType CurrentRootPage { get; }
        PageType CurrentChildPage { get; }
        /// <summary>
        /// 导航到某一个页面
        /// </summary>
        /// <param name="pageType">页面类型</param>
        /// <param name="isRoot">区分是主导航还是子导航</param>
        /// <param name="param">导航参数</param>
        void NavigateTo(PageType pageType, bool isRoot = false, object param = null);
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="isRoot">区分是主导航还是子导航</param>
        void GoBack(bool isRoot = false);
        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="isRoot">区分是主导航还是子导航</param>
        void GoForward(bool isRoot = false);
    }
}
