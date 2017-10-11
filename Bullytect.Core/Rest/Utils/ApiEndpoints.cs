

using Bullytect.Core.Config;

namespace Bullytect.Core.Rest.Utils
{
    public static class ApiEndpoints
    {

        public static string GET_AUTHORIZATION_TOKEN = string.Concat(SharedConfig.BASE_API_URL, "/parents/auth/");
        public static string GET_AUTHORIZATION_TOKEN_BY_FACEBOOK = string.Concat(SharedConfig.BASE_API_URL, "/parents/auth/facebook");
        public static string GET_SON_BY_ID = string.Concat(SharedConfig.BASE_API_URL, "/children/:id");
        public static string GET_ALL_SOCIAL_MEDIA_BY_SON_ID = string.Concat(SharedConfig.BASE_API_URL, "/children/:id/social");
        public static string GET_INVALID_SOCIAL_MEDIA_BY_SON_ID = string.Concat(SharedConfig.BASE_API_URL, "/children/:id/social/invalid");
        public static string SAVE_SOCIAL_MEDIA = string.Concat(SharedConfig.BASE_API_URL, "/children/social/save");
        public static string SAVE_ALL_SOCIAL_MEDIA = string.Concat(SharedConfig.BASE_API_URL, "/children/:id/social/save/all");
        public static string DELETE_SOCIAL_MEDIA = string.Concat(SharedConfig.BASE_API_URL, "/:idson/social/delete/:idsocial");
        public static string SAVE_DEVICE = string.Concat(SharedConfig.BASE_API_URL, "/device-groups/devices/save");
        public static string GET_SELF_PARENT_INFORMATION = string.Concat(SharedConfig.BASE_API_URL, "/parents/self");
        public static string GET_CHILDREN_OF_SELF_PARENT = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/children");
        public static string REGISTER_PARENT = string.Concat(SharedConfig.BASE_API_URL, "/parents/");
        public static string UPDATE_SELF_PARENT = string.Concat(SharedConfig.BASE_API_URL, "/parents/self");
        public static string RESET_PASSWORD = string.Concat(SharedConfig.BASE_API_URL, "/parents/reset-password");
        public static string DELETE_ACCOUNT = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/delete");
		public static string UPLOAD_PROFILE_IMAGE = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/image");
        public static string ADD_SON_TO_SELF_PARENT = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/children/add");
        public static string UPDATE_SON_INFORMATION = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/children/update");
        public static string ALL_SCHOOL_NAMES = string.Concat(SharedConfig.BASE_API_URL, "/schools/all/names");
        public static string CREATE_SCHOOL = string.Concat(SharedConfig.BASE_API_URL, "/schools/");
        public static string GET_PARENT_PROFILE_IMAGE = string.Concat(SharedConfig.BASE_API_URL, "/images/parents/:id");
        public static string GET_SON_PROFILE_IMAGE = string.Concat(SharedConfig.BASE_API_URL, "/images/children/:id");
        public static string UPLOAD_SON_PROFILE_IMAGE = string.Concat(SharedConfig.BASE_API_URL, "/children/:id/image");
        public static string GET_LAST_SELF_ALERTS = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/alerts/last");
        public static string GET_SELF_ALERTS = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/alerts");
        public static string CLEAR_SELF_ALERTS = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/alerts");
        public static string GET_ALERTS_BY_SON = string.Concat(SharedConfig.BASE_API_URL, "/children/:id/alerts");
        public static string CLEAR_ALERTS_FOR_SON = string.Concat(SharedConfig.BASE_API_URL, "/children/:id/alerts");
        public static string DELETE_ALERT = string.Concat(SharedConfig.BASE_API_URL, "/children/:son/alerts/:alert");
        public static string GET_COMMENTS_BY_SON_FOR_LAST_ITERATION = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/iterations/last/comments-by-son");
        public static string GET_LAST_ITERATIONS_FOR_SELF_PARENT = string.Concat(SharedConfig.BASE_API_URL, "/parents/self/iterations");
       

                                                 


    }
}
