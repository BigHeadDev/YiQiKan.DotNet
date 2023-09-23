﻿using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Core.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YiQiKan.DotNet.UWP.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginDialog : ContentDialog, ILoginDialog {
        public LoginDialog() {
            this.InitializeComponent();
        }
        private DialogResult dialogResult = DialogResult.Close;


        public void Cancel() {
            dialogResult = DialogResult.Cancel;
            Hide();
        }

        public void Close() {
            dialogResult = DialogResult.Close;
            Hide();
        }

        public void Confirm() {
            dialogResult = DialogResult.OK;
            Hide();
        }

        public async Task<DialogResult> ShowDialog(LoginViewModel loginViewModel) {
            DataContext = loginViewModel;
            await ShowAsync();
            return dialogResult;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            if (PrimaryButtonCommand != null) {
                PrimaryButtonCommand.Execute(null);
                args.Cancel = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
