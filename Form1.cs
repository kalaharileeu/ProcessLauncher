using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using System.IO;
using System.Text.RegularExpressions;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

using System.Net;

namespace PyLauncher
{
    public partial class Form1 : Form
    {
        string status;
        string error;
        string test;
        Report Serverreporter;
        Readcmd CmdReader;
        XmlManager<Parameter> xmltestmanager;
        Parameter tests;
        Report report1;
        List<string> storedvalues;
        string valuereport;
        bool keepreporting;

        public Form1()
        {
            valuereport = "default";
            storedvalues = new List<string>();
            status = "unknown";
            error = ".";
            test = ".";
            Serverreporter = new Report();
            CmdReader = new Readcmd();
            InitializeComponent();
            xmltestmanager = new XmlManager<Parameter>();
            tests = new Parameter();
            report1 = new Report();
            keepreporting = true;
            tests = xmltestmanager.Load("Content/parameters.xml");// the the xmlfile the start off serialization
            foreach (Parameter.Test t in tests.tests)
            {
                //listBox1.Items.Add(t.id);
                storedvalues.Add(t.id);
            }
            textBox1.Focus();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            for (int k = listBox1.Items.Count - 1; k >= 0; --k )
            {
                listBox1.Items.RemoveAt(k);
            }
            //storedvalues = new List<string>();
            for (int n = storedvalues.Count - 1; n >= 0; --n)
            {
                string addlistitem = "Vogel";
                if (storedvalues[n].Contains(addlistitem))
                {
                    listBox1.Items.Add(storedvalues[n]);
                }
                /*   if (listBox1.Items[n].ToString().Contains(removelistitem))
                    {
                        storedvalues.Add((string)listBox1.Items[n]);
                        listBox1.Items.RemoveAt(n);
                    }*/
            }
/*            string buttonid = "Testtime";
            Parameter.Test Testtime = new Parameter.Test();
            Testtime = tests.tests.Find(x => x.id.Equals(buttonid));//workds to find test id
            if (Testtime != null)
                Console.WriteLine("Found it");
            else
                Console.WriteLine("Did not find it");
 */

        }

        private void button2_Click(object sender, EventArgs e)
        {
            keepreporting = true;
            Runmainprocess();
            Reporting();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int k = listBox1.Items.Count - 1; k >= 0; --k)
            {
                listBox1.Items.RemoveAt(k);
            }

            for (int n = storedvalues.Count - 1; n >= 0; --n)
            {
                string addlistitem = "Diogenes";
                if (storedvalues[n].Contains(addlistitem))
                {
                    listBox1.Items.Add(storedvalues[n]);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int k = listBox1.Items.Count - 1; k >= 0; --k)
            {
                listBox1.Items.RemoveAt(k);
            }

            for (int n = storedvalues.Count - 1; n >= 0; --n)
            {
                string addlistitem = "Corvus";
                if (storedvalues[n].Contains(addlistitem))
                {
                    listBox1.Items.Add(storedvalues[n]);
                }
            }
        }

        private void list_Box1DoubleClick(object sender, MouseEventArgs e)//list box 1 double cllick
        {
            if (textBox1.Text.Length < 12)
            {
                MessageBox.Show("invalid SN");
                return;
            }
            else
                textBox1.Text = textBox1.Text.Substring(textBox1.Text.Length - 12);

            textBox1.Refresh();
            
            int index = this.listBox1.IndexFromPoint(e.Location);
            
            if (index != System.Windows.Forms.ListBox.NoMatches)//check if item is there
            {
                string item = (string)(this.listBox1.Items[index]);//get cliked it fom listbox 1
                listBox2.Items.Add(item);//add item to litbox 2
                //MessageBox.Show(index.ToString());
            }
        }

        private void list_box2DoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox2.IndexFromPoint(e.Location);
          
            if (index != System.Windows.Forms.ListBox.NoMatches)//check if item is there
            {
                object item = this.listBox2.Items[index];//get clicked it fom listbox 1

                if (index >= 0)
                {
                    this.listBox2.Items.RemoveAt(index);//remove item
                }
            }
        }

        private void list_box2Click(object sender, MouseEventArgs e)
        {
            int index = this.listBox2.IndexFromPoint(e.Location);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (index != System.Windows.Forms.ListBox.NoMatches)//check if item is there
                {
                    object item = this.listBox2.Items[index];//get clicked it fom listbox 1

                    if (index > 0)
                    {
                        this.listBox2.Items.RemoveAt(index);//remove item
                        this.listBox2.Items.Insert(index - 1, item);
                    }
                }
            }

            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (index != System.Windows.Forms.ListBox.NoMatches)//check if item is there
                {
                    object item = this.listBox2.Items[index];//get cliked it fom listbox 1

                    if (index >= 0)
                    {
                        this.listBox2.Items.RemoveAt(index);//remove item
                    }
                }
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
           // textBox1.Text = textBox1.Text.Substring(textBox1.Text.Length - 12);
           // textBox1.Refresh();
        }

//       /* public async void reporttoserver()
//        {
//          /* if(listBox2.Items.Count != 0)
//            {
//                string value = await Task.Run(() => report1.Reporttoserver(status, test, ref error));
//            }
//            */
//    }

        //this method runs asynchronously
        public async void Runmainprocess()
        {
            if(listBox2.Items.Count != 0)
            {
                string testtorun = (string)(listBox2.Items[0]);
                test = testtorun;
                test = Regex.Replace(test, @"\s+", "%20");
                Debug.WriteLine(test);
                status = "Running";
                listBox3.Items.Add(testtorun);
                listBox2.Items.RemoveAt(0);
                //string value = report1.Reporttoserver(status, test, ref error);
                string t = await Task.Run(() => Runtest(testtorun));
                if (t.Contains("done"))
                {
                    status = "Complete";
                    ShortReport();//Force a quick report of complete
                    listBox3.Items.Clear();
                    listBox4.Items.Add(testtorun);
                }
                Runmainprocess();
            }
        }

        public async void ShortReport()//makes a single complete report
        {
            keepreporting = false;
            await Task.Run(() => ReportToServer());
        }

        public async void Reporting()//continuous reporting
        {
            textBox2.Text = await Task.Run(() => ReportToServer());
            if (keepreporting || listBox2.Items.Count != 0)
            {
                if (listBox3.Items.Count != 0)//if no tasks running do not report
                {
                    Reporting(); //calls reporting againg
                }
                else
                {
                    status = "Complete";
                    keepreporting = false;
                    Reporting();
                }
            }
        }

        private string ReportToServer()
        {
            string value = report1.Reporttoserver(status, test, ref error, textBox3.Text);
            Thread.Sleep(4000);
            //reporttoserver();
            return value;
        }

        public string Runtest(string itemtorun)
        {
            Parameter.Test Testtime = new Parameter.Test();
            Testtime = tests.tests.Find(x => x.id.Equals(itemtorun));
            if (Testtime != null)
            {
                string junk = "";
                foreach(string s in Testtime.Parsstring)
                {
                    if (s.Contains("SERIAL_NUMBER"))
                        junk += ( " " + (string)textBox1.Text.Substring(textBox1.Text.Length - 12));
                    else
                        junk += (" " + s);
                    //Console.WriteLine(junk);
                }
                //p.StartInfo.Arguments += junk;
                string runcommandarguments = Testtime.arguments + junk;
                Console.WriteLine(runcommandarguments);
                Process p = new Process(); // create process (i.e., the python program
                
                p.StartInfo.FileName = Testtime.filename;
                p.StartInfo.Arguments = runcommandarguments;
                p.StartInfo.RedirectStandardOutput = false;//check command line output
                p.StartInfo.RedirectStandardError = false;//Have to check error
                //p.StartInfo.RedirectStandardInput = false;
                p.StartInfo.UseShellExecute = false; //we can read or not the output from stdout
                p.StartInfo.WorkingDirectory = Testtime.workingdirectory;
                p.Start();
                //string g = p.StandardError.ReadToEnd();
                //string t = p.StandardOutput.ReadToEnd();//Reads the standard input
                //Console.WriteLine(t);//prints it out
                ///Console.WriteLine(g);//prints it out
                p.WaitForExit();
                if (p.ExitCode != 0)
                {               
                }

                //if (p.HasExited)
                //    p.Close();

               // itemtorun = Testtime.filename + Testtime.arguments + Testtime.Parsstring[0] +
               //     Testtime.Parsstring[1] + Testtime.Parsstring[2] + Testtime.Parsstring[3] + Testtime.Parsstring[4];
              //  Console.WriteLine(itemtorun);
            }
            else
            {
                Console.WriteLine(" Did not find the test id from XML file/n");
                return "Could not find the test";
            }

            return "done: ";
        }
    }
}

/*
        public string Runtest()//This test is run by runtest as a task
        {
            string s = "s 9";
            string t = "t 0";
            string c = "c 1";
            string mdl = "Frigate_Vogel_230V";
            Process p = new Process(); // create process (i.e., the python program
            p.StartInfo.FileName = "python.exe";
            //p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.UseShellExecute = false; // make sure we can read the output from stdout
            //p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.Arguments = 
            "C:\\Users\\cdelange\\workspace\\mercurial\\Smoketest\\smoke_test.py -" + s + " -" + t + " -" + c + " --config " + mdl; 
            //p.StartInfo.Arguments = "c:\\test\\testtime.py "; // start the python program with two parameters
            
            p.Start(); // start the process (the python program)

            p.WaitForExit();
            if (p.HasExited)
                p.Close();

            return "testid"  + " :done test";
        }

        public string Runtest3()//This test is run by runtest as a task
        {
            Process p = new Process(); // create process (i.e., the python program
            p.StartInfo.FileName = "python.exe";
            //p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.UseShellExecute = true; // make sure we can read the output from stdout
            //p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.Arguments = "c:\\test\\testtime.py "; // start the python program with two parameters

            p.Start(); // start the process (the python program)

            p.WaitForExit();
            if (p.HasExited)
                p.Close();

            return "done";
        }

        private string Runtest2()//WORKS take control of the input and the output pipe line
        {

            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = "python.exe";
            p.Arguments = "c:\\test\\testtime.py "; // start the python program with two parameters
            p.RedirectStandardOutput = true;
            p.UseShellExecute = false;
            p.RedirectStandardInput = true;
            p.CreateNoWindow = true;

            using (Process process = Process.Start(p))
            using (StreamWriter writer = process.StandardInput)
            using (StreamReader reader = process.StandardOutput)
            {
                Console.WriteLine("here");
                writer.WriteLine("doit");

                string result = null;

                while (!process.HasExited)
                { 
                    result = reader.ReadLine();
                    Console.WriteLine(result);
                }

                process.WaitForExit();
                if (process.HasExited)
                    process.Close();
            }

            return "done";

        }
*/
        /*
        public void Searchprocess()//searches through all the processes for cmd
        {
            List<Process> listprocess = new List<Process>();
            //var poList= Process.GetProcesses().Where(process => process.ProcessName.Contains("cmd"));
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName.Equals("cmd"))
                    listprocess.Add(p);
            }

            bool processloop = true;

            while (processloop)
            {
                foreach (Process p in listprocess)
                {
                    var a = p;
                    ExtFunc.FreeConsole();//if you use console application you must free self console
                    ExtFunc.AttachConsole((uint)a.Id);
                    var err = Marshal.GetLastWin32Error();
                    System.IntPtr ptr = ExtFunc.GetStdHandle(-11);
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    bool imrunning = true;
                    while (imrunning)
                    {
                        short cursor = (short)System.Console.CursorTop;//Find the bottomline where cursor resides
                        string checkforcursor = CmdReader.readvalue(ref ptr, cursor);
                        if (checkforcursor.StartsWith("C:\\>"))
                        {
                            status = "Complete";
                            imrunning = false;
                            Serverreporter.Reporttoserver(ref status, ref error);
                        }
                        else if (checkforcursor.StartsWith("C:\\"))
                        {
                            status = "Complete";
                            imrunning = false;
                            Serverreporter.Reporttoserver(ref status, ref error);
                        }
                        else
                        {
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            status = "Running";
                            System.Console.Write(".");
                            imrunning = true;
                            Serverreporter.Reporttoserver(ref status, ref error);
                            //System.Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        Thread.Sleep(300);//2000
                    }
                    System.Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(300);//10 000One process closed move to the next
                }
                processloop = false;//No processes
            }
            Console.WriteLine("all the cmd processes has ended");
        }
*/
