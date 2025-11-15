namespace EzeePdf.Core.Extensions
{
    public static class NumericConstants
    {
        public static long BytesToKb(this long sizeInBytes) => (long)((float)sizeInBytes / 1024);
        public static long BytesToMb(this long sizeInBytes) => (long)((float)sizeInBytes / (1024 * 1024));
        public static double BytesToMbWithDecimal(this long sizeInBytes) => (double)sizeInBytes / (1024 * 1024);
        public static long KbToByte(this int sizeInKB) => sizeInKB * 1024;
        public static long MbToBytes(this int sizeInMB) => sizeInMB * 1024 * 1024;
    }
}