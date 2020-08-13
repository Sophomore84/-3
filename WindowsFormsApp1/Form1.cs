using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            StartThread();

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            // Cancel the asynchronous operation.
            this.backgroundWorker1.CancelAsync();

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This event handler is called when the background thread finishes.
            // This method runs on the main thread.
            if (e.Error != null)
                MessageBox.Show("Error: " + e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("Word counting canceled.");
            else
                MessageBox.Show("Finished counting words.");

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // This event handler is where the actual work is done.
            // This method runs on the background thread.

            // Get the BackgroundWorker object that raised this event.
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;

            // Get the Words object and call the main method.
            Words WC = (Words)e.Argument;
            WC.CountWords(worker, e);

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This method runs on the main thread.
            Words.CurrentState state =
                (Words.CurrentState)e.UserState;
            this.LinesCounted.Text = state.LinesCounted.ToString();
            this.WordsCounted.Text = state.WordsMatched.ToString();

        }
        private void StartThread()
        {
            // This method runs on the main thread.
            this.WordsCounted.Text = "0";

            // Initialize the object that the background worker calls.
            Words WC = new Words();
            WC.CompareString = this.CompareString.Text;
            WC.SourceFile = this.SourceFile.Text;

            // Start the asynchronous operation.
            backgroundWorker1.RunWorkerAsync(WC);
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            resultsTextBox.Clear();
            await CreateMultipleTasksAsync();
            resultsTextBox.Text += "\r\n\r\nControl returned to startButton_Click.\r\n";
        }
        async Task<int> ProcessURLAsync(string url, HttpClient client)
        {
            var byteArray = await client.GetByteArrayAsync(url);
            DisplayResults(url, byteArray);
            return byteArray.Length;
        }


        private void DisplayResults(string url, byte[] content)
        {
            // Display the length of each website. The string format 
            // is designed to be used with a monospaced font, such as
            // Lucida Console or Global Monospace.
            var bytes = content.Length;
            // Strip off the "http://".
            var displayURL = url.Replace("http://", "");
            resultsTextBox.Text += string.Format("\n{0,-58} {1,8}", displayURL, bytes);
        }
        private async Task CreateMultipleTasksAsync()
        {
            // Declare an HttpClient object, and increase the buffer size. The
            // default buffer size is 65,536.
            HttpClient client =
                new HttpClient() { MaxResponseContentBufferSize = 1000000 };

            // Create and start the tasks. As each task finishes, DisplayResults 
            // displays its length.
            Task<int> download1 =
                ProcessURLAsync("http://msdn.microsoft.com", client);
            Task<int> download2 =
                ProcessURLAsync("http://msdn.microsoft.com/en-us/library/hh156528(VS.110).aspx", client);
            Task<int> download3 =
                ProcessURLAsync("http://msdn.microsoft.com/en-us/library/67w7t67f.aspx", client);

            // Await each task.
            int length1 = await download1;
            int length2 = await download2;
            int length3 = await download3;

            int total = length1 + length2 + length3;

            // Display the total count for the downloaded websites.
            resultsTextBox.Text +=
                string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", total);
        }



    }
}
