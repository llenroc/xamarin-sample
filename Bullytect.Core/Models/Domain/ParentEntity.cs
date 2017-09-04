using System;

namespace Bullytect.Core.Models.Domain
{
    public class ParentEntity
    {

        public string Identity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Email { get; set; }
        public long Children { get; set; }


		public override string ToString()
		{
            return String.Format("Identity: {0}, FirstName:{1}, LastName:{2}, Age:{3}, Email:{4}, Children:{5}", Identity, FirstName, LastName, Age, Email, Children);
		}
    }
}
