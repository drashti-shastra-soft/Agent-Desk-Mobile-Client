using AgentDesk.ViewModels.Popups;
using AgentDesk.Views.Popups;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Services;

//public static class NetworkService
//{
//    public static async Task<bool> CheckInternetAsync()
//    {
//        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
//            return true;

//        var page = Application.Current?.Windows[0].Page;

//        if (page != null)
//        {
//            await page.DisplayAlert(
//                "No Internet",
//                "Please check your internet connection and try again.",
//                "OK");
//        }

//        //var popup = new CommonAlertPopupPage(
//        //               "Alert",
//        //               "Please check your internet connection and try again.",
//        //               "OK");

//        //popup.ActionEvent += async (s, e) =>
//        //{
//        //    await MopupService.Instance.PopAsync();
//        //};

//        //await MopupService.Instance.PushAsync(popup);

//        return false;
//    }
//}


public static class NetworkService
{
    public static async Task<bool> CheckInternetAsync()
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            return true;

        var viewModel = new CommonAlertPopupPageViewModel(
            "Alert",
            "Please check your internet connection and try again.",
            "OK");

        var popup = new CommonAlertPopupPage(viewModel);

        await MopupService.Instance.PushAsync(popup);

        return false;
    }
}