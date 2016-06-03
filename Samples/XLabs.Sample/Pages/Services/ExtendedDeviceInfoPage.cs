// ***********************************************************************
// Assembly         : XLabs.Sample
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedDeviceInfoPage.cs" company="XLabs Team">
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
using Xamarin.Forms;
using XLabs.Platform.Device;

namespace XLabs.Sample.Pages.Services
{
    /// <summary>
    /// Class ExtendedDeviceInfoPage.
    /// </summary>
    public class ExtendedDeviceInfoPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedDeviceInfoPage"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public ExtendedDeviceInfoPage(IDevice device)
        {
            this.Title ="Extended Device Info";
            if (device == null)
            {
                this.Content = new Label()
                {
                    TextColor = Color.Red,
                    Text = "IDevice has not been configured with the dependency service."
                };
                return;
            }

            var scroll = new ScrollView();
            var stack = new StackLayout();

            #region Display information
            var display = device.Display;
            var displayFrame = new Frame();
            if (display != null)
            {
                displayFrame.Content = new StackLayout()
                {
                    Children =
                    {
                        new Label { Text = display.ToString() },
                        new Label { Text = $"Screen width is\t {display.ScreenWidthInches():0.0} inches." },
                        new Label { Text = $"Screen height is\t {display.ScreenHeightInches():0.0} inches." },
                        new Label { Text = $"Screen diagonal size is\t {display.ScreenSizeInches():0.0} inches." }
                    }
                };
            }
            else
            {
                displayFrame.Content = new Label { TextColor = Color.Red, Text = "Device does not contain display information." };
            }

            stack.Children.Add(displayFrame); 
            #endregion

            #region Battery information
            var battery = device.Battery;
            var batteryFrame = new Frame();
            if (battery != null)
            {
                var level = new Label();
                var charger = new Label();

                var levelAction = new Action(() => level.Text = $"Battery level is {battery.Level}%.");
                var chargerAction = new Action(() => charger.Text =
                    $"Charger is {(battery.Charging ? "Connected" : "Disconnected")}.");

                levelAction.Invoke();
                chargerAction.Invoke();

                batteryFrame.Content = new StackLayout()
                {
                    Children = { level, charger }
                };

                battery.OnLevelChange += (s, e) => Device.BeginInvokeOnMainThread(levelAction);

                battery.OnChargerStatusChanged += (s, e) => Device.BeginInvokeOnMainThread(chargerAction);
            }
            else
            {
                batteryFrame.Content = new Label { TextColor = Color.Red, Text = "Device does not contain battery information." };
            }

            stack.Children.Add(batteryFrame); 
            #endregion

            #region RAM information
            var ramLabel = new Label { Text = "Total Memory:" };

            var ramText = new Label();

            stack.Children.Add(new Frame
            {
                Content = new StackLayout
                {
                    Children = { ramLabel, ramText }
                }
            });

            double mem;
            string format;

            if (device.TotalMemory < 1073741824)
            {
                mem = device.TotalMemory / 1024 / 1024d;
                format = "{0:#,0.00} MB";
            }
            else
            {
                mem = device.TotalMemory / 1024 / 1024 / 1024d;
                format = "{0:#,0.00} GB";
            }

            ramText.Text = string.Format(format, mem);
            #endregion

            #region Device Info
            var idLabel = new Label { Text = "Device Id:" };

            var idText = new Label();

            stack.Children.Add(new Frame
            {
                Content = new StackLayout
                {
                    Children = { idLabel, idText }
                }
            });

            try
            {
                idText.Text = device.Id;
            }
            catch (Exception ex)
            {
                idText.Text = ex.Message;
            }

            #endregion
            
            #region Device Language Information
            var languageLabel = new Label { Text = "Device Language Code:" };

            var languageText = new Label();

            stack.Children.Add(new Frame
            {
                Content = new StackLayout
                {
                    Children = { languageLabel, languageText }
                }
            });

            try
            {
                languageText.Text = device.LanguageCode;

               
            }
            catch (Exception ex)
            {
                idText.Text = ex.Message;
            }

            #endregion
            
             #region Network Connetctions Information
            var networkLabel = new Label { Text = "Network Connections:" };

            var networkText = new Label();

            stack.Children.Add(new Frame
            {
                Content = new StackLayout
                {
                    Children = { networkLabel, networkText }
                }
            });

            try
            {
                var internetConnectionStatus = device.Network.InternetConnectionStatus();
                switch (internetConnectionStatus)
                {
                    case NetworkStatus.NotReachable:
                        networkText.Text = "No Connetcions";
                        break;
                    case NetworkStatus.ReachableViaCarrierDataNetwork:
                        networkText.Text = "Mobil device Internet connetions";
                        break;
                    case NetworkStatus.ReachableViaWiFiNetwork:
                        networkText.Text = "Wi-fi Internet connetcions";
                        break;
                    case NetworkStatus.ReachableViaUnknownNetwork:
                        networkText.Text = "Unknow connetcions";
                        break;
                    default:
                        networkText.Text = "Error";
                        break;
                }
            }
            catch (Exception ex)
            {
                idText.Text = ex.Message;
            }

            #endregion

            #region Hardware Version Information
            var nameLabel = new Label { Text = "Hardware Version:" };

            var nameText = new Label();

            stack.Children.Add(new Frame
            {
                Content = new StackLayout
                {
                    Children = { nameLabel, nameText }
                }
            });

            try
            {
                nameText.Text = device.HardwareVersion;
            }
            catch (Exception ex)
            {
                idText.Text = ex.Message;
            }

            #endregion

            scroll.Content = stack;

            this.Content = scroll;
        }
    }
}
