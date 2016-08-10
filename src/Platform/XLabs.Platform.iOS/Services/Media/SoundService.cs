// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
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
using AVFoundation;
using Foundation;

namespace XLabs.Platform.Services.Media
{
    /// <summary>
    /// Class SoundService.
    /// </summary>
    public class SoundService : ISoundService
    {
        /// <summary>
        /// The _is scrubbing
        /// </summary>
        private bool isScrubbing;

        /// <summary>
        /// The _player
        /// </summary>
        private AVAudioPlayer player;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundService"/> class.
        /// </summary>
        public SoundService()
        {
            IsPlaying = false;
            CurrentFile = null;
        }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public double Volume
        {
            get
            {
                return this.player == null ? 0.5 : this.player.Volume;
            }
            set
            {
                if (this.player != null)
                {
                    this.player.Volume = (float)value;
                }
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
                return this.player == null ? 0 : this.player.CurrentTime;
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
            if (this.player != null && !IsPlaying)
            {
                this.player.Play();
                IsPlaying = true;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (this.player == null) return;
            this.player.Stop();
            IsPlaying = false;
            this.player.CurrentTime = 0.0;
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            if (this.player != null && IsPlaying)
            {
                this.player.Pause();
                IsPlaying = false;
            }
        }

        /// <summary>
        /// Occurs when [sound file finished].
        /// </summary>
        public event EventHandler SoundFileFinished;

        /// <summary>
        /// Handles the <see cref="E:FileFinished" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SoundFinishedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileFinished(SoundFinishedEventArgs e)
        {
            if (SoundFileFinished != null)
            {
                SoundFileFinished(this, e);
            }
        }

        /// <summary>
        /// Sets the media asynchronous.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>Task&lt;SoundFile&gt;.</returns>
        public Task<SoundFile> SetMediaAsync(string filename)
        {
            return Task.Run(
                () =>
                    {
                        CurrentFile = new SoundFile {Filename = filename};
                        var url = NSUrl.FromFilename(CurrentFile.Filename);
                        this.player = AVAudioPlayer.FromUrl(url);
                        this.player.FinishedPlaying += (object sender, AVStatusEventArgs e) =>
                            {
                                if (e.Status)
                                {
                                    OnFileFinished(new SoundFinishedEventArgs(CurrentFile));
                                }
                            };
                        CurrentFile.Duration = TimeSpan.FromSeconds(this.player.Duration);
                        return CurrentFile;
                    });
        }

        /// <summary>
        /// Plays the asynchronous.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>Task&lt;SoundFile&gt;.</returns>
        public Task<SoundFile> PlayAsync(string filename, string extension = null)
        {
            return Task.Run<SoundFile>(
                async () =>
                    {
                        if (this.player == null || string.Compare(filename, CurrentFile.Filename) != 0)
                        {
                            await SetMediaAsync(filename);
                        }
                        this.Play();
                        return CurrentFile;
                    });
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
                        if (this.isScrubbing) return;
                        this.isScrubbing = true;
                        this.player.CurrentTime = position;
                        this.isScrubbing = false;
                    });
        }
    }
}
