using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Folk_CourseProject_Part2
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult=DialogResult.Cancel;
            this.Hide();
        }

        private void HoursWorkedTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void HourlyRateTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void HourlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if ( HourlyRadioButton.Checked )
            {
                // hide the salary items
                SalaryLabel.Visible = false;
                SalaryTextBox.Visible = false;
                 
                // show hourly items
                HourlyRateLabel.Visible = true;
                HourlyRateTextBox.Visible = true;
                HoursWorkedLabel.Visible = true;
                HoursWorkedTextBox.Visible = true;
            }
        }

        private void SalaryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SalaryRadioButton.Checked)
            {
                // show the salary items
                SalaryLabel.Visible = true;
                SalaryTextBox.Visible = true;

                // hide hourly items
                HourlyRateLabel.Visible = false;
                HourlyRateTextBox.Visible = false;
                HoursWorkedLabel.Visible = false;
                HoursWorkedTextBox.Visible = false;
            }
        }
    }
}
