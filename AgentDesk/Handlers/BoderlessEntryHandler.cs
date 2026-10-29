using AgentDesk.CustomeControls;
using Microsoft.Maui.Handlers;
#if ANDROID 
using Android.Widget;
#elif IOS
using UIKit;
#endif

namespace AgentDesk.Handlers;

public class BoderlessEntryHandler
{
    public static void RemoveBorder()
    {
        EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
#if ANDROID
            if (view is BorderlessEntry && handler.PlatformView is EditText editText)
            {
                // Remove background/border
                editText.Background = null;
                editText.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
#elif IOS
            if (view is BorderlessEntry && handler.PlatformView is UITextField uiTextField)
            {
                uiTextField.BorderStyle = UITextBorderStyle.None;
            }
#endif
        });

        DatePickerHandler.Mapper.AppendToMapping(nameof(BorderlessDatePicker), (handler, view) =>
        {
#if ANDROID
            if (view is BorderlessDatePicker && handler.PlatformView is EditText editText)
            {
                // Remove background/border
                editText.Background = null;
                editText.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
#elif IOS
                        if (view is BorderlessDatePicker && handler.PlatformView is UITextField uiTextField)
                        {
                            uiTextField.BorderStyle = UITextBorderStyle.None;
                        }
#endif
        });

        SearchBarHandler.Mapper.AppendToMapping(nameof(BorderlessSearchBar), (handler, view) =>
        {
#if ANDROID
            if (view is BorderlessSearchBar)
            {
                var context = handler.PlatformView.Context!;

                // Appcompat SearchView ids live in the app package; fall back to the android: id
                var plateId = context.Resources!.GetIdentifier("search_plate", "id", context.PackageName);
                if (plateId == 0)
                    plateId = context.Resources.GetIdentifier("android:id/search_plate", null, null);

                var plate = handler.PlatformView.FindViewById(plateId);
                plate?.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
#elif IOS
    if (view is BorderlessSearchBar)
    {
        handler.PlatformView.BackgroundImage = new UIKit.UIImage();
        handler.PlatformView.SearchTextField.BorderStyle = UITextBorderStyle.None;
    }
#endif
        });

        //        TimePickerHandler.Mapper.AppendToMapping(nameof(BorderlessTimePicker), (handler, view) =>
        //        {
        //#if ANDROID
        //            if (handler.PlatformView is Android.Widget.EditText editText)
        //            {
        //                editText.Background = null;
        //                editText.SetBackgroundColor(Android.Graphics.Color.Transparent);
        //                editText.BackgroundTintList =
        //                    Android.Content.Res.ColorStateList.ValueOf(
        //                        Android.Graphics.Color.Transparent);
        //            }
        //#elif IOS
        //               if (handler.PlatformView is UIKit.UITextField uiTextField)
        //    {
        //        uiTextField.BorderStyle = UIKit.UITextBorderStyle.None;
        //        uiTextField.BackgroundColor = UIKit.UIColor.Clear;
        //    }
        //#endif
        //        });

        //        EditorHandler.Mapper.AppendToMapping(nameof(BorderlessEditor), (handler, view) =>
        //        {
        //#if ANDROID
        //            if (view is BorderlessEditor && handler.PlatformView is EditText editText)
        //            {
        //                // Remove border/background
        //                editText.Background = null;
        //                editText.SetBackgroundColor(Android.Graphics.Color.Transparent);
        //            }
        //#elif IOS
        //                if (view is BorderlessEditor && handler.PlatformView is UITextView uiTextView)
        //                {
        //                    uiTextView.BackgroundColor = UIKit.UIColor.Clear;
        //                    uiTextView.Layer.BorderWidth = 0;
        //                }
        //#endif
        //        });


        PickerHandler.Mapper.AppendToMapping(nameof(BorderlessPicker), (handler, view) =>
        {
#if ANDROID
            if (view is BorderlessPicker && handler.PlatformView is EditText editText)
            {
                editText.Background = null;
                editText.SetBackgroundColor(Android.Graphics.Color.Transparent);
                editText.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(
                        Android.Graphics.Color.Transparent);
            }
#elif IOS
            if (view is BorderlessPicker && handler.PlatformView is UITextField uiTextField)
            {
                uiTextField.BorderStyle = UITextBorderStyle.None;
                uiTextField.BackgroundColor = UIColor.Clear;
            }
#endif
        });
    }
}
