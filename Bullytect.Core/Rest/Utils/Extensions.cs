using System;
using System.Collections.Generic;
using System.Text;

namespace Bullytect.Core.Rest.Utils
{
    public static class Extensions
    {
		public static Uri AttachParameters(this Uri uri, Dictionary<string, string> parameters)
		{
			var stringBuilder = new StringBuilder();
			string str = "?";
			foreach (KeyValuePair<string, string> entry in parameters)
			{
                stringBuilder.Append(str + entry.Key + "=" + entry.Value);
                str = "&";
			}
			return new Uri(uri + stringBuilder.ToString());
		}
    }
}
