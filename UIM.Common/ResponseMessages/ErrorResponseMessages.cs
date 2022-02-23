namespace UIM.Common.ResponseMessages
{
    public static class ErrorResponseMessages
    {
        public const string BadRequest = "Invalid request";
        public const string FailedLogin = "Email or password is incorrect";
        public const string FailedResetPassword = "Failed to reset password";
        public const string Forbidden = "Request rejected";
        public const string IncorrectOldPassword = "The old password is incorrect";
        public const string Unauthenticated = "Failed to authenticate with this resource";
        public const string Unauthorized = "Unauthorized to access this resource";
        public const string UnexpectedError = "Something went wrong";
        public const string UserAlreadyExists = "This username or email already exists";
    }
}