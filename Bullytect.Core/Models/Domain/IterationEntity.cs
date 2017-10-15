using System;
namespace Bullytect.Core.Models.Domain
{
    public class IterationEntity
    {
		public DateTime StartDate { get; set; }
		public DateTime FinishDate { get; set; }
		public int TotalTasks { get; set; }
		public int TotalFailedTasks { get; set; }
		public double TotalComments { get; set; }

        public string FinishDateAsString {
            get => String.Format("{0:d/M/yyyy HH:mm:ss}", FinishDate);
        }

    }
}
