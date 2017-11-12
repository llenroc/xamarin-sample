using System;
using System.Collections.Generic;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface ISchoolService
    {

        IObservable<Dictionary<string, string>> AllNames();
        IObservable<SchoolEntity> CreateSchool(string Name, string Residence, double Latitude, double Longitude, string Province, string Tfno, string Email);
        IObservable<long> CountSchools();
        IObservable<IList<SchoolEntity>> FindSchools(string Name);

    }
}
