using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirsModel
{
    class GridUnit
    {

        private double _s;
        private double _i;
        private double _r;

        public long S { get; set; } // number of susceptible individuals
        public long I { get; set; } // number of infected individuals
        public long R { get; set; } // number of recovered individuals
        public long D { get; set; } // number of deaths
        public long N { get; set; } // total Population, initially set by population density info
        public int xCoord;
        public int yCoord;

        public double s
        {
            get { return _s; }
            set
            {
                if (value > 1)
                {
                    S = N;
                    _r = 1;
                }
                else
                {
                    S = Convert.ToInt64(value * N);
                    _s = value;
                }
                
            } 
        }

        public double i {
            get { return _i; }
            set
            {
                
                if (value > 1)
                {
                    _i = 1;
                    I = N;
                }
                else
                {
                    I = Convert.ToInt64(value * N);
                    _i = value;
                }
            }
        }
        public double r {
            get { return _r; }
            set
            {
                if (value > 1)
                {
                    _r = 1;
                    R = N;
                }
                else
                {
                    R = Convert.ToInt64(value * N);
                    _r = value;
                }
            }
        }

        public GridUnit(long S, long I, long R, long N, int xCoord, int yCoord)
        {
            this.s = S / N;
            this.i = I / N;
            this.r = R / N;
            this.N = N;
            D = 0;
            this.xCoord = xCoord;
            this.yCoord = yCoord;

        }

    }
}
