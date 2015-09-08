using System;
using Newtonsoft.Json.Serialization;

namespace XLabs.Serialization.JsonNET
{
	/// <summary>
	/// Contract resolver delegate.
	/// </summary>
	public class ContractResolverDelegate : DefaultContractResolver
	{
		private readonly Func<Type, bool> canCreate;
		private readonly Func<Type, object> creator;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContractResolverDelegate"/> class.
		/// </summary>
		/// <param name="canCreate">Can create function delegate.</param>
		/// <param name="creator">Creator function delegate.</param>
		public ContractResolverDelegate(Func<Type, bool> canCreate, Func<Type, object> creator)
		{
			this.canCreate = canCreate;
			this.creator = creator;
		}

		/// <summary>
		/// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonObjectContract" /> for the given type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>A <see cref="T:Newtonsoft.Json.Serialization.JsonObjectContract" /> for the given type.</returns>
		protected override JsonObjectContract CreateObjectContract(Type objectType)
		{
			var contract = base.CreateObjectContract(objectType);

			if (canCreate == null || this.canCreate(objectType))
			{
				contract.DefaultCreator = () => this.creator(objectType);
			}

			return contract;
		}
	}
}