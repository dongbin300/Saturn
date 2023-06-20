namespace Saturn.Disassembly
{
    public class Instruction
    {
        public uint Address { get; set; }
        public string AddressString => Address.ToString("X");
        public byte[] Hex { get; set; }
        public string HexString => BitConverter.ToString(Hex).Replace("-", string.Empty);
        public string Description { get; set; }

        public Instruction(uint address, byte[] hex, string description)
        {
            Address = address;
            Hex = hex;
            Description = description;
        }
    }
}
