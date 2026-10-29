using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AgentDesk.ViewModels.Popups;

//public partial class CommonPopupPageViewModel:BaseViewModel
//{
//    [ObservableProperty]
//    private string title = "Alert";

//    [ObservableProperty]
//    private string message = string.Empty;

//    public event EventHandler? Confirmed;
//    public event EventHandler? Cancelled;

//    public CommonPopupPageViewModel(bool isDelete = false,bool isExit = false)
//    {       
//        if (isDelete)
//        {
//            Message = "Are you sure want to delete this item?";
//        }
//        else
//        {
//            Message = "Are you sure want to logout?";
//        }

//        if (isExit)
//        {
//            Title = "Exit App";
//            Message = "Are you sure you want to exit?";
//        }
//    }

//    [RelayCommand]
//    private void Yes()
//    {
//        Confirmed?.Invoke(this, EventArgs.Empty);
//    }

//    [RelayCommand]
//    private void No()
//    {
//        Cancelled?.Invoke(this, EventArgs.Empty);
//    }
//}

public partial class CommonPopupPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string title = "Alert";

    [ObservableProperty]
    private string message = string.Empty;

    public event EventHandler? Confirmed;
    public event EventHandler? Cancelled;

    public CommonPopupPageViewModel(
        string title = "Alert",
        string message = "Are you sure?")
    {
        Title = title;
        Message = message;
    }

    [RelayCommand]
    private void Yes()
    {
        Confirmed?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void No()
    {
        Cancelled?.Invoke(this, EventArgs.Empty);
    }
}
