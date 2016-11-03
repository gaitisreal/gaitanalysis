using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Kinect
{
    class SystemDB
    {
        static MySqlConnection dbconn;
        MySqlCommand cmd;
        MySqlDataReader rdr;
        static string connString, query;
        public SystemDB()
        {
            connString = "server=localhost;uid=root;password=;database=gasystemdb;";
            dbconn =  new MySqlConnection(connString);
        }
        public string AnalyzeData(string type, int age, char gender, double strideVelocity)
        {
            // string type determines which table to be analyzed. if speed(velocity), steplength(stridelength), or stepfrequency
            dbconn.Open();
            double ci_lower = 0.00, ci_upper = 0.00, pi_lower = 0.00, pi_upper = 0.00;
            string output = "", table, varName;
            switch (type)
            {
                case "spd": table = "gspeed"; varName = "speed"; break;
                case "len": table = "gsteplen"; varName = "stride length"; break;
                case "freq": table = "gstepfreq"; varName = "step frequency"; break;
                default: table = ""; varName = ""; break;
            }
            query = String.Format("SELECT ci_lower, ci_upper, pi_lower, pi_upper FROM {0} WHERE gender = '{1}' AND '{2}' BETWEEN age_lower AND age_upper", table, gender, age);
            cmd = new MySqlCommand(query, dbconn);
            rdr = cmd.ExecuteReader();
            
            while (rdr.Read())
            {
                ci_lower = double.Parse(rdr.GetString(0));
                ci_upper = double.Parse(rdr.GetString(1));
                pi_lower = double.Parse(rdr.GetString(2));
                pi_upper = double.Parse(rdr.GetString(3));
            }

            if (strideVelocity > ci_lower && strideVelocity < ci_upper)
                output = "The " + varName + " within the normal range.";
            else if (strideVelocity < ci_lower)
                output = "The " + varName + " is less than the normal range.";
            else if (strideVelocity > ci_upper)
                output = "The " + varName + " is greater than the normal range.";
            dbconn.Close();
            return output;
        }
    }
}
