using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2_Komplettering
{
    class Vehicle
    {
        private Enum type;
        private string reg;
        private DateTime date;

        public Vehicle()
        {

        }

        public Vehicle(Enum type, string reg, DateTime date)
        {
            this.type = type;
            this.reg = reg;
            this.date = date;
        }

        public Enum Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public string Reg
        {
            get { return this.reg; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Name can not be empty");
                this.reg = value;
            }
        }


        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.type, this.reg, this.date);
        }
    }
}
