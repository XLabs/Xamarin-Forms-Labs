// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="XFormsApplicationDroid.cs" company="XLabs Team">
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

using System;

using Xamarin.Forms.Platform.Android;

namespace XLabs.Forms
{
    /// <summary>
    /// Class XFormsApplicationDroid.
    /// </summary>
    public class XFormsApplicationDroid : FormsApplicationActivity
    {
        /// <summary>
        /// Gets or sets the destroy.
        /// </summary>
        /// <value>The destroy.</value>
        public EventHandler<EventArgs> Destroy { get; set; }

        /// <summary>
        /// Gets or sets the pause.
        /// </summary>
        /// <value>The pause.</value>
        public EventHandler<EventArgs> Pause { get; set; }

        /// <summary>
        /// Gets or sets the restart.
        /// </summary>
        /// <value>The restart.</value>
        public EventHandler<EventArgs> Restart { get; set; }

        /// <summary>
        /// Gets or sets the resume.
        /// </summary>
        /// <value>The resume.</value>
        public EventHandler<EventArgs> Resume { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        public EventHandler<EventArgs> Start { get; set; }

        /// <summary>
        /// Gets or sets the stop.
        /// </summary>
        /// <value>The stop event handler.</value>
        public EventHandler<EventArgs> Stop { get; set; }

        /// <summary>
        /// Called when [destroy].
        /// </summary>
        protected override void OnDestroy()
        {
            var handler = this.Destroy;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }

            base.OnDestroy();
        }

        /// <summary>
        /// Called as part of the activity lifecycle when an activity is going into
        /// the background, but has not (yet) been killed.
        /// </summary>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnResume" />
        /// <altmember cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" />
        /// <altmember cref="M:Android.App.Activity.OnStop" />
        /// <remarks><para tool="javadoc-to-mdoc">Called as part of the activity lifecycle when an activity is going into
        /// the background, but has not (yet) been killed.  The counterpart to
        /// <c><see cref="M:Android.App.Activity.OnResume" /></c>.
        /// </para>
        /// <para tool="javadoc-to-mdoc">When activity B is launched in front of activity A, this callback will
        /// be invoked on A.  B will not be created until A's <c><see cref="M:Android.App.Activity.OnPause" /></c> returns,
        /// so be sure to not do anything lengthy here.
        /// </para>
        /// <para tool="javadoc-to-mdoc">This callback is mostly used for saving any persistent state the
        /// activity is editing, to present a "edit in place" model to the user and
        /// making sure nothing is lost if there are not enough resources to start
        /// the new activity without first killing this one.  This is also a good
        /// place to do things like stop animations and other things that consume a
        /// noticeable amount of CPU in order to make the switch to the next activity
        /// as fast as possible, or to close resources that are exclusive access
        /// such as the camera.
        /// </para>
        /// <para tool="javadoc-to-mdoc">In situations where the system needs more memory it may kill paused
        /// processes to reclaim resources.  Because of this, you should be sure
        /// that all of your state is saved by the time you return from
        /// this function.  In general <c><see cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" /></c> is used to save
        /// per-instance state in the activity and this method is used to store
        /// global persistent data (in content providers, files, etc.)
        /// </para>
        /// <para tool="javadoc-to-mdoc">After receiving this call you will usually receive a following call
        /// to <c><see cref="M:Android.App.Activity.OnStop" /></c> (after the next activity has been resumed and
        /// displayed), however in some cases there will be a direct call back to
        /// <c><see cref="M:Android.App.Activity.OnResume" /></c> without going through the stopped state.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <i>Derived classes must call through to the super class's
        /// implementation of this method.  If they do not, an exception will be
        /// thrown.</i>
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onPause()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        protected override void OnPause()
        {
            var handler = this.Pause;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }

            base.OnPause();
        }

        /// <summary>
        /// Called after <c><see cref="M:Android.App.Activity.OnStop" /></c> when the current activity is being
        /// re-displayed to the user (the user has navigated back to it).
        /// </summary>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnStop" />
        /// <altmember cref="M:Android.App.Activity.OnStart" />
        /// <altmember cref="M:Android.App.Activity.OnResume" />
        /// <remarks><para tool="javadoc-to-mdoc">Called after <c><see cref="M:Android.App.Activity.OnStop" /></c> when the current activity is being
        /// re-displayed to the user (the user has navigated back to it).  It will
        /// be followed by <c><see cref="M:Android.App.Activity.OnStart" /></c> and then <c><see cref="M:Android.App.Activity.OnResume" /></c>.
        /// </para>
        /// <para tool="javadoc-to-mdoc">For activities that are using raw <c><see cref="T:Android.Database.ICursor" /></c> objects (instead of
        /// creating them through
        /// <c><see cref="M:Android.App.Activity.ManagedQuery(Android.Net.Uri, System.String[], System.String[], System.String[], System.String[])" /></c>,
        /// this is usually the place
        /// where the cursor should be required (because you had deactivated it in
        /// <c><see cref="M:Android.App.Activity.OnStop" /></c>.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <i>Derived classes must call through to the super class's
        /// implementation of this method.  If they do not, an exception will be
        /// thrown.</i>
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onRestart()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        protected override void OnRestart()
        {
            var handler = this.Restart;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }

            base.OnRestart();
        }

        /// <summary>
        /// Called after <c><see cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" /></c>, <c><see cref="M:Android.App.Activity.OnRestart" /></c>, or
        /// <c><see cref="M:Android.App.Activity.OnPause" /></c>, for your activity to start interacting with the user.
        /// </summary>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" />
        /// <altmember cref="M:Android.App.Activity.OnRestart" />
        /// <altmember cref="M:Android.App.Activity.OnPostResume" />
        /// <altmember cref="M:Android.App.Activity.OnPause" />
        /// <remarks><para tool="javadoc-to-mdoc">Called after <c><see cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" /></c>, <c><see cref="M:Android.App.Activity.OnRestart" /></c>, or
        /// <c><see cref="M:Android.App.Activity.OnPause" /></c>, for your activity to start interacting with the user.
        /// This is a good place to begin animations, open exclusive-access devices
        /// (such as the camera), etc.
        /// </para>
        /// <para tool="javadoc-to-mdoc">Keep in mind that onResume is not the best indicator that your activity
        /// is visible to the user; a system window such as the key guard may be in
        /// front.  Use <c><see cref="M:Android.App.Activity.OnWindowFocusChanged(System.Boolean)" /></c> to know for certain that your
        /// activity is visible to the user (for example, to resume a game).
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <i>Derived classes must call through to the super class's
        /// implementation of this method.  If they do not, an exception will be
        /// thrown.</i>
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onResume()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        protected override void OnResume()
        {
            var handler = this.Resume;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }

            base.OnResume();
        }

        /// <summary>
        /// Called after <c><see cref="M:Android.App.Activity.OnCreate(Android.OS.Bundle)" /></c> or after <c><see cref="M:Android.App.Activity.OnRestart" /></c> when
        /// the activity had been stopped, but is now again being displayed to the
        /// user.
        /// </summary>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnCreate(Android.OS.Bundle)" />
        /// <altmember cref="M:Android.App.Activity.OnStop" />
        /// <altmember cref="M:Android.App.Activity.OnResume" />
        /// <remarks><para tool="javadoc-to-mdoc">Called after <c><see cref="M:Android.App.Activity.OnCreate(Android.OS.Bundle)" /></c> or after <c><see cref="M:Android.App.Activity.OnRestart" /></c> when
        /// the activity had been stopped, but is now again being displayed to the
        /// user.  It will be followed by <c><see cref="M:Android.App.Activity.OnResume" /></c>.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <i>Derived classes must call through to the super class's
        /// implementation of this method.  If they do not, an exception will be
        /// thrown.</i>
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onStart()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        protected override void OnStart()
        {
            var handler = this.Start;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }

            base.OnStart();
        }

        /// <summary>
        /// Called when you are no longer visible to the user.
        /// </summary>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnRestart" />
        /// <altmember cref="M:Android.App.Activity.OnResume" />
        /// <altmember cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" />
        /// <altmember cref="M:Android.App.Activity.OnDestroy" />
        /// <remarks><para tool="javadoc-to-mdoc">Called when you are no longer visible to the user.  You will next
        /// receive either <c><see cref="M:Android.App.Activity.OnRestart" /></c>, <c><see cref="M:Android.App.Activity.OnDestroy" /></c>, or nothing,
        /// depending on later user activity.
        /// </para>
        /// <para tool="javadoc-to-mdoc">Note that this method may never be called, in low memory situations
        /// where the system does not have enough memory to keep your activity's
        /// process running after its <c><see cref="M:Android.App.Activity.OnPause" /></c> method is called.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <i>Derived classes must call through to the super class's
        /// implementation of this method.  If they do not, an exception will be
        /// thrown.</i>
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onStop()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        protected override void OnStop()
        {
            var handler = this.Stop;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }

            base.OnStop();
        }
    }
}