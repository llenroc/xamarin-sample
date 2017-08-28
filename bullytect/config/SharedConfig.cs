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
	}
}
