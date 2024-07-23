namespace Fwbs.Office.Outlook
{
    internal enum PT
    {
        Unspecified = 0x0000,
        Null = 0x0001,
        I2  = 0x0002,
        Long = 0x0003,
        R4  = 0x0004,
        Double = 0x0005,
        Currency = 0x0006,
        AppTime = 0x0007,
        Error = 0x000a, /* means the given attr contains no value */
        Boolean = 0x000b,
        Object = 0x000d,
        I8  = 0x0014,
        String8 = 0x001e,
        Unicode = 0x001f,
        SystemTime = 0x0040,
        ClassID = 0x0048,
        Binary =0x0102 
    }
}
