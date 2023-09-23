using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Service;
using YiQiKan.DotNet.Service.Exceptions;

namespace YiQiKan.DotNet.Core.ViewModel {
    public partial class LoginViewModel : ObservableObject {
        public LoginViewModel(IApp app, YiQiKanService yiQiKanService, ISetting setting,ILocalization l10n, ILoginDialog loginDialog) {
            this.app = app;
            this.yiQiKanService = yiQiKanService;
            this.setting = setting;
            l10N = l10n;
            this.loginDialog = loginDialog;
        }
        public LoginViewModel() {

        }
        [ObservableProperty]
        private int loginTypeIndex = 0;
        [ObservableProperty]
        private string loginPhone;
        [ObservableProperty]
        private string loginValidCode;
        [ObservableProperty]
        private bool isPhoneValid;
        [ObservableProperty]
        private Stream validCodeData;
        [ObservableProperty]
        private string validCode;
        [ObservableProperty]
        private string msgCode;
        [ObservableProperty]
        private string password;
        private readonly IApp app;
        private readonly YiQiKanService yiQiKanService;
        private readonly ISetting setting;
        private readonly ILocalization l10N;
        private readonly ILoginDialog loginDialog;

        partial void OnLoginPhoneChanged(string value) {
            IsPhoneValid = IsMobilePhone(value);
        }
        partial void OnIsPhoneValidChanged(bool value) {
            if (value) {
                _ = RefreshValideCodeImage();
            }
        }
        private static bool IsMobilePhone(string input) {
            Regex regex = new Regex("^1[34578]\\d{9}$");
            return regex.IsMatch(input);
        }
        [RelayCommand]
        private async Task RefreshValideCodeImage() {
            ValidCodeData = await yiQiKanService?.Account?.GetCodeImageStream("86", loginPhone);
        }
        [RelayCommand]
        private async Task SendPhoneCode() {
            if (!IsPhoneValid) {
                app.ShowToast(l10N.Get("InvalidPhoneTip"), 2000);
                return;
            }
            if (string.IsNullOrEmpty(ValidCode)) {
                app.ShowToast(l10N.Get("EmptyImageCode"), 2000);
                return;
            }
            try {
                await yiQiKanService.Account.SendPhoneCode(setting.DeviceId, "86", "中国大陆", ValidCode, false, LoginPhone);
            }catch(YiQiKanRequestException ex) {
                app.ShowToast(l10N.Get("CodeGetError"), 2000);
                await RefreshValideCodeImage();
            }
        }


        [RelayCommand]
        private async Task Login() {
            if (!IsPhoneValid) {
                app.ShowToast(l10N.Get("InvalidPhoneTip"), 2000);
                return;
            }
            try {
                if (LoginTypeIndex == 0) {
                    await LoginByPhoneAndCode();
                } else {
                    await LoginByPhoneAndPassword();
                }
            } catch (YiQiKanRequestException ex) {
                app.ShowToast(l10N.Get("LoginError"), 2000);
            }
        }

        private async Task LoginByPhoneAndCode() {
            if (string.IsNullOrEmpty(MsgCode)) {
                app.ShowToast(l10N.Get("EmptyMsgCode"), 2000);
                return;
            }
            var loginData = await yiQiKanService?.Account?.LoginByPhoneAndCode(setting.DeviceId, "86", "中国大陆", MsgCode, "", false, "RM-1111", "2.3.0", LoginPhone);
            setting.Token = loginData?.oAuth.token;
            loginDialog.Confirm();
        }

        private async Task LoginByPhoneAndPassword() {
            var loginData = await yiQiKanService?.Account?.LoginByAccountAndPassword(LoginPhone, Password);
            setting.Token = loginData?.oAuth.token;
            loginDialog.Confirm();
        }

    }
}
