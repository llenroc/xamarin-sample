using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bullytect.Core.Utils
{
	public class IsDirtyViewModelJsonContractResolver : DefaultContractResolver
	{
		public static readonly IsDirtyViewModelJsonContractResolver Instance = new IsDirtyViewModelJsonContractResolver();

		public readonly List<Type> TypeFilterList = new List<Type> { typeof(IMvxCommand), typeof(ICommand) };

		/// <summary>
		/// The list of property name strings to use to filter out untracked properties
		/// </summary>
		public readonly List<string> PropertyNameFilterList = new List<string> { "CleanHash", "RequestedBy", "IsDirty", "Validation", "Error", "Valid" };

		/// <summary>
		/// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract"/>.
		/// if the object has any properties or fields marked with IsDirtyMonitoringAttribute only these properties will be included in the contract or the filters are applied to the complete base collection.
		/// </summary>
		/// <param name="type">The type to create properties for.</param>/// <param name="memberSerialization">The member serialization mode for the type.</param>
		/// <returns>
		/// Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract"/>.
		/// </returns>
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			var properties = type.GetProperties().Where(p => p.GetCustomAttribute(typeof(IsDirtyMonitoringAttribute)) != null).ToList();
			var fields = type.GetFields().Where(f => f.GetCustomAttribute(typeof(IsDirtyMonitoringAttribute)) != null).ToList();

			if (properties.Count > 0 || fields.Count > 0)
			{
				List<JsonProperty> props = properties.Select(propInfo => CreateProperty(propInfo, memberSerialization)).ToList();
				props.AddRange(fields.Select(fieldInfo => CreateProperty(fieldInfo, memberSerialization)));
				return props;
			}

			var filterprops = base.CreateProperties(type, memberSerialization)
				.Where(p =>
					!PropertyNameFilterList.Any(f => p.PropertyName.Contains(f)) &&
					!TypeFilterList.Any(f => p.PropertyType.IsAssignableFrom(f))
					).ToList();
			return filterprops;
		}
	}
}
