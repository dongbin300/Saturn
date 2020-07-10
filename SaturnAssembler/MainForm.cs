using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Saturn.Assembly;
using Saturn.Build;
using Saturn.Build.Assemble;
using SyntaxHighlighter;

namespace SaturnAssembler
{
    public partial class MainForm : Form
    {
        SyntaxRichTextBox richTextBox;

        string BindFilePath = string.Empty;
        string FileName = string.Empty;
        string AllText = string.Empty;
        bool save = false;

        public MainForm()
        {
            InitializeComponent();

            alertLabel.Text = "";

            InitializeRichTextBox();
            LoadInitialAssemblyFile();

            //richTextBox.ProcessAllLines();
        }

        void InitializeRichTextBox()
        {
            richTextBox = new SyntaxRichTextBox();
            richTextBox.Location = new Point(-1, 50);
            richTextBox.Size = new Size(560, 425);
            richTextBox.Font = new Font("Consolas", 12);
            richTextBox.TextChanged += RichTextBox_TextChanged;
            richTextBox.SelectionChanged += RichTextBox_SelectionChanged;
            //richTextBox.ForeColor = Color.FromArgb(222, 222, 222);
            //richTextBox.BackColor = Color.FromArgb(44, 44, 44);

            richTextBox.Settings.Keywords.Add("PTR");
            richTextBox.Settings.Keywords.AddRange(Enum.GetNames(typeof(IAssembly.OpcodeType)).ToList());
            richTextBox.Settings.Keywords.AddRange(Enum.GetNames(typeof(IAssembly.DataType)).ToList());
            richTextBox.Settings.Keywords.AddRange(Enum.GetNames(typeof(IAssembly.R8)).ToList());
            richTextBox.Settings.Keywords.AddRange(Enum.GetNames(typeof(IAssembly.R16)).ToList());
            richTextBox.Settings.Keywords.AddRange(Enum.GetNames(typeof(IAssembly.R32)).ToList());
            richTextBox.Settings.Keywords.AddRange(Enum.GetNames(typeof(IAssembly.R64)).ToList());
            richTextBox.Settings.Keywords.AddRange(Enum.GetNames(typeof(IAssembly.S)).ToList());
            richTextBox.Settings.Keywords.AddRange(Enum.GetNames(typeof(IAssembly.PTR8)).ToList());
            richTextBox.Settings.Comment = ";";

            richTextBox.Settings.KeywordColor = Color.DeepSkyBlue;
            richTextBox.Settings.StringColor = Color.OrangeRed;
            richTextBox.Settings.CommentColor = Color.LightGray;

            richTextBox.CompileKeywords();

            Controls.Add(richTextBox);
        }

        private void RichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            int line = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart) + 1;

            lineLabel.Text = "line: " + line; 
        }

        void LoadInitialAssemblyFile()
        {
            BindFilePath = @"C:\Users\dongb\OneDrive\Desktop\test.asm";
            FileName = BindFilePath.Substring(BindFilePath.LastIndexOf('\\') + 1).Replace(".asm", "");
            filename.Text = FileName;
            richTextBox.Text = File.ReadAllText(BindFilePath);
            AllText = richTextBox.Text;

            RichTextBox_TextChanged(null, null);

            Alert($"Loaded {FileName}.");
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            save = AllText == richTextBox.Text;
            title.ForeColor = save ? Color.ForestGreen : Color.Gold;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Assembly File|*.asm";
                ofd.Title = "Open Assembly File";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    BindFilePath = ofd.FileName;
                    FileName = BindFilePath.Substring(BindFilePath.LastIndexOf('\\') + 1).Replace(".asm", "");
                    filename.Text = FileName;
                    richTextBox.Text = File.ReadAllText(BindFilePath);
                    AllText = richTextBox.Text;

                    RichTextBox_TextChanged(null, null);

                    Alert($"Opened {FileName}.");
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (BindFilePath == string.Empty)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Assembly File|*.asm";
                    sfd.Title = "Save Assembly File";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        BindFilePath = sfd.FileName;
                        FileName = BindFilePath.Substring(BindFilePath.LastIndexOf('\\') + 1).Replace(".asm", "");
                        filename.Text = FileName;
                        File.WriteAllText(BindFilePath, richTextBox.Text);
                        AllText = richTextBox.Text;

                        RichTextBox_TextChanged(null, null);

                        Alert($"Saved {FileName}.");
                    }
                }
                else
                {
                    File.WriteAllText(BindFilePath, richTextBox.Text);
                    AllText = richTextBox.Text;

                    RichTextBox_TextChanged(null, null);

                    Alert($"Saved {FileName}.");
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }
        }

        private void assembleButton_Click(object sender, EventArgs e)
        {
            try
            {
                /* Pre Parse */
                var (assemblyBytesTemp, dataBytesTemp) = PreParse();

                var textSectionRawSize = AddressManager.GetSectionRawSize(assemblyBytesTemp);
                var dataSectionRawSize = AddressManager.GetSectionRawSize(dataBytesTemp);

                /* Real Parse */
                var (instructions, variables) = AssemblyParser.Parse(richTextBox.Text, textSectionRawSize, dataSectionRawSize);

                var assemblyBytes = AssembleHelper.BuildAssemblyBytes(instructions);
                var dataBytes = AssembleHelper.BuildDataBytes(variables);

                PEBuilder peBuilder = new PEBuilder(assemblyBytes, dataBytes);

                using (FileStream stream = new FileStream("petest.exe", FileMode.Create, FileAccess.Write))
                {
                    peBuilder.WriteFile(stream);
                }

                Alert("Assemble Completed");
            }
            catch (Exception ex)
            {
                Alert("Assemble Failed : " + ex.Message);
            }
        }

        (byte[], byte[]) PreParse()
        {
            var (instructions, variables) = AssemblyParser.Parse(richTextBox.Text);

            var assemblyBytes = AssembleHelper.BuildAssemblyBytes(instructions);
            var dataBytes = AssembleHelper.BuildDataBytes(variables);

            return (assemblyBytes, dataBytes);
        }

        void Alert(string message) => alertLabel.Text = message;
    }
}
