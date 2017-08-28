using System;
namespace bullytect.config
{
    public static class SharedConfig
    {
        #if DEBUG
		    public const string BASE_API_URL = "debug string";
        #else
            public const string BASE_API_URL = "release string";
        #endif

		public const uint COMMON_ANIMATION_SPEED = 250;
		public const uint COMMON_DELAY_SPEED = 300;
	}
}
