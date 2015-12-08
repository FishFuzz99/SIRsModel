using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SirsModel
{
    class Program
    {

        public static double deltaT { get; set; } = 1;
        public static double totalT { get; set; } = 100;
        public static double sumT = 0;
        public static float k { get; set; } = .1F; // fraction of infected group that will recover
        public static float b { get; set; } = .5F;
        public static float deathRate { get; set; } = .2F;// odds of an infected person dying after leaving the infected group (infectious, really)
        public static float borderTravelRate = .01F;
        public static List<GridUnit> activeGridUnits = new List<GridUnit>();
        public static int gridXLength = 20;
        public static int gridYLength = 20;
        public static GridUnit[,] array = new GridUnit[gridXLength, gridYLength];
        public static DataParser parser = new DataParser();
        static void Main(string[] args)
        {

            // parse the data
            parser.parseCSV("input.txt", "output.txt");

            // fill the array
            


            for (int i = 0; i < gridXLength; i++)
            {
                for (int j = 0; j < gridYLength; j++)
                {
                    array[i, j] = new GridUnit(10000, 0, 0, 10000, i, j);
                }
            }

            array[1, 2].i = Convert.ToDouble(1000.0 / array[1, 2].N);
            activeGridUnits.Add(array[1, 2]);

        


            // figure out the beginning GridUnit and add it to activeGridUnits


            while (sumT < totalT) 
            {
                Console.WriteLine("-----------------------");
                for (int i = 0; i < gridXLength; i++)
                {
                    for (int j = 0; j < gridYLength; j++)
                    {
                        if (array[i,j].I > 0)
                        {
                            Console.Write("X");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                    Console.Write("\n");                    
                }
                Console.WriteLine("-----------------------");
                //for each grid, call doIteration if I is not 0;
                
                for (int i = 0; i < activeGridUnits.Count; i++)
                {
                    doIteration(activeGridUnits.ElementAt(i), deltaT);
                    //overWriteGrid(i, temp);
                }
                int current = activeGridUnits.Count;
                //for each grid, call calculateSpreadToNeighbors() on it
                for (int i = 0; i < current; i++)
                {
                    calculateSpreadToNeighbors(activeGridUnits.ElementAt(i), borderTravelRate);
                }

                
                sumT += deltaT;
            }

            int ifasdf = 0;
        }

        public static void doIteration(GridUnit grid, double deltaT)
        {

            /*
            S = S(t)	is the number of susceptible individuals,
            I = I(t)	is the number of infected individuals, and
            R = R(t)	is the number of recovered individuals.

            s(t) = S(t)/N,	the susceptible fraction of the population,
            i(t) = I(t)/N,	the infected fraction of the population, and
            r(t) = R(t)/N,	the recovered fraction of the population.

            S' = -b s(t)I(t)
            s' = -b s(t)i(t)
            r' = k i(t)
            s' + i' + r' = 0
            i' = b s(t) i(t) - k i(t)

            */


            // calculate the rate of change for this iteration
            double sPrime = -(b) * grid.s * grid.i;
            double iPrime = b * grid.s * grid.i - (k * grid.i);
            double rPrime = k * grid.i;

            // apply the change to the population 
            grid.s += (sPrime * deltaT);
            grid.i += (iPrime * deltaT);
            grid.r += (rPrime * deltaT);

            // calculate losses
            long theDead = Convert.ToInt64(rPrime * grid.N * deltaT * deathRate);
            grid.D += theDead;
            grid.N -= theDead; // remove losses from total population, needs testing.]

            if (grid.I <= 0 || grid.N <= 0 || grid.R == grid.N)
            {
                activeGridUnits.Remove(grid);
            }
        }

        public static void calculateSpreadToNeighbors(GridUnit centerGrid, float borderTravelRate) // spreads infection out, one way, from each infected grid
        {


            long infectedOut = Convert.ToInt64(centerGrid.i * borderTravelRate * centerGrid.N);
            long recoveredOut = Convert.ToInt64(centerGrid.r * borderTravelRate * centerGrid.N);
            long susceptableOut = Convert.ToInt64(centerGrid.s * borderTravelRate * centerGrid.N);

            // readjust populations due to people leaving
            
            // calculate each direction
            if (!(centerGrid.yCoord <= 0)) // go down
            {
                GridUnit target = array[centerGrid.xCoord, centerGrid.yCoord - 1];
                if (target.N > 0)
                {
                    

                    target.N += infectedOut + recoveredOut + susceptableOut;
                    target.i += Convert.ToSingle(infectedOut) / target.N;
                    target.r += Convert.ToSingle(recoveredOut) / target.N;
                    target.s += Convert.ToSingle(susceptableOut) / target.N;

                    centerGrid.N -= infectedOut + recoveredOut + susceptableOut;
                    centerGrid.i -= Convert.ToSingle(infectedOut) / centerGrid.N;
                    centerGrid.r -= Convert.ToSingle(recoveredOut) / centerGrid.N;
                    centerGrid.s -= Convert.ToSingle(susceptableOut) / centerGrid.N;

                    overWriteGrid(centerGrid);
                    if (target.I > 0 && (!activeGridUnits.Contains(target)))
                    { 
                        activeGridUnits.Add(target);
                    }
                }
            }
            if (!(centerGrid.yCoord >= gridYLength - 1)) // go up
            {
                GridUnit target = array[centerGrid.xCoord, centerGrid.yCoord + 1];
                if (target.N > 0)
                {

                    target.N += infectedOut + recoveredOut + susceptableOut;
                    target.i += Convert.ToSingle(infectedOut) / target.N;
                    target.r += Convert.ToSingle(recoveredOut) / target.N;
                    target.s += Convert.ToSingle(susceptableOut) / target.N;

                    centerGrid.N -= infectedOut + recoveredOut + susceptableOut;
                    centerGrid.i -= Convert.ToSingle(infectedOut) / centerGrid.N;
                    centerGrid.r -= Convert.ToSingle(recoveredOut) / centerGrid.N;
                    centerGrid.s -= Convert.ToSingle(susceptableOut) / centerGrid.N;

                    if (target.I > 0 && (!activeGridUnits.Contains(target)))
                    {
                        activeGridUnits.Add(target);
                    }
                }
            }
            if (!(centerGrid.xCoord <= 0)) // go left
            {
                GridUnit target = array[centerGrid.xCoord - 1, centerGrid.yCoord];
                if (target.N > 0)
                {

                    target.N += infectedOut + recoveredOut + susceptableOut;
                    target.i += Convert.ToSingle(infectedOut) / target.N;
                    target.r += Convert.ToSingle(recoveredOut) / target.N;
                    target.s += Convert.ToSingle(susceptableOut) / target.N;

                    centerGrid.N -= infectedOut + recoveredOut + susceptableOut;
                    centerGrid.i -= Convert.ToSingle(infectedOut) / centerGrid.N;
                    centerGrid.r -= Convert.ToSingle(recoveredOut) / centerGrid.N;
                    centerGrid.s -= Convert.ToSingle(susceptableOut) / centerGrid.N;

                    if (target.I > 0 && (!activeGridUnits.Contains(target)))
                    {
                        activeGridUnits.Add(target);
                    }
                }
            }
            if (!(centerGrid.xCoord >= gridXLength - 1)) // go right
            {
                GridUnit target = array[centerGrid.xCoord + 1, centerGrid.yCoord];
                if (target.N > 0)
                {

                    target.N += infectedOut + recoveredOut + susceptableOut;
                    target.i += Convert.ToSingle(infectedOut) / target.N;
                    target.r += Convert.ToSingle(recoveredOut) / target.N;
                    target.s += Convert.ToSingle(susceptableOut) / target.N;

                    centerGrid.N -= infectedOut + recoveredOut + susceptableOut;
                    centerGrid.i -= Convert.ToSingle(infectedOut) / centerGrid.N;
                    centerGrid.r -= Convert.ToSingle(recoveredOut) / centerGrid.N;
                    centerGrid.s -= Convert.ToSingle(susceptableOut) / centerGrid.N;

                    if (target.I > 0 && (!activeGridUnits.Contains(target)))
                    {
                        activeGridUnits.Add(target);
                    }
                }
            }
        }
        public static void overWriteGrid(int i, GridUnit temp) 
        {
            activeGridUnits.ElementAt(i).N = temp.N;
            activeGridUnits.ElementAt(i).i = temp.i;
            activeGridUnits.ElementAt(i).r = temp.r;
            activeGridUnits.ElementAt(i).s = temp.s;
            activeGridUnits.ElementAt(i).D = temp.D;
        } 
        public static void overWriteGrid(GridUnit temp)
        {
            array[temp.xCoord, temp.yCoord].N = temp.N;
            array[temp.xCoord, temp.yCoord].i = temp.i;
            array[temp.xCoord, temp.yCoord].r = temp.r;
            array[temp.xCoord, temp.yCoord].s = temp.s;
            array[temp.xCoord, temp.yCoord].D = temp.D;
        }
    }
}
