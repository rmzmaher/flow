using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiphase
{
    class Gray
    {   
        public static double Rv(double P, double T)
        {
            double Vsg = TwoPhaseVariables.superficial_gas_velocity(P, T);
            double Vsl = TwoPhaseVariables.superficial_liquid_velocity(P, T);
            return Math.Round((Vsl/Vsg),5);
        }

        public static double modified_liquid_ift(double P, double T)
        {
            double IftStar;
            double alfa = 1/0.00220462;
            double qo = Inputs.oil_rate;
            double qw = Inputs.water_rate;
            double ift_o =Inputs.oil_interficial_tension;
            double ift_w =Inputs.water_interficial_tension;
            IftStar = alfa*((qo*ift_o + .617*qw*ift_w)/(qo + .617*qw));
            return IftStar;
        }

        public static double N1(Double P, Double T)
        {
            double rho_ns;
            double vm;
            double rho_l;
            double rho_g;
            rho_ns = PVT.NoSlip_density(P, T);
            vm = TwoPhaseVariables.mixture_velocity(P, T);
            rho_l = PVT.liquid_density(P, T);
            rho_g = PVT.gas_density(P, T);
            return ((Math.Pow(rho_ns, 2)*Math.Pow(vm, 4))/(32* modified_liquid_ift(P,T)*(rho_l-rho_g)));
        }
        
        public static double N2(Double P, Double T)
        {
            double rho_liq;
            double rho_gas;
            double ift;
            double diam_ft;
            rho_liq = PVT.oil_density(P, T);
            rho_gas = PVT.gas_density(P, T);
            ift = modified_liquid_ift(P, T);
            diam_ft = Inputs.well_inner_diameter/12;
            return ((32*Math.Pow(diam_ft, 2)*(rho_liq-rho_gas))/ift);
        }

        public static double N3(Double P, Double T)
        {
            return (0.0814 * (1 - 0.0554 * Math.Log((1 + (730 * Gray.Rv(P, T) / (Gray.Rv(P, T)+1))), Math.E)));
        }

        public static double F1(Double P, Double T)
        {
            // -2.314*(N1*(1+205/N2))^N3
            return (-2.314 * Math.Pow((Gray.N1(P, T) * (1 + 205 / Gray.N2(P, T))),Gray.N3(P, T)));
        }

        public static double input_liquid_fric(Double P, Double T)
        {
            // Ql/(Ql+(0.1781075952*Qg)*Bg
            double Ql = Inputs.liquid_rate;
            double Qg = Inputs.gas_production_rate;
            double Bg = PVT.fvfg(P, T);
            return Ql/(Ql + (0.1781075952*Qg)*Bg);
        }

        public static double liquid_hold_up(Double P, Double T)
        {
            //=1-((1-CL)*(1-EXP(f1)))
            double cl = input_liquid_fric(P, T);
            double f1 = F1(P, T);
            return 1 - (1 - cl)*(1 - Math.Exp(f1));
        }

        public static double Knode(Double P, Double T)
        {
            double ift = Gray.modified_liquid_ift(P, T);
            double rho_ns = PVT.NoSlip_density(P, T);
            double Vm = TwoPhaseVariables.mixture_velocity(P, T);
            return 28.5*ift/(rho_ns*(Math.Pow(Vm, 2)));
        }

        public static double Ke(Double P, Double T)
        {
            double Rv = Gray.Rv(P, T);
            double k = Inputs.absolute_roughness;
            double Knode = Gray.Knode(P, T);
            double Ke = 0;
            if ( Rv >= .007 )
            {
                Ke = Knode;
            }
            else
            {
                Ke = k + Rv*((Knode - k)/.007);
            }
            return Ke;
        }

        public static double hydrostatic_dp(Double P, Double T)
        {
            // =1/144*(AO22)
            return TwoPhaseVariables.two_phase_density(P, T)/(144);
        }
    }
}
