using System;
using MvvmHelpers;

namespace Bullytect.Core.Models.Domain
{
    public class ParentEntity: ObservableObject
    {
		string _identity;

		public string Identity
		{
			get { return _identity; }
			set { SetProperty(ref _identity, value); }
		}

		string _firstName;

		public string FirstName
		{
			get { return _firstName; }
			set { 
                SetProperty(ref _firstName, value);
                OnPropertyChanged("FullName");
            }
		}

		string _lastName;
		public string LastName
		{
			get { return _lastName; }
			set { 
                
                SetProperty(ref _lastName, value);
                OnPropertyChanged("FullName");
			}

		}

		DateTime _birthdate = new DateTime();

		public DateTime Birthdate
		{
			get { return _birthdate; }
			set { SetProperty(ref _birthdate, value); }

		}

		int _age;
		public int Age
		{
			get { return _age; }
			set { SetProperty(ref _age, value); }

		}

		string _email;
		public string Email
		{
			get { return _email; }
			set { SetProperty(ref _email, value); }

		}

		string _phonePrefix;
		public string PhonePrefix
		{
			get { return _phonePrefix ?? "+34"; }
			set { SetProperty(ref _phonePrefix, value); }

		}

		string _phoneNumber;
		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set { SetProperty(ref _phoneNumber, value); }

		}

		string _fbId;
		public string FbId
		{
			get { return _fbId; }
			set { SetProperty(ref _fbId, value); }

		}

		long _children;
		public long Children
		{
			get { return _children; }
			set { SetProperty(ref _children, value); }

		}

		string _locale;
		public string Locale
		{
			get { return _locale; }
			set { SetProperty(ref _locale, value); }

		}

        string _profileImage;
		public string ProfileImage
		{
			get { return _profileImage; }
			set { SetProperty(ref _profileImage, value); }

		}

		public void HydrateWith(ParentEntity OtherParentEntity)
		{

			Identity = OtherParentEntity.Identity;
			FirstName = OtherParentEntity.FirstName;
			LastName = OtherParentEntity.LastName;
			Birthdate = OtherParentEntity.Birthdate;
			Age = OtherParentEntity.Age;
            Email = OtherParentEntity.Email;
            PhonePrefix = OtherParentEntity.PhonePrefix;
            PhoneNumber = OtherParentEntity.PhoneNumber;
            FbId = OtherParentEntity.FbId;
            Children = OtherParentEntity.Children;
            Locale = OtherParentEntity.Locale;
            ProfileImage = OtherParentEntity.ProfileImage;
		}

		public string FullName => string.Format("{0} {1}", FirstName, LastName);


		public override string ToString()
		{
            return String.Format("Identity: {0}, FirstName:{1}, LastName:{2}, Birthdate:{3}, " +
                                 "Age:{4}, Email:{5}, PhonePrefix:{6}, PhoneNumber:{7},  FbId:{8}, Children:{9}, Locale:{10}", Identity, FirstName, 
                                 LastName, Birthdate, Age, Email, PhonePrefix, PhoneNumber, FbId, Children, Locale);
		}
    }
}
