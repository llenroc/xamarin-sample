
using System;
using System.Reactive.Linq;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class CommentDetailViewModel : BaseViewModel
    {
        
        public CommentDetailViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger,
                                    AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {

        }

        public class CommentParameter
        {
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


        public void Init(CommentParameter commentParameter)
        {
            Message = commentParameter.Message;
            Likes = commentParameter.Likes;
            SocialMedia = commentParameter.SocialMedia;
            CreatedTime = commentParameter.CreatedTime;
            ExtractedAt = commentParameter.ExtractedAt;
            ExtractedAtSince = commentParameter.ExtractedAtSince;
            AuthorName = commentParameter.AuthorName;
            AuthorPhoto = commentParameter.AuthorPhoto;
            Sentiment = commentParameter.Sentiment;
            Violence = commentParameter.Violence;
            Drugs = commentParameter.Drugs;
            Bullying = commentParameter.Bullying;
            Adult = commentParameter.Adult;
        }


        #region properties

        string _message;

        public string Message { 
            get => _message; 
            set => SetProperty(ref _message, value); 
        }

        int _likes;

        public int Likes
        {
            get => _likes;
            set => SetProperty(ref _likes, value);
        }

        string _socialMedia;

        public string SocialMedia {
            get => _socialMedia;
            set => SetProperty(ref _socialMedia, value);
        }

        DateTime _createdTime;

        public DateTime CreatedTime
        {
            get => _createdTime;
            set => SetProperty(ref _createdTime, value);
        }

        DateTime _extractedAt;

        public DateTime ExtractedAt
        {
            get => _extractedAt;
            set => SetProperty(ref _extractedAt, value);
        }

        string _extractedAtSince;

        public string ExtractedAtSince {
            get => _extractedAtSince;
            set => SetProperty(ref _extractedAtSince, value);
        }

        string _authorName;

        public string AuthorName
        {
            get => _authorName;
            set => SetProperty(ref _authorName, value);

        }

        string _authorPhoto;

        public string AuthorPhoto
        {
            get => _authorPhoto;
            set => SetProperty(ref _authorPhoto, value);

        }

        SentimentLevelEnum _sentiment;

        public SentimentLevelEnum Sentiment
        {
            get => _sentiment;
            set => SetProperty(ref _sentiment, value);

        }

        ViolenceLevelEnum _violence;

        public ViolenceLevelEnum Violence
        {
            get => _violence;
            set => SetProperty(ref _violence, value);

        }

        DrugsLevelEnum _drugs;

        public DrugsLevelEnum Drugs
        {
            get => _drugs;
            set => SetProperty(ref _drugs, value);

        }

        BullyingLevelEnum _bullying;

        public BullyingLevelEnum Bullying
        {
            get => _bullying;
            set => SetProperty(ref _bullying, value);

        }

        AdultLevelEnum _adult;

        public AdultLevelEnum Adult
        {
            get => _adult;
            set => SetProperty(ref _adult, value);

        }

        #endregion
    }
}
