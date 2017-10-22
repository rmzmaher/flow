using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiphase
{
    class TwoPhaseVariables
    {
        
   
        public static double gas_rate_cuftsec(double P, double T)
        {
            double GLR = Inputs.GLR;
            double liquid_rate = Inputs.liquid_rate;
            return Math.Round(((3.27 * Math.Pow(10, -7) * PVT.z_factor(P, T) * liquid_rate * (GLR - PVT.Rs(P, T)) * (T + 460)) / P), 5);
        }
        // well area
        public static double area()
        {
            double diameter;
            diameter = Inputs.well_inner_diameter;
            return Math.Round(((Math.PI * Math.Pow(diameter, 2)) / (4 * 144)), 4);
        }
        // velocity of mixture calculations
        public static double superficial_liquid_velocity(double P, double T)
        {
            //M15*5.615/86400/P6
            double liquidrate = Inputs.liquid_rate;
            return Math.Round( (liquidrate*5.615)/(86400*area()), 3);
        }
        public static double superficial_gas_velocity(double P, double T)
        {
            // 1/Area*Gas Production*Z*(460+Temp)/(460+60)*(14.7/P)/86400
            double gas_production_rate = TwoPhaseVariables.gas_rate_cuftsec(P,T);

            return Math.Round(gas_production_rate/(area()),3);
        }

        public static double mixture_velocity(double P, double T)
        {
            return Math.Round((superficial_gas_velocity(P, T) + superficial_liquid_velocity(P, T)), 3);
        }

        public static double two_phase_density(double P, double T)
        {
            return (PVT.liquid_density(P, T) * Gray.liquid_hold_up(P, T)) + (PVT.gas_density(P, T) * (1 - Gray.liquid_hold_up(P, T)));
        }
        /*
        public static double mixture_viscosity(double P, double T)
        {
            return ((PVT.liquid_viscosity(P, T) * Gray.input_liquid_fric(P, T)) + (PVT.gas_viscosity(P, T) * (1 - Gray.input_liquid_fric(P, T)))) * (6.27 * Math.Pow(10, -4));
        }*/
    }
}
