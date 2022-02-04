using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Essy.Tools.InputBox;
using InstaInsightsV2.Domain.Campaign;

namespace InstaInsightsV2.Forms
{
    public partial class WebForm : Form
    {
        public List<Campaign> Campaigns = new List<Campaign>();
        public List<DataGridView> DataGridViews = new List<DataGridView>();

        public WebForm()
        {
            InitializeComponent();
        }

        private void WebForm_Load(object sender, EventArgs e)
        {
          
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
        }

        private async void toolStripLabel1_Click(object sender, EventArgs e)
        {
            if (!File.Exists("usernames.txt"))
            {
                File.WriteAllText("usernames.txt", "");
            }
            foreach (var input in File.ReadAllLines("usernames.txt"))
            {
                if (input.Length < 1)
                {
                    continue;
                }
                DataGridView grid = new DataGridView
                {
                    Dock = DockStyle.Fill
                };
                grid.Enabled = false;
                grid.Tag = input;
                DataGridViews.Add(grid);
                grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                Thread t = new Thread(() =>
                {
                    var campaign = new Campaign(input.Trim(), grid, checkBox1.Checked);
                    Campaigns.Add(campaign);
                    campaign.Update();
                });
                t.Start();
                TabPage page = new TabPage(input.Trim());
                page.Controls.Add(grid);
                Pages.TabPages.Add(page);
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var inputBox = InputBox.ShowInputBox("Enter Intsagram Handle");
            File.AppendAllText("usernames.txt","\r\n" +  inputBox.Trim() + "\r\n");
            var question = MessageBox.Show("Export Metrics?", "Would you like to run Export Metrics?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (question == DialogResult.Yes)
            {
                toolStripLabel1_Click(this, e);
            }
        }

        private void Pages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}