namespace EzeePdf.Core
{
    public class Constants
    {
        public const string APP_ID = "ezeepdf";
        public const string ENV_DB_CONN = "ezeepdf_dev_dbconn";

        public const string APP_TOKEN_COOKIE = "tok_id_ck";
        public const string APP_USER_COOKIE = "user_inf_ck";
        public const string LOCAL_STORAGE_ACC_TOKEN = "acc_token";
        public const string LOCAL_STORAGE_REF_TOKEN = "ref_token";

        public const int ACCESS_TOKEN_EXPIRY_MINUTES = 2;
        public const int USER_INFO_EXPIRY_DAYS = 30;

        public const string CURRENT_HTTP_USER = "current_http_user";
        public const string CLAIM_USER_ID = "1";
        public const string CLAIM_USER_TYPE = "2";
        public const string CLAIM_EMAIL = "e";
        public const string AUTH_TYPE = "jwt";

        public const string DEFAULT_DEVICE_NAME = "web browser";

        public const int PDF_DAILY_UPLOAD_SIZE_MB = 100;
        public const int PDF_MAX_UPLOAD_SIZE_MB = 2;  
        public const int WATERMARK_MAX_SIZE_KB = 100;   
        public const int FREE_VERSION_PAGE_COUNT = 5;
        public const int FEEDBACK_MIN_LENGTH = 10;
        public const int FEEDBACK_MAX_LENGTH = 300;
        public const int CONSECUTIVE_FEATURE_USAGE_WAIT_MINUTES = 10;
    }
    public class CacheConstants
    {
        public const string SETTINGS_CACHE = "all_settings";
        public const int SETTINGS_CACHE_EXPIRY_MINUTES = 30;

    }
}
