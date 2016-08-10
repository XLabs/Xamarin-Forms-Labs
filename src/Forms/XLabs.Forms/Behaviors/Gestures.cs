// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Gestures.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XLabs.Exceptions;
using XLabs.Forms.Controls;
using XLabs.Forms.Exceptions;

namespace XLabs.Forms.Behaviors
{
    /// <summary>
    /// Provides Gesture attached properties for the GesturesContentView
    /// This class has no involvement beyond setting up the interests
    /// It is here simply to make the users xaml a bit 
    /// more readable
    /// </summary>
    public class Gestures : BindableObject
    {
        /// <summary>
        /// Definition for the attachable Interests Property
        /// </summary>
        public static BindableProperty InterestsProperty =BindableProperty.CreateAttached<Gestures, GestureCollection>(
                                                            x => x.GetValue<GestureCollection>(InterestsProperty),
                                                            null,
                                                            BindingMode.OneWay,  
                                                            null,
                                                            InterestsChanged);

        /// <summary>
        /// ctor guarenttess that <see cref="Interests"/> is not null
        /// </summary>
        public Gestures()
        {
            Interests= new GestureCollection();
        }

        /// <summary>
        /// The set of interests for this view
        /// </summary>
        public GestureCollection Interests
        {
            get { return (GestureCollection)GetValue(InterestsProperty); }
            set { SetValue(InterestsProperty,value);}
        }

        private static void InterestsChanged(BindableObject bo, GestureCollection oldvalue, GestureCollection newvalue)
        {
            var view = bo as View;
            if (view == null)
                throw new InvalidBindableException(bo, typeof(View));

            var gcv = FindContentViewParent(view,false);
            if (gcv == null)
            {
                PendingInterestParameters.Add(new PendingInterestParams { View = view, Interests = newvalue });
                view.PropertyChanged += ViewPropertyChanged;

            }
            else
                gcv.RegisterInterests(view,newvalue);            
        }

        private static void ViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Unfortunately the Parent property doesn't signal a change
            //However when the renderer is set it should have it's parent
            if (e.PropertyName == "Renderer")
            {
                var view = sender as View;
                if (view == null) return;
                var gcv = FindContentViewParent(view);
                var pending = PendingInterestParameters.Where(x => x.View == view).ToList();
                foreach (var pendingparam in pending)
                {
                    gcv.RegisterInterests(view,pendingparam.Interests);
                    PendingInterestParameters.Remove(pendingparam);
                }
                view.PropertyChanged -= ViewPropertyChanged;
            }
        }

        /// <summary>
        /// Utility function to find the first containing <see cref="GesturesContentView"/>
        /// </summary>
        /// <param name="view">View to find the parent from.</param>
        /// <param name="throwException">True to throw an excpetion if the parent is not found</param>
        /// <returns></returns>
        private static GesturesContentView FindContentViewParent(View view, bool throwException=true)
        {
            var history = new List<string>();
            var viewParent = view as GesturesContentView;
            if (viewParent != null) return viewParent;

            history.Add(view.GetType().Name);
            var parent = view.Parent;
            while (parent != null && !(parent is GesturesContentView))
            {
                history.Add(parent.GetType().Name);
                parent = parent.Parent;
            }

            if (parent == null && throwException)
            {
                throw new InvalidNestingException(typeof (Gestures), typeof (GesturesContentView), history);
            }

            return (GesturesContentView) parent;
        }
        
        private static readonly List<PendingInterestParams>  PendingInterestParameters = new List<PendingInterestParams>();

        private class PendingInterestParams
        {
            public View View { get; set; }
            public GestureCollection Interests { get; set; }
        }
    }
}
