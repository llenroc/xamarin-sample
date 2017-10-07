using System;

namespace Bullytect.Core.Models.Domain
{
    public class ParentEntity
    {


		public string Identity { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public DateTime Birthdate { get; set; } = new DateTime();
		public int Age { get; set; }
		public string Email { get; set; }
		public string Telephone { get; set; }
		public string FbId { get; set; }
		public long Children { get; set; }
		public string Locale { get; set; }
		public string FullName => string.Format("{0} {1}", FirstName, LastName);


		public override string ToString()
		{
            return String.Format("Identity: {0}, FirstName:{1}, LastName:{2}, Birthdate:{3}, " +
                                 "Age:{4}, Email:{5}, Telephone:{6}, FbId:{7}, Children:{8}, Locale:{9}", Identity, FirstName, 
                                 LastName, Birthdate, Age, Email, Telephone, FbId, Children, Locale);
		}
    }
}
