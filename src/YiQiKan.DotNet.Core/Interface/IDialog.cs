using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.ViewModel;

namespace YiQiKan.DotNet.Core.Interface {
    public interface IDialog {
        void Close();
        void Confirm();
        void Cancel();
        Task<DialogResult> ShowDialog(LoginViewModel loginViewModel);
    }
}
