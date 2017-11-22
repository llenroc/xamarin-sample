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

            [JsonProperty("external_id")]
            public string Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

            [JsonProperty("profile_image")]
            public string ProfileImage { get; set; }

            [JsonProperty("since")]
            public string ValueLabel { get; set; }

        }

    }
}
