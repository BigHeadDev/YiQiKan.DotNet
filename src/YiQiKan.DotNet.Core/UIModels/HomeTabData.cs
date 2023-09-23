using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Model.Enums;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet.Core.UIModels {
    public partial class HomeTabData : ObservableObject {
        public HomeTabData(string title, Category category) {
            Title = title;
            Category = category;
        }
        public string Title { get; set; }
        public Category Category { get; set; }
        [ObservableProperty]
        private HomeListData data;
    }
}
