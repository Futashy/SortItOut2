using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SotItOut2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            string dir = Directory.GetCurrentDirectory();
            DirectoryInfo di = new DirectoryInfo(dir);
            DirectoryInfo[] dirs = di.GetDirectories();


            InitializeComponent();



            foreach (var folder in dirs)
            {
                if (folder.Name != "base" && folder.Name != "dmf")
                {
                    checkedListBox1.Items.Add(folder);
                }
            }

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string dir = Directory.GetCurrentDirectory();
            string name = new DirectoryInfo(dir).Name;

            if (name != "mods")
            {
                MessageBox.Show("This program should be located in 'mods' folder!", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

            toolTip1.SetToolTip(Sort, "Sorts current list with compatibility in mind");
            toolTip1.SetToolTip(DoneButton, "Creates mod list file using checked items in current list");
            toolTip1.SetToolTip(ExitButton, "Closes the program");
            toolTip1.SetToolTip(SelectAll, "Selects everything in current list");
            toolTip1.SetToolTip(UnselectAll, "Unselects everything in current list");
            toolTip1.SetToolTip(TopButton, "Moves selected item on the top of the list");
            toolTip1.SetToolTip(UpButton, "Moves selected item one line higher in the list");
            toolTip1.SetToolTip(DownButton, "Moves selected item one line lower in the list");
            toolTip1.SetToolTip(BottomButton, "Moves selected item on the bottom of the list");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dir = Directory.GetCurrentDirectory();
            string file = (dir + @"\mod_load_order.txt");

            using (StreamWriter ml = new StreamWriter(file))
            {
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    ml.WriteLine(item);
                }
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItems.Count > 0)
            {
                object item = checkedListBox1.SelectedItem;
                int index = checkedListBox1.SelectedIndex - 1;
                bool isChecked = checkedListBox1.GetItemCheckState(index + 1) == CheckState.Checked;
                if (index >= 0)
                {
                    checkedListBox1.Items.Remove(item);
                    checkedListBox1.Items.Insert(index, item);
                    index = checkedListBox1.Items.IndexOf(item);
                    checkedListBox1.SetSelected(index, true);
                    if (isChecked)
                    {
                        checkedListBox1.SetItemChecked(index, true);
                    }
                }
            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItems.Count > 0)
            {
                object item = checkedListBox1.SelectedItem;
                int index = checkedListBox1.SelectedIndex + 1;
                bool isChecked = checkedListBox1.GetItemCheckState(index - 1) == CheckState.Checked;
                if (index < checkedListBox1.Items.Count)
                {
                    checkedListBox1.Items.Remove(item);
                    checkedListBox1.Items.Insert(index, item);
                    index = checkedListBox1.Items.IndexOf(item);
                    checkedListBox1.SetSelected(index, true);
                    if (isChecked)
                    {
                        checkedListBox1.SetItemChecked(index, true);
                    }
                }
            }
        }

        private void topButton_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItems.Count > 0)
            {
                object item = checkedListBox1.SelectedItem;
                int index = checkedListBox1.Items.IndexOf(item);
                bool isChecked = checkedListBox1.GetItemCheckState(index) == CheckState.Checked;
                if (item != null)
                {
                    checkedListBox1.Items.Remove(item);
                    checkedListBox1.Items.Insert(0, item);
                    index = checkedListBox1.Items.IndexOf(item);
                    checkedListBox1.SetSelected(index, true);
                    if (isChecked)
                    {
                        checkedListBox1.SetItemChecked(index, true);
                    }
                }
            }
        }

        private void bottomButton_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItems.Count > 0)
            {
                object item = checkedListBox1.SelectedItem;
                int index = checkedListBox1.Items.IndexOf(item);
                bool isChecked = checkedListBox1.GetItemCheckState(index) == CheckState.Checked;
                if (item != null)
                {
                    checkedListBox1.Items.Remove(item);
                    checkedListBox1.Items.Add(item);
                    index = checkedListBox1.Items.IndexOf(item);
                    checkedListBox1.SetSelected(index, true);
                    if (isChecked)
                    {
                        checkedListBox1.SetItemChecked(index, true);
                    }
                }
            }
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void unselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void Sort_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            string[] topList = { "LogMeIn" };
            string[] bottomList = { "animation_events",
                                    "ui_extension",
            };

            foreach (var item in checkedListBox1.Items)
            {
                list.Add(item.ToString());
            }

            checkedListBox1.Items.Clear();
            list.Sort();

            foreach (var item in list)
            {
                checkedListBox1.Items.Add(item.ToString());
            }

            for (int i = 0; i < topList.Length; i++)
            {
                if (checkedListBox1.Items.Contains(topList[i]))
                {
                    checkedListBox1.Items.Remove(topList[i]);
                    checkedListBox1.Items.Insert(0, topList[i]);
                }
            }

            for (int i = 0; i < bottomList.Length; i++)
            {
                if (checkedListBox1.Items.Contains(bottomList[i]))
                {
                    checkedListBox1.Items.Remove(bottomList[i]);
                    checkedListBox1.Items.Add(bottomList[i]);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }
    }
}
