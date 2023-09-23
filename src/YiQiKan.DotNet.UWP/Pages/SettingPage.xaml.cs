using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YiQiKan.DotNet.Core.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YiQiKan.DotNet.UWP.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page {
        public SettingPage() {
            this.InitializeComponent();
            var version = Package.Current.Id.Version;
            txtVersion.Text = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            txtName.Text = Package.Current.DisplayName;
            txtYear.Text = Package.Current.InstalledDate.Year.ToString();
            txtAuthor.Text = Package.Current.PublisherDisplayName;
            DataContext = DependencyInjectionExtensions.ServiceProvider.GetService<SettingViewModel>();
        }
        private static StoreContext storeContext = StoreContext.GetDefault();
        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e) {
            try {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?productid=9MTT8J8521SW"));
            } catch (Exception) {

            }
        }
        

        private async void Button_Click(object sender, RoutedEventArgs e) {
            try {
                await storeContext.RequestRateAndReviewAppAsync();
            } catch (Exception) {

            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            string[] productKinds = { "Durable", "Consumable", "UnmanagedConsumable" };
            List<String> filterList = new List<string>(productKinds);
            StoreProductQueryResult storeProductQueryResult = await storeContext.GetAssociatedStoreProductsAsync(filterList);
            var gifts = storeProductQueryResult.Products.Select(g => new Gift {
                Id = g.Key,
                Image = g.Value.Images.FirstOrDefault()?.Uri,
                Title = g.Value.Title,
                Description = g.Value.Description,
                Price = g.Value.Price.FormattedPrice
            });
            giftSettingExpander.ItemsSource = gifts.Reverse();
        }
        public class Gift {
            public string Id { get; set; }
            public Uri Image { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
            private RelayCommand<string> purchaseGiftCommand = null;
            public RelayCommand<string> PurchaseGiftCommand {
                get {
                    return purchaseGiftCommand ?? new RelayCommand<string>((id) => {
                        PurchaseAddOn(id);
                    });
                }
            }
            public async void PurchaseAddOn(string storeId) {
                if (storeContext == null) {
                    storeContext = StoreContext.GetDefault();
                    // If your app is a desktop app that uses the Desktop Bridge, you
                    // may need additional code to configure the StoreContext object.
                    // For more info, see https://aka.ms/storecontext-for-desktop.
                }

                StorePurchaseResult result = await storeContext.RequestPurchaseAsync(storeId);
                

                // Capture the error message for the operation, if any.
                string extendedError = string.Empty;
                if (result.ExtendedError != null) {
                    extendedError = result.ExtendedError.Message;
                }

                switch (result.Status) {
                    case StorePurchaseStatus.AlreadyPurchased:
                        //textBlock.Text = "The user has already purchased the product.";
                        break;

                    case StorePurchaseStatus.Succeeded:
                        //textBlock.Text = "The purchase was successful.";
                        break;

                    case StorePurchaseStatus.NotPurchased:
                        //textBlock.Text = "The purchase did not complete. " +
                           // "The user may have cancelled the purchase. ExtendedError: " + extendedError;
                        break;

                    case StorePurchaseStatus.NetworkError:
                        //textBlock.Text = "The purchase was unsuccessful due to a network error. " +
                           // "ExtendedError: " + extendedError;
                        break;

                    case StorePurchaseStatus.ServerError:
                        //textBlock.Text = "The purchase was unsuccessful due to a server error. " +
                            //"ExtendedError: " + extendedError;
                        break;

                    default:
                        //textBlock.Text = "The purchase was unsuccessful due to an unknown error. " +
                            //"ExtendedError: " + extendedError;
                        break;
                }
            }
        }
    }
}
