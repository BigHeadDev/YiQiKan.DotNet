using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using YiQiKan.DotNet.Core.Interface;

namespace YiQiKan.DotNet.UWP.AppClasses {
    internal class Localization : ILocalization {
        private ResourceLoader resourceLoader;
        public Localization() {
            resourceLoader = ResourceLoader.GetForCurrentView("Statics");
        }

        public string Get(string key) {
            try {
                return resourceLoader.GetString(key);
            } catch { return $"#{key}#"; }
        }
    }
}
