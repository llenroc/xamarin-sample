using System;
namespace bullytect.Models.Utils
{
    public interface IDirty
    {
		bool IsDirty
		{
			get;
			set;
		}
    }
}
