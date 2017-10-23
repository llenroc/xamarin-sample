using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class AddSchoolDTO
    {
        #pragma warning disable CS1701

		string _name;

		[JsonProperty("name")]
		public string Name
		{
			get => _name;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_name = value;
			}
		}

		string _residence;

		[JsonProperty("residence")]
		public string Residence
		{
			get => _residence;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_residence = value;
			}
		}

		string _location;

		[JsonProperty("location")]
		public string Location
		{
			get => _location;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_location = value;
			}
		}

		string _province;

		[JsonProperty("province")]
		public string Province
		{
			get => _province;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_province = value;
			}
		}

		string _tfno;

		[JsonProperty("tfno")]
		public string Tfno
		{
			get => _tfno;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_tfno = value;
			}
		}

		string _email;

		[JsonProperty("email")]
		public string Email
		{
			get => _email;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_email = value;
			}
		}
    }
}
