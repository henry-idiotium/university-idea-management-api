namespace UIM.Core.Helpers;

public static class EnvVars
{

    public static string ClientDomain => GetEnvVar("CLIENT_DOMAIN");
    public static bool UseEmailService => bool.Parse(GetEnvVar("USE_EMAIL_SERVICE"));
    public static string UserProfilePath => GetEnvVar("USER_PROFILE_PATH");
    public static string CoreEnv => GetEnvVar("ASPNETCORE_ENVIRONMENT").ToLower();
    public static bool InitRolesPwrUser => bool.Parse(GetEnvVar("INIT_ROLES_PWRUSER"));
    public static string[] ValidOrigins
    {
        get
        {
            var origins = GetEnvVar("VALID_ORIGINS").Split(';').ToList();
            origins.Add(GetEnvVar("CLIENT_DOMAIN"));
            return origins.ToArray();
        }
    }

    private static string GetEnvVar(string variable)
    {
        var result = Environment.GetEnvironmentVariable(variable);
        if (result == null)
            throw new ArgumentNullException(result, "EnvVars");

        return result;
    }

    public static class ExternalProvider
    {
        public static class SendGrid
        {
            public static string ApiKey => GetEnvVar("SENDGRID_API_KEY");
            public static string SenderEmail => GetEnvVar("SENDGRID_SENDER_EMAIL");
            public static string SenderName => GetEnvVar("SENDGRID_SENDER_NAME");
            public static string TemplateId => GetEnvVar("SENDGRID_TEMPLATE_ID");
        }
    }

    public static class Jwt
    {
        public static int AccessExpiredDate => int.Parse(GetEnvVar("JWT_ACCESS_EXPIRED_DATE"));
        public static string Audience => GetEnvVar("JWT_AUDIENCE");
        public static string Issuer => GetEnvVar("JWT_ISSUER");
        public static int RefreshExpiredDate => int.Parse(GetEnvVar("JWT_REFRESH_EXPIRED_DATE"));
        public static string Secret => GetEnvVar("JWT_SECRET");
    }

    public static class Pgsql
    {
        public static string Db => GetEnvVar("PGSQL_DB");
        public static string Host => GetEnvVar("PGSQL_HOST");
        public static string IntegratedSecurity => GetEnvVar("PGSQL_INTEGRATED_SECURITY");
        public static string Password => GetEnvVar("PGSQL_PASSWORD");
        public static string Pooling => GetEnvVar("PGSQL_POOLING");
        public static string Port => GetEnvVar("PGSQL_PORT");
        public static string SslMode => GetEnvVar("PGSQL_SSL_MODE");
        public static string TrustServer => GetEnvVar("PGSQL_TRUST_SERVER_CERTIFICATE");
        public static string UserId => GetEnvVar("PGSQL_USER_ID");
    }

    public static class SocialAuth
    {
        public static string GoogleClientId => GetEnvVar("AUTH_GOOGLE_CLIENT_ID");
    }

    public static class System
    {
        public static class PwrUserAuth
        {
            public static string Email => GetEnvVar("AUTH_PWRUSER_EMAIL");
            public static string Password => GetEnvVar("AUTH_PWRUSER_PASSWORD");
            public static string UserName => GetEnvVar("AUTH_PWRUSER_USERNAME");
        }

        public static class Role
        {
            public static string Manager => GetEnvVar("SYSTEM_ROLE_MANAGER");
            public static string PwrUser => GetEnvVar("SYSTEM_ROLE_PWRUSER");
            public static string Staff => GetEnvVar("SYSTEM_ROLE_STAFF");
            public static string Supervisor => GetEnvVar("SYSTEM_ROLE_SUP");
        }
    }
}