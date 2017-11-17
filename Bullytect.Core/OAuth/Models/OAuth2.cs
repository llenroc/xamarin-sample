using System;
namespace Bullytect.Core.OAuth.Models
{
	public partial class OAuth2 : OAuth
	{
		public OAuth2()
		{
		}

		public string OAuth2_Scope
		{
			get;
			set;
		}

		public string OAuth_SecretKey_ConsumerSecret_APISecret
		{
			get;
			set;
		}

        public bool UsingNativeUI { get; set; } = true;

	}
}
