using System.Net.NetworkInformation;

namespace Saturn.Disassembly
{
    public class DisassemblyParser
    {
        private static string[] R8 =
        {
            "AL", "CL", "DL", "BL", "AH", "CH", "DH", "BH"
        };

        private static string[] R32 =
        {
            "EAX", "ECX", "EDX", "EBX", "ESP", "EBP", "ESI", "EDI"
        };

        private static string C32(byte[] data, int index)
        {
            byte[] le = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                le[i] = data[index + 3 - i];
            }
            return BitConverter.ToString(le).Replace("-", "");
        }

        private static string VT8(byte data) => data switch
        {
            >= 0x00 and < 0x08 => $"BYTE PTR DS:[{R32[data]}+EAX*1]",
            >= 0x08 and < 0x10 => $"BYTE PTR DS:[{R32[data - 0x08]}+ECX*1]",
            >= 0x10 and < 0x18 => $"BYTE PTR DS:[{R32[data - 0x10]}+EDX*1]",
            >= 0x18 and < 0x20 => $"BYTE PTR DS:[{R32[data - 0x18]}+EBX*1]",
            >= 0x20 and < 0x28 => $"BYTE PTR DS:[{R32[data - 0x20]}+ESP*1]",
            >= 0x28 and < 0x30 => $"BYTE PTR DS:[{R32[data - 0x28]}+EBP*1]",
            >= 0x30 and < 0x38 => $"BYTE PTR DS:[{R32[data - 0x30]}+ESI*1]",
            >= 0x38 and < 0x40 => $"BYTE PTR DS:[{R32[data - 0x38]}+EDI*1]",
            >= 0x40 and < 0x48 => $"BYTE PTR DS:[{R32[data - 0x40]}+EAX*2]",
            >= 0x48 and < 0x50 => $"BYTE PTR DS:[{R32[data - 0x48]}+ECX*2]",
            >= 0x50 and < 0x58 => $"BYTE PTR DS:[{R32[data - 0x50]}+EDX*2]",
            >= 0x58 and < 0x60 => $"BYTE PTR DS:[{R32[data - 0x58]}+EBX*2]",
            >= 0x60 and < 0x68 => $"BYTE PTR DS:[{R32[data - 0x60]}+ESP*2]",
            >= 0x68 and < 0x70 => $"BYTE PTR DS:[{R32[data - 0x68]}+EBP*2]",
            >= 0x70 and < 0x78 => $"BYTE PTR DS:[{R32[data - 0x70]}+ESI*2]",
            >= 0x78 and < 0x80 => $"BYTE PTR DS:[{R32[data - 0x78]}+EDI*2]",
            >= 0x80 and < 0x88 => $"BYTE PTR DS:[{R32[data - 0x80]}+EAX*4]",
            >= 0x88 and < 0x90 => $"BYTE PTR DS:[{R32[data - 0x88]}+ECX*4]",
            >= 0x90 and < 0x98 => $"BYTE PTR DS:[{R32[data - 0x90]}+EDX*4]",
            >= 0x98 and < 0xA0 => $"BYTE PTR DS:[{R32[data - 0x98]}+EBX*4]",
            >= 0xA0 and < 0xA8 => $"BYTE PTR DS:[{R32[data - 0xA0]}+ESP*4]",
            >= 0xA8 and < 0xB0 => $"BYTE PTR DS:[{R32[data - 0xA8]}+EBP*4]",
            >= 0xB0 and < 0xB8 => $"BYTE PTR DS:[{R32[data - 0xB0]}+ESI*4]",
            >= 0xB8 and < 0xC0 => $"BYTE PTR DS:[{R32[data - 0xB8]}+EDI*4]",
            >= 0xC0 and < 0xC8 => $"BYTE PTR DS:[{R32[data - 0xC0]}+EAX*8]",
            >= 0xC8 and < 0xD0 => $"BYTE PTR DS:[{R32[data - 0xC8]}+ECX*8]",
            >= 0xD0 and < 0xD8 => $"BYTE PTR DS:[{R32[data - 0xD0]}+EDX*8]",
            >= 0xD8 and < 0xE0 => $"BYTE PTR DS:[{R32[data - 0xD8]}+EBX*8]",
            >= 0xE0 and < 0xE8 => $"BYTE PTR DS:[{R32[data - 0xE0]}+ESP*8]",
            >= 0xE8 and < 0xF0 => $"BYTE PTR DS:[{R32[data - 0xE8]}+EBP*8]",
            >= 0xF0 and < 0xF8 => $"BYTE PTR DS:[{R32[data - 0xF0]}+ESI*8]",
            >= 0xF8 => $"BYTE PTR DS:[{R32[data - 0xF8]}+EDI*8]"
        };

        private static string VT32(byte data) => data switch
        {
            >= 0x00 and < 0x08 => $"DWORD PTR DS:[{R32[data]}+EAX*1]",
            >= 0x08 and < 0x10 => $"DWORD PTR DS:[{R32[data - 0x08]}+ECX*1]",
            >= 0x10 and < 0x18 => $"DWORD PTR DS:[{R32[data - 0x10]}+EDX*1]",
            >= 0x18 and < 0x20 => $"DWORD PTR DS:[{R32[data - 0x18]}+EBX*1]",
            >= 0x20 and < 0x28 => $"DWORD PTR DS:[{R32[data - 0x20]}+ESP*1]",
            >= 0x28 and < 0x30 => $"DWORD PTR DS:[{R32[data - 0x28]}+EBP*1]",
            >= 0x30 and < 0x38 => $"DWORD PTR DS:[{R32[data - 0x30]}+ESI*1]",
            >= 0x38 and < 0x40 => $"DWORD PTR DS:[{R32[data - 0x38]}+EDI*1]",
            >= 0x40 and < 0x48 => $"DWORD PTR DS:[{R32[data - 0x40]}+EAX*2]",
            >= 0x48 and < 0x50 => $"DWORD PTR DS:[{R32[data - 0x48]}+ECX*2]",
            >= 0x50 and < 0x58 => $"DWORD PTR DS:[{R32[data - 0x50]}+EDX*2]",
            >= 0x58 and < 0x60 => $"DWORD PTR DS:[{R32[data - 0x58]}+EBX*2]",
            >= 0x60 and < 0x68 => $"DWORD PTR DS:[{R32[data - 0x60]}+ESP*2]",
            >= 0x68 and < 0x70 => $"DWORD PTR DS:[{R32[data - 0x68]}+EBP*2]",
            >= 0x70 and < 0x78 => $"DWORD PTR DS:[{R32[data - 0x70]}+ESI*2]",
            >= 0x78 and < 0x80 => $"DWORD PTR DS:[{R32[data - 0x78]}+EDI*2]",
            >= 0x80 and < 0x88 => $"DWORD PTR DS:[{R32[data - 0x80]}+EAX*4]",
            >= 0x88 and < 0x90 => $"DWORD PTR DS:[{R32[data - 0x88]}+ECX*4]",
            >= 0x90 and < 0x98 => $"DWORD PTR DS:[{R32[data - 0x90]}+EDX*4]",
            >= 0x98 and < 0xA0 => $"DWORD PTR DS:[{R32[data - 0x98]}+EBX*4]",
            >= 0xA0 and < 0xA8 => $"DWORD PTR DS:[{R32[data - 0xA0]}+ESP*4]",
            >= 0xA8 and < 0xB0 => $"DWORD PTR DS:[{R32[data - 0xA8]}+EBP*4]",
            >= 0xB0 and < 0xB8 => $"DWORD PTR DS:[{R32[data - 0xB0]}+ESI*4]",
            >= 0xB8 and < 0xC0 => $"DWORD PTR DS:[{R32[data - 0xB8]}+EDI*4]",
            >= 0xC0 and < 0xC8 => $"DWORD PTR DS:[{R32[data - 0xC0]}+EAX*8]",
            >= 0xC8 and < 0xD0 => $"DWORD PTR DS:[{R32[data - 0xC8]}+ECX*8]",
            >= 0xD0 and < 0xD8 => $"DWORD PTR DS:[{R32[data - 0xD0]}+EDX*8]",
            >= 0xD8 and < 0xE0 => $"DWORD PTR DS:[{R32[data - 0xD8]}+EBX*8]",
            >= 0xE0 and < 0xE8 => $"DWORD PTR DS:[{R32[data - 0xE0]}+ESP*8]",
            >= 0xE8 and < 0xF0 => $"DWORD PTR DS:[{R32[data - 0xE8]}+EBP*8]",
            >= 0xF0 and < 0xF8 => $"DWORD PTR DS:[{R32[data - 0xF0]}+ESI*8]",
            >= 0xF8 => $"DWORD PTR DS:[{R32[data - 0xF8]}+EDI*8]"
        };

        public static (byte[], string) Parse(byte[] data, ref int startIndex)
        {
            var result = string.Empty;
            var index = startIndex;

            var b1 = data[index++];
            byte b2 = default!;
            switch (b1)
            {
                #region ADD (0x00 ~ 0x05) 
                case 0x00:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADD BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x3C: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case 0x3C: result = $"ADD {VT8(data[index++])}, BH"; break;
                        case 0x3D: result = $"-"; break;
                        case 0x3E: result = $"ADD BYTE PTR DS:[ESI], BH"; break;
                        case 0x3F: result = $"ADD BYTE PTR DS:[EDI], BH"; break;
                        case >= 0x40 and < 0x48: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"ADD BYTE PTR SS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ADD BYTE PTR SS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ADD BYTE PTR SS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ADD BYTE PTR SS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ADD BYTE PTR SS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ADD BYTE PTR SS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ADD BYTE PTR SS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ADD BYTE PTR SS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ADD {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"ADD {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADD {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"ADD {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"ADD {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"ADD {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"ADD {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"ADD {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x01:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADD DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ADD DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ADD DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ADD DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ADD DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ADD {R8[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"ADD {R8[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADD {R8[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"ADD {R8[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"ADD {R8[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"ADD {R8[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"ADD {R8[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"ADD {R8[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x02:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADD AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"ADD CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"ADD DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"ADD BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"ADD AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"ADD CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"ADD DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"ADD BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"ADD AL, BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"ADD CL, BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"ADD DL, BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"ADD BL, BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"ADD AH, BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"ADD CH, BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"ADD DH, BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"ADD BH, BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"ADD AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ADD CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ADD DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ADD BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ADD AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ADD CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ADD DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ADD BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ADD AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"ADD CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADD DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"ADD BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"ADD AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"ADD CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"ADD DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"ADD BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x03:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADD EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"ADD ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"ADD EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"ADD EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"ADD ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"ADD EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"ADD ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"ADD EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"ADD EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"ADD ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"ADD EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"ADD EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"ADD ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"ADD EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"ADD ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"ADD EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"ADD EAX, DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ADD ECX, DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ADD EDX, DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ADD EBX, DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ADD ESP, DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ADD EBP, DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ADD ESI, DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ADD EDI, DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ADD EAX, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"ADD ECX, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADD EDX, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"ADD EBX, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"ADD ESP, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"ADD EBP, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"ADD ESI, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"ADD EDI, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x04: result = $"ADD AL, {data[index++]:X2}"; break;
                case 0x05: result = $"ADD EAX, {C32(data, index)}"; index += 4; break;
                #endregion

                case 0x06: result = "PUSH ES"; break;
                case 0x07: result = "POP ES"; break;

                #region OR (0x08 ~ 0x0C) 
                case 0x08:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"OR BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"OR BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"OR BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"OR BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"OR BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"OR BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"OR BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"OR BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"OR BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"OR BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"OR BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"OR BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"OR BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"OR BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"OR BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"OR BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"OR BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"OR BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"OR BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"OR BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"OR BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"OR BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"OR BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"OR BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"OR {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"OR {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"OR {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"OR {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"OR {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"OR {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"OR {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"OR {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x09:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"OR DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"OR DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"OR DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"OR DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"OR DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"OR DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"OR DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"OR DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"OR DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"OR DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"OR DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"OR DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"OR DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"OR DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"OR DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"OR DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"OR DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"OR DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"OR DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"OR DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"OR DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"OR DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"OR DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"OR DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"OR {R8[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"OR {R8[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"OR {R8[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"OR {R8[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"OR {R8[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"OR {R8[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"OR {R8[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"OR {R8[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x0A:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"OR AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"OR CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"OR DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"OR BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"OR AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"OR CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"OR DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"OR BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"OR AL, BYTE PTR SS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"OR CL, BYTE PTR SS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"OR DL, BYTE PTR SS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"OR BL, BYTE PTR SS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"OR AH, BYTE PTR SS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"OR CH, BYTE PTR SS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"OR DH, BYTE PTR SS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"OR BH, BYTE PTR SS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"OR AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"OR CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"OR DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"OR BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"OR AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"OR CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"OR DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"OR BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"OR AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"OR CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"OR DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"OR BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"OR AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"OR CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"OR DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"OR BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x0B:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"OR EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"OR ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"OR EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"OR EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"OR ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"OR EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"OR ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"OR EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"OR EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"OR ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"OR EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"OR EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"OR ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"OR EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"OR ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"OR EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"OR EAX, DWORD PTR SS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"OR ECX, DWORD PTR SS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"OR EDX, DWORD PTR SS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"OR EBX, DWORD PTR SS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"OR ESP, DWORD PTR SS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"OR EBP, DWORD PTR SS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"OR ESI, DWORD PTR SS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"OR EDI, DWORD PTR SS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"OR EAX, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"OR ECX, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"OR EDX, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"OR EBX, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"OR ESP, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"OR EBP, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"OR ESI, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"OR EDI, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x0C: result = $"OR AL, {data[index++]:X2}"; break;
                case 0x0D: result = $"OR EAX, {C32(data, index)}"; index += 4; break;
                #endregion

                case 0x0E: result = "PUSH CS"; break;

                #region ADC (0x10 ~ 0x14) 
                case 0x10:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADC BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ADC BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ADC BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ADC BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ADC BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ADC {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"ADC {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADC {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"ADC {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"ADC {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"ADC {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"ADC {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"ADC {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x11:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADC DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ADC DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ADC DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ADC DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ADC DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ADC {R8[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"ADC {R8[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADC {R8[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"ADC {R8[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"ADC {R8[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"ADC {R8[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"ADC {R8[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"ADC {R8[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x12:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADC AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"ADC CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"ADC DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"ADC BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"ADC AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"ADC CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"ADC DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"ADC BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"ADC AL, BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"ADC CL, BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"ADC DL, BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"ADC BL, BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"ADC AH, BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"ADC CH, BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"ADC DH, BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"ADC BH, BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"ADC AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ADC CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ADC DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ADC BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ADC AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ADC CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ADC DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ADC BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ADC AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"ADC CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADC DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"ADC BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"ADC AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"ADC CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"ADC DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"ADC BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x13:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADC EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"ADC ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"ADC EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"ADC EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"ADC ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"ADC EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"ADC ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"ADC EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"ADC EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"ADC ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"ADC EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"ADC EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"ADC ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"ADC EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"ADC ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"ADC EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"ADC EAX, DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ADC ECX, DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ADC EDX, DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ADC EBX, DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ADC ESP, DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ADC EBP, DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ADC ESI, DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ADC EDI, DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ADC EAX, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"ADC ECX, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADC EDX, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"ADC EBX, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"ADC ESP, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"ADC EBP, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"ADC ESI, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"ADC EDI, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x14: result = $"ADC AL, {data[index++]:X2}"; break;
                case 0x15: result = $"ADC EAX, {C32(data, index)}"; index += 4; break;
                #endregion

                case 0x16: result = "PUSH SS"; break;
                case 0x17: result = "POP SS"; break;

                #region SBB (0x18 ~ 0x1C) 
                case 0x18:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"SBB BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"SBB BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"SBB BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"SBB BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"SBB BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"SBB {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"SBB {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"SBB {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"SBB {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"SBB {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"SBB {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"SBB {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"SBB {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x19:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"SBB DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"SBB DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"SBB DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"SBB DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"SBB DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"SBB {R8[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"SBB {R8[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"SBB {R8[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"SBB {R8[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"SBB {R8[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"SBB {R8[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"SBB {R8[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"SBB {R8[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x1A:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"SBB AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"SBB CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"SBB DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"SBB BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"SBB AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"SBB CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"SBB DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"SBB BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"SBB AL, BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"SBB CL, BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"SBB DL, BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"SBB BL, BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"SBB AH, BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"SBB CH, BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"SBB DH, BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"SBB BH, BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"SBB AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"SBB CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"SBB DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"SBB BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"SBB AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"SBB CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"SBB DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"SBB BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"SBB AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"SBB CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"SBB DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"SBB BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"SBB AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"SBB CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"SBB DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"SBB BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x1B:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"SBB EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"SBB ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"SBB EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"SBB EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"SBB ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"SBB EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"SBB ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"SBB EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"SBB EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"SBB ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"SBB EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"SBB EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"SBB ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"SBB EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"SBB ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"SBB EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"SBB EAX, DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"SBB ECX, DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"SBB EDX, DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"SBB EBX, DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"SBB ESP, DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"SBB EBP, DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"SBB ESI, DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"SBB EDI, DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"SBB EAX, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"SBB ECX, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"SBB EDX, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"SBB EBX, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"SBB ESP, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"SBB EBP, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"SBB ESI, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"SBB EDI, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x1C: result = $"SBB AL, {data[index++]:X2}"; break;
                case 0x1D: result = $"SBB EAX, {C32(data, index)}"; index += 4; break;
                #endregion

                case 0x1E: result = "PUSH DS"; break;
                case 0x1F: result = "POP DS"; break;

                #region AND (0x20 ~ 0x24) 
                case 0x20:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"AND BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"AND BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"AND BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"AND BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"AND BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"AND BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"AND BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"AND BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"AND BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"AND BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"AND BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"AND BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"AND BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"AND BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"AND BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"AND BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"AND BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"AND BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"AND BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"AND BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"AND BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"AND BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"AND BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"AND BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"AND {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"AND {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"AND {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"AND {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"AND {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"AND {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"AND {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"AND {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x21:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"AND DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"AND DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"AND DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"AND DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"AND DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"AND DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"AND DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"AND DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"AND DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"AND DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"AND DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"AND DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"AND DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"AND DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"AND DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"AND DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"AND DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"AND DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"AND DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"AND DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"AND DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"AND DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"AND DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"AND DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"AND {R8[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"AND {R8[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"AND {R8[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"AND {R8[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"AND {R8[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"AND {R8[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"AND {R8[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"AND {R8[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x22:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"AND AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"AND CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"AND DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"AND BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"AND AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"AND CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"AND DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"AND BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"AND AL, BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"AND CL, BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"AND DL, BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"AND BL, BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"AND AH, BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"AND CH, BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"AND DH, BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"AND BH, BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"AND AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"AND CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"AND DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"AND BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"AND AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"AND CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"AND DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"AND BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"AND AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"AND CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"AND DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"AND BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"AND AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"AND CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"AND DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"AND BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x23:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"AND EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"AND ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"AND EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"AND EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"AND ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"AND EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"AND ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"AND EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"AND EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"AND ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"AND EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"AND EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"AND ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"AND EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"AND ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"AND EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"AND EAX, DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"AND ECX, DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"AND EDX, DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"AND EBX, DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"AND ESP, DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"AND EBP, DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"AND ESI, DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"AND EDI, DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"AND EAX, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"AND ECX, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"AND EDX, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"AND EBX, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"AND ESP, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"AND EBP, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"AND ESI, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"AND EDI, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x24: result = $"AND AL, {data[index++]:X2}"; break;
                case 0x25: result = $"AND EAX, {C32(data, index)}"; index += 4; break;
                #endregion

                case 0x26: result = "ES"; break;
                case 0x27: result = "DAA"; break;

                #region SUB (0x28 ~ 0x2C) 
                case 0x28:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"SUB BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x14: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case 0x14: result = $"SUB BYTE PTR DS:[{VT8(data[index++])}], DL"; break;
                        case 0x15: result = $"SUB BYTE PTR DS:[{C32(data, index)}], DL"; index += 4; break;
                        case 0x16: result = "SUB BYTE PTR DS:[ESI], DL"; break;
                        case 0x17: result = "SUB BYTE PTR DS:[EDI], DL"; break;
                        case >= 0x18 and < 0x20: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"SUB BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"SUB BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"SUB BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"SUB BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"SUB {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"SUB {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"SUB {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"SUB {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"SUB {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"SUB {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"SUB {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"SUB {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x29:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"SUB DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"SUB DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"SUB DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"SUB DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"SUB DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"SUB {R8[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"SUB {R8[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"SUB {R8[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"SUB {R8[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"SUB {R8[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"SUB {R8[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"SUB {R8[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"SUB {R8[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x2A:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"SUB AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"SUB CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"SUB DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"SUB BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"SUB AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"SUB CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"SUB DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"SUB BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"SUB AL, BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"SUB CL, BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"SUB DL, BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"SUB BL, BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"SUB AH, BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"SUB CH, BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"SUB DH, BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"SUB BH, BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"SUB AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"SUB CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"SUB DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"SUB BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"SUB AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"SUB CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"SUB DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"SUB BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"SUB AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"SUB CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"SUB DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"SUB BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"SUB AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"SUB CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"SUB DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"SUB BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x2B:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"SUB EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"SUB ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"SUB EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"SUB EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"SUB ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"SUB EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"SUB ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"SUB EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"SUB EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"SUB ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"SUB EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"SUB EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"SUB ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"SUB EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"SUB ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"SUB EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"SUB EAX, DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"SUB ECX, DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"SUB EDX, DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"SUB EBX, DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"SUB ESP, DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"SUB EBP, DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"SUB ESI, DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"SUB EDI, DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"SUB EAX, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"SUB ECX, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"SUB EDX, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"SUB EBX, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"SUB ESP, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"SUB EBP, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"SUB ESI, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"SUB EDI, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x2C: result = $"SUB AL, {data[index++]:X2}"; break;
                case 0x2D: result = $"SUB EAX, {C32(data, index)}"; index += 4; break;
                #endregion

                case 0x2E: result = "CS"; break;
                case 0x2F: result = "DAS"; break;

                #region XOR (0x30 ~ 0x34) 
                case 0x30:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"XOR BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"XOR BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"XOR BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"XOR BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"XOR BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"XOR {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"XOR {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"XOR {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"XOR {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"XOR {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"XOR {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"XOR {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"XOR {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x31:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"XOR DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"XOR DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"XOR DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"XOR DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"XOR DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"XOR {R8[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"XOR {R8[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"XOR {R8[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"XOR {R8[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"XOR {R8[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"XOR {R8[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"XOR {R8[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"XOR {R8[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x32:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"XOR AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"XOR CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"XOR DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"XOR BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"XOR AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"XOR CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"XOR DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"XOR BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"XOR AL, BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"XOR CL, BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"XOR DL, BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"XOR BL, BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"XOR AH, BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"XOR CH, BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"XOR DH, BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"XOR BH, BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"XOR AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"XOR CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"XOR DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"XOR BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"XOR AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"XOR CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"XOR DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"XOR BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"XOR AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"XOR CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"XOR DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"XOR BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"XOR AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"XOR CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"XOR DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"XOR BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x33:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"XOR EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"XOR ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"XOR EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"XOR EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"XOR ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"XOR EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"XOR ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"XOR EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"XOR EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"XOR ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"XOR EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"XOR EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"XOR ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"XOR EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"XOR ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"XOR EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"XOR EAX, DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"XOR ECX, DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"XOR EDX, DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"XOR EBX, DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"XOR ESP, DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"XOR EBP, DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"XOR ESI, DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"XOR EDI, DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"XOR EAX, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"XOR ECX, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"XOR EDX, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"XOR EBX, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"XOR ESP, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"XOR EBP, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"XOR ESI, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"XOR EDI, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x34: result = $"XOR AL, {data[index++]:X2}"; break;
                case 0x35: result = $"XOR EAX, {C32(data, index)}"; index += 4; break;
                #endregion

                case 0x36: result = "SS"; break;
                case 0x37: result = "AAA"; break;

                #region CMP (0x38 ~ 0x3C) 
                case 0x38:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"CMP BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"CMP BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"CMP BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"CMP BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"CMP BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"CMP {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"CMP {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"CMP {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"CMP {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"CMP {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"CMP {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"CMP {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"CMP {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x39:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"CMP DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"CMP DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"CMP DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"CMP DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"CMP DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"CMP {R8[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"CMP {R8[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"CMP {R8[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"CMP {R8[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"CMP {R8[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"CMP {R8[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"CMP {R8[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"CMP {R8[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x3A:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"CMP AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"CMP CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"CMP DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"CMP BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"CMP AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"CMP CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"CMP DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"CMP BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"CMP AL, BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"CMP CL, BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"CMP DL, BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"CMP BL, BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"CMP AH, BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"CMP CH, BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"CMP DH, BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"CMP BH, BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"CMP AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"CMP CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"CMP DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"CMP BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"CMP AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"CMP CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"CMP DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"CMP BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"CMP AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"CMP CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"CMP DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"CMP BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"CMP AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"CMP CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"CMP DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"CMP BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x3B:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"CMP EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"CMP ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"CMP EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"CMP EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"CMP ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"CMP EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"CMP ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"CMP EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"CMP EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"CMP ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"CMP EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"CMP EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"CMP ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"CMP EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"CMP ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"CMP EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"CMP EAX, DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"CMP ECX, DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"CMP EDX, DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"CMP EBX, DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"CMP ESP, DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"CMP EBP, DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"CMP ESI, DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"CMP EDI, DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"CMP EAX, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"CMP ECX, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"CMP EDX, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"CMP EBX, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"CMP ESP, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"CMP EBP, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"CMP ESI, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"CMP EDI, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x3C: result = $"CMP AL, {data[index++]:X2}"; break;
                case 0x3D: result = $"CMP EAX, {C32(data, index)}"; index += 4; break;
                #endregion

                case 0x3E: result = "DS"; break;
                case 0x3F: result = "AAS"; break;

                case 0x40: result = "INC EAX"; break;
                case 0x41: result = "INC ECX"; break;
                case 0x42: result = "INC EDX"; break;
                case 0x43: result = "INC EBX"; break;
                case 0x44: result = "INC ESP"; break;
                case 0x45: result = "INC EBP"; break;
                case 0x46: result = "INC ESI"; break;
                case 0x47: result = "INC EDI"; break;
                case 0x48: result = "DEC EAX"; break;
                case 0x49: result = "DEC ECX"; break;
                case 0x4A: result = "DEC EDX"; break;
                case 0x4B: result = "DEC EBX"; break;
                case 0x4C: result = "DEC ESP"; break;
                case 0x4D: result = "DEC EBP"; break;
                case 0x4E: result = "DEC ESI"; break;
                case 0x4F: result = "DEC EDI"; break;
                case 0x50: result = "PUSH EAX"; break;
                case 0x51: result = "PUSH ECX"; break;
                case 0x52: result = "PUSH EDX"; break;
                case 0x53: result = "PUSH EBX"; break;
                case 0x54: result = "PUSH ESP"; break;
                case 0x55: result = "PUSH EBP"; break;
                case 0x56: result = "PUSH ESI"; break;
                case 0x57: result = "PUSH EDI"; break;
                case 0x58: result = "POP EAX"; break;
                case 0x59: result = "POP ECX"; break;
                case 0x5A: result = "POP EDX"; break;
                case 0x5B: result = "POP EBX"; break;
                case 0x5C: result = "POP ESP"; break;
                case 0x5D: result = "POP EBP"; break;
                case 0x5E: result = "POP ESI"; break;
                case 0x5F: result = "POP EDI"; break;
                case 0x60: result = "PUSHA"; break;
                case 0x61: result = "POPA"; break;

                #region BOUND (0x62) 
                case 0x62:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"BOUND EAX, QWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"BOUND ECX, QWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"BOUND EDX, QWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"BOUND EBX, QWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"BOUND ESP, QWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"BOUND EBP, QWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"BOUND ESI, QWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"BOUND EDI, QWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"BOUND EAX, QWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"BOUND ECX, QWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"BOUND EDX, QWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"BOUND EBX, QWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"BOUND ESP, QWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"BOUND EBP, QWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"BOUND ESI, QWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"BOUND EDI, QWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"BOUND EAX, QWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"BOUND ECX, QWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"BOUND EDX, QWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"BOUND EBX, QWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"BOUND ESP, QWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"BOUND EBP, QWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"BOUND ESI, QWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"BOUND EDI, QWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                    }
                    break;
                #endregion

                #region ARPL (0x63) 
                case 0x63:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ARPL WORD PTR DS:[{R32[b2]}], AX"; break;
                        case >= 0x08 and < 0x10: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x08]}], CX"; break;
                        case >= 0x10 and < 0x18: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x10]}], DX"; break;
                        case >= 0x18 and < 0x20: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x18]}], BX"; break;
                        case >= 0x20 and < 0x28: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x20]}], SP"; break;
                        case >= 0x28 and < 0x30: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x28]}], BP"; break;
                        case >= 0x30 and < 0x38: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x30]}], SI"; break;
                        case >= 0x38 and < 0x40: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x38]}], DI"; break;
                        case >= 0x40 and < 0x48: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AX"; break;
                        case >= 0x48 and < 0x50: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CX"; break;
                        case >= 0x50 and < 0x58: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DX"; break;
                        case >= 0x58 and < 0x60: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BX"; break;
                        case >= 0x60 and < 0x68: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], SP"; break;
                        case >= 0x68 and < 0x70: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], BP"; break;
                        case >= 0x70 and < 0x78: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], SI"; break;
                        case >= 0x78 and < 0x80: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], DI"; break;
                        case >= 0x80 and < 0x88: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"ARPL WORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"ARPL WORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], SP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"ARPL WORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], BP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"ARPL WORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], SI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"ARPL WORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], DI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"ARPL {R8[b2 - 0xC0]}, AX"; break;
                        case >= 0xC8 and < 0xD0: result = $"ARPL {R8[b2 - 0xC8]}, CX"; break;
                        case >= 0xD0 and < 0xD8: result = $"ARPL {R8[b2 - 0xD0]}, DX"; break;
                        case >= 0xD8 and < 0xE0: result = $"ARPL {R8[b2 - 0xD8]}, BX"; break;
                        case >= 0xE0 and < 0xE8: result = $"ARPL {R8[b2 - 0xE0]}, SP"; break;
                        case >= 0xE8 and < 0xF0: result = $"ARPL {R8[b2 - 0xE8]}, BP"; break;
                        case >= 0xF0 and < 0xF8: result = $"ARPL {R8[b2 - 0xF0]}, SI"; break;
                        case >= 0xF8: result = $"ARPL {R8[b2 - 0xF8]}, DI"; break;
                    }
                    break;
                #endregion

                case 0x64: result = "FS"; break;
                case 0x65: result = "GS"; break;
                case 0x66: result = "DATA16"; break;
                case 0x67: result = "ADDR16"; break;
                case 0x6A: result = $"PUSH {data[index++]:X2}"; break;
                case 0x6C: result = $"INS BYTE PTR ES:[EDI], DX"; break;
                case 0x6D: result = $"INS DWORD PTR ES:[EDI], DX"; break;
                case 0x6E: result = $"OUTS DX,BYTE PTR DS:[ESI]"; break;
                case 0x6F: result = $"OUTS DX,DWORD PTR DS:[ESI]"; break;
                case 0x70: result = $"JO {data[index++] + 2:X2}"; break;
                case 0x71: result = $"JNO {data[index++] + 2:X2}"; break;
                case 0x72: result = $"JB {data[index++] + 2:X2}"; break;
                case 0x73: result = $"JAE {data[index++] + 2:X2}"; break;
                case 0x74: result = $"JE {data[index++] + 2:X2}"; break;
                case 0x75: result = $"JNE {data[index++] + 2:X2}"; break;
                case 0x76: result = $"JBE {data[index++] + 2:X2}"; break;
                case 0x77: result = $"JA {data[index++] + 2:X2}"; break;
                case 0x78: result = $"JS {data[index++] + 2:X2}"; break;
                case 0x79: result = $"JNS {data[index++] + 2:X2}"; break;
                case 0x7A: result = $"JP {data[index++] + 2:X2}"; break;
                case 0x7B: result = $"JNP {data[index++] + 2:X2}"; break;
                case 0x7C: result = $"JL {data[index++] + 2:X2}"; break;
                case 0x7D: result = $"JGE {data[index++] + 2:X2}"; break;
                case 0x7E: result = $"JLE {data[index++] + 2:X2}"; break;
                case 0x7F: result = $"JG {data[index++] + 2:X2}"; break;

                #region ADD, OR, ADC, SBB, AND, SUB, XOR, CMP (0x80, 0x82, 0x83) 
                case 0x80:
                case 0x82:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADD BYTE PTR DS:[{R32[b2]}], {data[index++]:X2}"; break;
                        case >= 0x08 and < 0x10: result = $"OR BYTE PTR DS:[{R32[b2 - 0x08]}], {data[index++]:X2}"; break;
                        case >= 0x10 and < 0x18: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x10]}], {data[index++]:X2}"; break;
                        case >= 0x18 and < 0x20: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x18]}], {data[index++]:X2}"; break;
                        case >= 0x20 and < 0x28: result = $"AND BYTE PTR DS:[{R32[b2 - 0x20]}], {data[index++]:X2}"; break;
                        case >= 0x28 and < 0x30: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x28]}], {data[index++]:X2}"; break;
                        case >= 0x30 and < 0x38: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x30]}], {data[index++]:X2}"; break;
                        case >= 0x38 and < 0x40: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x38]}], {data[index++]:X2}"; break;
                        case >= 0x40 and < 0x48: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x48 and < 0x50: result = $"OR BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x50 and < 0x58: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x58 and < 0x60: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x60 and < 0x68: result = $"AND BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x68 and < 0x70: result = $"SUB BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x70 and < 0x78: result = $"XOR BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x78 and < 0x80: result = $"CMP BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x80 and < 0x88: result = $"ADD BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0x88 and < 0x90: result = $"OR BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0x90 and < 0x98: result = $"ADC BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0x98 and < 0xA0: result = $"SBB BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xA0 and < 0xA8: result = $"AND BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xA8 and < 0xB0: result = $"SUB BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xB0 and < 0xB8: result = $"XOR BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xB8 and < 0xC0: result = $"CMP BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xC0 and < 0xC8: result = $"ADD {R8[b2 - 0xC0]}, {data[index++]:X2}"; break;
                        case >= 0xC8 and < 0xD0: result = $"OR {R8[b2 - 0xC8]}, {data[index++]:X2}"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADC {R8[b2 - 0xD0]}, {data[index++]:X2}"; break;
                        case >= 0xD8 and < 0xE0: result = $"SBB {R8[b2 - 0xD8]}, {data[index++]:X2}"; break;
                        case >= 0xE0 and < 0xE8: result = $"AND {R8[b2 - 0xE0]}, {data[index++]:X2}"; break;
                        case >= 0xE8 and < 0xF0: result = $"SUB {R8[b2 - 0xE8]}, {data[index++]:X2}"; break;
                        case >= 0xF0 and < 0xF8: result = $"XOR {R8[b2 - 0xF0]}, {data[index++]:X2}"; break;
                        case >= 0xF8: result = $"CMP {R8[b2 - 0xF8]}, {data[index++]:X2}"; break;
                    }
                    break;

                case 0x83:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ADD DWORD PTR DS:[{R32[b2]}], {data[index++]:X2}"; break;
                        case >= 0x08 and < 0x10: result = $"OR DWORD PTR DS:[{R32[b2 - 0x08]}], {data[index++]:X2}"; break;
                        case >= 0x10 and < 0x18: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x10]}], {data[index++]:X2}"; break;
                        case >= 0x18 and < 0x20: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x18]}], {data[index++]:X2}"; break;
                        case >= 0x20 and < 0x28: result = $"AND DWORD PTR DS:[{R32[b2 - 0x20]}], {data[index++]:X2}"; break;
                        case >= 0x28 and < 0x30: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x28]}], {data[index++]:X2}"; break;
                        case >= 0x30 and < 0x38: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x30]}], {data[index++]:X2}"; break;
                        case >= 0x38 and < 0x40: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x38]}], {data[index++]:X2}"; break;
                        case >= 0x40 and < 0x48: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x48 and < 0x50: result = $"OR DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x50 and < 0x58: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x58 and < 0x60: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x60 and < 0x68: result = $"AND DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x68 and < 0x70: result = $"SUB DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x70 and < 0x78: result = $"XOR DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x78 and < 0x80: result = $"CMP DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], {data[index++]:X2}"; break;
                        case >= 0x80 and < 0x88: result = $"ADD DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0x88 and < 0x90: result = $"OR DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0x90 and < 0x98: result = $"ADC DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0x98 and < 0xA0: result = $"SBB DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xA0 and < 0xA8: result = $"AND DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xA8 and < 0xB0: result = $"SUB DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xB0 and < 0xB8: result = $"XOR DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xB8 and < 0xC0: result = $"CMP DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], {data[index + 4]:X2}"; index += 5; break;
                        case >= 0xC0 and < 0xC8: result = $"ADD {R32[b2 - 0xC0]}, {data[index++]:X2}"; break;
                        case >= 0xC8 and < 0xD0: result = $"OR {R32[b2 - 0xC8]}, {data[index++]:X2}"; break;
                        case >= 0xD0 and < 0xD8: result = $"ADC {R32[b2 - 0xD0]}, {data[index++]:X2}"; break;
                        case >= 0xD8 and < 0xE0: result = $"SBB {R32[b2 - 0xD8]}, {data[index++]:X2}"; break;
                        case >= 0xE0 and < 0xE8: result = $"AND {R32[b2 - 0xE0]}, {data[index++]:X2}"; break;
                        case >= 0xE8 and < 0xF0: result = $"SUB {R32[b2 - 0xE8]}, {data[index++]:X2}"; break;
                        case >= 0xF0 and < 0xF8: result = $"XOR {R32[b2 - 0xF0]}, {data[index++]:X2}"; break;
                        case >= 0xF8: result = $"CMP {R32[b2 - 0xF8]}, {data[index++]:X2}"; break;
                    }
                    break;
                #endregion

                #region TEST (0x84 ~ 0x85) 
                case 0x84:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"TEST BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"TEST BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"TEST BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"TEST BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"TEST BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"TEST BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"TEST {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"TEST {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"TEST {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"TEST {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"TEST {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"TEST {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"TEST {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"TEST {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x85:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"TEST DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"TEST DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"TEST DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"TEST DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"TEST DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"TEST {R32[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"TEST {R32[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"TEST {R32[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"TEST {R32[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"TEST {R32[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"TEST {R32[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"TEST {R32[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"TEST {R32[b2 - 0xF8]}, EDI"; break;
                    }
                    break;
                #endregion

                #region XCHG (0x86 ~ 0x87) 
                case 0x86:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"XCHG BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"XCHG BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"XCHG {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"XCHG {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"XCHG {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"XCHG {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"XCHG {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"XCHG {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"XCHG {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"XCHG {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x87:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"XCHG DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"XCHG DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"XCHG {R32[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"XCHG {R32[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"XCHG {R32[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"XCHG {R32[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"XCHG {R32[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"XCHG {R32[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"XCHG {R32[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"XCHG {R32[b2 - 0xF8]}, EDI"; break;
                    }
                    break;
                #endregion

                #region MOV (0x88 ~ 0x8C) 
                case 0x88:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"MOV BYTE PTR DS:[{R32[b2]}], AL"; break;
                        case >= 0x08 and < 0x10: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x10]}], DL"; break;
                        case >= 0x18 and < 0x20: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x18]}], BL"; break;
                        case >= 0x20 and < 0x28: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x20]}], AH"; break;
                        case >= 0x28 and < 0x30: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x28]}], CH"; break;
                        case >= 0x30 and < 0x38: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x30]}], DH"; break;
                        case >= 0x38 and < 0x40: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x38]}], BH"; break;
                        case >= 0x40 and < 0x48: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], AL"; break;
                        case >= 0x48 and < 0x50: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], CL"; break;
                        case >= 0x50 and < 0x58: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], DL"; break;
                        case >= 0x58 and < 0x60: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], BL"; break;
                        case >= 0x60 and < 0x68: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], AH"; break;
                        case >= 0x68 and < 0x70: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], CH"; break;
                        case >= 0x70 and < 0x78: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], DH"; break;
                        case >= 0x78 and < 0x80: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], BH"; break;
                        case >= 0x80 and < 0x88: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], AL"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], CL"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], DL"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"MOV BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], BL"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"MOV BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], AH"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"MOV BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], CH"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"MOV BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], DH"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"MOV BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], BH"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"MOV {R8[b2 - 0xC0]}, AL"; break;
                        case >= 0xC8 and < 0xD0: result = $"MOV {R8[b2 - 0xC8]}, CL"; break;
                        case >= 0xD0 and < 0xD8: result = $"MOV {R8[b2 - 0xD0]}, DL"; break;
                        case >= 0xD8 and < 0xE0: result = $"MOV {R8[b2 - 0xD8]}, BL"; break;
                        case >= 0xE0 and < 0xE8: result = $"MOV {R8[b2 - 0xE0]}, AH"; break;
                        case >= 0xE8 and < 0xF0: result = $"MOV {R8[b2 - 0xE8]}, CH"; break;
                        case >= 0xF0 and < 0xF8: result = $"MOV {R8[b2 - 0xF0]}, DH"; break;
                        case >= 0xF8: result = $"MOV {R8[b2 - 0xF8]}, BH"; break;
                    }
                    break;

                case 0x89:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"MOV DWORD PTR DS:[{R32[b2]}], EAX"; break;
                        case >= 0x08 and < 0x10: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x08]}], ECX"; break;
                        case >= 0x10 and < 0x18: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x10]}], EDX"; break;
                        case >= 0x18 and < 0x20: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x18]}], EBX"; break;
                        case >= 0x20 and < 0x28: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x20]}], ESP"; break;
                        case >= 0x28 and < 0x30: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x28]}], EBP"; break;
                        case >= 0x30 and < 0x38: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x30]}], ESI"; break;
                        case >= 0x38 and < 0x40: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x38]}], EDI"; break;
                        case >= 0x40 and < 0x48: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}], EAX"; break;
                        case >= 0x48 and < 0x50: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}], ECX"; break;
                        case >= 0x50 and < 0x58: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}], EDX"; break;
                        case >= 0x58 and < 0x60: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}], EBX"; break;
                        case >= 0x60 and < 0x68: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}], ESP"; break;
                        case >= 0x68 and < 0x70: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}], EBP"; break;
                        case >= 0x70 and < 0x78: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}], ESI"; break;
                        case >= 0x78 and < 0x80: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}], EDI"; break;
                        case >= 0x80 and < 0x88: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}], EAX"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}], ECX"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}], EDX"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"MOV DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}], EBX"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"MOV DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}], ESP"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"MOV DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}], EBP"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"MOV DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}], ESI"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"MOV DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}], EDI"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"MOV {R32[b2 - 0xC0]}, EAX"; break;
                        case >= 0xC8 and < 0xD0: result = $"MOV {R32[b2 - 0xC8]}, ECX"; break;
                        case >= 0xD0 and < 0xD8: result = $"MOV {R32[b2 - 0xD0]}, EDX"; break;
                        case >= 0xD8 and < 0xE0: result = $"MOV {R32[b2 - 0xD8]}, EBX"; break;
                        case >= 0xE0 and < 0xE8: result = $"MOV {R32[b2 - 0xE0]}, ESP"; break;
                        case >= 0xE8 and < 0xF0: result = $"MOV {R32[b2 - 0xE8]}, EBP"; break;
                        case >= 0xF0 and < 0xF8: result = $"MOV {R32[b2 - 0xF0]}, ESI"; break;
                        case >= 0xF8: result = $"MOV {R32[b2 - 0xF8]}, EDI"; break;
                    }
                    break;

                case 0x8A:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"MOV AL, BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"MOV CL, BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"MOV DL, BYTE PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"MOV BL, BYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"MOV AH, BYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"MOV CH, BYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"MOV DH, BYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"MOV BH, BYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"MOV AL, BYTE PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"MOV CL, BYTE PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"MOV DL, BYTE PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"MOV BL, BYTE PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"MOV AH, BYTE PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"MOV CH, BYTE PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"MOV DH, BYTE PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"MOV BH, BYTE PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"MOV AL, BYTE PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"MOV CL, BYTE PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"MOV DL, BYTE PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"MOV BL, BYTE PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"MOV AH, BYTE PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"MOV CH, BYTE PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"MOV DH, BYTE PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"MOV BH, BYTE PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"MOV AL, {R8[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"MOV CL, {R8[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"MOV DL, {R8[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"MOV BL, {R8[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"MOV AH, {R8[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"MOV CH, {R8[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"MOV DH, {R8[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"MOV BH, {R8[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x8B:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"MOV EAX, DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"MOV ECX, DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"MOV EDX, DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"MOV EBX, DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"MOV ESP, DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"MOV EBP, DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"MOV ESI, DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"MOV EDI, DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"MOV EAX, DWORD PTR DS:[{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"MOV ECX, DWORD PTR DS:[{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"MOV EDX, DWORD PTR DS:[{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"MOV EBX, DWORD PTR DS:[{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"MOV ESP, DWORD PTR DS:[{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"MOV EBP, DWORD PTR DS:[{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"MOV ESI, DWORD PTR DS:[{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"MOV EDI, DWORD PTR DS:[{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"MOV EAX, DWORD PTR DS:[{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"MOV ECX, DWORD PTR DS:[{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"MOV EDX, DWORD PTR DS:[{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"MOV EBX, DWORD PTR DS:[{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"MOV ESP, DWORD PTR DS:[{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"MOV EBP, DWORD PTR DS:[{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"MOV ESI, DWORD PTR DS:[{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"MOV EDI, DWORD PTR DS:[{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xC0 and < 0xC8: result = $"MOV EAX, {R32[b2 - 0xC0]}"; break;
                        case >= 0xC8 and < 0xD0: result = $"MOV ECX, {R32[b2 - 0xC8]}"; break;
                        case >= 0xD0 and < 0xD8: result = $"MOV EDX, {R32[b2 - 0xD0]}"; break;
                        case >= 0xD8 and < 0xE0: result = $"MOV EBX, {R32[b2 - 0xD8]}"; break;
                        case >= 0xE0 and < 0xE8: result = $"MOV ESP, {R32[b2 - 0xE0]}"; break;
                        case >= 0xE8 and < 0xF0: result = $"MOV EBP, {R32[b2 - 0xE8]}"; break;
                        case >= 0xF0 and < 0xF8: result = $"MOV ESI, {R32[b2 - 0xF0]}"; break;
                        case >= 0xF8: result = $"MOV EDI, {R32[b2 - 0xF8]}"; break;
                    }
                    break;

                case 0x8C:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"MOV WORD PTR DS:[{R32[b2]}], ES"; break;
                        case >= 0x08 and < 0x10: result = $"MOV WORD PTR DS:[{R32[b2 - 0x08]}], CS"; break;
                        case >= 0x10 and < 0x18: result = $"MOV WORD PTR DS:[{R32[b2 - 0x10]}], SS"; break;
                        case >= 0x18 and < 0x20: result = $"MOV WORD PTR DS:[{R32[b2 - 0x18]}], DS"; break;
                        case >= 0x20 and < 0x28: result = $"MOV WORD PTR DS:[{R32[b2 - 0x20]}], FS"; break;
                        case >= 0x28 and < 0x30: result = $"MOV WORD PTR DS:[{R32[b2 - 0x28]}], GS"; break;
                    }
                    break;
                #endregion

                #region LEA (0x8D) 
                case 0x8D:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"LEA EAX, [{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"LEA ECX, [{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"LEA EDX, [{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"LEA EBX, [{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"LEA ESP, [{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"LEA EBP, [{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"LEA ESI, [{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"LEA EDI, [{R32[b2 - 0x38]}]"; break;
                        case >= 0x40 and < 0x48: result = $"LEA EAX, [{R32[b2 - 0x40]}+{data[index++]:X2}]"; break;
                        case >= 0x48 and < 0x50: result = $"LEA ECX, [{R32[b2 - 0x48]}+{data[index++]:X2}]"; break;
                        case >= 0x50 and < 0x58: result = $"LEA EDX, [{R32[b2 - 0x50]}+{data[index++]:X2}]"; break;
                        case >= 0x58 and < 0x60: result = $"LEA EBX, [{R32[b2 - 0x58]}+{data[index++]:X2}]"; break;
                        case >= 0x60 and < 0x68: result = $"LEA ESP, [{R32[b2 - 0x60]}+{data[index++]:X2}]"; break;
                        case >= 0x68 and < 0x70: result = $"LEA EBP, [{R32[b2 - 0x68]}+{data[index++]:X2}]"; break;
                        case >= 0x70 and < 0x78: result = $"LEA ESI, [{R32[b2 - 0x70]}+{data[index++]:X2}]"; break;
                        case >= 0x78 and < 0x80: result = $"LEA EDI, [{R32[b2 - 0x78]}+{data[index++]:X2}]"; break;
                        case >= 0x80 and < 0x88: result = $"LEA EAX, [{R32[b2 - 0x80]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x88 and < 0x90: result = $"LEA ECX, [{R32[b2 - 0x88]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x90 and < 0x98: result = $"LEA EDX, [{R32[b2 - 0x90]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0x98 and < 0xA0: result = $"LEA EBX, [{R32[b2 - 0x98]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA0 and < 0xA8: result = $"LEA ESP, [{R32[b2 - 0xA0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xA8 and < 0xB0: result = $"LEA EBP, [{R32[b2 - 0xA8]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB0 and < 0xB8: result = $"LEA ESI, [{R32[b2 - 0xB0]}+{C32(data, index)}]"; index += 4; break;
                        case >= 0xB8 and < 0xC0: result = $"LEA EDI, [{R32[b2 - 0xB8]}+{C32(data, index)}]"; index += 4; break;
                    }
                    break;
                #endregion

                #region MOV (0x8E) 
                case 0x8E:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"MOV ES, WORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"MOV CS, WORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"MOV SS, WORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"MOV DS, WORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"MOV FS, WORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"MOV GS, WORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                    }
                    break;
                #endregion

                case 0x8F:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"POP DWORD PTR DS:[{R32[b2]}]"; break;
                    }
                    break;

                case 0x90: result = "NOP"; break;
                case 0x91: result = "XCHG ECX, EAX"; break;
                case 0x92: result = "XCHG EDX, EAX"; break;
                case 0x93: result = "XCHG EBX, EAX"; break;
                case 0x94: result = "XCHG ESP, EAX"; break;
                case 0x95: result = "XCHG EBP, EAX"; break;
                case 0x96: result = "XCHG ESI, EAX"; break;
                case 0x97: result = "XCHG EDI, EAX"; break;
                case 0x98: result = "CWDE"; break;
                case 0x99: result = "CDQ"; break;

                case 0x9B: result = "FWAIT"; break;
                case 0x9C: result = "PUSHF"; break;
                case 0x9D: result = "POPF"; break;
                case 0x9E: result = "SAHF"; break;
                case 0x9F: result = "LAHF"; break;

                case 0xA0: result = $"MOV AL, DS:{C32(data, index)}"; index += 4; break;
                case 0xA1: result = $"MOV EAX, DWORD PTR DS:[{C32(data, index)}]"; index += 4; break;
                case 0xA2: result = $"MOV DS:{C32(data, index)}, AL"; index += 4; break;
                case 0xA3: result = $"MOV DS:{C32(data, index)}, EAX"; index += 4; break;
                case 0xA4: result = "MOVS BYTE PTR ES:[EDI], BYTE PTR DS:[ESI]"; break;
                case 0xA5: result = "MOVS DWORD PTR ES:[EDI], DWORD PTR DS:[ESI]"; break;
                case 0xA6: result = "CMPS BYTE PTR ES:[EDI], BYTE PTR DS:[ESI]"; break;
                case 0xA7: result = "CMPS DWORD PTR ES:[EDI], DWORD PTR DS:[ESI]"; break;
                case 0xA8: result = $"TEST AL, {data[index++]:X2}"; break;
                case 0xA9: result = $"TEST EAX, {C32(data, index)}"; index += 4; break;
                case 0xAA: result = "STOS BYTE PTR ES:[EDI], AL"; break;
                case 0xAB: result = "STOS DWORD PTR ES:[EDI], EAX"; break;
                case 0xAC: result = "LODS AL, BYTE PTR DS:[ESI]"; break;
                case 0xAD: result = "LODS EAX, DWORD PTR DS:[ESI]"; break;
                case 0xAE: result = "SCAS AL, BYTE PTR ES:[EDI]"; break;
                case 0xAF: result = "SCAS EAX, DWORD PTR ES:[EDI]"; break;

                case 0xB0: result = $"MOV AL, {data[index++]:X2}"; break;
                case 0xB1: result = $"MOV CL, {data[index++]:X2}"; break;
                case 0xB2: result = $"MOV DL, {data[index++]:X2}"; break;
                case 0xB3: result = $"MOV BL, {data[index++]:X2}"; break;
                case 0xB4: result = $"MOV AH, {data[index++]:X2}"; break;
                case 0xB5: result = $"MOV CH, {data[index++]:X2}"; break;
                case 0xB6: result = $"MOV DH, {data[index++]:X2}"; break;
                case 0xB7: result = $"MOV BH, {data[index++]:X2}"; break;

                case 0xC3: result = "RET"; break;

                #region LES (0xC4) 
                case 0xC4:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"LES EAX, FWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"LES ECX, FWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"LES EDX, FWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"LES EBX, FWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"LES ESP, FWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"LES EBP, FWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"LES ESI, FWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"LES EDI, FWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;
                #endregion

                #region LDS (0xC5) 
                case 0xC5:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"LDS EAX, FWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"LDS ECX, FWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"LDS EDX, FWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"LDS EBX, FWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"LDS ESP, FWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"LDS EBP, FWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"LDS ESI, FWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"LDS EDI, FWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;
                #endregion

                case 0xC9: result = "LEAVE"; break;

                case 0xCB: result = "RETF"; break;
                case 0xCC: result = "INT3"; break;
                case 0xCD: result = $"INT {data[index++]:X2}"; break;
                case 0xCE: result = "INTO"; break;
                case 0xCF: result = "IRET"; break;

                #region ROL, ROR, RCL, RCR, SHL, SHR (0xD0 ~ 0xD3) 
                case 0xD0:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ROL BYTE PTR DS:[{R32[b2]}], 1"; break;
                        case >= 0x08 and < 0x10: result = $"ROR BYTE PTR DS:[{R32[b2 - 0x08]}], 1"; break;
                        case >= 0x10 and < 0x18: result = $"RCL BYTE PTR DS:[{R32[b2 - 0x10]}], 1"; break;
                        case >= 0x18 and < 0x20: result = $"RCR BYTE PTR DS:[{R32[b2 - 0x18]}], 1"; break;
                        case >= 0x20 and < 0x28: result = $"SHL BYTE PTR DS:[{R32[b2 - 0x20]}], 1"; break;
                        case >= 0x28 and < 0x30: result = $"SHR BYTE PTR DS:[{R32[b2 - 0x28]}], 1"; break;
                    }
                    break;

                case 0xD1:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ROL DWORD PTR DS:[{R32[b2]}], 1"; break;
                        case >= 0x08 and < 0x10: result = $"ROR DWORD PTR DS:[{R32[b2 - 0x08]}], 1"; break;
                        case >= 0x10 and < 0x18: result = $"RCL DWORD PTR DS:[{R32[b2 - 0x10]}], 1"; break;
                        case >= 0x18 and < 0x20: result = $"RCR DWORD PTR DS:[{R32[b2 - 0x18]}], 1"; break;
                        case >= 0x20 and < 0x28: result = $"SHL DWORD PTR DS:[{R32[b2 - 0x20]}], 1"; break;
                        case >= 0x28 and < 0x30: result = $"SHR DWORD PTR DS:[{R32[b2 - 0x28]}], 1"; break;
                    }
                    break;

                case 0xD2:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ROL BYTE PTR DS:[{R32[b2]}], CL"; break;
                        case >= 0x08 and < 0x10: result = $"ROR BYTE PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"RCL BYTE PTR DS:[{R32[b2 - 0x10]}], CL"; break;
                        case >= 0x18 and < 0x20: result = $"RCR BYTE PTR DS:[{R32[b2 - 0x18]}], CL"; break;
                        case >= 0x20 and < 0x28: result = $"SHL BYTE PTR DS:[{R32[b2 - 0x20]}], CL"; break;
                        case >= 0x28 and < 0x30: result = $"SHR BYTE PTR DS:[{R32[b2 - 0x28]}], CL"; break;
                    }
                    break;

                case 0xD3:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"ROL DWORD PTR DS:[{R32[b2]}], CL"; break;
                        case >= 0x08 and < 0x10: result = $"ROR DWORD PTR DS:[{R32[b2 - 0x08]}], CL"; break;
                        case >= 0x10 and < 0x18: result = $"RCL DWORD PTR DS:[{R32[b2 - 0x10]}], CL"; break;
                        case >= 0x18 and < 0x20: result = $"RCR DWORD PTR DS:[{R32[b2 - 0x18]}], CL"; break;
                        case >= 0x20 and < 0x28: result = $"SHL DWORD PTR DS:[{R32[b2 - 0x20]}], CL"; break;
                        case >= 0x28 and < 0x30: result = $"SHR DWORD PTR DS:[{R32[b2 - 0x28]}], CL"; break;
                    }
                    break;
                #endregion

                case 0xD4: result = $"AAM {data[index++]:X2}"; break;
                case 0xD5: result = $"AAD {data[index++]:X2}"; break;

                case 0xD7: result = "XLAT BYTE PTR DS:[EBX]"; break;

                #region F___ (0xD8 ~ 0xDF) 
                case 0xD8:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"FADD DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"FMUL DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"FCOM DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"FCOMP DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"FSUB DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"FSUBR DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"FDIV DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"FDIVR DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;

                case 0xD9:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"FLD DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x10 and < 0x18: result = $"FST DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"FSTP DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"FLDENV [{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"FLDCW WORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"FNSTENV [{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"FNSTCW WORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;

                case 0xDA:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"FIADD DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"FIMUL DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"FICOM DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"FICOMP DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"FISUB DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"FISUBR DWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"FIDIV DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"FIDIVR DWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;

                case 0xDB:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"FILD DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"FISTTP DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"FIST DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"FISTP DWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x28 and < 0x30: result = $"FLD TBYTE PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x38 and < 0x40: result = $"FSTP TBYTE PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;

                case 0xDC:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"FADD QWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"FMUL QWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"FCOM QWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"FCOMP QWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"FSUB QWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"FSUBR QWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"FDIV QWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"FDIVR QWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;

                case 0xDD:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"FLD QWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"FISTTP QWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"FST QWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"FSTP QWORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"FRSTOR [{R32[b2 - 0x20]}]"; break;
                        case >= 0x30 and < 0x38: result = $"FNSAVE [{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"FNSTSW WORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;

                case 0xDE:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"FIADD WORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"FIMUL WORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"FICOM WORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"FICOMP WORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"FISUB WORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"FISUBR WORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"FIDIV WORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"FIDIVR WORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;

                case 0xDF:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"FILD WORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"FISTTP WORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"FIST WORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"FISTP WORD PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"FBLD TBYTE PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"FILD QWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"FBSTP TBYTE PTR DS:[{R32[b2 - 0x30]}]"; break;
                        case >= 0x38 and < 0x40: result = $"FISTP QWORD PTR DS:[{R32[b2 - 0x38]}]"; break;
                    }
                    break;
                #endregion

                case 0xE0: result = $"LOOPNE {data[index++] + 2:X2}"; break;
                case 0xE1: result = $"LOOPE {data[index++] + 2:X2}"; break;
                case 0xE2: result = $"LOOP {data[index++] + 2:X2}"; break;
                case 0xE3: result = $"JECXZ {data[index++] + 2:X2}"; break;
                case 0xE4: result = $"IN AL, {data[index++]:X2}"; break;
                case 0xE5: result = $"IN EAX, {data[index++]:X2}"; break;
                case 0xE6: result = $"OUT {data[index++]:X2}, AL"; break;
                case 0xE7: result = $"OUT {data[index++]:X2}, EAX"; break;

                case 0xEB: result = $"JMP {data[index++] + 2:X2}"; break;
                case 0xEC: result = "IN AL, DX"; break;
                case 0xED: result = "IN EAX, DX"; break;
                case 0xEE: result = "OUT DX, AL"; break;
                case 0xEF: result = "OUT DX, EAX"; break;
                case 0xF0: result = "LOCK"; break;
                case 0xF1: result = "ICEBP"; break;
                case 0xF2: result = "REPNZ"; break;
                case 0xF3: result = "REPZ"; break;
                case 0xF4: result = "HLT"; break;
                case 0xF5: result = "CMC"; break;

                case 0xF7:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"TEST DWORD PTR DS:[{R32[b2]}], {C32(data, index)}"; index += 4; break;
                        case >= 0x08 and < 0x10: result = $"TEST DWORD PTR DS:[{R32[b2 - 0x08]}], {C32(data, index)}"; index += 4; break;
                        case >= 0x10 and < 0x14: result = $"NOT DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case 0x14: result = $"NOT {VT32(data[index++])}"; break;
                        case 0x15: result = $"NOT DWORD PTR DS:{C32(data, index)}"; index += 4; break;
                        case 0x16: result = $"NOT DWORD PTR DS:[ESI]"; break;
                        case 0x17: result = $"NOT DWORD PTR DS:[EDI]"; break;

                        case >= 0x18 and < 0x1C: result = $"NEG DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case 0x1C: result = $"NEG {VT32(data[index++])}"; break;
                        case 0x1D: result = $"NEG DWORD PTR DS:{C32(data, index)}"; index += 4; break;
                        case 0x1E: result = $"NEG DWORD PTR DS:[ESI]"; break;
                        case 0x1F: result = $"NEG DWORD PTR DS:[EDI]"; break;

                        case >= 0x20 and < 0x24: result = $"MUL DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case 0x24: result = $"MUL {VT32(data[index++])}"; break;
                        case 0x25: result = $"MUL DWORD PTR DS:{C32(data, index)}"; index += 4; break;
                        case 0x26: result = $"MUL DWORD PTR DS:[ESI]"; break;
                        case 0x27: result = $"MUL DWORD PTR DS:[EDI]"; break;

                        case >= 0x28 and < 0x2C: result = $"IMUL DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case 0x2C: result = $"IMUL {VT32(data[index++])}"; break;
                        case 0x2D: result = $"IMUL DWORD PTR DS:{C32(data, index)}"; index += 4; break;
                        case 0x2E: result = $"IMUL DWORD PTR DS:[ESI]"; break;
                        case 0x2F: result = $"IMUL DWORD PTR DS:[EDI]"; break;

                        case >= 0x30 and < 0x34: result = $"DIV DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case 0x34: result = $"DIV {VT32(data[index++])}"; break;
                        case 0x35: result = $"DIV DWORD PTR DS:{C32(data, index)}"; index += 4; break;
                        case 0x36: result = $"DIV DWORD PTR DS:[ESI]"; break;
                        case 0x37: result = $"DIV DWORD PTR DS:[EDI]"; break;

                        case >= 0x38 and < 0x3C: result = $"IDIV DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case 0x3C: result = $"IDIV {VT32(data[index++])}"; break;
                        case 0x3D: result = $"IDIV DWORD PTR DS:{C32(data, index)}"; index += 4; break;
                        case 0x3E: result = $"IDIV DWORD PTR DS:[ESI]"; break;
                        case 0x3F: result = $"IDIV DWORD PTR DS:[EDI]"; break;
                    }
                    break;

                case 0xF8: result = "CLC"; break;
                case 0xF9: result = "STC"; break;
                case 0xFA: result = "CLI"; break;
                case 0xFB: result = "STI"; break;
                case 0xFC: result = "CLD"; break;
                case 0xFD: result = "STD"; break;

                case 0xFE:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"INC BYTE PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"DEC BYTE PTR DS:[{R32[b2 - 0x08]}]"; break;
                    }
                    break;

                case 0xFF:
                    b2 = data[index++];
                    switch (b2)
                    {
                        case >= 0x00 and < 0x08: result = $"INC DWORD PTR DS:[{R32[b2]}]"; break;
                        case >= 0x08 and < 0x10: result = $"DEC DWORD PTR DS:[{R32[b2 - 0x08]}]"; break;
                        case >= 0x10 and < 0x18: result = $"CALL DWORD PTR DS:[{R32[b2 - 0x10]}]"; break;
                        case >= 0x18 and < 0x20: result = $"CALL TBYTE PTR DS:[{R32[b2 - 0x18]}]"; break;
                        case >= 0x20 and < 0x28: result = $"JMP DWORD PTR DS:[{R32[b2 - 0x20]}]"; break;
                        case >= 0x28 and < 0x30: result = $"JMP FWORD PTR DS:[{R32[b2 - 0x28]}]"; break;
                        case >= 0x30 and < 0x38: result = $"PUSH DWORD PTR DS:[{R32[b2 - 0x30]}]"; break;
                    }
                    break;
            }

            var hexBytes = new byte[index - startIndex];
            Array.Copy(data, startIndex, hexBytes, 0, index - startIndex);
            startIndex = index;

            return (hexBytes, result);
        }
    }
}
