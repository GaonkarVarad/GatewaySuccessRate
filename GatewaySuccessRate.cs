using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace GatewaySuccessRate
{
    class Program
    {
        static void Main(string[] args)
        {
            string basepath = "D:/AMI/Projects/Pune/Readings/15th/";
            string[] csvFiles = Directory.GetFiles(basepath, "*.csv");

            int dinterval = 30; // Time tolerance in minutes

            foreach (string filename in csvFiles)
            {
                string coltitle = filename.Substring(filename.IndexOf("RDS_") + 4, filename.LastIndexOf(".") - filename.IndexOf("RDS_") - 4);
                int checkoms = filename.IndexOf("OMS");
                string tcoltitle = "t" + coltitle;
                string fdate = filename.Substring(filename.IndexOf("RDS_") + 4, filename.IndexOf("_", filename.IndexOf("RDS_") + 4) - filename.IndexOf("RDS_") - 4);
                string ftime = filename.Substring(filename.LastIndexOf("_") + 1, filename.LastIndexOf(".") - filename.LastIndexOf("_") - 1);
                string fhr = ftime.Substring(0, 2);
                string fmin = ftime.Substring(2, 2);

                int nu = 0;

                using (StreamReader csvinput = new StreamReader(filename))
                using (StreamWriter csvoutput = new StreamWriter("D:\\AMI\\Projects\\Pune\\output.csv"))
                using (StreamReader csvinput1 = new StreamReader("D:\\AMI\\Projects\\Pune\\temp.csv"))
                {
                    csvoutput.NewLine = "\n";
                    string line;
                    string line1;

                    while ((line1 = csvinput1.ReadLine()) != null)
                    {
                        csvoutput.WriteLine(line1 + "," + coltitle);
                    }

                    while ((line = csvinput.ReadLine()) != null)
                    {
                        string[] row = line.Split(',');
                        string tdate = row[1];

                        DateTime tdateTime = DateTime.ParseExact(tdate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        DateTime fdateTime = DateTime.ParseExact(fdate + " " + fhr + ":" + fmin + ":00", "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);

                        if (row[0] == tcoltitle && Math.Abs((tdateTime - fdateTime).TotalMinutes) < dinterval)
                        {
                            csvoutput.WriteLine(string.Join(",", row[1], row[2]));
                            nu++;
                        }
                        else
                        {
                            csvoutput.WriteLine(string.Join(",", row[1], ""));
                        }
                    }
                }

                Console.WriteLine("Numbers: " + nu);
            }
        }
    }
}
