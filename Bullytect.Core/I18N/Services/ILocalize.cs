using System;
using System.Globalization;

namespace Bullytect.Core.I18N.Services
{
    public interface ILocalize
    {
		CultureInfo GetCurrentCultureInfo();
		void SetLocale(CultureInfo ci);
    }
}
