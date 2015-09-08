//
// https://github.com/ServiceStack/ServiceStack.Text
// ServiceStack.Text: .NET C# POCO JSON, JSV and CSV Text Serializers.
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2012 ServiceStack Ltd.
//
// Licensed under the same terms of ServiceStack: new BSD license.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text.Controller
{
	public class CommandProcessor 
	{
		private object[] Controllers { get; set; }

		private readonly Dictionary<string, object> contextMap;

		public CommandProcessor(object[] controllers)
		{
			this.Controllers = controllers;

			this.contextMap = new Dictionary<string, object>();
            foreach (var x in controllers.ToList())
            {
                contextMap[x.GetType().Name] = x;
            }
        }

		public void Invoke(string commandUri)
		{
			var actionParts = commandUri.Split(new[] { "://" }, StringSplitOptions.None);

			var controllerName = actionParts[0];

			var pathInfo = PathInfo.Parse(actionParts[1]);

			object context;
			if (!this.contextMap.TryGetValue(controllerName, out context))
			{
				throw new Exception("UnknownContext: " + controllerName);
			}

			var methodName = pathInfo.ActionName;

            var method = context.GetType().GetMethodInfos().First(
                c => c.Name == methodName && c.GetParameters().Count() == pathInfo.Arguments.Count);

			var methodParamTypes = method.GetParameters().Select(x => x.ParameterType);

			var methodArgs = ConvertValuesToTypes(pathInfo.Arguments, methodParamTypes.ToList());

			try
			{
				method.Invoke(context, methodArgs);
			}
			catch (Exception ex)
			{
				throw new Exception("InvalidCommand", ex);
			}
		}

		private static object[] ConvertValuesToTypes(IList<string> values, IList<Type> types)
		{
			var convertedValues = new object[types.Count];
			for (var i = 0; i < types.Count; i++)
			{
				var propertyValueType = types[i];
				var propertyValueString = values[i];
				var argValue = TypeSerializer.DeserializeFromString(propertyValueString, propertyValueType);
				convertedValues[i] = argValue;
			}
			return convertedValues;
		}
	}
}