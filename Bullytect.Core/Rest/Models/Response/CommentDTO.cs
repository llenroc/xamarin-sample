using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class CommentDTO
    {
        #pragma warning disable CS1701

        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("social_media")]
        public string SocialMedia { get; set; }

        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("extracted_at")]
        public DateTime ExtractedAt { get; set; }

        [JsonProperty("extracted_at_since")]
        public string ExtractedAtSince { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_photo")]
        public string authorPhoto { get; set; }

        [JsonProperty("adult")]
        public string Adult { get; set; }

        [JsonProperty("bullying")]
        public string Bullying { get; set; }

        [JsonProperty("drugs")]
        public string Drugs { get; set; }

        [JsonProperty("sentiment")]
        public string Sentiment { get; set; }

        [JsonProperty("violence")]
        public string Violence { get; set; }

    }
}
