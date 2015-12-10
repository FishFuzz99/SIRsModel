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
        public static int topLattitude = 500;
        public static int bottomLattitude = 250;
        public static int westernLongitude = 550; // actual -1250
        public static int easternLongitude = 1140; // actual -660
        public int width = easternLongitude - westernLongitude;
        public int height = topLattitude - bottomLattitude;
        public void parseCSV(string input, string output, GridUnit[,] array, List<GridUnit> airportGridUnits, List<GridUnit> activeGridUnits)
        {
            using (StreamWriter outputFile = new StreamWriter(output))
            {
                using (StreamReader inputFile = new StreamReader(input))
                {
                    
                    string[] lineArr;
                    string line;
                    string writeLine = String.Empty;
                    int lineCounter = 850;
                    bool oneAdded = true;

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
                            else if (i > easternLongitude - 1)
                            {
                                break;
                            }
                            else
                            {
                                writeLine += lineArr[i] + ",";
                                
                                if (lineArr[i] != "99999.0" && lineArr[i] != "1.0369266")
                                {
                                    array[i - westernLongitude, lineCounter - bottomLattitude - 1] = new GridUnit(Convert.ToInt64(Convert.ToSingle(lineArr[i]) * 100), 0, 0, Convert.ToInt64(Convert.ToSingle(lineArr[i]) * 100), i - westernLongitude, lineCounter - bottomLattitude - 1);
                                    if (array[i - westernLongitude, lineCounter - bottomLattitude - 1].N > 100000)
                                    {
                                        if (oneAdded)
                                        {
                                            activeGridUnits.Add(array[i - westernLongitude, lineCounter - bottomLattitude - 1]);
                                            activeGridUnits.ElementAt(0).i = .1;
                                            oneAdded = false;
                                        }
                                        airportGridUnits.Add(array[i - westernLongitude, lineCounter - bottomLattitude - 1]);
                                    }
                                }
                                else
                                {
                                    array[i - westernLongitude, lineCounter - bottomLattitude - 1] = new GridUnit(0, 0, 0, 0, i - westernLongitude, lineCounter - bottomLattitude - 1);
                                    
                                }
                            }
                            
                        }
                        outputFile.WriteLine(writeLine);
                        lineCounter--;
                    }
                }
            }
        }
    }
}
