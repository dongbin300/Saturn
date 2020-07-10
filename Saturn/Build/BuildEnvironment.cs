namespace Saturn.Build
{
    public class BuildEnvironment
    {
        /// <summary>
        /// PE32 (32bit)
        /// Standard fields : 28 bytes
        /// Windows-specific fields : 68 bytes
        /// Data directories : ~
        /// 
        /// PE32+ (64bit)
        /// Standard fields : 24 bytes
        /// Windows-specific fields : 88 bytes
        /// Data directories : ~
        /// </summary>
        public enum PEFormat : ushort
        {
            PE32 = 0x010B,
            PE32PLUS = 0x020B
        }

        public static PEFormat format = PEFormat.PE32;
    }
}
