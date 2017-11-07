using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace AESEncrypter
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InputFile.Text = dialog.FileName;
            }
        }

        private void Output_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OutputFile.Text = dialog.SelectedPath;
            }
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            var aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 256,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                IV = Encoding.UTF8.GetBytes(IV.Text),
                Key = Encoding.UTF8.GetBytes(Key.Text)
            };
            
            var inputFileName = new FileInfo(InputFile.Text).Name;

            using (var encrypter = aes.CreateEncryptor())
            using (var inputStream = new FileStream(InputFile.Text, FileMode.Open))
            using (var outputStream = new FileStream(OutputFile.Text + @"/" + inputFileName, FileMode.CreateNew))
            using (var cryptoStream = new CryptoStream(outputStream, encrypter, CryptoStreamMode.Write))
            {
                var buffer = new byte[1024];
                var length = 0;
                while((length = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cryptoStream.Write(buffer, 0, length);
                }
            }
        }
    }
}
