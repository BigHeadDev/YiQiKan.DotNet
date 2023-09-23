using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core;
using YiQiKan.DotNet.Core.Enums;

namespace YiQiKan.DotNet.Core.Interface {
    /// <summary>
    /// 这个接口是App平台的一些功能抽象，每个框架都不一样，需要在各自的平台去实现
    /// 但是Core项目，就不需要管具体的实现方式，直接调用这个接口
    /// </summary>
    public interface IApp {
        //平台data文件夹
        string DataPath { get; }
        //平台名称
        Assembly Assembly { get; }
        //弹窗
        DialogResult ShowDialog(string title, string message);
        //土司通知
        void ShowToast(string message, double seconds);
        Task UIInvoke(Action action);

        void ChangeTheme(ThemeType themeType);
    }
}
