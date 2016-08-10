// ***********************************************************************
// Assembly         : XLabs.Platform.WP81
// Author           : XLabs Team
// Created          : 01-01-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SoundService.cs" company="XLabs Team">
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
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace XLabs.Platform.Services.Media
{
    /// <summary>
    /// SoundService implementation on the Windows Phone platform
    /// Nees a GlobalMEdiaElement instance on the App resources dictionary
    /// </summary>
    public class SoundService : ISoundService
    {
        /// <summary>
        /// The _is scrubbing
        /// </summary>
        private bool _isScrubbing;

        //// <summary>
        //// The _TCS set media
        //// </summary>
        //private TaskCompletionSource<SoundFile> tcsSetMedia;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundService"/> class.
        /// </summary>
        public SoundService()
        {
            IsPlaying = false;
            CurrentFile = null;
        }

        /// <summary>
        /// Gets the global media element.
        /// </summary>
        /// <value>The global media element.</value>
        /// <exception cref="ArgumentNullException">GlobalMedia is missing</exception>
        public static MediaElement GlobalMediaElement
        {
            get
            {
                if (Application.Current.Resources.ContainsKey("GlobalMedia"))
                {
                    return Application.Current.Resources["GlobalMedia"] as MediaElement;
                }

                throw new ArgumentNullException("Pre-requisite for use: Add a new MediaElement called 'GlobalMedia' instance to the System.Windows.Application.Current.Resources dictionary. Do not replace this instance at any point.");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <value>The current time.</value>
        public double CurrentTime
        {
            get
            {
                return GlobalMediaElement == null ? 0 : GlobalMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            }
        }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public double Volume
        {
            get
            {
                return GlobalMediaElement == null ? 0 : GlobalMediaElement.Volume;
            }
            set
            {
                if (GlobalMediaElement != null)
                {
                    GlobalMediaElement.Volume = value;
                }
            }
        }

        /// <summary>
        /// Gets the current file.
        /// </summary>
        /// <value>The current file.</value>
        public SoundFile CurrentFile { get; private set; }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        public void Play()
        {
            if (GlobalMediaElement != null && !IsPlaying)
            {
                GlobalMediaElement.Play();
                IsPlaying = true;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (GlobalMediaElement != null)
            {
                GlobalMediaElement.Stop();
                IsPlaying = false;
                //   player.du = 0.0;
            }
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            if (GlobalMediaElement != null && IsPlaying)
            {
                GlobalMediaElement.Pause();
                IsPlaying = false;
            }
        }

        /// <summary>
        /// Occurs when [sound file finished].
        /// </summary>
        public event EventHandler SoundFileFinished;

        /// <summary>
        /// Raises the <see cref="E:FileFinished" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SoundFinishedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnFileFinished(SoundFinishedEventArgs e)
        {
            if (SoundFileFinished != null)
            {
                SoundFileFinished(this, e);
            }
        }

        /// <summary>
        /// Plays the asynchronous.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>Task&lt;SoundFile&gt;.</returns>
        public async Task<SoundFile> PlayAsync(string filename, string extension = null)
        {
            if (GlobalMediaElement == null && string.Compare(filename, CurrentFile.Filename) <= 0) return null;

            await SetMediaAsync(filename);

            this.Play();

            return CurrentFile;
        }

        /// <summary>
        /// Sets the media asynchronous.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>Task&lt;SoundFile&gt;.</returns>
        public async Task<SoundFile> SetMediaAsync(string filename)
        {
            CurrentFile = new SoundFile {Filename = filename};

            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(CurrentFile.Filename);

            //TODO: need to clean this events
            GlobalMediaElement.MediaEnded += GlobalMediaElementMediaEnded;
            GlobalMediaElement.MediaOpened += GlobalMediaElementMediaOpened;

            GlobalMediaElement.Source = new Uri(CurrentFile.Filename, UriKind.Relative);

            return CurrentFile;
        }

        /// <summary>
        /// Globals the media element media opened.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GlobalMediaElementMediaOpened(object sender, RoutedEventArgs e)
        {
            //if (this.tcsSetMedia != null)
            //{
                CurrentFile.Duration = TimeSpan.FromSeconds(GlobalMediaElement.NaturalDuration.TimeSpan.TotalSeconds);
                //this.tcsSetMedia.SetResult(CurrentFile);
            //}
        }

        /// <summary>
        /// Handles the MediaEnded event of the player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void GlobalMediaElementMediaEnded(object sender, RoutedEventArgs e)
        {
            OnFileFinished(new SoundFinishedEventArgs(CurrentFile));
        }

        /// <summary>
        /// Goes to asynchronous.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Task.</returns>
        public Task GoToAsync(double position)
        {
            return Task.Run(
                () =>
                    {
                        if (this._isScrubbing) return;
                        this._isScrubbing = true;
                        //    player.CurrentTime = position;
                        this._isScrubbing = false;
                    });
        }
    }
}