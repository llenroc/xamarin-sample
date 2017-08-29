using System;
using System.Globalization;

namespace bullytect.PatformServices
{
    public interface ILocalize
    {
		CultureInfo GetCurrentCultureInfo();
		void SetLocale(CultureInfo ci);
    }
}
