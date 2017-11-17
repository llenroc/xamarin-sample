using System;
namespace Bullytect.Core.Models.Domain
{
    public class CommentEntity
    {
        public string Identity { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
        public string SocialMedia { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ExtractedAt { get; set; }
        public string ExtractedAtSince { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPhoto { get; set; }
        public SentimentLevelEnum Sentiment { get; set; }
        public ViolenceLevelEnum Violence { get; set; }
        public DrugsLevelEnum Drugs { get; set; }
        public BullyingLevelEnum Bullying { get; set; }
        public AdultLevelEnum Adult { get; set; }
    }
}
