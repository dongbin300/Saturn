using Saturn.Disassembly;

using System;
using System.Linq;
using System.Windows;

namespace SaturnDecompiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fileName = "test4.exe";
            var instructions = DisassemblyManager.GetInstructions(fileName);
            DisassembleDataGrid.ItemsSource = instructions;

            var count = instructions.Count;
            var successCount = instructions.Count(x => x.Description != string.Empty);
            var rate = Math.Round((double)successCount / count * 100, 2);
            Title = $"{fileName}, {successCount}/{count}, {rate}%";
        }
    }
}
