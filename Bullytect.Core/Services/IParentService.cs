
namespace Bullytect.Core.Services
{

	using System;
	using System.Collections.Generic;
    using System.IO;
    using Bullytect.Core.Models.Domain;
    using Bullytect.Core.ViewModels.Core.Models;

    public interface IParentService
    {

        IObservable<ParentEntity> GetProfileInformation();
        IObservable<List<SonEntity>> GetChildren();
		IObservable<ParentEntity> Register(string FirstName, string LastName, DateTime Birthdate,
												  string Email, string PasswordClear, string ConfirmPassword, string Telephone);
        IObservable<ParentEntity> Update(string FirstName, string LastName, DateTime Birthdate, string Email, string Telephone);
        IObservable<string> ResetPassword(string email);
        IObservable<string> DeleteAccount();
        IObservable<ImageEntity> UploadProfileImage(Stream FileStream);
        IObservable<SonEntity> AddSonToSelfParent(string FirstName, string Lastname, DateTime Birthdate, string School);
        IObservable<SonEntity> UpdateSonInformation(string Identity, string FirstName, string Lastname, DateTime Birthdate, string School);
        IObservable<SonEntity> GetSonById(string Id);
        IObservable<ImageEntity> UploadSonProfileImage(string identity, Stream stream);
        IObservable<string> DeleteSonById(string Id);
		IObservable<UserSystemPreferencesEntity> SavePreferences(bool PushNotificationsEnabled, string RemoveAlertsEvery);
		IObservable<UserSystemPreferencesEntity> GetPreferences();
        IObservable<IList<CommentEntity>> GetComments(string SonId, string AuthorId, int DaysAgo, IList<SocialMediaTypeEnum> SocialMedia, Dictionary<DimensionCategoryEnum, string> Dimensions);
    }
}
