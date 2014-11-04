namespace Xamarin.Forms.Labs.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// Uses attached properties to 
    /// </summary>
    public class GesturesContentView : ContentView
    {
        private readonly List<ViewInterest> viewInterests=new List<ViewInterest>();

        /// <summary>
        /// Property Definition for the <see cref="Accuracy"/> Property
        /// </summary>
        public static BindableProperty AccuracyProperty =BindableProperty.Create<GesturesContentView, float>(x=>x.Accuracy,5.0f,BindingMode.OneWay,(bo, val) => val >= 5 && val<=25);

        /// <summary>
        /// Property Definition for the Bindable <see cref="MinimumSwipeLength"/> property
        /// </summary>
        public static BindableProperty MinimumSwipeLengthProperty =BindableProperty.Create<GesturesContentView, float>(x => x.MinimumSwipeLength,25,BindingMode.OneWay,(bo, val) => val >= 10);

        /// <summary>
        /// The minimum gesture length to be considered a valid swipe
        /// Default value is 25
        /// Minimum value is 10 there is no predefined maximum
        /// </summary>
        public float MinimumSwipeLength
        {
            get { return (float)GetValue(MinimumSwipeLengthProperty); }
            set { SetValue(MinimumSwipeLengthProperty,value);}
        }
        /// <summary>
        /// The maximum distance a gesture origin point can be from
        /// an interested view.  ie: How close the user must be to the view
        /// Minimum value is 5 maximum is 25
        /// </summary>
        public float Accuracy
        {
            get { return (float)GetValue(AccuracyProperty); }
            set { SetValue(AccuracyProperty,value);}
        }
        /// <summary>
        /// Event that can be hooked from codebehind files.
        /// When invoked the sender is the view where the gesture originated.
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRecognized;
        /// <summary>
        /// Used to register interest ina gesture on behalf of a contained view
        /// </summary>
        /// <param name="view">The contained View <see cref="Xamarin.Forms.View"/></param>
        /// <param name="gesture">The <see cref="RawGestures"/> of interest</param>
        /// <param name="direction">The <see cref="Directionality"/> of interest</param>
        /// <param name="command">The command to execute when the gesture is received</param>
        public void RegisterInterest(View view, RawGestures gesture,Directionality direction, ICommand command)
        {
          
            FindInterest(view,gesture,direction).Command = command;
        }
        /// <summary>
        /// Attach a command parameter to the getsure command
        /// </summary>
        /// <param name="view">The contained View <see cref="Xamarin.Forms.View"/></param>
        /// <param name="gesture">The <see cref="RawGestures"/> of interest</param>
        /// <param name="direction">The <see cref="Directionality"/> of interest</param>
        /// <param name="param">The parameter can be an object or a binding expression</param>
        public void RegisterInterestParamaeter(View view, RawGestures gesture, Directionality direction, object param)
        {
            FindInterest(view,gesture,direction).ComamndParameter = param;
        }
        /// <summary>
        /// Utility function to locate a specific interest
        /// </summary>
        /// <param name="view">The view that has the interest</param>
        /// <param name="gesture">The gesture of interest</param>
        /// <param name="direction">The direction of itnerest</param>
        /// <returns></returns>
        private ViewInterest FindInterest(View view, RawGestures gesture, Directionality direction)
        {
            var vi = viewInterests.FirstOrDefault(x => x.View == view && x.Gesture == gesture && x.Directon == direction);
            if (vi == null)
            {
                vi = new ViewInterest { View = view, Gesture = gesture, Directon = direction };
                viewInterests.Add(vi);
            }
            return vi;
        }
        /// <summary>
        /// Used by the Xamarin.Forms.Labs.Droid.Controls.GesturesContentView
        /// </summary>
        /// <param name="gesture">The gesture</param>
        /// <param name="nvgi"><see cref="NonVectorGestureLoci"/></param>
        /// <returns>True if the gesture was handled,false otherwise</returns>
        internal bool NonVectorGesture(RawGestures gesture,NonVectorGestureLoci nvgi)
        {
            var interestedview = InterestedView(gesture,new Point( nvgi.RelativeX, nvgi.RelativeY));
            if(interestedview != null)
                SatisfyInterest(interestedview, new GestureEventArgs { Direction = Directionality.None, Gesture = gesture, Origin = new Point(nvgi.RelativeX, nvgi.RelativeY) });
            return interestedview != null;
        }

        /// <summary>
        /// Used by the Xamarin.Forms.Labs.Droid.Controls.GesturesContentView
        /// </summary>
        /// <param name="gesture">The gesture</param>
        /// <param name="vgi"><see cref="NonVectorGestureLoci"/></param>
        /// <returns>True if the gesture was handled,false otherwise</returns>
        internal bool VectorGesture(RawGestures gesture, VectorGestureLoci vgi)
        {
            var interestedview = InterestedView(gesture, vgi.Direction, new Point(vgi.RelativeX, vgi.RelativeY));
            if(interestedview != null)
               SatisfyInterest(interestedview, new GestureEventArgs{Direction = vgi.Direction,Gesture = gesture,Origin = new Point(vgi.RelativeX,vgi.RelativeY)});
            
            return interestedview != null;
        }

        /// <summary>
        /// For non swipe gestures we only look at containment
        /// </summary>
        /// <param name="rawGesture">The raw gesture</param>
        /// <param name="pt">The origin point of the gesture (relative to the top left of the <see cref="GesturesContentView"/></param>
        /// <returns></returns>
        private ViewInterest InterestedView(RawGestures rawGesture, Point pt)
        {            
            return viewInterests.FirstOrDefault(v => v.Gesture==rawGesture && v.View.Bounds.Contains(pt));
        }

        /// <summary>
        /// For now only consider the origin point.
        /// Once the kinks are worked out switch to a 
        /// closest approach based on nearest point intersection
        /// ordering by area on the presumption that the smallest
        /// view will be the innermost
        /// </summary>
        /// <param name="rawGesture">The <see cref="RawGestures"/></param>
        /// <param name="d">The directionality of the gesture <see cref="Directionality"/></param>
        /// <param name="point">The origin point of the gesture</param>
        /// <returns></returns>
        private ViewInterest InterestedView(RawGestures rawGesture,Directionality d, Point point)
        {
            var allinterested = viewInterests.Where(v =>v.Gesture == rawGesture && 
                                (
                                    (v.Directon & Directionality.HorizontalMask) == (d & Directionality.HorizontalMask) || 
                                    (v.Directon & Directionality.VerticalMask) == (d & Directionality.VerticalMask))
                                ).ToList();

            if (!allinterested.Any()) return null;

            //Smallest view that contains the origin point wins
            //In most cases smallest will be the innermost
            //TODO:Check to see if RaiseView and LowerView have an effect on this
            var originview = allinterested.Where(v => v.View.Bounds.Contains(point)).OrderBy(v => v.View.Bounds.Width * v.View.Bounds.Height).FirstOrDefault();
            if (originview == null)
            {
                //No result Check for interescection based on Accuracy
                var range = Accuracy;
                var inflaterect = new Rectangle(point.X - range, point.Y - range, point.X + range, point.Y + range);
                var candidates = allinterested.Where(v => v.View.Bounds.IntersectsWith(inflaterect)).ToList();
                if (candidates.Any())
                {
                    originview = candidates.Count() == 1? candidates.First(): candidates.OrderBy(v => DistanceToClosestEdge(v.View.Bounds, point)).First();
                }
            }
            return originview;
        }

        private double DistanceToClosestEdge(Rectangle r, Point pt)
        {
            //Distance from the top edge of the rectangle
            // ReSharper disable InconsistentNaming
            var distAB = DistanceToEdge(pt, new Point(r.Left, r.Top),new Point(r.Left + r.Width, r.Top));
            //Distance from the left edge of the rectangle
            var distAC = DistanceToEdge(pt, new Point(r.Left, r.Top), new Point(r.Left, r.Top + r.Height));
            //Distance from the bottom edge of the rectangle
            var distCD = DistanceToEdge(pt,new Point(r.Left, r.Top + r.Height),new Point(r.Left + r.Width, r.Top + r.Height));
            //Distance from the right edge of the rectable
            var distBD = DistanceToEdge(pt,new Point(r.Left + r.Width, r.Top),new Point(r.Left + r.Width, r.Top + r.Height));
            // ReSharper restore InconsistentNaming

            return Math.Min(distAB, Math.Min(distAC, Math.Min(distCD, distBD)));
        }

        private double DistanceToEdge(Point originPoint, Point vertex1Point, Point vertex2Point)
        {
            // normalize points
            var cn = new Point(vertex2Point.X - originPoint.X, vertex2Point.Y - originPoint.Y);
            var bn = new Point(vertex1Point.X - originPoint.X, vertex1Point.Y - originPoint.Y);

            var angle = Math.Atan2(bn.Y, bn.X) - Math.Atan2(cn.Y, cn.X);
            var abLength = Math.Sqrt(bn.X * bn.X + bn.Y * bn.Y);

            return Math.Sin(angle) * abLength;
        }
        private void SatisfyInterest(ViewInterest vi,GestureEventArgs args)
        {
            var commandparam = vi.ComamndParameter??BindingContext;
            if(vi.Command.CanExecute(commandparam))
                vi.Command.Execute(commandparam);
            var handler = GestureRecognized;
            if (handler != null)
            {
                handler(vi.View, args);
            }

        }
        /// <summary>
        /// Class used to record a view's interest in a gesture
        /// </summary>
        private class ViewInterest
        {
            public View View { get; set; }
            public RawGestures Gesture { get; set; }
            public Directionality Directon { get; set; }
            public ICommand Command { get; set; }
            public object ComamndParameter { get; set; }
        }

    }

    /// <summary>
    /// Passed to the eventhandler for gestures
    /// </summary>
    public class GestureEventArgs : EventArgs
    {
        /// <summary>
        /// The origin point of the gesture, relative to the container
        /// </summary>
        public Point Origin { get; set; }
        /// <summary>
        /// The actual gesture <see cref="RawGestures"/>
        /// </summary>
        public RawGestures Gesture { get; set; }
        /// <summary>
        /// The directionof the gesture, will be None for none swipe events
        /// <see cref="Directionality"/>
        /// </summary>
        public Directionality Direction { get; set; }       
    }
    /// <summary>
    /// Class to store information about non swipe events
    /// internal as it is of no use to a user
    /// </summary>
    internal class NonVectorGestureLoci
    {
        public float RelativeX { get; set; }
        public float RelativeY { get; set; }
    }

    /// <summary>
    /// Class to store information about swipe events
    /// internal as it is of no use to a user
    /// </summary>
    internal class VectorGestureLoci : NonVectorGestureLoci
    {
        public float RelativeX2 { get; set; }
        public float RelativeY2 { get; set; }
        public double Scale 
        {
            get { return Math.Sqrt(Math.Pow(RelativeX - RelativeX2, 2) + Math.Pow(RelativeY - RelativeY2, 2)); }  
        }

        public Directionality Direction
        {
            get
            {
                return 
                    (Math.Abs(RelativeX - RelativeX2) < 3 ? Directionality.None : RelativeX < RelativeX2 ? Directionality.Right : Directionality.Left)
                    | (Math.Abs(RelativeY-RelativeY2)<3 ? Directionality.None: RelativeY < RelativeY2 ? Directionality.Down : Directionality.Up);
            }    
        } 
    }

    /// <summary>
    /// For swipe gestures determines the general
    /// direction of the swipe
    /// </summary>
    [Flags]
    public enum Directionality
    {
        /// <summary>
        /// No direction
        /// </summary>
        None=0,
        /// <summary>
        /// Swiping from Right to Left
        /// </summary>
        Left=0x01,
        /// <summary>
        /// Swiping from Left to Right
        /// </summary>
        Right=0x02,
        /// <summary>
        /// Swiping from Bottom to Top
        /// </summary>
        Up=0x10,
        /// <summary>
        /// Swiping from Top to Bottom
        /// </summary>
        Down=0x20,
        /// <summary>
        /// Mask to isolate the Horizontal component
        /// </summary>
        HorizontalMask=0x0F,
        /// <summary>
        /// Mask to isolate the Vertical component
        /// </summary>
        VerticalMask=0xF0
    }
    /// <summary>
    /// The base supported gestures
    /// </summary>
    public enum RawGestures
    {
        /// <summary>
        /// No Gesture
        /// </summary>
        Unknown=0,
        /// <summary>
        /// Single tap
        /// </summary>
        SingleTap,
        /// <summary>
        /// Double Tap
        /// </summary>
        DoubleTap,
        /// <summary>
        /// LongPress
        /// </summary>
        LongPress,
        /// <summary>
        /// Swipe, Swipe is combined with Directionality to support:
        /// SwipeLeft
        /// SwipeRight
        /// SwipeUp
        /// SwipeDown
        /// It is very possible for a single swipe action to trigger two Swipe events:
        /// ie:  SwipeUp and SwipeLeft
        /// </summary>
        Swipe
    }
}
