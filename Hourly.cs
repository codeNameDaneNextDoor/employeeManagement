using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folk_CourseProject_Part2
{
    [Serializable]
    public class Hourly : Employee
    {
        //attributes
        private double hourlyRate;
        private double hoursWorked;

        //constructors
        public Hourly() : base()
        {
            hourlyRate = 0.0;
            hoursWorked = 0.0;
        }
        public Hourly(string firstName, string lastName, string ssn, DateTime hireDate, Benefits benefits,
            double hourlyRate, double hoursWorked)
            : base(firstName, lastName, ssn, hireDate, benefits)
        {
            this.hourlyRate = hourlyRate;
            this.hoursWorked = hoursWorked;
        }

        //behaviors
        public override double CalculatePay()
        {
            double pay = 0.0;

            if( hoursWorked > 40.0 )
            {
                double basePay = 40.0 * hourlyRate;
                double overtime = (hoursWorked - 40.0) * hourlyRate * 1.5;
                pay = basePay + overtime;
            }
            else
            {
                pay = hoursWorked * hourlyRate;
            }

            return pay;
        }

        public override string ToString()
        {
            return base.ToString() + ", hourlyRate=" + hourlyRate.ToString("C2")
                + ", hoursWorked=" + hoursWorked.ToString("F2");

        }

        //properties
        public double HourlyRate
        {
            get { return hourlyRate; }
            set { hourlyRate = value; }
        }

        public double HoursWorked
        {
            get { return hoursWorked; }
            set { hoursWorked = value; }
        }

    }
}
