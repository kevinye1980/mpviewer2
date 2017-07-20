using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Microsoft.EnterpriseManagement.Configuration;

namespace MPViewer
{
    public partial class MultipleMPSelectionForm : Form
    {
        public IList<ManagementPack> ChosenMP;
        private IList<ManagementPack> MPList;

        public MultipleMPSelectionForm(IList<ManagementPack> MPListInput)
        {

            this.MPList = MPListInput;

            this.MPSortableList = new SortableListView();
            this.MPSortableList.Items.Clear();
            this.MPSortableList.Columns.Clear();

            this.MPSortableList.Columns.Add("Management Pack");
            this.MPSortableList.Columns.Add("Version");
            this.MPSortableList.Columns.Add("Sealed");
            this.MPSortableList.MultiSelect = true;
            foreach (ManagementPack mp in MPListInput)
            {
                ListViewItem item = new ListViewItem();
                item.Text = mp.Name;
                item.SubItems.Add(mp.Version.ToString());
                item.SubItems.Add(mp.Sealed.ToString());
                item.Tag = mp;
                this.MPSortableList.Items.Add(item);
            }

            this.MPSortableList.AdjustColumnSizes();

            InitializeComponent();
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            ChosenMP = new List<ManagementPack>();
            //since the form has multi-select disabled, the selected item is always index 0
            foreach (ListViewItem item in MPSortableList.SelectedItems)
            {
                ChosenMP.Add((ManagementPack)item.Tag);
            }
            this.Close();
            this.DialogResult = DialogResult.OK;
            return;
        }

        private void MPSortableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MPSortableList.SelectedItems.Count == 0)
            {
                this.buttonOK.Enabled = false;
            }
            else
            {
                this.buttonOK.Enabled = true;
            }

            return;
        }

    }
}
