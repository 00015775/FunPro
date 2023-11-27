using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace Recapa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // this code snippet already has features to save files however we need to modify it and add modifications to SaveData function

        private void tbTeacherBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tbTeacherBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dbDataSet);

        }

        // for save button we create a function SaveData
        // Added error handling to the function as well

        private void SaveData()
        {
            try
            {
                if (this.Validate())
                {
                    this.tbTeacherBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.dbDataSet);
                    MessageBox.Show("Successfully Saved", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Added function to the button

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }


        //  Handled error in case of the file path cannot be found

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.tbCountryTableAdapter.Fill(this.dbDataSet.tbCountry);
                this.tbTeacherTableAdapter.Fill(this.dbDataSet.tbTeacher);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           

        }

        // Now adding a saving option when the changes are done but the user is already closing the application
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Validate())
            {
                this.tbTeacherBindingSource.EndEdit();
                if (dbDataSet.HasChanges())
                {
                    if(MessageBox.Show("Do you want to save changes?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        SaveData();
                    }
                }
            }
            else
                e.Cancel = true;
        }

        // Adding buttons to move between data

        private void btnFirst_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MoveFirst();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MovePrevious();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MoveNext();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MoveLast();
        }

        // Adding a function to disable buttons when they reach either the first or the last data in the given listBox

        private void DisableButtons()
        {
            // If the data is at the top or position equals to 0 then move buttons will be disabled

            if(tbTeacherBindingSource.Position == 0)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
            }
            else
            {
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
            }

            // if the data is at the bottom thne move buttons will be disabled as well

            if(tbTeacherBindingSource.Position == tbTeacherBindingSource.Count - 1)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }
        }

        // Added DisableButtons function to the CurrentChanged of the tbTeacherBindingSource

        private void tbTeacherBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            DisableButtons();
        }

        // Checks if the file is empty and asks of the user wants to delete a file or not

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(tbTeacherBindingSource.Count == 0)
            {
                MessageBox.Show("No more data to delete!", "Empty file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var result = MessageBox.Show("Do you want to delete file?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if ( result == DialogResult.Yes)
                {
                    tbTeacherBindingSource.RemoveCurrent();
                    MessageBox.Show("Successfully deleted!", "File deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Pop up message will be shown if the first and last name inputs are left empty and it will persist until the user fills the input
        private void firstNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(firstNameTextBox.Text))
            {
                MessageBox.Show("First name cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void lastNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(lastNameTextBox.Text))
            {
                MessageBox.Show("Last name cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        // Filter to search for data by its First Name

        private void tbxFilter_TextChanged(object sender, EventArgs e)
        {
            tbTeacherBindingSource.Filter = $"firstName LIKE '{tbxFilter.Text}%'";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        // For adding new files to the listBox

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var selectedThing = ((DataRowView)addCountry.SelectedItem).Row;
            dbDataSet.tbTeacher.AddtbTeacherRow(
            addName.Text,
            addLastName.Text,
            addDataTime.Value,
            addPhone.Text,
            (int)addGrade.Value,
            addActive.Checked,
            (dbDataSet.tbCountryRow)selectedThing
            );
        }
    }
}
