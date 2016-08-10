// ***********************************************************************
// Assembly         : XLabs.Platform.WP81
// Author           : XLabs Team
// Created          : 01-01-2016
//
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BluetoothDevice.cs" company="XLabs Team">
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
using Windows.Networking;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Class BluetoothDevice.
    /// </summary>
    public class BluetoothDevice : IBluetoothDevice
    {
        /// <summary>
        /// The _socket
        /// </summary>
        private StreamSocket _socket;

        /// <summary>
        /// The _device
        /// </summary>
        private readonly PeerInformation _device;

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothDevice"/> class.
        /// </summary>
        /// <param name="peerInfo">The peer information.</param>
        public BluetoothDevice(PeerInformation peerInfo)
        {
            _device = peerInfo;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return _device.DisplayName;
            }
        }

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address
        {
            get
            {
#if WINDOWS_APP && !WINDOWS_PHONE_APP
                return _device.DisplayName;
#else
                return _device.HostName.DisplayName;
#endif
            }
        }

        /// <summary>
        /// Gets the input stream.
        /// </summary>
        /// <value>The input stream.</value>
        public Stream InputStream
        {
            get
            {
                return _socket == null ? null : _socket.InputStream.AsStreamForRead();
            }
        }

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        /// <value>The output stream.</value>
        public Stream OutputStream
        {
            get
            {
                return _socket == null ? null : _socket.OutputStream.AsStreamForWrite();
            }
        }


        private string ServiceName
        {
            get
            {
#if WINDOWS_APP && !WINDOWS_PHONE_APP
                return "1";
#else
                return _device.ServiceName;
#endif
            }
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Connect()
        {
            if (_socket != null)
            {
                _socket.Dispose();
            }

            try
            {
                _socket = new StreamSocket();

                await _socket.ConnectAsync(new HostName(Address), ServiceName);

                //return true;
            }
            catch //(Exception ex)
            {
                if (_socket != null)
                {
                    _socket.Dispose();
                    _socket = null;
                }

                throw;
            }
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }
    }
}