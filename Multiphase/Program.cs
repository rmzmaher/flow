using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiphase
{
    class Program
    {
        static void Main(string[] args)
        {
            double P = 584.3;
            double T = 52.43;
            double Vsl;
            double val2;
            double Vsg;
            double val4;
           // TwoPhaseVariables ob = new TwoPhaseVariables();
            Gray pipe1 = new Gray();
            Vsl = TwoPhaseVariables.superficial_liquid_velocity(P, T);
            val2 = TwoPhaseVariables.area();
            Vsg = TwoPhaseVariables.superficial_gas_velocity(P, T);
            val4 = TwoPhaseVariables.mixture_velocity(P, T);
            Console.WriteLine(PVT.gas_density(P, T));
            Console.WriteLine(PVT.liquid_density(P, T));
            //Console.WriteLine(pipe1.Rv(Vsl,Vsg));
            Console.WriteLine("Liquid Superfacial: {0} Gas Superfacial {1} : Area: {2} MIx vel : {3}",Vsl,Vsg,val2,val4);
            double z = PVT.z_factor(P, T);
            Console.WriteLine("Z= {0}",z);
            Console.WriteLine("rho ns{0}",PVT.NoSlip_density(P,T));
            Console.WriteLine("n1= {0}", Gray.N1(P,T));
            double n2 = Gray.N2(P, T);
            double rv = Gray.Rv(P, T);
            double n3 = Gray.N3(P, T);
            double f1 = Gray.F1(P, T);
            double CL = Gray.input_liquid_fric(P, T);
            double El = Gray.liquid_hold_up(P, T);
            double rho_mix = TwoPhaseVariables.two_phase_density(P, T);
            Console.WriteLine("N2= {0}// R2={1} // N3={2} // f1= {3} // CL= {4} // El = {5} // rho_mix= {6}",n2,rv,n3,f1,CL,El,rho_mix);
            double Knode = Gray.Knode(P, T);
            double dp_head = Gray.hydrostatic_dp(P, T);
            double visco = PVT.liquid_viscosity(P, T);
            double oil = PVT.oil_viscosity(P, T);
            double gas = Inputs.gas_viscosity;
            double water = PVT.water_viscosity(P, T);
            double vis = FrictionPressureLoss.mixture_viscosity(P, T);
            Console.WriteLine("Knode= {0} dp_head = {1} // liquid viscosity = {2}//oilvisc={3}// visc{4}",Knode,dp_head,visco,oil,vis);
            Console.Read();
        }
    }
}
