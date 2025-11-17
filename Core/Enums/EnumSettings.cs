namespace EzeePdf.Core.Enums
{
    public enum EnumSettings
    {
        None = 0,
        PublicPasswordExpiryInDays = 1,
        StaffPasswordExpiryInDays = 2,
        LockAfterFailLoginAttempts = 3,
        FreeVersionAllowedPageCount = 4,
        FreeVersionSaveCount = 5,
        FreeVersionLockHours = 6,
        MaxPages = 7,
        MaxUploadPerDay = 8,
        WatermarkMaxImageSizeKB = 9,
        MaxPdfSizeMB = 10,
        MaxPdfSizeMBPerDay = 11,
        ConsecutiveUsageWait = 12,
        ConsecutiveFeedbackWait = 13
    }
}
