using System;
using System.Collections.Generic;

namespace Bullytect.Core.Models.Domain
{
    public class SonEntity
    {
        public string Identity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; } = new DateTime();
        public int Age { get; set; }
        public string SchoolIdentity { get; set; }
		public string SchoolName { get; set; }

        public string FullName => string.Format("{0} {1}", FirstName, LastName);

		public override string ToString()
		{
			return String.Format("Identity: {0}, FirstName:{1}, LastName:{2}, Age:{3}, School:{4}", Identity, FirstName, LastName,Age,  SchoolIdentity);
		}
    }
}
