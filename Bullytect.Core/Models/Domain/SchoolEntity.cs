using System;
using MvvmHelpers;

namespace Bullytect.Core.Models.Domain
{
    public class SchoolEntity: ObservableObject
    {

        string _identity;

        public string Identity
        {
            get { return _identity; }
            set
            {

                SetProperty(ref _identity, value);
                OnPropertyChanged(nameof(Identity));
            }

        }

        string _name;

        public string Name
        {
            get { return _name; }
            set
            {

                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(Name));
            }

        }


        string _residence;

        public string Residence
        {
            get { return _residence; }
            set
            {

                SetProperty(ref _residence, value);
                OnPropertyChanged(nameof(Residence));
                OnPropertyChanged(nameof(Location));
            }

        }

        double _latitude;

        public double Latitude
        {
            get { return _latitude; }
            set
            {

                SetProperty(ref _latitude, value);
                OnPropertyChanged(nameof(Latitude));
            }

        }

        double _longitude;

        public double Longitude
        {
            get { return _longitude; }
            set
            {

                SetProperty(ref _longitude, value);
                OnPropertyChanged(nameof(Longitude));
            }

        }

        string _province;

        public string Province
        {
            get { return _province; }
            set
            {

                SetProperty(ref _province, value);
                OnPropertyChanged(nameof(Province));
                OnPropertyChanged(nameof(Location));
            }

        }

        string _tfno;

        public string Tfno
        {
            get { return _tfno; }
            set
            {

                SetProperty(ref _tfno, value);
                OnPropertyChanged(nameof(Tfno));
            }

        }

        string _email;

        public string Email
        {
            get { return _email; }
            set
            {

                SetProperty(ref _email, value);
                OnPropertyChanged(nameof(Email));
            }

        }

        public string Location { get => !string.IsNullOrWhiteSpace(Residence) && !string.IsNullOrWhiteSpace(Province)
                                               ? string.Concat(Residence, " - ", Province) : string.Empty; }

        public void HydrateWith(SchoolEntity OtherSchoolEntity)
        {

            Identity = OtherSchoolEntity.Identity;
            Name = OtherSchoolEntity.Name;
            Residence = OtherSchoolEntity.Residence;
            Latitude = OtherSchoolEntity.Latitude;
            Longitude = OtherSchoolEntity.Longitude;
            Province = OtherSchoolEntity.Email;
            Tfno = OtherSchoolEntity.Tfno;
            Email = OtherSchoolEntity.Email;
        }
    
    }
}
