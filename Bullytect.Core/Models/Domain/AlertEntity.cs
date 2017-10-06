using System;
namespace Bullytect.Core.Models.Domain
{
    public class  AlertEntity
    {
        #pragma warning disable CS1701

		public string Identity { get; set; }
		public AlertLevelEnum Level { get; set; }
        public string Title { get; set; }
		public string Payload { get; set; }
		public DateTime CreateAt { get; set; }
		public SonEntity Son { get; set; }
    }
}
