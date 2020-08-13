using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /*
 The following example demonstrates the 'Invoke(Delegate)' method of 'Control class.
 A 'ListBox' and a 'Button' control are added to a form, containing a delegate
 which encapsulates a method that adds items to the listbox. This function is executed
 on the thread that owns the underlying handle of the form. When user clicks on button
 the above delegate is executed using 'Invoke' method.


 */

   

    public class MyFormControl : Form
    {
        public delegate void AddListItem();
        public AddListItem myDelegate;
        private Button myButton;
        private Thread myThread;
        private Button button1;
        private TextBox textBox1;
        private ListBox myListBox;
        public MyFormControl()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(622, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(612, 178);
            this.textBox1.Name = "textBox1";
            
            myButton = new Button();
            myListBox = new ListBox();
            myButton.Location = new Point(72, 160);
            myButton.Size = new Size(152, 32);
            myButton.TabIndex = 1;
            myButton.Text = "Add items in list box";
            myButton.Click += new EventHandler(Button_Click);
            myListBox.Location = new Point(48, 32);
            myListBox.Name = "myListBox";
            myListBox.Size = new Size(200, 95);
            myListBox.TabIndex = 2;
            ClientSize = new Size(292, 273);
            Controls.AddRange(new Control[] { myListBox, myButton, button1, textBox1 });
            Text = " 'Control_Invoke' example";
            myDelegate = new AddListItem(AddListItemMethod);

        }
        //static void Main()
        //{
        //    MyFormControl myForm = new MyFormControl();
        //    myForm.ShowDialog();
        //}
        public void AddListItemMethod()
        {
            String myItem;
            for (int i = 1; i < 6; i++)
            {
                myItem = "MyListItem" + i.ToString();
                myListBox.Items.Add(myItem);
                myListBox.Update();
                Thread.Sleep(300);
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            tokenSource2.Cancel();
            myThread = new Thread(new ThreadStart(ThreadFunction));
            myThread.Start();
        }
        private void ThreadFunction()
        {
            MyThreadClass myThreadClassObject = new MyThreadClass(this);
            myThreadClassObject.Run();
        }

        //private void InitializeComponent()
        //{
        //    this.button1 = new System.Windows.Forms.Button();
        //    this.textBox1 = new System.Windows.Forms.TextBox();
        //    this.SuspendLayout();
        //    // 
        //    // button1
        //    // 
        //    this.button1.Location = new System.Drawing.Point(622, 205);
        //    this.button1.Name = "button1";
        //    this.button1.Size = new System.Drawing.Size(75, 23);
        //    this.button1.TabIndex = 0;
        //    this.button1.Text = "button1";
        //    this.button1.UseVisualStyleBackColor = true;
        //    this.button1.Click += new System.EventHandler(this.button1_Click);
        //    // 
        //    // textBox1
        //    // 
        //    this.textBox1.Location = new System.Drawing.Point(612, 178);
        //    this.textBox1.Name = "textBox1";
        //    this.textBox1.Size = new System.Drawing.Size(100, 21);
        //    this.textBox1.TabIndex = 1;
        //    // 
        //    // MyFormControl
        //    // 
        //    this.ClientSize = new System.Drawing.Size(741, 261);
        //    this.Controls.Add(this.textBox1);
        //    this.Controls.Add(this.button1);
        //    this.Name = "MyFormControl";
        //    this.ResumeLayout(false);
        //    this.PerformLayout();

        //}
        CancellationTokenSource tokenSource2 = new CancellationTokenSource();
        CancellationToken ct;
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string result = await WaitAsynchronouslyAsync(tokenSource2.Token);
                textBox1.Text += result;
            }
            catch(OperationCanceledException op)
            {
                MessageBox.Show(op.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                tokenSource2.Dispose();
            }
            //ct = tokenSource2.Token;
            // Call the method that runs asynchronously.


            // Call the method that runs synchronously.
            //string result = await WaitSynchronously ();

            // Display the result.


        }
        // The following method runs asynchronously. The UI thread is not
        // blocked during the delay. You can move or resize the Form1 window 
        // while Task.Delay is running.
        public async Task<string> WaitAsynchronouslyAsync(CancellationToken ct)
        {
            await Task.Delay(10000);
            if (ct.IsCancellationRequested)
            {
                MessageBox.Show("取消任务");
                // Clean up here, then...
                ct.ThrowIfCancellationRequested();
            }
            return "Finished";
        }

    }

    // The following code assumes a 'ListBox' and a 'Button' control are added to a form, 
    // containing a delegate which encapsulates a method that adds items to the listbox.

    public class MyThreadClass
    {
        MyFormControl myFormControl1;
        public MyThreadClass(MyFormControl myForm)
        {
            myFormControl1 = myForm;
        }

        public void Run()
        {
            // Execute the specified delegate on the thread that owns
            // 'myFormControl1' control's underlying window handle.
            myFormControl1.Invoke(myFormControl1.myDelegate);
        }
    }
}
