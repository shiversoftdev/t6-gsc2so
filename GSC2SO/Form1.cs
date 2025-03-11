using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using System.IO;
using System.Reflection;

/* Bugs:
 * 
 * 
 * 
 * 
 * Stablility Rating (out of 10): 666
 * 
 * 
*/

namespace GSC2SO
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public bool DEBUG = false; //Set to false before release
        public bool COMPILE_ONLY = false;
        public bool Clean = true;
        public bool Symbolize = true;
        public bool Defaults = true;
        public bool IncludedHeaders = false;
        public string[] files;
        public string PATH = "";
        public bool PC = false;
        public bool SHOW_COUNT = false;
        public List<string> allfiletext;
        public string[] ARGS;
        public bool OP_GLOBALS = true;
        public GSCOptimizer gsc;

        public Form1(string[] args)
        {
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            InitializeComponent();
            dragdrop.DragDrop += new DragEventHandler(Main);
            dragdrop.DragEnter += new DragEventHandler(_DragEnter);
            BackgroundWorker_gsc.DoWork += BackgroundWorker1_DoWork;
            BackgroundWorker_gsc.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            BackgroundWorker_gsc.WorkerReportsProgress = true;
            BackgroundWorker_gsc.WorkerSupportsCancellation = true;
            BackgroundWorker_gsc.ProgressChanged += BackgroundWorker1_ProgressChanged;
            cout("GSC String Optimizer and Mini Obfuscator Version 1 by SeriousHD-");
            if(args != null && args.Length > 0)
            {
                ARGS = args;
                Main(null, null);
            }
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == 0)
            {
                string[] args = (string[])e.UserState;
                cout(args[0], args[1], args[2]);
            }
            else
            {
                StringCount.Text = e.UserState.ToString();
            }
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
                cout("Compilation Complete");
            else
                cout("Compilation Failed");
            canceler.Visible = false;
            canceler.Enabled = false;
            AttachHeader.Visible = true;
            AttachHeader.Enabled = true;
        }

        public void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = ParseGSCFiles();
        }

        private void Main(object sender, DragEventArgs e)
        {
            // Handle FileDrop data.
            if (e != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Assign the file names to a string array, in 
                // case the user has selected multiple files.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                this.files = files;
                allfiletext = new List<string>();
                bool hasmain = false;
                foreach (string file in files)
                {
                    if (file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1).ToLower().Equals("main.gsc"))
                    {
                        allfiletext.Insert(0, File.ReadAllText(file));
                        hasmain = true;
                    }
                    else
                        allfiletext.Add(File.ReadAllText(file));
                }
                if (!hasmain)
                {
                    scout("Couldn't find the main GSC! Please include a main.gsc file", "Error");
                    return;
                }
                SaveGSCSelector.Filter = "GSC File|*.gsc";
                DialogResult r = SaveGSCSelector.ShowDialog();
                if (r != DialogResult.OK)
                {
                    scout("User canceled operation");
                    return;
                }
                canceler.Visible = true;
                canceler.Enabled = true;
                ClearHeaders.Visible = false;
                ClearHeaders.Enabled = false;
                AttachHeader.Visible = false;
                AttachHeader.Enabled = false;
                PATH = SaveGSCSelector.FileName;
                BackgroundWorker_gsc.RunWorkerAsync();
            }
            else if(ARGS != null)
            {
                string[] files = ARGS;
                this.files = files;
                allfiletext = new List<string>();
                bool hasmain = false;
                foreach (string file in files)
                {
                    if (file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1).ToLower().Equals("main.gsc"))
                    {
                        allfiletext.Insert(0, File.ReadAllText(file));
                        hasmain = true;
                    }
                    else
                        allfiletext.Add(File.ReadAllText(file));
                }
                if (!hasmain)
                {
                    scout("Couldn't find the main GSC! Please include a main.gsc file", "Error");
                    return;
                }
                SaveGSCSelector.Filter = "GSC File|*.gsc";
                DialogResult r = SaveGSCSelector.ShowDialog();
                if (r != DialogResult.OK)
                {
                    scout("User canceled operation");
                    return;
                }
                canceler.Visible = true;
                canceler.Enabled = true;
                ClearHeaders.Visible = false;
                ClearHeaders.Enabled = false;
                AttachHeader.Visible = false;
                AttachHeader.Enabled = false;
                PATH = SaveGSCSelector.FileName;
                BackgroundWorker_gsc.RunWorkerAsync();
            }
        }

        private bool ParseGSCFiles()
        {
            gsc = new GSCOptimizer(this);
            return gsc.Main();
        }

        public void scout(string a1, string a2 = "Notification", string a3 = null)
        {
            BackgroundWorker_gsc.ReportProgress(0, new string[] { a1, a2, a3 });
        }

        public void ReportStrings( int i)
        {
            BackgroundWorker_gsc.ReportProgress(1, i);
        }

        public void _DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void cout(string text, string label = "Notification", string header = null)
        {
            Color prior = Console.ForeColor;
            if (header != null)
            {
                Console.SelectionStart = Console.Text.Length;
                Console.SelectionLength = 0;
                Console.SelectionColor = Color.FromArgb(255, 255, 255, 255);
                Console.SelectionFont = new Font(Console.SelectionFont, FontStyle.Bold);
                Console.AppendText(header + "\n");
                Console.SelectionFont = Console.Font;
                Console.SelectionColor = Console.ForeColor;
            }
            Console.SelectionStart = Console.Text.Length;
            Console.SelectionLength = 0;
            Console.SelectionColor = Color.FromArgb(255, 255, 255, 0);
            Console.AppendText("[" + DateTime.Now.ToShortTimeString() + "] ");
            Console.SelectionColor = Console.ForeColor;
            Console.SelectionStart = Console.Text.Length;
            Console.SelectionLength = 0;
            Console.SelectionColor = Color.FromArgb(255, 255, 255, 0);
            Console.SelectionFont = new Font(Console.SelectionFont, FontStyle.Underline);
            Console.AppendText(label);
            Console.SelectionFont = Console.Font;
            Console.SelectionColor = Console.ForeColor;
            Console.SelectionStart = Console.Text.Length;
            Console.SelectionLength = 0;
            Console.SelectionColor = Color.FromArgb(255, 255, 255, 0);
            Console.AppendText(": " + text + "\n");
            Console.SelectionColor = Console.ForeColor;
        }

       


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        

        private void dragdrop_Click(object sender, EventArgs e)
        {
            // Assign the file names to a string array, in 
            // case the user has selected multiple files.
            GSCFiles.Filter = "GSC Source Files|*.gsc";
            if (GSCFiles.ShowDialog() != DialogResult.OK)
            {
                cout("User canceled operation");
                return;
            }
            string[] files = GSCFiles.FileNames;
            this.files = files;
            allfiletext = new List<string>();
            bool hasmain = false;
            foreach (string file in files)
            {
                if (file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1).ToLower().Equals("main.gsc"))
                {
                    allfiletext.Insert(0, File.ReadAllText(file));
                    hasmain = true;
                }
                else
                    allfiletext.Add(File.ReadAllText(file));
            }
            if (!hasmain)
            {
                scout("Couldn't find the main GSC! Please include a main.gsc file", "Error");
                return;
            }
            SaveGSCSelector.Filter = "Compiled GSC File|*.gsc";
            DialogResult r = SaveGSCSelector.ShowDialog();
            if (r != DialogResult.OK)
            {
                scout("User canceled operation");
                return;
            }
            canceler.Visible = true;
            canceler.Enabled = true;
            PATH = SaveGSCSelector.FileName;
            BackgroundWorker_gsc.RunWorkerAsync();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            OP_GLOBALS = checkEdit1.Checked;
        }

        

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            BackgroundWorker_gsc.CancelAsync();
            cout("Operation Canceled by User");
            canceler.Visible = false;
            canceler.Enabled = false;
            AttachHeader.Visible = true;
            AttachHeader.Enabled = true;
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            PC = checkEdit2.Checked;
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            IncludedHeaders = false;
            ClearHeaders.Visible = false;
            ClearHeaders.Enabled = false;
            GSCFiles.Filter = "GSC Source File|*.gsc|Header File|*.gsh|Text Document|*.txt";
            if(GSCFiles.ShowDialog() != DialogResult.OK)
            {
                cout("User canceled action");
                return;
            }
            try
            {
                if(gsc == null)
                {
                    gsc = new GSCOptimizer(this);
                }
                gsc.ResetGlobals();
                IncludedHeaders = true;
                ClearHeaders.Visible = true;
                ClearHeaders.Enabled = true;
                //TODO Parse user header
            }
            catch(Exception ex)
            {
                cout("One or more of the included files is not a valid header file!", "Error");
                cout("Please include only files with only forceaway,stringaway, or protect lists");
                cout("Example: //#protect level.*");
                if (DEBUG)
                    cout(ex.GetBaseException().ToString());
            }
        }

        private void ClearHeaders_Click(object sender, EventArgs e)
        {
            if (gsc == null)
            {
                gsc = new GSCOptimizer(this);
            }
            gsc.ResetGlobals();
            IncludedHeaders = false;
            ClearHeaders.Visible = false;
            ClearHeaders.Enabled = false;
            cout("Cleared Headers");
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if(!checkEdit3.Checked)
            {
                MessageBox.Show("Warning! Disabling default headers may result in your script failing!");
                scout("Disabled default headers - Please link a header or make sure you cover the protections in your script");
            }
            Defaults = checkEdit3.Checked;
        }

        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            Clean = checkEdit4.Checked;
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            Symbolize = checkEdit5.Checked;
            if(!Symbolize)
            {
                cout("Removing the symbols will remove the decompiler protection from your GSC!", "Warning");
            }
        }

        private void StringUsage_CheckedChanged(object sender, EventArgs e)
        {
            SHOW_COUNT = StringUsage.Checked;
        }

        private void checkEdit6_CheckedChanged(object sender, EventArgs e)
        {
            COMPILE_ONLY = checkEdit6.Checked;
        }
    }
}
