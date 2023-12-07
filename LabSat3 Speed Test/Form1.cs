using System;
using System.IO;
using System.Windows.Forms;

namespace LabSat3_Speed_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Drive letter input box
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Run speed calc
        private void button1_Click(object sender, EventArgs e)
        {
            // Get user input for the drive
            string drive = textBox1.Text.ToUpper();

            // Append colon to the drive letter if not present
            if (!drive.EndsWith(":"))
            {
                drive += ":";
            }

            // Construct the file path
            string filePath = Path.Combine(drive, "Labsat3 Media Benchmark.log");

            try
            {
                // Read and process the hidden file
                (int writeSpeed, int readSpeed) = ReadHiddenFile(filePath);

                // Display the speeds in the result TextBox
                textBox2.Text = $"Write speed: {writeSpeed * 0.001} MB/s\r\nRead speed: {readSpeed * 0.001} MB/s";
            }
            catch (FileNotFoundException)
            {
                textBox2.Text = "File not found.";
            }
            catch (Exception ex)
            {
                textBox2.Text = $"An error occurred: {ex.Message}";
            }
        }

        private (int, int) ReadHiddenFile(string filePath)
        {
            byte[] content = File.ReadAllBytes(filePath);

            // Extract bytes for write speed (24 and 25)
            byte byte25Write = content[24];
            byte byte26Write = content[25];

            // Extract bytes for read speed (16 and 17)
            byte byte17Read = content[16];
            byte byte18Read = content[17];

            // Convert bytes to integers
            int writeSpeed = (byte26Write << 8) | byte25Write;
            int readSpeed = (byte18Read << 8) | byte17Read;

            return (writeSpeed, readSpeed);
        }

        // Results text box
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
