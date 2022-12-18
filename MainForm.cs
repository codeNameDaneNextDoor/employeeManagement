using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Folk_CourseProject_Part2
{
    public partial class MainForm : Form
    {
        // class level references
        private const string FILENAME = "Employee.dat";

        public MainForm()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // add item to the employee listbox
            InputForm frmInput = new InputForm();

            using (frmInput)
            {
                DialogResult result = frmInput.ShowDialog();

                // see if input form was cancelled
                if (result == DialogResult.Cancel)
                    return;    //  end the method since the user cancelled the input

                // get user's input and create an Employee object
                string fName = frmInput.FirstNameTextBox.Text;
                string lName = frmInput.LastNameTextBox.Text;
                string ssn = frmInput.SSNTextBox.Text;
                string date = frmInput.HireDateTextBox.Text;
                DateTime hireDate = DateTime.Parse(date);
                string healthIns = frmInput.HealthInsuranceTextBox.Text;
                int lifeIns = int.Parse(frmInput.LifeInsuranceTextBox.Text);
                int vacation = int.Parse(frmInput.VacationDaysTextBox.Text);

                Benefits ben = new Benefits(healthIns, lifeIns, vacation);
                Employee emp = null; // empty reference


                if ( frmInput.SalaryRadioButton.Checked)
                {
                    double salary = double.Parse(frmInput.SalaryTextBox.Text);
                    emp = new Salary(fName, lName, ssn, hireDate, ben, salary);
                }
                else if (frmInput.HourlyRadioButton.Checked)
                {
                    double hourlyRate = double.Parse(frmInput.HourlyRateTextBox.Text);
                    double hoursWorked = double.Parse(frmInput.HoursWorkedTextBox.Text);
                    emp = new Hourly(fName, lName, ssn, hireDate, ben, hourlyRate, hoursWorked);
                }
                else
                {
                    MessageBox.Show("Error, Please Select an Employee Type.");
                    return; //end the method
                }

                // add the Employee object to the employees listbox
                EmployeeListBox.Items.Add(emp);

                // write all data to file
                WriteEmpsToFile();


            }
        }

        private void WriteEmpsToFile()
        {
            //convert ListBox items to generic list
            List<Employee> empList = new List<Employee>();

            foreach(Employee emp in EmployeeListBox.Items)
            {
                empList.Add(emp);
            }

            // open a pipe to the file and create a translator
            FileStream fs = new FileStream(FILENAME, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            // write the generic list to the file
            formatter.Serialize(fs, empList);

            // close the pipe
            fs.Close();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            // remove the selected item from the listbox

            int itemNumber = EmployeeListBox.SelectedIndex;

            if(itemNumber > -1)
            {
                EmployeeListBox.Items.RemoveAt(itemNumber);
                WriteEmpsToFile();
            }
            else
            {
                MessageBox.Show("Please Select Employee to Remove.");
            }
        }

        private void DisplayButton_Click(object sender, EventArgs e)
        {


            // check to see if file exists
            if (File.Exists(FILENAME) && new FileInfo(FILENAME).Length > 0)
            {
                //create a pipe to the file and create a translator
                FileStream fs = new FileStream(FILENAME, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();

                //read generic list from the file
                List<Employee> list = (List<Employee>)formatter.Deserialize(fs);

                //close the pipe
                fs.Close();

                //clear employee list box
                EmployeeListBox.Items.Clear();

                foreach (Employee emp in list)
                    EmployeeListBox.Items.Add(emp);
            }

            
        }


        private void PrintPaychecksButton_Click(object sender, EventArgs e)
        {
            foreach (Employee emp in EmployeeListBox.Items)
            {
                string line1 = "Pay To: " + emp.FirstName + " " + emp.LastName;
                string line2 = "Amount Of: " + emp.CalculatePay().ToString("C2");

                string output = "Paycheck:\n\n" + line1 + "\n" + line2;

                MessageBox.Show(output);
            }
        }

        private void EmployeeListBox_DoubleClick(object sender, EventArgs e)
        {
            //get selected employee object
            Employee emp = EmployeeListBox.SelectedItem as Employee;

            // show Input/Update Form with Employee info

            InputForm frmUpdate = new InputForm();
            frmUpdate.Text = "Update Employee Information";
            frmUpdate.SubmitButton.Text = "Update";
            frmUpdate.FirstNameTextBox.Text = emp.FirstName;
            frmUpdate.LastNameTextBox.Text = emp.LastName;
            frmUpdate.SSNTextBox.Text = emp.SSN;
            frmUpdate.HireDateTextBox.Text = emp.HireDate.ToShortDateString();
            frmUpdate.HealthInsuranceTextBox.Text = emp.BenefitsEmp.HealthInsurance;
            frmUpdate.LifeInsuranceTextBox.Text = emp.BenefitsEmp.LifeInsurance.ToString();
            frmUpdate.VacationDaysTextBox.Text = emp.BenefitsEmp.Vacation.ToString();
            
            // check if emp is salary or hourly
            if (emp is Salary)
            {
                frmUpdate.HourlyRateLabel.Visible = false;
                frmUpdate.HourlyRateTextBox.Visible = false;
                frmUpdate.HoursWorkedLabel.Visible = false;
                frmUpdate.HoursWorkedTextBox.Visible = false;
                frmUpdate.SalaryLabel.Visible = true;
                frmUpdate.SalaryTextBox.Visible = true;

                // mark radio button
                frmUpdate.SalaryRadioButton.Checked = true;

                // convert the Employee 
                Salary sal = (Salary)emp;

                // show the Salary information
                frmUpdate.SalaryTextBox.Text = sal.AnnualSalary.ToString("F2");
            }
            else if (emp is Hourly)
            {
                           
                frmUpdate.HourlyRateLabel.Visible = true;
                frmUpdate.HourlyRateTextBox.Visible = true;
                frmUpdate.HoursWorkedLabel.Visible = true;
                frmUpdate.HoursWorkedTextBox.Visible = true;
                frmUpdate.SalaryLabel.Visible = false;
                frmUpdate.SalaryTextBox.Visible = false;

                // mark radio button
                frmUpdate.HourlyRadioButton.Checked = true;

                // convert the Employee 
                Hourly hrly = (Hourly)emp;

                // show the Hourly information
                frmUpdate.HourlyRateTextBox.Text = hrly.HourlyRate.ToString("F2");
                frmUpdate.HoursWorkedTextBox.Text = hrly.HoursWorked.ToString("F2");
            }
            else
            {
                MessageBox.Show("Error. Invalid employee type found.");
                return; // end the method
            }

            DialogResult result = frmUpdate.ShowDialog();

            //if cancelled stop method
            if (result == DialogResult.Cancel)
                return; //end method

            //Delete selected object
            int position = EmployeeListBox.SelectedIndex;
            EmployeeListBox.Items.RemoveAt(position);

            //create new employee using updated info
            Employee newEmp = null;

            string fName = frmUpdate.FirstNameTextBox.Text;
            string lName = frmUpdate.LastNameTextBox.Text;
            string ssn = frmUpdate.SSNTextBox.Text;
            DateTime hireDate = DateTime.Parse(frmUpdate.HireDateTextBox.Text);
            string HealthInsurance = frmUpdate.HealthInsuranceTextBox.Text;
            int LifeInsurance = int.Parse(frmUpdate.LifeInsuranceTextBox.Text);
            int Vacation = int.Parse(frmUpdate.VacationDaysTextBox.Text);

            Benefits ben = new Benefits(HealthInsurance, LifeInsurance, Vacation);

            if (frmUpdate.SalaryRadioButton.Checked)
            {
                double salary = double.Parse(frmUpdate.SalaryTextBox.Text);
                newEmp = new Salary(fName, lName, ssn, hireDate, ben, salary);
            }
            else if (frmUpdate.HourlyRadioButton.Checked)
            {
                double hourlyRate = double.Parse(frmUpdate.HourlyRateTextBox.Text);
                double hoursWorked = double.Parse(frmUpdate.HoursWorkedTextBox.Text);
                newEmp = new Hourly(fName, lName, ssn, hireDate, ben, hourlyRate, hoursWorked);
            }
            else
            {
                MessageBox.Show("Error, Invalid Employee Type.");
                return; // end the method
            }

            //add the new employee to the listbox
            EmployeeListBox.Items.Add(newEmp); 
        }

        private void EmployeeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
