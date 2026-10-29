using AgentDesk.Views.Popups;
using Mopups.Services;

namespace AgentDesk.Services;

public static class PopLoaderService
{
    private static CustomLoaderPopup? _loaderPopup;
    private static bool _isShowing;


    public static async Task ShowAsync()
    {
        if (_isShowing)
            return;

        _isShowing = true;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            _loaderPopup = new CustomLoaderPopup();

            await MopupService.Instance.PushAsync(_loaderPopup);
        });
    }



    public static async Task HideAsync()
    {
        if (!_isShowing || _loaderPopup == null)
            return;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await MopupService.Instance.RemovePageAsync(_loaderPopup);

            _loaderPopup = null;
            _isShowing = false;
        });
    }
}

//public static class PopLoaderService
//{
//    private static CustomLoaderPopup? _loaderPopup;
//    private static bool _isShowing;

//    public static async Task ShowAsync()
//    {
//        await MainThread.InvokeOnMainThreadAsync(async () =>
//        {
//            if (_isShowing)
//                return;

//            var popup = new CustomLoaderPopup();

//            // Set state only after PushAsync succeeds.
//            await MopupService.Instance.PushAsync(popup);

//            _loaderPopup = popup;
//            _isShowing = true;
//        });
//    }

//    public static async Task HideAsync()
//    {
//        await MainThread.InvokeOnMainThreadAsync(async () =>
//        {
//            var popup = _loaderPopup;

//            if (!_isShowing || popup == null)
//                return;

//            try
//            {
//                // Remove only our loader, and only if it is
//                // still present in the popup navigation stack.
//                if (MopupService.Instance.PopupStack.Contains(popup))
//                {
//                    await MopupService.Instance.RemovePageAsync(popup);
//                }
//            }
//            finally
//            {
//                _loaderPopup = null;
//                _isShowing = false;
//            }
//        });
//    }
//}
