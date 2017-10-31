using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class RegisterParentDTO
    {
#pragma warning disable CS1701

        string _firstName;

        [JsonProperty("first_name")]
        public string FirstName {
            get => _firstName; 
            set {
                if (!string.IsNullOrEmpty(value))
                    _firstName = value;
            }
        }

        string _lastName;

        [JsonProperty("last_name")]
        public string LastName { 
            get => _lastName; 
            set {
				if (!string.IsNullOrEmpty(value))
					_lastName = value;
            } 
        }

        [JsonProperty("birthdate")]
        public DateTime Birthdate { get; set; }

        string _email;

        [JsonProperty("email")]
        public string Email { 
            get => _email; 
            set {
				if (!string.IsNullOrEmpty(value))
					_email = value;  
            } 
        }

        string _passwordClear;

        [JsonProperty("password_clear")]
        public string PasswordClear { 
            get => _passwordClear; 
            set {
				if (!string.IsNullOrEmpty(value))
					_passwordClear = value;
            }
        }

        string _confirmPassword;

        [JsonProperty("confirm_password")]
        public string ConfirmPassword { 
            get => _confirmPassword; 
            set {
				if (!string.IsNullOrEmpty(value))
					_confirmPassword = value;
            } 
        }

        string _telephone;

		[JsonProperty("telephone")]
		public string Telephone { 
            get => _telephone;
			set {
				if (!string.IsNullOrEmpty(value))
					_telephone = value;
			}
		}
    }
}
