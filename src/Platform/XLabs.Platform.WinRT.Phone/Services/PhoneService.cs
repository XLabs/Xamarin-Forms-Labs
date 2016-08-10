// ***********************************************************************
// Assembly         : XLabs.Platform.WP81
// Author           : XLabs Team
// Created          : 01-01-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="PhoneService.cs" company="XLabs Team">
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
using System.Linq;
using Windows.ApplicationModel.Chat;
using Windows.Networking.Connectivity;

namespace XLabs.Platform.Services
{
	/// <summary>
	///     Phone service for Windows Phone devices.
	/// </summary>
	public class PhoneService : IPhoneService
	{
		private readonly ConnectionProfile _networkConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

		/// <summary>
		///     Gets the cellular provider.
		/// </summary>
		/// <value>The cellular provider.</value>
		public string CellularProvider
		{
			get
			{
				return
					NetworkInformation.GetConnectionProfiles()
						.Where(x => x.IsWwanConnectionProfile)
						.Select(x => x.GetNetworkNames().DefaultIfEmpty().FirstOrDefault())
						.DefaultIfEmpty(null)
						.FirstOrDefault();
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance has cellular data enabled.
		/// </summary>
		/// <value>
		///     <c>null</c> if [is cellular data enabled] contains no value, <c>true</c> if [is cellular data enabled];
		///     otherwise, <c>false</c>.
		/// </value>
		public bool? IsCellularDataEnabled
		{
			get { return _networkConnectionProfile.IsWwanConnectionProfile; }
		}

		/// <summary>
		///     Gets a value indicating whether this instance has cellular data roaming enabled.
		/// </summary>
		/// <value>
		///     <c>null</c> if [is cellular data roaming enabled] contains no value, <c>true</c> if [is cellular data roaming
		///     enabled]; otherwise, <c>false</c>.
		/// </value>
		public bool? IsCellularDataRoamingEnabled
		{
			get { return null; }
		}

		/// <summary>
		///     Gets a value indicating whether this instance is network available.
		/// </summary>
		/// <value>
		///     <c>null</c> if [is network available] contains no value, <c>true</c> if [is network available]; otherwise,
		///     <c>false</c>.
		/// </value>
		public bool? IsNetworkAvailable
		{
			get
			{
				return _networkConnectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
			}
		}

		/// <summary>
		///     Gets the ISO Country Code
		/// </summary>
		/// <value>The icc.</value>
		public string ICC
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		///     Gets the Mobile Country Code
		/// </summary>
		/// <value>The MCC.</value>
		public string MCC
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		///     Gets the Mobile Network Code
		/// </summary>
		/// <value>The MNC.</value>
		public string MNC
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance can send SMS.
		/// </summary>
		/// <value><c>true</c> if this instance can send SMS; otherwise, <c>false</c>.</value>
		public bool CanSendSMS
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		///     Opens native dialog to dial the specified number
		/// </summary>
		/// <param name="number">Number to dial.</param>
		public void DialNumber(string number)
		{
			Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(number, number);
		}

		/// <summary>
		///     Sends the SMS.
		/// </summary>
		/// <param name="to">To.</param>
		/// <param name="body">The body.</param>
		public async void SendSMS(string to, string body)
		{
			var msg = new ChatMessage();
			msg.Recipients.Add(to);
			msg.Body = body;

			await ChatMessageManager.ShowComposeSmsMessageAsync(msg);
		}
	}
}