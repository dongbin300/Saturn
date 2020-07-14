using Saturn.Decompile.Disassemble;
using Saturn.Decompile.Disassemble.Disassembly;
using System;
using System.Windows.Forms;
using static Saturn.Util.StringUtil;
using static Saturn.Util.ByteUtil;

namespace SaturnDecompiler
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadDisassembly("test3.exe");

            TopMost = true;

            int index = 2450;

            dataGrid.FirstDisplayedScrollingRowIndex = index;
            dataGrid.Rows[index].Selected = true;
        }

        /// <summary>
        /// 디스어셈블리를 화면에 표시
        /// 1. PE File을 분석해서 시작주소와 디스어셈블리바이트를 얻음(GetPEFileDisassembly)
        /// 2. 디스어셈블리바이트를 분석해서 디스어셈블리를 주소 / 바이트값 / 디스어셈블리 구조로 작성(MakeInstructionTable)
        /// 3. 화면에 표시
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadDisassembly(string fileName)
        {
            // 1. PE File을 분석해서 시작주소와 디스어셈블리바이트를 얻음
            var peFileDisassembly = PEFile.GetPEFileDisassembly(fileName);

            // 2. 디스어셈블리바이트를 분석해서 디스어셈블리를 주소 / 바이트값 / 디스어셈블리 구조로 작성
            var instructions = DisassemblyHelper.MakeInstructionTable(peFileDisassembly);

            // [임시] 진행도 표시
            dataGrid.Rows.Add(new string[]
            {
                DisassemblyHelper.success.ToString(),
                (DisassemblyHelper.success + DisassemblyHelper.fail).ToString(),
                string.Format("{0:F2}%", (double)DisassemblyHelper.success / (DisassemblyHelper.success + DisassemblyHelper.fail) * 100)
            });

            // 3. 화면에 표시
            foreach (Instruction i in instructions)
            {
                string addressString = GetHexString(ReverseBytes(BitConverter.GetBytes(i.Address)));
                string byteString = GetHexString(i.Bytes);
                string disassemblyString = i.Disassembly;

                dataGrid.Rows.Add(new string[]
                { 
                    addressString, byteString, disassemblyString
                });
            }
        }
    }
}
