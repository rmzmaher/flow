using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiphase
{

    class FrictionPressureLoss
    {

        public static double mixture_viscosity(double P, double T)
        {
            return (Math.Pow(PVT.liquid_viscosity(P, T),  Gray.liquid_hold_up(P, T)) + Math.Pow(Inputs.gas_viscosity, (1 - Gray.liquid_hold_up(P, T))));
        }
/*        //friction factor calculations
        //s

        public static double s(double P, double T)
        {
            double y = Gray.input_liquid_fric(P, T) / (Math.Pow(Gray.liquid_hold_up(P, T), 2));

            if (y > 1 && y < 1.2)
                return ((Math.Log((2.2 * y), Math.E)) - 1.2);
            else
                return ((Math.Log(y, Math.E)) / (-0.0523 + (3.182 * (Math.Log(y, Math.E))) - (0.8725 * (Math.Pow((Math.Log(y, Math.E)), 2))) + (0.01853 * (Math.Pow((Math.Log(y, Math.E)), 4)))));
        }

        public static double friction_ratio(double P, double T)
        {
            return Math.Pow(Math.E, s(P, T));
        }

        public static double no_slip_friction_factor(double P, double T)
        {
            return Math.Pow((2 * (Math.Log10(Calculations.no_slip_reynolds_number(P, T) / ((4.5223 * (Math.Log10(Calculations.no_slip_reynolds_number(P, T)))) - 3.8215)))), -2);
        }

        public static double two_phase_friction_factor(double P, double T)
        {
            return friction_ratio(P, T) * no_slip_friction_factor(P, T);
        }*/
    }

}
