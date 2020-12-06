using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ダークソウルbackupツール
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string dataDir = System.Environment.ExpandEnvironmentVariables(this.textBox1.Text);
            string bakDir = this.textBox2.Text;

            if (!Directory.Exists(dataDir))
            {
                return;
            }
            if (!Directory.Exists(bakDir))
            {
                return;
            }

            this.comboBox1.Items.AddRange(this.getBackupNodes(this.textBox2.Text));

            this.toolStripStatusLabel1.Text = "";
            this.toolStripStatusLabel2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dataDir = System.Environment.ExpandEnvironmentVariables(this.textBox1.Text);
            string bakDir = this.textBox2.Text;

            if (!Directory.Exists(dataDir))
            {
                MessageBox.Show(this.label1.Text + " のフォルダは存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(bakDir))
            {
                MessageBox.Show(this.label2.Text + " のフォルダは存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.toolStripStatusLabel2.Text = "■■[executing]■■■■";
            this.Refresh();

            DateTime now = DateTime.Now;
            string nodeDir = now.ToString("yyyyMMddHHmm");
            string destDir = Path.Combine(bakDir, nodeDir, Path.GetFileNameWithoutExtension(dataDir));
            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(dataDir, destDir, true);

            this.comboBox1.Items.Insert(0, nodeDir);
            this.comboBox1.SelectedIndex = -1;

            this.textBox3.AppendText(now.ToString("yyyy/MM/dd HH:mm:ss") + " | [backup] @" + nodeDir + System.Environment.NewLine);
            this.toolStripStatusLabel2.Text = "[backup] @" + nodeDir;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dataDir = System.Environment.ExpandEnvironmentVariables(this.textBox1.Text);
            string bakDir = this.textBox2.Text;

            if (!Directory.Exists(dataDir))
            {
                MessageBox.Show(this.label1.Text + " のフォルダは存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(bakDir))
            {
                MessageBox.Show(this.label2.Text + " のフォルダは存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("復元対象の日時を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.toolStripStatusLabel2.Text = "■■[executing]■■■■";
            this.Refresh();

            string nodeFullDir = Path.Combine(bakDir, this.comboBox1.Text, Path.GetFileNameWithoutExtension(dataDir));
            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(nodeFullDir, dataDir, true);

            this.textBox3.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " | [recovery] @" + this.comboBox1.Text + System.Environment.NewLine);
            this.toolStripStatusLabel2.Text = "[recovery] @" + this.comboBox1.Text;
        }

        private string[] getBackupNodes(string bakDir){
            string[] childDirs = System.IO.Directory.GetDirectories(bakDir, "*", System.IO.SearchOption.TopDirectoryOnly);

            var query = (from s in childDirs
                        orderby s descending
                        select System.IO.Path.GetFileNameWithoutExtension(s));

            string[] ret = query.ToArray<string>();
            return ret;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = DateTime.Now.ToString("MM-dd HH:mm:ss");
        }
    }
}
