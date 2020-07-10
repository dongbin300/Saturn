using Saturn.Assembly;
using Saturn.MachineCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace Saturn.Build.Assemble
{
    public class AssembleHelper
    {
        /// <summary>
        /// Instructions to Assembly Bytes
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public static byte[] BuildAssemblyBytes(List<Instruction> instructions)
        {
            List<byte> assemblyBytes = new List<byte>();

            foreach (Instruction i in instructions)
            {
                var machineCode = i.MachineCode;

                if (machineCode == null)
                {
                    throw new Exception($"[AssemblyParseError][{i.String}]");
                }

                Util.ConcatenateBytes(ref assemblyBytes, machineCode.Bytes);
            }

            return assemblyBytes.ToArray();
        }

        public static byte[] BuildDataBytes(List<Variable> variables)
        {
            List<byte> dataBytes = new List<byte>();

            foreach (Variable v in variables)
            {
                Util.ConcatenateBytes(ref dataBytes, v.Value);
            }

            return dataBytes.ToArray();
        }
    }
}
