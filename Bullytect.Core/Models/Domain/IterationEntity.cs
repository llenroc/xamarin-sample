using System;
namespace Bullytect.Core.Models.Domain
{
    public class IterationEntity
    {
		public DateTime StartDate { get; set; }
		public DateTime FinishDate { get; set; }
		public int TotalTasks { get; set; }
		public int TotalFailedTasks { get; set; }
		public int TotalComments { get; set; }
    }
}
