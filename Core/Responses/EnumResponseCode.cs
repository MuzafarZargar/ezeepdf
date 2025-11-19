using System.ComponentModel;

namespace EzeePdf.Core.Responses
{
    public enum EnumResponseCode
    {
        [Description("Unknown error")]
        Unknown = 0,

        [Description("Bad Request")]
        BadRequest = 1,

        [Description("Success")]
        Success = 2,

        [Description("Unauthorized")]
        Unauthorized = 3,

        [Description("Request timed out")]
        Timeout = 4,

        [Description("Request is null or empty")]
        RequestMissing = 5,

        [Description("Invalid username or password")]
        InvalidCredentials = 6,

        [Description($"Each feature can be used only once in a minute")]
        RateLimitApplied = 7,

        [Description("Invalid password, account has been locked")]
        InvalidCredentialsAndLocked = 10,

        [Description("User does not exist")]
        UserNotFound = 15,

        [Description("User is no longer active")]
        UserDisabled = 20,

        [Description("User is locked")]
        UserLocked = 25,

        [Description("Error in login")]
        SigninError = 30,

        [Description("Error in renewing token")]
        RefreshTokenError = 35,

        [Description("Error in user registration")]
        RegistrationError = 40,

        [Description("Error while changing password")]
        PasswordChangeError = 45,

        [Description("Invalid password format or length, expected length of 8 with alpha-numeric and special characters")]
        InvalidPasswordFormat = 50,

        [Description("New password cannot be same as old")]
        PasswordsAreSame = 55,

        [Description("Password changed too frequently")]
        PasswordChangeFrequencyError = 60,

        [Description("Invalid Old Password")]
        InvalidOldPassword = 65,

        [Description("Error while signing out")]
        SignoutError = 70,

        [Description("Invalid Email Address")]
        InvalidEmailAddress = 75,

        [Description("Email Address exceeded allowed number of characters")]
        InvalidEmailAddressLength = 80,

        [Description("FirstName has not be specified")]
        FirstNameMissing = 85,

        [Description("FirstName exceeded allowed number of characters")]
        FirstNameInvalidLength = 90,

        [Description("LastName exceeded allowed number of characters")]
        LastNameInvalidLength = 95,

        [Description("SirName exceeded allowed number of characters")]
        SirNameInvalidLength = 100,

        [Description("City name is missing")]
        CityMissing = 105,

        [Description("City exceeded allowed number of characters")]
        CityInvalidLength = 110,

        [Description("Id is missing")]
        IdMissing = 115,

        [Description("Invalid Id")]
        InvalidId = 120,

        [Description("Email address in use")]
        DuplicateEmailAddress = 125,

        [Description("Error in search")]
        SearchError = 130,

        [Description("Search criteria must be specified")]
        SearchCriteriaMissing = 135,

        [Description("Error fetching detail")]
        DetailError = 140,

        [Description("Error while saving data")]
        SaveError = 145,

        [Description("Error while uploading file")]
        UploadError = 150,

        [Description("File is missing in request")]
        FileMissing = 155,

        [Description("File size exceeded (Max Size={0})")]
        FileSizeExceed = 160,

        [Description("File page exceeded (Max Number={0})")]
        FilePageCountExceed = 165,

        [Description("Error saving pdf/image data")]
        UsageSaveError = 170,

        [Description("Error saving feedback")]
        FeedbackSaveError = 175,

        [Description("Daily upload/download limit for application reached, please try next day (UTC)")]
        DailyUploadLimitReached = 180,

        [Description("Daily limit fetch error")]
        DailyLimitFetchError = 185,
    }
}
