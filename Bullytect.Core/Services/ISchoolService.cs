using System;
using System.Collections.Generic;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface ISchoolService
    {

        IObservable<IList<string>> AllNames();
        IObservable<SchoolEntity> CreateSchool(string Name, string Residence, string Location, string Province, string Tfno, string Email);


    }
}
