using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lecture10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tbTeacherBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tbTeacherBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dbDataSet);

        }

        // Creating a function to save the data from copying and pasting the data from the SaveItem_Click on the menubar
        // also creating a validation function insude of save data function in order to know if the provided data is empty or not
        // before saving it and if it is empty show the error message to fill up the data.

        private void SaveData()
        {

            if (this.Validate())
            {
                try
                {
                    this.tbTeacherBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.dbDataSet);
                    MessageBox.Show("Saved new File", "Saved");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            
        }

        // Creating a save button to save data

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // inside of the Form1_Load I put the code inside of the try and catch in case if the date cannot be found or other issues
            // catch(Exception ex)

            try
            {
                // TODO: This line of code loads data into the 'dbDataSet.tbCountry' table. You can move, or remove it, as needed.
                this.tbCountryTableAdapter.Fill(this.dbDataSet.tbCountry);
                // TODO: This line of code loads data into the 'dbDataSet.tbTeacher' table. You can move, or remove it, as needed.
                this.tbTeacherTableAdapter.Fill(this.dbDataSet.tbTeacher);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }        
        }


        private void dobDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void isActiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void isActiveLabel_Click(object sender, EventArgs e)
        {

        }

        // Move between data with visual buttons

        private void btnLast_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MoveLast();
        }

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

        // disable the buttons when it reaches either the first or the last data in the order

        private void EnableDisableButtons()
        {
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

            if(tbTeacherBindingSource.Position == tbTeacherBindingSource.Count - 1)
            {
                btnNext.Enabled = false;
                btnLast.Enabled= false;
            }
            else
            {
                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }
        }

        // now apply the new disable function to the events in the property of tbTeacherBindingSource

        private void tbTeacherBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        // Creating a delete button, and show pop up message to clarify whether the user wants to delete the data and show error 
        // if the listbox is empty with no data to delete

        private void btnDelete_Click(object sender, EventArgs e)
        {
           if (tbTeacherBindingSource.Count == 0)
            {
                MessageBox.Show("No more data to delete!", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var result = MessageBox.Show("Are you sure to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    tbTeacherBindingSource.RemoveCurrent();
                }
               
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Validate())
            {
                this.tbCountryBindingSource.EndEdit();
                if(dbDataSet.HasChanges())
                {
                    if (MessageBox.Show("Save?", "Save new file", MessageBoxButtons.YesNo) ==  DialogResult.Yes)
                    {
                        SaveData();
                    }
                }
            }
            else 
                e.Cancel = true;
        }
    }
}

