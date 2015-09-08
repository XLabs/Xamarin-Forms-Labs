using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace XLabs.Forms
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Xamarin.Forms;
    using NativeView = global::Android.Views.View;

    public static class ViewExtensions
    {
        public static View FindFormsViewFromAccessibilityId(this View view, NativeView nativeView)
        {
            View formsView = null;

            var id = nativeView.ContentDescription;

            if (string.IsNullOrWhiteSpace(id))
            {
                formsView = null;
            }
            else if (view.StyleId == id)
            {
                formsView = view;
            }
            else
            {
                var d = view.GetInternalChildren();

                formsView = d == null ? null : d.OfType<View>().FirstOrDefault(a => a.StyleId == id);
            }

            return formsView;
        }

        public static ObservableCollection<Element> GetInternalChildren(this View view)
        {
            var internalPropertyInfo = view.GetType().GetProperty("InternalChildren", BindingFlags.NonPublic | BindingFlags.Instance);

            return (internalPropertyInfo == null) ? null : internalPropertyInfo.GetValue(view) as ObservableCollection<Element>;
        }

        public static NativeView GetNativeContent(this View view)
        {
            PropertyInfo controlProperty= view.GetType().GetProperty("Control", BindingFlags.Public | BindingFlags.Instance);

            return (controlProperty == null) ? null : controlProperty.GetValue(view) as NativeView;
        }
    }
}