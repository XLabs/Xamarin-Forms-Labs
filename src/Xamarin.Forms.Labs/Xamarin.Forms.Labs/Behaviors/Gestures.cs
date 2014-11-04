namespace Xamarin.Forms.Labs.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms.Labs.Controls;
    using Xamarin.Forms.Labs.Exceptions;

    /// <summary>
    /// Provides Gesture attached properties for the GesturesContentView
    /// This class has no involvement beyond setting up the interests
    /// It is here simply to make the users xaml a bit 
    /// more readable
    /// </summary>
    public class Gestures : BindableObject
    {   
        /// <summary>
        /// Definition for the Bindable LongPress Property.
        /// </summary>
        public static BindableProperty LongPressProperty = BindableProperty.CreateAttached<GesturesContentView, ICommand>((x) => x.GetValue<ICommand>(LongPressProperty), default(ICommand), BindingMode.OneWay, null,(bo,o,n)=> RegisterInterest(bo,n,RawGestures.LongPress,Directionality.None));
        /// <summary>
        /// Definition for the Bindable SingleTapProperty
        /// </summary>
        public static BindableProperty SingleTapProperty = BindableProperty.CreateAttached<GesturesContentView, ICommand>((x) => x.GetValue<ICommand>(SingleTapProperty), default(ICommand), BindingMode.OneWay, null, (bo, o, n) => RegisterInterest(bo, n, RawGestures.SingleTap, Directionality.None));
        /// <summary>
        /// Definition for the Bindable DoubleTapProperty
        /// </summary>
        public static BindableProperty DoubleTapProperty = BindableProperty.CreateAttached<GesturesContentView, ICommand>((x) => x.GetValue<ICommand>(DoubleTapProperty), default(ICommand), BindingMode.OneWay, null, (bo, o, n) => RegisterInterest(bo, n, RawGestures.DoubleTap, Directionality.None));
        /// <summary>
        /// Defintion for the Bindable SwipeLeft Proprety
        /// </summary>
        public static BindableProperty SwipeLeftProperty = BindableProperty.CreateAttached<GesturesContentView, ICommand>((x) => x.GetValue<ICommand>(SwipeLeftProperty), default(ICommand), BindingMode.OneWay, null, (bo, o, n) => RegisterInterest(bo, n, RawGestures.Swipe, Directionality.Left));
        /// <summary>
        /// Defintion for the Bindable SwipeRight Property
        /// </summary>
        public static BindableProperty SwipeRightProperty = BindableProperty.CreateAttached<GesturesContentView, ICommand>((x) => x.GetValue<ICommand>(SwipeRightProperty), default(ICommand), BindingMode.OneWay, null, (bo, o, n) => RegisterInterest(bo, n, RawGestures.Swipe, Directionality.Right));
        /// <summary>
        /// Defintion for the Bindable SwipeUp Property
        /// </summary>
        public static BindableProperty SwipeUpProperty = BindableProperty.CreateAttached<GesturesContentView, ICommand>((x) => x.GetValue<ICommand>(SwipeUpProperty), default(ICommand), BindingMode.OneWay, null, (bo, o, n) => RegisterInterest(bo, n, RawGestures.Swipe, Directionality.Up));
        /// <summary>
        /// Defintion for the Bindable SwipeDown Property
        /// </summary>
        public static BindableProperty SwipeDownProperty = BindableProperty.CreateAttached<GesturesContentView, ICommand>((x) => x.GetValue<ICommand>(SwipeDownProperty), default(ICommand), BindingMode.OneWay, null, (bo, o, n) => RegisterInterest(bo, n, RawGestures.Swipe, Directionality.Down));


        /// <summary>
        /// Definition for the Bindable LongPressParameter Property.
        /// </summary>
        public static BindableProperty LongPressParameterProperty = BindableProperty.CreateAttached<GesturesContentView, object>((x) => x.GetValue<object>(LongPressParameterProperty), default(object), BindingMode.OneWay, null, (bo, o, n) => RegisterInterestParameter(bo, n, RawGestures.LongPress, Directionality.None));
        /// <summary>
        /// Definition for the Bindable SingleTapParameter Property
        /// </summary>
        public static BindableProperty SingleTapParameterProperty = BindableProperty.CreateAttached<GesturesContentView, object>((x) => x.GetValue<object>(SingleTapParameterProperty), default(object), BindingMode.OneWay, null, (bo, o, n) => RegisterInterestParameter(bo, n, RawGestures.SingleTap, Directionality.None));
        /// <summary>
        /// Definition for the Bindable DoubleTapParameter Property
        /// </summary>
        public static BindableProperty DoubleTapParameterProperty = BindableProperty.CreateAttached<GesturesContentView, object>((x) => x.GetValue<object>(DoubleTapParameterProperty), default(object), BindingMode.OneWay, null, (bo, o, n) => RegisterInterestParameter(bo, n, RawGestures.DoubleTap, Directionality.None));
        /// <summary>
        /// Defintion for the Bindable SwipeLeftParameter Proprety
        /// </summary>
        public static BindableProperty SwipeLeftParameterProperty = BindableProperty.CreateAttached<GesturesContentView, object>((x) => x.GetValue<object>(SwipeLeftParameterProperty), default(object), BindingMode.OneWay, null, (bo, o, n) => RegisterInterestParameter(bo, n, RawGestures.Swipe, Directionality.Left));
        /// <summary>
        /// Defintion for the Bindable SwipeRightParameter Property
        /// </summary>
        public static BindableProperty SwipeRightParameterProperty = BindableProperty.CreateAttached<GesturesContentView, object>((x) => x.GetValue<object>(SwipeRightParameterProperty), default(object), BindingMode.OneWay, null, (bo, o, n) => RegisterInterestParameter(bo, n, RawGestures.Swipe, Directionality.Right));
        /// <summary>
        /// Defintion for the Bindable SwipeUpParameter Property
        /// </summary>
        public static BindableProperty SwipeUpParameterProperty = BindableProperty.CreateAttached<GesturesContentView, object>((x) => x.GetValue<object>(SwipeUpParameterProperty), default(object), BindingMode.OneWay, null, (bo, o, n) => RegisterInterestParameter(bo, n, RawGestures.Swipe, Directionality.Up));
        /// <summary>
        /// Defintion for the Bindable SwipeDownParameter Property
        /// </summary>
        public static BindableProperty SwipeDownParameterProperty = BindableProperty.CreateAttached<GesturesContentView, object>((x) => x.GetValue<object>(SwipeDownParameterProperty), default(object), BindingMode.OneWay, null, (bo, o, n) => RegisterInterestParameter(bo, n, RawGestures.Swipe, Directionality.Down));


        /// <summary>
        /// Used by the lamda expression in the various property definions
        /// to register interest in a gesture with the containing <see cref="GesturesContentView"/>
        /// </summary>
        /// <param name="bo">The bindable object this property is attached to</param>
        /// <param name="command">The Command to execute</param>
        /// <param name="r">The <see cref="RawGestures"/> of interest</param>
        /// <param name="d">The <see cref="Directionality"/> of interest (SingleTap,DoubleTap and LongPress have a directionality of None)</param>
        internal static void RegisterInterest(BindableObject bo, ICommand command,RawGestures r,Directionality d)
        {
            //Find the specific GesturesContentView that is needed....
            var gcv = FindContentViewParent(bo);
            gcv.RegisterInterest(bo as View,r,d,command);

        }

        /// <summary>
        /// Used by the lamda expression in the various property parameter definions
        /// to register interest in a gesture with the containing <see cref="GesturesContentView"/>
        /// If the param is not a bound item (ie a constant like "boo") this will fail
        /// constants are set before there is a parent.  
        /// </summary>
        /// <param name="bo">The bindable object this property is attached to</param>
        /// <param name="param">The Command Parameter</param>
        /// <param name="r">The <see cref="RawGestures"/> of interest</param>
        /// <param name="d">The <see cref="Directionality"/> of interest (SingleTap,DoubleTap and LongPress have a directionality of None)</param>
        internal static void RegisterInterestParameter(BindableObject bo, object param, RawGestures r, Directionality d)
        {
            var gcv = FindContentViewParent(bo,false);
            if (gcv == null)//This can happen if the param is a non bound value.  
            {
                var view = bo as View;
                if (view != null)
                {
                    PendingInterestParameterss.Add(new PendingInterestParams { View = view, Gesture = r, Direction = d, ParamValue = param });
                    view.PropertyChanged += ViewPropertyChanged;
                }
                else
                {
                    throw new InvalidBindableException(bo, typeof(View));
                }
            }
            else
                gcv.RegisterInterestParamaeter(bo as View,r,d,param);

        }

        private static void ViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Unfortunately the Parent property doesn't signal a change
            //However when the renderer is set it should have it's parent
            if (e.PropertyName == "Renderer")
            {
                var view = sender as View;
                var pending = PendingInterestParameterss.Where(x => x.View == view).ToList();
                foreach (var pendingparam in pending)
                {
                    var gcv = FindContentViewParent(sender as BindableObject);
                    gcv.RegisterInterestParamaeter(view,pendingparam.Gesture,pendingparam.Direction,pendingparam.ParamValue);
                    PendingInterestParameterss.Remove(pendingparam);
                }
                view.PropertyChanged -= ViewPropertyChanged;
            }

        }

        /// <summary>
        /// Utility function to find the first containing <see cref="GesturesContentView"/>
        /// </summary>
        /// <param name="bo">The Bindable object to start from</param>
        /// <param name="throwException">True to throw an excpetion if the parent is not found</param>
        /// <returns></returns>
        private static GesturesContentView FindContentViewParent(BindableObject bo,bool throwException=true)
        {
            var view = bo as View;
            if (view == null)
                throw new InvalidBindableException(bo, typeof(View));
            var history = new List<string>();
            if (view is GesturesContentView) return view as GesturesContentView;
            history.Add(view.GetType().Name);
            var parent = view.Parent;
            while (parent != null && !(parent is GesturesContentView))
            {
                history.Add(parent.GetType().Name);
                parent = parent.Parent;
            }
            if (parent == null && throwException)
                throw new InvalidNestingException(typeof(Gestures),typeof(GesturesContentView),history);
            return parent as GesturesContentView;
        }

        private static readonly List<PendingInterestParams>  PendingInterestParameterss=new List<PendingInterestParams>();

        private class PendingInterestParams
        {
            public View View { get; set; }
            public RawGestures Gesture { get; set; }
            public Directionality Direction { get; set; }
            public object ParamValue { get; set; }
        }
    }


}
