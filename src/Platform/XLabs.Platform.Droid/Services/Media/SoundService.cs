// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
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
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content.Res;
using Android.Media;

namespace XLabs.Platform.Services.Media
{
    /// <summary>
    /// Class SoundService.
    /// </summary>
    public class SoundService : ISoundService , IDisposable
    {
        /// <summary>
        /// The _is player prepared
        /// </summary>
        private bool isPlayerPrepared;

        /// <summary>
        /// The _is scrubbing
        /// </summary>
        private bool isScrubbing;

        /// <summary>
        /// The _player
        /// </summary>
        private MediaPlayer player;

        /// <summary>
        /// Starts the player asynchronous from assets folder.
        /// </summary>
        /// <param name="fp">The fp.</param>
        /// <returns>Task.</returns>
        /// <exception cref="FileNotFoundException">Make sure you set your file in the Assets folder</exception>
        private async Task StartPlayerAsyncFromAssetsFolder(AssetFileDescriptor fp)
        {
            try
            {
                if (this.player == null)
                {
                    this.player = new MediaPlayer();
                }
                else
                {
                    this.player.Reset();
                }

                if (fp == null)
                {
                    throw new FileNotFoundException("Make sure you set your file in the Assets folder");
                }

                await this.player.SetDataSourceAsync(fp.FileDescriptor, fp.StartOffset, fp.Length);
                this.player.Prepared += (s, e) =>
                    {
                        this.player.SetVolume(0, 0);
                        this.isPlayerPrepared = true;
                    };
                this.player.Prepare();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }

        #region IDisposable implementation
        bool _disposed;
        /// <summary>
        /// To be added.
        /// </summary>
        /// <remarks>To be added.</remarks>
        public void Dispose ()
        {
            if (_disposed && this.player != null) {
                _disposed = true;
                this.player.Dispose ();
                this.player = null;
                this.CurrentFile = null;
            }
        }

        #endregion

        #region ISoundService implementation

        /// <summary>
        /// Occurs when [sound file finished].
        /// </summary>
        public event EventHandler SoundFileFinished;

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
                        if (this.player == null || string.Compare (filename, CurrentFile.Filename, StringComparison.Ordinal) > 0)
                        {
                            await SetMediaAsync(filename);
                        }
                        this.player.Start();
                        return CurrentFile;
                    });
        }

        /// <summary>
        /// set media as an asynchronous operation.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>Task&lt;SoundFile&gt;.</returns>
        public async Task<SoundFile> SetMediaAsync(string filename)
        {
            CurrentFile = new SoundFile {Filename = filename};
            await StartPlayerAsyncFromAssetsFolder(Application.Context.Assets.OpenFd(filename));
            CurrentFile.Duration = TimeSpan.FromSeconds(this.player.Duration);
            return CurrentFile;
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
                        this.player.SeekTo(TimeSpan.FromSeconds(position).Milliseconds);
                        this.isScrubbing = false;
                    });
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        public void Play()
        {
            if (this.player != null && !this.player.IsPlaying)
            {
                this.player.Start();
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if ((this.player == null)) return;

            if (this.player.IsPlaying)
            {
                this.player.Stop();
            }

            this.player.Release();
            this.player = null;
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            if (this.player != null && this.player.IsPlaying)
            {
                this.player.Pause();
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
                return 0.5;
            }
            set
            {
                if (this.player != null && this.isPlayerPrepared)
                {
                    this.player.SetVolume((float)value, (float)value);
                }
            }
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <value>The current time.</value>
        public double CurrentTime
        {
            get
            {
                return this.player == null ? 0 : TimeSpan.FromMilliseconds(this.player.CurrentPosition).TotalSeconds;
            }
            set { }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
        public bool IsPlaying
        {
            get
            {
                return this.player.IsPlaying;
            }
        }

        /// <summary>
        /// Gets the current file.
        /// </summary>
        /// <value>The current file.</value>
        public SoundFile CurrentFile { get; private set; }

        #endregion
    }
}