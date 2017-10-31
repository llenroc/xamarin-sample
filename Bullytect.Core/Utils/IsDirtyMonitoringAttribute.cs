using System;

namespace Bullytect.Core.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class IsDirtyMonitoringAttribute : Attribute
	{
	}
}
