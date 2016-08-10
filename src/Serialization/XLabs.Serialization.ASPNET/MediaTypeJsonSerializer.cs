// ***********************************************************************
// <copyright file="MediaTypeJsonSerializer.cs" company="XLabs Team">
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
using System.Text;

namespace XLabs.Serialization.AspNet
{
	public class MediaTypeJsonSerializer : MediaTypeSerializer
	{
		public MediaTypeJsonSerializer(IJsonSerializer jsonSerializer)
			: base(jsonSerializer, "application/json", new Encoding[] {new UTF8Encoding(false), new UnicodeEncoding(false, true)}
				)
		{
		}

		public MediaTypeJsonSerializer(IJsonSerializer jsonSerializer, IEnumerable<Encoding> supportedEncodings)
			: base(jsonSerializer, "application/json", supportedEncodings)
		{
		}
	}
}