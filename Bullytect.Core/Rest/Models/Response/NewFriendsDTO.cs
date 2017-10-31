using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class NewFriendsDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("title")]
		public string Title { get; set; }

        [JsonProperty("users")]
        public IList<UserDTO> Users { get; set; }


        public class UserDTO {

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("value")]
            public int Value { get; set; }

        }

    }
}
