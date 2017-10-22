using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiphase
{
    class PVT
    {
        //Gas calculations
        public static double z_factor(double P, double T)
        {
            double CO2_fraction_H2S_fraction = Inputs.CO2_fraction + Inputs.H2S_fraction;
            double pseudo_critical_pressure = 709.6 - (58.7 * Inputs.gas_gravity);
            double pseudo_critical_temperature = 170.5 + (307.3 * Inputs.gas_gravity);
            double e = (120 * (Math.Pow(CO2_fraction_H2S_fraction, 0.9) - Math.Pow(CO2_fraction_H2S_fraction, 1.6))) + (15 * (Math.Pow(Inputs.H2S_fraction, 0.5) - Math.Pow(Inputs.H2S_fraction, 4)));
            double pseudo_modified_temperature = pseudo_critical_temperature - e;
            double pseudo_modified_pressure = pseudo_critical_pressure * (pseudo_modified_temperature / (pseudo_critical_temperature + (Inputs.H2S_fraction * (1 - Inputs.H2S_fraction) * e)));
            double pseudo_reduced_pressure = P / pseudo_modified_pressure;
            double pseudo_reduced_temperature = (T + 460) / pseudo_modified_temperature;
            return Math.Round(1 - ((3.52 * pseudo_reduced_pressure) / (Math.Pow(10, (0.9813 * pseudo_reduced_temperature)))) + ((0.274 * Math.Pow(pseudo_reduced_pressure, 2)) / (Math.Pow(10, (0.8157 * pseudo_reduced_temperature)))), 3);
        }

        public static double Rs(double P, double T)
        {
            double V = Math.Round((Inputs.gas_gravity * (Math.Pow((((P / 18.2) + 1.4) * (Math.Pow(10, ((0.0125 * Inputs.API) - (0.00091 * T))))), 1.2048))), 1);
            if (V < Inputs.CGR)
            {
                return V;
            }
            else if (V >= Inputs.CGR)
            {
                return Inputs.Rsb;
            }
            else
            {
                return 0;
            }
        }

        //oil calculations
        // gamma gas 
        public static double oil_gravity()
        {
            return Math.Round((141.5 / (131.5 + Inputs.API)), 3);
        }

        //formation volume factor
        public static double fvfo(double P, double T)
        {
            double F = (Rs(P, T) * (Math.Pow((Inputs.gas_gravity / oil_gravity()), 0.5))) + (1.25 * T);
            return Math.Round((0.9759 + (0.00012 * (Math.Pow(F, 1.2)))), 6);
        }

        public static double bubble_point(double T)
        {
            return Math.Round((18.2 * (((Math.Pow((Inputs.Rsb / Inputs.gas_gravity), 0.83)) * (Math.Pow(10, ((0.00091 * T) - (0.0125 * Inputs.API))))) - 1.4)), 0);
        }

        public static double fvfoabp(double P, double T)
        {
            double F = ((Rs(P, T) * (Math.Pow((Inputs.gas_gravity / oil_gravity()), 0.5))) + (1.25 * T));
            double fvfobp = Math.Round((0.9759 + (0.00012 * (Math.Pow(F, 1.2)))), 3);
            double N = 4.1646 * Math.Pow(10, -7) * Math.Pow(Inputs.Rsb, 0.69357) * Math.Pow(Inputs.gas_gravity, 0.1885) * Math.Pow(Inputs.API, 0.3272) * Math.Pow(T, 0.6729);
            return Math.Round((fvfobp * (Math.Pow(Math.E, (-N * ((Math.Pow(P, 0.4094)) - (Math.Pow(bubble_point(T), 0.4094))))))), 5);
        }

        public static double fvfg(Double P, Double T)
        {
            double z_factor = PVT.z_factor(P, T);
            // =0.0283*Z*(T+460)/(P+14.5)
            return 0.0283*z_factor*(T + 460)/(P + 14.5);
        }

        // oil density
        public static double oil_density(double P, double T)
        {
            if (Rs(P, T) < Inputs.CGR)
            {
                return Math.Round((((350 * oil_gravity()) + (0.0764 * Inputs.gas_gravity * Rs(P, T))) / (5.615 * fvfo(P, T))), 2);
            }
            else
            {
                return Math.Round((((350 * oil_gravity()) + (0.0764 * Inputs.gas_gravity * Inputs.Rsb)) / (5.615 * fvfoabp(P, T))), 2);
            }
        }
        //  Density calculations
        public static double gas_density(double P, double T)
        {
            return Math.Round(((2.7 * Inputs.gas_gravity * P) / (z_factor(P, T) * (T + 460))), 3);
        }

        public static double fvfw(double P, double T)
        {
            return Math.Round((1 + (1.21 * Math.Pow(10, -4) * (T - 60)) + (Math.Pow(10, -6) * (Math.Pow((T - 60), 2))) - (3.33 * Math.Pow(10, -6) * P)), 5);
        }

        public static double water_density(double P, double T)
        {
            return Math.Round(((62.5*Inputs.water_gravity)/fvfw(P, T)), 2);
        }

        // liquid density
        public static double liquid_density(double P, double T)
        {
            return Math.Round(((oil_density(P, T) ) + Inputs.WC*(water_density(P, T) - oil_density(P, T))), 3);
        }

        public static double NoSlip_density(double P, double T)
        {
            double gas_friction;
            gas_friction = (0.1781075952*Inputs.gas_production_rate)/((0.1781075952*Inputs.gas_production_rate) + Inputs.liquid_rate);
            return Math.Round(((gas_density(P, T)) + (1-gas_friction) * (liquid_density(P, T) - gas_density(P, T))), 3);
        }

        // viscosity 

        public static double water_viscosity(double P, double T)
        {
            double a = (-4.518 * Math.Pow(10, -2)) + (9.313 * (Math.Pow(10, -7)) * Inputs.water_salinity) - (3.93 * (Math.Pow(10, -12)) * (Math.Pow(Inputs.water_salinity, 2)));
            double b = 70.634 + (9.576 * (Math.Pow(10, -10)) * (Math.Pow(Inputs.water_salinity, 2)));
            double dead_water_viscosity = a + (b / T);
            return Math.Round((dead_water_viscosity * (1 + (3.5 * (Math.Pow(10, -12)) * (Math.Pow(P, 2)) * (T - 40)))), 2);
        }

        public static double oil_viscosity(double P, double T)
        {
            double z = 3.0324 - (0.02023 * Inputs.API);
            double Y = Math.Pow(10, z);
            double x = Y * Math.Pow(T, -1.163);
            double dead_oil_viscosity = Math.Pow(10, x) - 1;
            if (Rs(P, T) < Inputs.CGR)
            {
                double I = Rs(P, T) * ((2.2 * (Math.Pow(10, -7)) * Rs(P, T)) - (7.4 * (Math.Pow(10, -4))));
                double J = 8.62 * (Math.Pow(10, -5)) * Rs(P, T);
                double K = 1.1 * (Math.Pow(10, -3)) * Rs(P, T);
                double L = 3.74 * (Math.Pow(10, -3)) * Rs(P, T);
                double M = (0.68 / (Math.Pow(10, J))) + (0.25 / (Math.Pow(10, K))) + (0.062 / (Math.Pow(10, L)));
                return (Math.Pow(10, I)) * (Math.Pow(dead_oil_viscosity, M));
            }
            else
            {
                double I = 10.715 * (Math.Pow((Inputs.Rsb + 100), -0.515));
                double J = 5.44 * (Math.Pow((Inputs.Rsb + 150), -0.338));
                double Pb_oil_viscosity = I * (Math.Pow(dead_oil_viscosity, J));
                double N = (-3.9 * P * Math.Pow(10, -5)) - 5;
                double M = 2.6 * Math.Pow(P, 1.187) * Math.Pow(10, N);
                return Pb_oil_viscosity * Math.Pow((P / bubble_point(T)), M);
            }
        }
        // water oil ratio

        public static double WOR()
        {
            return Math.Round((Inputs.WC / (1 - Inputs.WC)), 3);
        }

        public static double fo()
        {
            if (!double.IsInfinity(WOR()))
            {
                return Math.Round((1 / (1 + WOR())), 2);
            }
            else
            {
                return 0;
            }
                   
        }

        // water fractional 

        public static double fw()
        {
            if (!double.IsInfinity(WOR()))
            {
                return Math.Round((WOR() / (1 + WOR())), 2);
            }
            else
            {
                return 1;
            }
            
        }

        public static double liquid_viscosity(double P, double T)
        {
            return Math.Round(((oil_viscosity(P, T) * fo()) + (Inputs.water_viscosity * fw())), 2);
        }
    }
}
