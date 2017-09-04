using System;
namespace Bullytect.Core.Models.Utils
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
