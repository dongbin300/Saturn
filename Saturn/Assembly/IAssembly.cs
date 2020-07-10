using System;
using System.Collections.Generic;
using System.Text;
using static Saturn.Util;

namespace Saturn.Assembly
{
    public interface IAssembly
    {
        enum OpcodeType : byte
        {
            AAA,
            AAD,
            AAM,
            AAS,
            ADC,
            ADD,
            ADDR16,
            AND,
            ARPL,
            BOUND,
            BSWAP,
            CALL,
            CDQ,
            CLC,
            CLD,
            CLI,
            CMC,
            CMP,
            CMPS,
            CS,
            CWDE,
            DAA,
            DAS,
            DATA16,
            DEC,
            DIV,
            DS,
            ES,
            FADD,
            FBLD,
            FBSTP,
            FCOM,
            FCOMP,
            FDIV,
            FDIVR,
            FIADD,
            FICOM,
            FICOMP,
            FIDIV,
            FIDIVR,
            FILD,
            FIMUL,
            FIST,
            FISTP,
            FISTTP,
            FISUB,
            FISUBR,
            FLD,
            FLDCW,
            FLDENV,
            FMUL,
            FNSTCW,
            FNSTENV,
            FS,
            FST,
            FSTP,
            FSUB,
            FSUBR,
            FWAIT,
            GS,
            HLT,
            ICEBP,
            IDIV,
            IMUL,
            IN,
            INC,
            INS,
            INSB,
            INSD,
            INT,
            INT1,
            INT3,
            INTO,
            IRET,
            IRETD,
            JA,
            JAE,
            JB,
            JBE,
            JE,
            JECXZ,
            JG,
            JGE,
            JL,
            JLE,
            JMP,
            JNE,
            JNO,
            JNP,
            JNS,
            JO,
            JP,
            JS,
            LAHF,
            LDS,
            LEA,
            LEAVE,
            LES,
            LOCK,
            LODS,
            LODSB,
            LODSD,
            LOOP,
            LOOPE,
            LOOPNE,
            MOV,
            MOVS,
            MOVSB,
            MOVSD,
            MUL,
            NEG,
            NOP,
            NOT,
            OR,
            OUT,
            OUTS,
            OUTSB,
            OUTSD,
            POP,
            POPA,
            POPAD,
            POPF,
            POPFD,
            PUSH,
            PUSHA,
            PUSHAD,
            PUSHF,
            PUSHFD,
            REPNZ,
            REPZ,
            RET,
            RETF,
            RETFAR,
            RCL,
            RCR,
            ROL,
            ROR,
            SAHF,
            SBB,
            SCAS,
            SCASB,
            SCASD,
            SHL,
            SHR,
            SS,
            STC,
            STD,
            STI,
            STOS,
            STOSB,
            STOSD,
            SUB,
            TEST,
            XCHG,
            XLAT,
            XOR
        }

        enum DataType
        {
            BYTE,
            WORD,
            DWORD,
            QWORD
        }

        /// <summary>
        /// AL
        /// CL
        /// DL
        /// BL
        /// ...
        /// </summary>
        enum R8 : byte
        {
            AL,
            CL,
            DL,
            BL,
            AH,
            CH,
            DH,
            BH
        }

        /// <summary>
        /// AX
        /// CX
        /// DX
        /// BX
        /// ...
        /// </summary>
        enum R16 : byte
        {
            AX,
            CX,
            DX,
            BX,
            SP,
            BP,
            SI,
            DI
        }

        /// <summary>
        /// EAX
        /// ECX
        /// EDX
        /// EBX
        /// ...
        /// </summary>
        enum R32 : byte
        {
            EAX,
            ECX,
            EDX,
            EBX,
            ESP,
            EBP,
            ESI,
            EDI
        }

        /// <summary>
        /// RAX
        /// RCX
        /// RDX
        /// RBX
        /// ...
        /// </summary>
        enum R64 : byte
        {
            RAX,
            RCX,
            RDX,
            RBX,
            RSP,
            RBP,
            RSI,
            RDI
        }

        /// <summary>
        /// ES
        /// CS
        /// SS
        /// DS
        /// ...
        /// </summary>
        enum S : byte
        {
            ES,
            CS,
            SS,
            DS,
            FS,
            GS
        }

        /// <summary>
        /// BYTE[EAX]
        /// BYTE[ECX]
        /// BYTE[EDX]
        /// BYTE[EBX]
        /// ...
        /// </summary>
        enum PTR8 : byte
        {
            EAX,
            ECX,
            EDX,
            EBX,
            ESP,
            EBP,
            ESI,
            EDI
        }

        /// <summary>
        /// WORD[EAX]
        /// WORD[ECX]
        /// WORD[EDX]
        /// WORD[EBX]
        /// ...
        /// </summary>
        enum PTR16 : byte
        {
            EAX,
            ECX,
            EDX,
            EBX,
            ESP,
            EBP,
            ESI,
            EDI
        }

        /// <summary>
        /// DWORD[EAX]
        /// DWORD[ECX]
        /// DWORD[EDX]
        /// DWORD[EBX]
        /// ...
        /// </summary>
        enum PTR32 : byte
        {
            EAX,
            ECX,
            EDX,
            EBX,
            ESP,
            EBP,
            ESI,
            EDI
        }

        /// <summary>
        /// FWORD[EAX]
        /// FWORD[ECX]
        /// FWORD[EDX]
        /// FWORD[EBX]
        /// ...
        /// </summary>
        enum PTR48 : byte
        {
            EAX,
            ECX,
            EDX,
            EBX,
            ESP,
            EBP,
            ESI,
            EDI
        }

        /// <summary>
        /// QWORD[EAX]
        /// QWORD[ECX]
        /// QWORD[EDX]
        /// QWORD[EBX]
        /// ...
        /// </summary>
        enum PTR64 : byte
        {
            EAX,
            ECX,
            EDX,
            EBX,
            ESP,
            EBP,
            ESI,
            EDI
        }

        /// <summary>
        /// BYTE[00405060]
        /// WORD[00405060]
        /// DWORD[00405060]
        /// QWORD[00405060]
        /// </summary>
        class PTRC
        {
            public DataType DataType { get; set; }
            public uint Value { get; set; }
            public string LEString => ToLittleEndianString();
            public string BEString => ToBigEndianString();

            public PTRC(uint value)
            {
                DataType = DataType.DWORD;
                Value = value;
            }

            public PTRC(DataType dataType, uint value)
            {
                DataType = dataType;
                Value = value;
            }

            private string ToLittleEndianString()
            {
                return GetHexString(BitConverter.GetBytes(Value));
            }

            private string ToBigEndianString()
            {
                return GetHexString(ReverseBytes(BitConverter.GetBytes(Value)));
            }
        }
    }
}
