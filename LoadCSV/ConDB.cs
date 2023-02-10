using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadCSV
{
    internal class ConDB
    {
        public string GetConnection()
        {
            string con = "Data Source=.;Initial Catalog=Student;Integrated Security=True";
            return con;
        }
    }
}
