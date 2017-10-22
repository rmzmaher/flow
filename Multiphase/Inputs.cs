using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiphase
{
    class Inputs
    {
        //PVT data
        public static double gas_gravity = 0.641;           //ratio
        public static double API = 45;                     //degree
        public static double water_gravity = 1.05;         //ratio
        public static double gas_viscosity = 0.010795;       //cp
        public static double water_viscosity = 0.5;
       // public static double Rs = 15000;                    //SCF/STB
        public static double Rsb = 820;                   //SCF/STB
        public static double WC = 1;                    //ratio
        public static double CO2_fraction = 0;
        public static double H2S_fraction = 0;
        public static double water_salinity = 150000;    //ppm
     //   public static double Overall_heat_coefficient = 8;
     //   public static double Cp_oil = 0.53;
     //   public static double Cp_gas = 0.51;
     //   public static double Cp_water = 1;

        //Well data
        public static double well_head_depth = 0;         //ft
        public static double perforation_depth = 11400;   //ft    
        public static double WHP = 584.3;                   //psi
        public static double Pwf = 5366;                   //psi
        public static double WHT = 52.43;                   //°F
        public static double formation_Temperature = 126.43;   //°F
        public static double well_inner_diameter = 1.9922232; //Unit: in
        public static double absolute_roughness = 0.0002; //Unit: in
        //"down-up" "up-down"
        
        //flow rate data
        public static double GLR = 1000000;                          //SCF/STB
        public static double CGR = 0 ;
        public static double oil_rate = 0;                //STB/day
        public static double water_rate = 9.0130943;              //STB/day
        public static double liquid_rate = 9.0130943;  //STB/day
        public static double gas_production_rate = 9013094.3;  // scf/d
        public static double oil_interficial_tension = 0;       //dyne/cm
        public static double water_interficial_tension = 30;     //dyne/cm

    }
}
