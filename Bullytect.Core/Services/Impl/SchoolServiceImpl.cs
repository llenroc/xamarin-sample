

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using AutoMapper;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Rest.Services;
using Bullytect.Core.Rest.Models.Request;
using Bullytect.Core.Rest.Models.Response;
using System.Linq;

namespace Bullytect.Core.Services.Impl
{
    public class SchoolServiceImpl: BaseService, ISchoolService
    {

        readonly ISchoolRestService _schoolRestService;

        public SchoolServiceImpl(ISchoolRestService schoolRestService){
            _schoolRestService = schoolRestService;
        }

        public IObservable<Dictionary<string, string>> AllNames()
        {
            Debug.WriteLine("All School Names");

            var observable = _schoolRestService
                .AllNames()
                .Select(response => response.Data)
                .Select((response) => response.ToDictionary(SchoolName => SchoolName.Identity, SchoolName => SchoolName.Name))
                .Finally(() =>
                {
                    Debug.WriteLine("All School Finish ...");
                });

            return operationDecorator(observable);

        }

        public IObservable<long> CountSchools()
        {
            Debug.WriteLine("Count Schools");

            var observable = _schoolRestService
                .Total()
                .Select(response => response.Data)
                .Select((response) => {
                    long temp;
                    return long.TryParse(response, out temp) ? temp : 0;
                })
                .Finally(() =>
                {
                    Debug.WriteLine("Count School Finish ...");
                });

            return operationDecorator(observable);
        }

        public IObservable<SchoolEntity> CreateSchool(string Name, string Residence, double Latitude, double Longitude, string Province, string Tfno, string Email)
        {
            Debug.WriteLine(string.Format("Create School -> Name: {0}, Residence: {1}, Latitude: {2}, Longitude:{3}, Province: {4}, Tfno: {5}, Email: {6}",
                                          Name, Residence, Latitude, Longitude, Province, Tfno, Email));

            var observable = _schoolRestService
                .CreateSchool(new AddSchoolDTO()
                {
                    Name = Name,
                    Residence = Residence,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Province = Province,
                    Tfno = Tfno,
                    Email = Email
                })
                .Select(response => response.Data)
                .Select(school => Mapper.Map<SchoolDTO, SchoolEntity>(school))
				.Finally(() =>
				{
					Debug.WriteLine("Create School Finish ...");
				});

            return operationDecorator(observable);
        }

        public IObservable<IList<SchoolEntity>> FindSchools(string Name)
        {
            Debug.WriteLine("Find schools by " + Name);

            var observable = _schoolRestService
                .FindSchools(Name)
                .Select(response => response.Data)
                .Select((schools) => Mapper.Map<IList<SchoolDTO>, IList<SchoolEntity>>(schools))
                .Finally(() =>
                {
                    Debug.WriteLine("Find schools finished ...");
                });

            return operationDecorator(observable);
        }
    }
}
