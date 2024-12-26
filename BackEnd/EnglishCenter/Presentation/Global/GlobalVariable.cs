namespace EnglishCenter.Presentation.Global
{
    public static class GlobalVariable
    {
        public const string DATABASE = "EnglishCenter";
        public const string CLIENT_URL = "https://localhost:5173/";

        public const string SYSTEM = "SYSTEM";

        // Identity
        public const int MAX_FAILED_ACCESS = 5;
        public const int LOCKED_TIME_SPAN_MINUTES = 5;
        public const string REFRESH_TOKEN = "Refresh Token";

        // Token Expire
        public const int TOKEN_EXPIRED = 60;

        // Policy
        public const string ALL_ROLES = "all-roles";
        public const string ADMIN_TEACHER = "admin-teacher";
        public const string ADMIN_STUDENT = "admin-student";

        // Toeic
        public const int TOEIC_NUM = 200;

    }
}
