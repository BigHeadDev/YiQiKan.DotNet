using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YiQiKan.DotNet.Core;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.WPF.Pages;

namespace YiQiKan.DotNet.WPF.AppClasses {
    public class WPFApp : IApp {
        public string Platform => "WPF .Net 7";

        public void OnInitSucess() {
            
        }

        public DialogResult ShowDialog(string title, string message) {
            var result = MessageBox.Show(title, message);
            return result switch {
                MessageBoxResult.OK => DialogResult.OK,
                MessageBoxResult.Yes => DialogResult.OK,
                MessageBoxResult.No => DialogResult.Cancel,
                MessageBoxResult.Cancel => DialogResult.Cancel,
                MessageBoxResult.None => DialogResult.Close,
                _ => DialogResult.Close
            } ;
        }

        public void ShowToast(string message, double seconds) {
             MessageBox.Show(message, message);
        }
    }
}
