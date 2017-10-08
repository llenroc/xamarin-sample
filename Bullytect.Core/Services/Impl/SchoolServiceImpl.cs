﻿

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

        public IObservable<SchoolEntity> CreateSchool(string Name, string Residence, string Location, string Province, string Tfno, string Email)
        {
            Debug.WriteLine(string.Format("Create School -> Name: {0}, Residence: {1}, Location: {2}, Province: {3}, Tfno: {4}, Email: {5}",
                                          Name, Residence, Location, Province, Tfno, Email));

            var observable = _schoolRestService
                .CreateSchool(new AddSchoolDTO()
                {
                    Name = Name,
                    Residence = Residence,
                    Location = Location,
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
    }
}
