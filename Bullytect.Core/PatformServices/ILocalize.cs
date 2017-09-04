using System;
using System.Globalization;

namespace Bullytect.Core.PatformServices
{
    public interface ILocalize
    {
		CultureInfo GetCurrentCultureInfo();
		void SetLocale(CultureInfo ci);
    }
}
