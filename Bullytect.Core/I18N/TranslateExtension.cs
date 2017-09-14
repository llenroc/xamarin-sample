using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Bullytect.Core.I18N.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bullytect.Core.I18N
{
	// You exclude the 'Extension' suffix when using in Xaml markup
	[ContentProperty("Text")]
	public class TranslateExtension : IMarkupExtension
	{
		readonly CultureInfo ci;
        string ResourceId;

        private readonly Lazy<ResourceManager> ResMgr;

		public TranslateExtension()
		{

            ResourceId = typeof(Bullytect.Core.I18N.AppResources).FullName; 

			ResMgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

			if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
			{
				ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
			}
		}

		public string Text { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Text == null)
				return "";

			var translation = ResMgr.Value.GetString(Text, ci);

			if (translation == null)
			{
                #if DEBUG
                	throw new ArgumentException(
                					String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                					"Text");
                #else
                                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
                #endif
			}
			return translation;
		}
	}
}
