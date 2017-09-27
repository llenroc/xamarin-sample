using System;
namespace Bullytect
{
	public static class AssemblyGlobal
	{
		public const string Company = "BISITE Research Group";

		public const string ProductLine = "BullTect";

		public const string Year = "2017";

		public const string Copyright = Company + " - " + Year;

#if DEBUG
		public const string Configuration = "Debug";
#elif RELEASE
		public const string Configuration = "Release";
#else
		public const string Configuration = "Unkown";
#endif
	}
}
