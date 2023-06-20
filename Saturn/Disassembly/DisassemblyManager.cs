using System.Reflection.PortableExecutable;

namespace Saturn.Disassembly
{
    public class DisassemblyManager
    {
        public static List<Instruction> GetInstructions(string fileName)
        {
            try
            {
                uint baseAddress = 0;
                //int baseAddressInterval = 0;
                byte[] data = default!;

                using (Stream stream = File.OpenRead(fileName))
                {
                    using var reader = new PEReader(stream);
                    var header = reader.PEHeaders;

                    // TEXT Section Header에 적혀있는 Virtual Address부터 Virtual Size만큼 바이트를 읽는다.
                    var textSectionHeader = header.SectionHeaders.Where(h => h.Name.Equals(".text")).First();
                    data = reader.GetSectionData(textSectionHeader.VirtualAddress).GetContent(0, textSectionHeader.VirtualSize).ToArray();

                    if (header.PEHeader == null)
                    {
                        throw new NullReferenceException(nameof(header.PEHeader));
                    }

                    baseAddress = (uint)((int)header.PEHeader.ImageBase + textSectionHeader.VirtualAddress);
                    //bassAddressInterval = (int)((ulong)baseAddress - reader.PEHeaders.PEHeader.ImageBase);
                }

                var instructions = new List<Instruction>();
                int index = 0;

                while (index < data.Length)
                {
                    var currentAddress = (uint)(baseAddress + index);
                    var description = string.Empty;
                    var hexBytes = new byte[1];
                    try
                    {
                        (hexBytes, description) = DisassemblyParser.Parse(data, ref index);
                    }
                    catch
                    {
                        // Temp
                        if(index >= data.Length - 2)
                        {
                            break;
                        }
                    }

                    instructions.Add(new Instruction(
                        currentAddress, hexBytes, description.ToLower()
                        ));
                }

                return instructions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
