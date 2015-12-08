using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirsModel
{
    public class DataParser
    {
        public void parseCSV(string input, string output)
        {
            using (StreamWriter outputFile = new StreamWriter(output))
            {
                using (StreamReader inputFile = new StreamReader(input))
                {
                    int topLattitude = 500;
                    int bottomLattitude = 250;
                    int westernLongitude = 55; // actual -1250
                    int easternLongitude = 114; // actual -660
                    string[] lineArr;
                    string line;
                    string writeLine = String.Empty;
                    int lineCounter = 900;
                    while ((line = inputFile.ReadLine()) != null && lineCounter > topLattitude)
                    {
                        lineCounter--; // remove the top data
                    }
                    while ((line = inputFile.ReadLine()) != null && lineCounter > bottomLattitude)
                    {
                        writeLine = String.Empty;
                        lineArr = line.Split(',');
                        for (int i = 0; i < lineArr.Length; i++)
                        {
                            if (i < westernLongitude)
                            {
                                // do nothing
                            }
                            else if (i > easternLongitude)
                            {
                                // do nothing
                            }
                            else
                            {
                                writeLine += lineArr[i] + ",";
                            }
                        }
                        outputFile.WriteLine(writeLine);
                    }
                }
            }
        }
    }
}
