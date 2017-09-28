using System;
namespace Bullytect.Core.Models.Domain
{
    public class SchoolEntity
    {
		public string Identity { get; set; }
		public string Name { get; set; }
		public string Residence { get; set; }
		public string Location { get; set; }
		public string Province { get; set; }
		public string Tfno { get; set; }
		public string Email { get; set; }


		public override string ToString()
		{
			return String.Format("Identity: {0}, Name:{1}, Residence:{2}, Location:{3}, " +
								 "Province:{4}, Tfno:{5}, Email:{6}, FbId:{7}, Children:{8}, Locale:{9}", Identity, Name,
								 Residence, Location, Province, Email, Tfno, Email);
		}
    }
}
