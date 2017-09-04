﻿using System;


namespace Bullytect.Core.Models.Domain
{
    public class SonEntity
    {
        public string Identity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string School { get; set; }

		public override string ToString()
		{
			return String.Format("Identity: {0}, FirstName:{1}, LastName:{2}, Age:{3}, School:{4}", Identity, FirstName, LastName, Age, School);
		}
    }
}
