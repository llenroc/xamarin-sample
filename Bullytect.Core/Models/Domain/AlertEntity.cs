using System;
namespace Bullytect.Core.Models.Domain
{
    public class AlertEntity
    {
        #pragma warning disable CS1701

		public string Identity { get; set; }
		public string Level { get; set; }
		public string Payload { get; set; }
		public string CreateAt { get; set; }
		public SonEntity Son { get; set; }
    }
}
