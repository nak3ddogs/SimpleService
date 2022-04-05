using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SimpleApp.RestApi;

namespace SimpleApp
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        private Dictionary<ListViewItem, DocsRecord> binding = new Dictionary<ListViewItem, DocsRecord>(); //I guess there is better solution than this

        public Form1()
        {
            InitializeComponent();
            RefreshDocs();
        }

        public void Log(string msg)
        {
            var temp = logView.Lines.ToList();
            temp.Add(msg);
            logView.Lines = temp.ToArray();
        }

        private void downloadBtn_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0 || listView1.SelectedItems.Count == 0)
            {
                Program.Log("There is no selected doc");
                return;
            }
            string path = GetDownloadPath();

            var target = listView1.SelectedItems[0];
            var record = binding[target];
            RestApi.DownloadFile(record.name, path);
        }

        private string GetDownloadPath()
        {
            string defaultPath = System.IO.Directory.GetCurrentDirectory();
            using (var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select download directory";
                folderBrowserDialog.SelectedPath = defaultPath;
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return folderBrowserDialog.SelectedPath;
                }
                else
                {
                    return Environment.CurrentDirectory;
                }
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            RefreshDocs();
        }

        private void RefreshDocs()
        {
            const string URL = "http://localhost:8080/api/dokumentumok";
            var result = RestApi.GetDocs();

            binding.Clear();
            listView1.Items.Clear();
            foreach (var record in result)
            {
                var item = new ListViewItem(record.name);
                item.SubItems.Add(record.path);
                listView1.Items.Add(item);
                binding.Add(item, record);
            }
        }
    }

}
