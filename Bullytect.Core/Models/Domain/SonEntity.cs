using System;
using System.Collections.Generic;
using MvvmHelpers;

namespace Bullytect.Core.Models.Domain
{
    public class SonEntity: ObservableObject
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
			set { SetProperty(ref _firstName, value); }
		}

        string _lastName;
		public string LastName
		{
			get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
            
        }

        DateTime _birthdate = new DateTime();

		public DateTime Birthdate
		{
			get { return _birthdate; }
			set { SetProperty(ref _birthdate, value); }

		}

		string _age;
		public string Age
		{
			get { return _age; }
			set { SetProperty(ref _age, value); }

		}

        public SchoolEntity School { get; set; } = new SchoolEntity();


		string _profileImage;
		public string ProfileImage
		{
			get { return _profileImage; }
			set { SetProperty(ref _profileImage, value); }

		}


        public string FullName => string.Format("{0} {1}", FirstName, LastName);


        public void HydrateWith(SonEntity OtherSonEntity) {


            Identity = OtherSonEntity.Identity;
            FirstName = OtherSonEntity.FirstName;
            LastName = OtherSonEntity.LastName;
            Birthdate = OtherSonEntity.Birthdate;
            Age = OtherSonEntity.Age;
            School.HydrateWith(OtherSonEntity.School);
            ProfileImage = OtherSonEntity.ProfileImage;
        }


		public override string ToString()
		{
            return String.Format("Identity: {0}, FirstName:{1}, LastName:{2}, Age:{3}, ", Identity, FirstName, LastName,Age);
		}
    }
}
