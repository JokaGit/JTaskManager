using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace JTaskManager
{
    public partial class MainForm : Form
    {
        Process[] ProcessArray;
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();        
        }

        private void MainForm_Load(object sender, EventArgs e)
        {            
            LoadProcesses();
        }

        private void LoadProcesses()
        {
            dgv.Rows.Clear();
            ProcessArray = Process.GetProcesses();
            foreach (Process pr in ProcessArray)
            {              
                dgv.Rows.Add(pr.Id, pr.ProcessName, pr.MainWindowTitle, pr.BasePriority, pr.VirtualMemorySize64, "");
            }
            dgv.Sort(dgv.Columns[2], ListSortDirection.Descending);
            dgv.ClearSelection();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {        
            LoadProcesses();
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Выберите процесс");
                return;
            }
            Process.GetProcessById(Int32.Parse(dgv.Rows[dgv.SelectedCells[0].RowIndex].Cells[0].Value.ToString())).Kill();
            LoadProcesses();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProcesses();

            for (int i = 0; i < dgv.RowCount; i++)
            {
                if (!dgv.Rows[i].Cells[1].Value.ToString().ToLower().Contains(tbSearch.Text.ToLower()))
                {
                    dgv.Rows.RemoveAt(i);
                    i--;
                }
            }
        }

        private void btnGetPorts_Click(object sender, EventArgs e)
        {
            string info = "";
            foreach (ProcessPort p in ProcessPorts.ProcessPortMap.FindAll(x => x.ProcessId == Convert.ToInt32(dgv.CurrentRow.Cells[0].Value)))
            {
                info += (p.ProcessPortDescription) + "\n";
            }
            if (info == "")
                MessageBox.Show("Ни один порт не используется");
            else MessageBox.Show(info, $"Используемые процессом {dgv.CurrentRow.Cells[1].Value} порты");
        }
    }
}
