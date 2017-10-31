using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class UpdateSonDTO
    {
        #pragma warning disable CS1701

		string _identity;

		[JsonProperty("identity")]
		public string Identity
		{
			get => _identity;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_identity = value;
			}
		}

		string _firstName;

		[JsonProperty("first_name")]
		public string FirstName
		{
			get => _firstName;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_firstName = value;
			}
		}

		string _lastName;

		[JsonProperty("last_name")]
		public string LastName
		{
			get => _lastName;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_lastName = value;
			}
		}

        [JsonProperty("birthdate")]
        public DateTime Birthdate { get; set; }

		string _school;

		[JsonProperty("school")]
		public string School
		{
			get => _school;
			set
			{
				if (!string.IsNullOrEmpty(value))
					_school = value;
			}
		}
    }
}
