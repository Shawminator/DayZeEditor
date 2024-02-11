using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    static class ExpansionLootDefaults
    {
        static void Weapons_Civilian(BindingList<ExpansionLoot> Loot)
        {
            //! General Variants
            BindingList<ExpansionLootVariant> battery = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Battery9V") };

            BindingList<ExpansionLootVariant> att_UniversalLight = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("UniversalLight" )};

            BindingList<ExpansionLootVariant> att_TLRLight = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("TLRLight" )};
            BindingList<ExpansionLootVariant> att_PistolSuppressor = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PistolSuppressor") };
            BindingList<ExpansionLootVariant> att_TLRLightAndPistolSuppressor = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("TLRLight"), new ExpansionLootVariant("PistolSuppressor") };

            BindingList<ExpansionLootVariant> att_PUScopeOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PUScopeOptic") };
            BindingList<ExpansionLootVariant> att_HuntingOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("HuntingOptic") };

            BindingList<ExpansionLootVariant> att_PSO1Optic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PSO1Optic") };
            BindingList<ExpansionLootVariant> att_PSO11Optic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PSO11Optic") };
            BindingList<ExpansionLootVariant> att_KazuarOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("KazuarOptic") };
            BindingList<ExpansionLootVariant> att_KobraOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("KobraOptic") };
            BindingList<ExpansionLootVariant> att_KashtanOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("KashtanOptic") };

            BindingList<ExpansionLootVariant> att_ReflexOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ReflexOptic") };
            BindingList<ExpansionLootVariant> att_ACOGOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ACOGOptic") };
            BindingList<ExpansionLootVariant> att_ACOGOptic_6x = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ACOGOptic_6x") };
            BindingList<ExpansionLootVariant> att_M68Optic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("M68Optic") };
            BindingList<ExpansionLootVariant> att_M4_T3NRDSOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("M4_T3NRDSOptic") };
            BindingList<ExpansionLootVariant> att_StarlightOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("StarlightOptic") };

            BindingList<ExpansionLootVariant> att_AKPlastic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_PlasticBttstck"), new ExpansionLootVariant("AK_PlasticHndgrd") };
            BindingList<ExpansionLootVariant> att_AK74 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK74_WoodBttstck"), new ExpansionLootVariant("AK74_Hndgrd") };
            BindingList<ExpansionLootVariant> att_AKM = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_WoodBttstck"), new ExpansionLootVariant("AK_WoodHndgrd") };
            //! End of General Variants

            BindingList<ExpansionLootVariant> ak_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_FoldingBttstck"), new ExpansionLootVariant("AK74_Hndgrd") };
            BindingList<ExpansionLootVariant> ak_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_FoldingBttstck"), new ExpansionLootVariant("AK74_Hndgrd"), new ExpansionLootVariant("AK_Bayonet") };
            BindingList<ExpansionLootVariant> ak_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("KobraOptic"), new ExpansionLootVariant("AK_WoodHndgrd"), new ExpansionLootVariant("AK74_WoodBttstck") };
            BindingList<ExpansionLootVariant> ak_4 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_FoldingBttstck"), new ExpansionLootVariant("AK_RailHndgrd") };
            BindingList<ExpansionLootVariant> ak_5 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_WoodBttstck"), new ExpansionLootVariant("AK_WoodHndgrd"), new ExpansionLootVariant("KobraOptic") };
            BindingList<ExpansionLootVariant> ak_6 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_PlasticBttstck"), new ExpansionLootVariant("AK_RailHndgrd") };
            BindingList<ExpansionLootVariant> ak_7 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_PlasticBttstck"), new ExpansionLootVariant("AK_RailHndgrd"), new ExpansionLootVariant("UniversalLight") };

            BindingList<ExpansionLootVariant> ak74u_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK74_WoodBttstck") };
            BindingList<ExpansionLootVariant> ak74u_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AKS74U_Bttstck") };

            BindingList<ExpansionLootVariant> expansionm16_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ExpansionEXPS3HoloOptic"), new ExpansionLootVariant("UniversalLight") };
            BindingList<ExpansionLootVariant> expansionm16_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("M68Optic"), new ExpansionLootVariant("UniversalLight") };
            BindingList<ExpansionLootVariant> expansionm16_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ReflexOptic"), new ExpansionLootVariant("UniversalLight") };

            BindingList<ExpansionLootVariant> m4a1_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("M4_CarryHandleOptic"), new ExpansionLootVariant("M4_OEBttstck"), new ExpansionLootVariant("M4_PlasticHndgrd") };
            BindingList<ExpansionLootVariant> m4a1_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("BUISOptic"), new ExpansionLootVariant("M4_CQBBttstck"), new ExpansionLootVariant("M4_RisHndgrd") };
            BindingList<ExpansionLootVariant> m4a1_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ACOGOptic"), new ExpansionLootVariant("M4_MPBttstck"), new ExpansionLootVariant("M4_MPHndgrd") };
            BindingList<ExpansionLootVariant> m4a1_4 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("BUISOptic"), new ExpansionLootVariant("M4_CQBBttstck"), new ExpansionLootVariant("M4_RisHndgrd"), new ExpansionLootVariant("UniversalLight") };

            BindingList<ExpansionLootVariant> fal_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Fal_OeBttstck") };
            BindingList<ExpansionLootVariant> fal_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Fal_FoldingBttstck") };
            BindingList<ExpansionLootVariant> fal_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Fal_OeBttstck"), new ExpansionLootVariant("ACOGOptic") };
            BindingList<ExpansionLootVariant> fal_4 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Fal_FoldingBttstck"), new ExpansionLootVariant("ReflexOptic") };

            BindingList<ExpansionLootVariant> saiga_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Saiga_Bttstck"), new ExpansionLootVariant("KobraOptic") };
            BindingList<ExpansionLootVariant> saiga_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Saiga_Bttstck") };

            BindingList<ExpansionLootVariant> sks_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("SKS_Bayonet") };

            BindingList<ExpansionLootVariant> mosin_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Mosin_Bayonet") };
            BindingList<ExpansionLootVariant> mosin_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Mosin_Compensator") };
            BindingList<ExpansionLootVariant> mosin_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Mosin_Compensator"), new ExpansionLootVariant("PUScopeOptic") };

            BindingList<ExpansionLootVariant> winchester70_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("HuntingOptic") };

            BindingList<ExpansionLootVariant> sksVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "SKS", sks_1, (decimal)0.2 ),
                new ExpansionLootVariant( "SKS", null, (decimal)0.6 )
            };

            BindingList<ExpansionLootVariant> umpVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "UMP45", null, (decimal)0.6 )
            };

            BindingList<ExpansionLootVariant> mosinVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "Mosin9130", mosin_3, (decimal)0.25 ),
                new ExpansionLootVariant( "Mosin9130", mosin_2, (decimal)0.25 ),
                new ExpansionLootVariant( "Mosin9130", mosin_1, (decimal)0.25 ),
                new ExpansionLootVariant( "Mosin9130", null, (decimal)0.25 )
            };

            BindingList<ExpansionLootVariant> b95Variants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "B95", null, (decimal)0.666667 )
            };

            BindingList<ExpansionLootVariant> cz527Variants = new BindingList<ExpansionLootVariant>() {
                new ExpansionLootVariant( "CZ527", null, (decimal)0.4 )
            };

            BindingList<ExpansionLootVariant> cz75Variants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "CZ75", att_TLRLightAndPistolSuppressor, (decimal)0.1 ),
                new ExpansionLootVariant( "CZ75", att_PistolSuppressor, (decimal)0.2 ),
                new ExpansionLootVariant( "CZ75", null, (decimal)0.5 )
            };

            BindingList<ExpansionLootVariant> fnxVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "FNX45", att_TLRLightAndPistolSuppressor, (decimal)0.1 ),
                new ExpansionLootVariant( "FNX45", att_PistolSuppressor, (decimal)0.15 ),
                new ExpansionLootVariant( "FNX45", null, (decimal)0.5 )
            };

            BindingList<ExpansionLootVariant> kedrVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "Expansion_Kedr", null, (decimal)0.6 )
            };

            BindingList<ExpansionLootVariant> winchester70Variants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "Winchester70", null, (decimal)0.8 )
            };

            BindingList<ExpansionLootVariant> ak74Variants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "AK74", ak_1, (decimal)0.1 ),
                new ExpansionLootVariant( "AK74", ak_2, (decimal)0.05 ),
                new ExpansionLootVariant( "AK74", ak_3, (decimal)0.05 ),
                new ExpansionLootVariant( "AK74", ak_4, (decimal)0.1 ),
                new ExpansionLootVariant( "AK74", ak_5, (decimal)0.05 ),
                new ExpansionLootVariant( "AK74", ak_6, (decimal)0.05 ),
                new ExpansionLootVariant( "AK74", ak_7, (decimal)0.05 )
            };
            BindingList<ExpansionLootVariant> akmVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "AKM", ak_1, (decimal)0.1 ),
                new ExpansionLootVariant( "AKM", ak_2, (decimal)0.05 ),
                new ExpansionLootVariant( "AKM", ak_3, (decimal)0.05 ),
                new ExpansionLootVariant( "AKM", ak_4, (decimal)0.1 ),
                new ExpansionLootVariant( "AKM", ak_5, (decimal)0.05 ),
                new ExpansionLootVariant( "AKM", ak_6, (decimal)0.05 ),
                new ExpansionLootVariant( "AKM", ak_7, (decimal)0.05 )
            };
            BindingList<ExpansionLootVariant> ak101Variants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "AK101", ak_1, (decimal)0.1 ),
                new ExpansionLootVariant( "AK101", ak_2, (decimal)0.05 ),
                new ExpansionLootVariant( "AK101", ak_3, (decimal)0.05 ),
                new ExpansionLootVariant( "AK101", ak_4, (decimal)0.1 ),
                new ExpansionLootVariant( "AK101", ak_5, (decimal)0.05 ),
                new ExpansionLootVariant( "AK101", ak_6, (decimal)0.05 ),
                new ExpansionLootVariant( "AK101", ak_7, (decimal)0.05 )
            };

            BindingList<ExpansionLootVariant> ak74uVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "AKS74U", ak74u_2, (decimal)0.545454 )
            };

            BindingList<ExpansionLootVariant> expansionm16Variants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "Expansion_M16", expansionm16_1, (decimal)0.1 ),
                new ExpansionLootVariant( "Expansion_M16", expansionm16_2, (decimal)0.05 ),
                new ExpansionLootVariant( "Expansion_M16", expansionm16_3, (decimal)0.05 ),
                new ExpansionLootVariant( "Expansion_M16", att_ACOGOptic_6x, (decimal)0.1 ),
                new ExpansionLootVariant( "Expansion_M16", att_StarlightOptic, (decimal)0.05 ),
                new ExpansionLootVariant( "Expansion_M16", null, (decimal)0.05 ),
                new ExpansionLootVariant( "Expansion_M16", att_UniversalLight, (decimal)0.05 )
            };

            BindingList<ExpansionLootVariant> m4a1Variants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "M4A1", m4a1_2, (decimal)0.220339 ),
                new ExpansionLootVariant( "M4A1", m4a1_3, (decimal)0.237288 ),
                new ExpansionLootVariant( "M4A1", m4a1_4, (decimal)0.305085 )
            };

            BindingList<ExpansionLootVariant> falVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "FAL", fal_2, (decimal)0.40 ),
                new ExpansionLootVariant( "FAL", fal_3, (decimal)0.20 ),
                new ExpansionLootVariant( "FAL", fal_4, (decimal)0.20 )
            };

            BindingList<ExpansionLootVariant> scoutVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "Scout", null, (decimal)0.642857 ),
                new ExpansionLootVariant( "Scout", att_ReflexOptic, (decimal)0.2 ),
                new ExpansionLootVariant( "Scout", att_ACOGOptic, (decimal)0.1 ),
                new ExpansionLootVariant( "Scout", att_M68Optic, (decimal)0.15 ),
                new ExpansionLootVariant( "Scout", att_M4_T3NRDSOptic, (decimal)0.1 )
            };

            BindingList<ExpansionLootVariant> expansionAWMVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "Expansion_AWM", null, (decimal)0.642857 ),
                new ExpansionLootVariant( "Expansion_AWM", att_ReflexOptic, (decimal)0.2 ),
                new ExpansionLootVariant( "Expansion_AWM", att_ACOGOptic, (decimal)0.1 ),
                new ExpansionLootVariant( "Expansion_AWM", att_M68Optic, (decimal)0.15 ),
                new ExpansionLootVariant( "Expansion_AWM", att_M4_T3NRDSOptic, (decimal)0.1 )
            };

            BindingList<ExpansionLootVariant> vssVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "VSS", null, (decimal)0.642857 ),
                new ExpansionLootVariant( "VSS", att_KazuarOptic, (decimal)0.082857 ),
            };

            BindingList<ExpansionLootVariant> asvalVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "ASVAL", null, (decimal)0.642857 ),
                new ExpansionLootVariant( "ASVAL", att_ReflexOptic, (decimal)0.2 ),
                new ExpansionLootVariant( "ASVAL", att_ACOGOptic, (decimal)0.1 ),
                new ExpansionLootVariant( "ASVAL", att_M68Optic, (decimal)0.15 ),
                new ExpansionLootVariant( "ASVAL", att_M4_T3NRDSOptic, (decimal)0.1 )
            };

            BindingList<ExpansionLootVariant> svdVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "SVD", null, (decimal)0.642857 ),
                new ExpansionLootVariant( "SVD", att_KazuarOptic, (decimal)0.082857 )
            };

            BindingList<ExpansionLootVariant> saigaVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "Saiga", saiga_2, (decimal)0.545454 )
            };

            Loot.Add(new ExpansionLoot("SKS", att_PUScopeOptic, (decimal)0.5, -1, sksVariants));
            Loot.Add(new ExpansionLoot("UMP45", att_UniversalLight, (decimal)0.25, -1, umpVariants));
            Loot.Add(new ExpansionLoot("Mosin9130", att_PUScopeOptic, (decimal)0.4, -1, mosinVariants));
            Loot.Add(new ExpansionLoot("B95", att_HuntingOptic, (decimal)0.3, -1, b95Variants));
            Loot.Add(new ExpansionLoot("CZ527", att_HuntingOptic, (decimal)0.5, -1, cz527Variants));
            Loot.Add(new ExpansionLoot("CZ75", att_TLRLight, (decimal)0.2, -1, cz75Variants));
            Loot.Add(new ExpansionLoot("FNX45", att_TLRLight, (decimal)0.4, -1, fnxVariants));
            Loot.Add(new ExpansionLoot("Glock19", att_TLRLight, (decimal)0.4, -1, fnxVariants));
            Loot.Add(new ExpansionLoot("MKII", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Magnum", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("Deagle", null, (decimal)0.125));
            Loot.Add(new ExpansionLoot("P1", null, (decimal)0.175));
            Loot.Add(new ExpansionLoot("Longhorn", null, (decimal)0.125));
            Loot.Add(new ExpansionLoot("Mp133Shotgun", null, (decimal)0.8));
            Loot.Add(new ExpansionLoot("Winchester70", winchester70_1, (decimal)0.5, -1, winchester70Variants));
            Loot.Add(new ExpansionLoot("AK101", att_AKPlastic, (decimal)0.12, -1, ak101Variants));
            Loot.Add(new ExpansionLoot("AK74", att_AK74, (decimal)0.495, -1, ak74Variants));
            Loot.Add(new ExpansionLoot("AKS74U", ak74u_1, (decimal)0.55, -1, ak74uVariants));
            Loot.Add(new ExpansionLoot("AKM", att_AKM, (decimal)0.6, -1, akmVariants));
            Loot.Add(new ExpansionLoot("M4A1", m4a1_1, (decimal)0.375, -1, m4a1Variants));
            Loot.Add(new ExpansionLoot("M16A2", null, (decimal)0.35));
            Loot.Add(new ExpansionLoot("FAMAS", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("Aug", null, (decimal)0.3));

            Loot.Add(new ExpansionLoot("RGD5Grenade", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("M67Grenade", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_Red", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_Green", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_Yellow", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_Purple", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_White", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("RDG2SmokeGrenade_Black", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("RDG2SmokeGrenade_White", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("LandMineTrap", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("Mag_AK101_30Rnd", null, (decimal)0.10));
            Loot.Add(new ExpansionLoot("Mag_AK74_30Rnd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Mag_AKM_30Rnd", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("Mag_AKM_Palm30Rnd", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("Mag_CMAG_20Rnd", null, (decimal)0.3));
            Loot.Add(new ExpansionLoot("Mag_CMAG_30Rnd", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("Mag_STANAG_30Rnd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Mag_CZ527_5rnd", null, (decimal)0.9));
            Loot.Add(new ExpansionLoot("Mag_Deagle_9Rnd", null, 1));
            Loot.Add(new ExpansionLoot("Mag_FNX45_15Rnd", null, 1));
            Loot.Add(new ExpansionLoot("Mag_UMP_25Rnd", null, (decimal)0.5));

        }
        static void Weapons_Military(BindingList<ExpansionLoot> Loot)
        {
            //! General Variants
            BindingList<ExpansionLootVariant> battery = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Battery9V") };

            BindingList<ExpansionLootVariant> att_UniversalLight = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("UniversalLight") };

            BindingList<ExpansionLootVariant> att_TLRLight = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("TLRLight") };
            BindingList<ExpansionLootVariant> att_PistolSuppressor = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PistolSuppressor") };
            BindingList<ExpansionLootVariant> att_TLRLightAndPistolSuppressor = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("TLRLight"), new ExpansionLootVariant("PistolSuppressor") };

            BindingList<ExpansionLootVariant> att_PUScopeOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PUScopeOptic") };
            BindingList<ExpansionLootVariant> att_HuntingOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("HuntingOptic") };

            BindingList<ExpansionLootVariant> att_PSO1Optic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PSO1Optic") };
            BindingList<ExpansionLootVariant> att_PSO11Optic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PSO11Optic") };
            BindingList<ExpansionLootVariant> att_KazuarOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("KazuarOptic") };
            BindingList<ExpansionLootVariant> att_KobraOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("KobraOptic") };
            BindingList<ExpansionLootVariant> att_KashtanOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("KashtanOptic") };

            BindingList<ExpansionLootVariant> att_ReflexOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ReflexOptic") };
            BindingList<ExpansionLootVariant> att_ACOGOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ACOGOptic") };
            BindingList<ExpansionLootVariant> att_ACOGOptic_6x = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ACOGOptic_6x") };
            BindingList<ExpansionLootVariant> att_M68Optic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("M68Optic") };
            BindingList<ExpansionLootVariant> att_M4_T3NRDSOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("M4_T3NRDSOptic") };
            BindingList<ExpansionLootVariant> att_StarlightOptic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("StarlightOptic") };

            BindingList<ExpansionLootVariant> att_AKPlastic = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_PlasticBttstck"), new ExpansionLootVariant("AK_PlasticHndgrd") };
            BindingList<ExpansionLootVariant> att_AK74 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK74_WoodBttstck"), new ExpansionLootVariant("AK74_Hndgrd") };
            BindingList<ExpansionLootVariant> att_AKM = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_WoodBttstck"), new ExpansionLootVariant("AK_WoodHndgrd") };
            //! End of General Variants

            BindingList<ExpansionLootVariant> ak_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_FoldingBttstck"), new ExpansionLootVariant("AK74_Hndgrd") };
            BindingList<ExpansionLootVariant> ak_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_FoldingBttstck"), new ExpansionLootVariant("AK74_Hndgrd"), new ExpansionLootVariant("AK_Bayonet") };
            BindingList<ExpansionLootVariant> ak_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("KobraOptic"), new ExpansionLootVariant("AK_WoodHndgrd"), new ExpansionLootVariant("AK74_WoodBttstck") };
            BindingList<ExpansionLootVariant> ak_4 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_FoldingBttstck"), new ExpansionLootVariant("AK_RailHndgrd") };
            BindingList<ExpansionLootVariant> ak_5 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_WoodBttstck"), new ExpansionLootVariant("AK_WoodHndgrd"), new ExpansionLootVariant("KobraOptic") };
            BindingList<ExpansionLootVariant> ak_6 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_PlasticBttstck"), new ExpansionLootVariant("AK_RailHndgrd") };
            BindingList<ExpansionLootVariant> ak_7 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK_PlasticBttstck"), new ExpansionLootVariant("AK_RailHndgrd"), new ExpansionLootVariant("UniversalLight") };

            BindingList<ExpansionLootVariant> ak74u_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AK74_WoodBttstck") };
            BindingList<ExpansionLootVariant> ak74u_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("AKS74U_Bttstck") };

            BindingList<ExpansionLootVariant> expansionm16_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ExpansionEXPS3HoloOptic"), new ExpansionLootVariant("UniversalLight") };
            BindingList<ExpansionLootVariant> expansionm16_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("M68Optic"), new ExpansionLootVariant("UniversalLight") };
            BindingList<ExpansionLootVariant> expansionm16_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ReflexOptic"), new ExpansionLootVariant("UniversalLight") };

            BindingList<ExpansionLootVariant> m4a1_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("M4_CarryHandleOptic"), new ExpansionLootVariant("M4_OEBttstck"), new ExpansionLootVariant("M4_PlasticHndgrd") };
            BindingList<ExpansionLootVariant> m4a1_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("BUISOptic"), new ExpansionLootVariant("M4_CQBBttstck"), new ExpansionLootVariant("M4_RisHndgrd") };
            BindingList<ExpansionLootVariant> m4a1_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("ACOGOptic"), new ExpansionLootVariant("M4_MPBttstck"), new ExpansionLootVariant("M4_MPHndgrd") };
            BindingList<ExpansionLootVariant> m4a1_4 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("BUISOptic"), new ExpansionLootVariant("M4_CQBBttstck"), new ExpansionLootVariant("M4_RisHndgrd"), new ExpansionLootVariant("UniversalLight") };

            BindingList<ExpansionLootVariant> fal_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Fal_OeBttstck") };
            BindingList<ExpansionLootVariant> fal_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Fal_FoldingBttstck") };
            BindingList<ExpansionLootVariant> fal_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Fal_OeBttstck"), new ExpansionLootVariant("ACOGOptic") };
            BindingList<ExpansionLootVariant> fal_4 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Fal_FoldingBttstck"), new ExpansionLootVariant("ReflexOptic") };

            BindingList<ExpansionLootVariant> saiga_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Saiga_Bttstck"), new ExpansionLootVariant("KobraOptic") };
            BindingList<ExpansionLootVariant> saiga_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Saiga_Bttstck") };

            BindingList<ExpansionLootVariant> sks_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("SKS_Bayonet") };

            BindingList<ExpansionLootVariant> mosin_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Mosin_Bayonet") };
            BindingList<ExpansionLootVariant> mosin_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Mosin_Compensator") };
            BindingList<ExpansionLootVariant> mosin_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Mosin_Compensator"), new ExpansionLootVariant("PUScopeOptic") };

            BindingList<ExpansionLootVariant> winchester70_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("HuntingOptic") };

            BindingList<ExpansionLootVariant> sksVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "SKS", sks_1, (decimal)0.2 ),
            new ExpansionLootVariant( "SKS", null, (decimal)0.6 )
        };

            BindingList<ExpansionLootVariant> umpVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "UMP45", null, (decimal)0.6 )
        };

            BindingList<ExpansionLootVariant> mosinVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "Mosin9130", mosin_3, (decimal)0.25 ),
            new ExpansionLootVariant( "Mosin9130", mosin_2, (decimal)0.25 ),
            new ExpansionLootVariant( "Mosin9130", mosin_1, (decimal)0.25 ),
            new ExpansionLootVariant( "Mosin9130", null, (decimal)0.25 )
        };

            BindingList<ExpansionLootVariant> b95Variants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "B95", null, (decimal)0.666667 )
        };

            BindingList<ExpansionLootVariant> cz527Variants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "CZ527", null, (decimal)0.4 )
        };

            BindingList<ExpansionLootVariant> cz75Variants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "CZ75", att_TLRLightAndPistolSuppressor, (decimal)0.1 ),
            new ExpansionLootVariant( "CZ75", att_PistolSuppressor, (decimal)0.2 ),
            new ExpansionLootVariant( "CZ75", null, (decimal)0.5 )
        };

            BindingList<ExpansionLootVariant> fnxVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "FNX45", att_TLRLightAndPistolSuppressor, (decimal)0.1 ),
            new ExpansionLootVariant( "FNX45", att_PistolSuppressor, (decimal)0.15 ),
            new ExpansionLootVariant( "FNX45", null, (decimal)0.5 )
        };

            BindingList<ExpansionLootVariant> kedrVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "Expansion_Kedr", null, (decimal)0.6 )
        };

            BindingList<ExpansionLootVariant> winchester70Variants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "Winchester70", null, (decimal)0.8 )
        };

            BindingList<ExpansionLootVariant> ak74Variants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "AK74", ak_1, (decimal)0.1 ),
            new ExpansionLootVariant( "AK74", ak_2, (decimal)0.05 ),
            new ExpansionLootVariant( "AK74", ak_3, (decimal)0.05 ),
            new ExpansionLootVariant( "AK74", ak_4, (decimal)0.1 ),
            new ExpansionLootVariant( "AK74", ak_5, (decimal)0.05 ),
            new ExpansionLootVariant( "AK74", ak_6, (decimal)0.05 ),
            new ExpansionLootVariant( "AK74", ak_7, (decimal)0.05 )
        };
            BindingList<ExpansionLootVariant> akmVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "AKM", ak_1, (decimal)0.1 ),
            new ExpansionLootVariant( "AKM", ak_2, (decimal)0.05 ),
            new ExpansionLootVariant( "AKM", ak_3, (decimal)0.05 ),
            new ExpansionLootVariant( "AKM", ak_4, (decimal)0.1 ),
            new ExpansionLootVariant( "AKM", ak_5, (decimal)0.05 ),
            new ExpansionLootVariant( "AKM", ak_6, (decimal)0.05 ),
            new ExpansionLootVariant( "AKM", ak_7, (decimal)0.05 )
        };
            BindingList<ExpansionLootVariant> ak101Variants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "AK101", ak_1, (decimal)0.1 ),
            new ExpansionLootVariant( "AK101", ak_2, (decimal)0.05 ),
            new ExpansionLootVariant( "AK101", ak_3, (decimal)0.05 ),
            new ExpansionLootVariant( "AK101", ak_4, (decimal)0.1 ),
            new ExpansionLootVariant( "AK101", ak_5, (decimal)0.05 ),
            new ExpansionLootVariant( "AK101", ak_6, (decimal)0.05 ),
            new ExpansionLootVariant( "AK101", ak_7, (decimal)0.05 )
        };

            BindingList<ExpansionLootVariant> ak74uVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "AKS74U", ak74u_2, (decimal)0.545454 )
        };

            BindingList<ExpansionLootVariant> expansionm16Variants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "Expansion_M16", expansionm16_1, (decimal)0.1 ),
            new ExpansionLootVariant( "Expansion_M16", expansionm16_2, (decimal)0.05 ),
            new ExpansionLootVariant( "Expansion_M16", expansionm16_3, (decimal)0.05 ),
            new ExpansionLootVariant( "Expansion_M16", att_ACOGOptic_6x, (decimal)0.1 ),
            new ExpansionLootVariant( "Expansion_M16", att_StarlightOptic, (decimal)0.05 ),
            new ExpansionLootVariant( "Expansion_M16", null, (decimal)0.05 ),
            new ExpansionLootVariant( "Expansion_M16", att_UniversalLight, (decimal)0.05 )
        };

            BindingList<ExpansionLootVariant> m4a1Variants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "M4A1", m4a1_2, (decimal)0.220339 ),
            new ExpansionLootVariant( "M4A1", m4a1_3, (decimal)0.237288 ),
            new ExpansionLootVariant( "M4A1", m4a1_4, (decimal)0.305085 )
        };

            BindingList<ExpansionLootVariant> falVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "FAL", fal_2, (decimal)0.40 ),
            new ExpansionLootVariant( "FAL", fal_3, (decimal)0.20 ),
            new ExpansionLootVariant( "FAL", fal_4, (decimal)0.20 )
        };

            BindingList<ExpansionLootVariant> scoutVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "Scout", null, (decimal)0.642857 ),
            new ExpansionLootVariant( "Scout", att_ReflexOptic, (decimal)0.2 ),
            new ExpansionLootVariant( "Scout", att_ACOGOptic, (decimal)0.1 ),
            new ExpansionLootVariant( "Scout", att_M68Optic, (decimal)0.15 ),
            new ExpansionLootVariant( "Scout", att_M4_T3NRDSOptic, (decimal)0.1 )
        };

            BindingList<ExpansionLootVariant> expansionAWMVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "Expansion_AWM", null, (decimal)0.642857 ),
            new ExpansionLootVariant( "Expansion_AWM", att_ReflexOptic, (decimal)0.2 ),
            new ExpansionLootVariant( "Expansion_AWM", att_ACOGOptic, (decimal)0.1 ),
            new ExpansionLootVariant( "Expansion_AWM", att_M68Optic, (decimal)0.15 ),
            new ExpansionLootVariant( "Expansion_AWM", att_M4_T3NRDSOptic, (decimal)0.1 )
        };

            BindingList<ExpansionLootVariant> vssVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "VSS", null, (decimal)0.642857 ),
            new ExpansionLootVariant( "VSS", att_KazuarOptic, (decimal)0.082857 )
        };

            BindingList<ExpansionLootVariant> asvalVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "ASVAL", null, (decimal)0.642857 ),
            new ExpansionLootVariant( "ASVAL", att_ReflexOptic, (decimal)0.2 ),
            new ExpansionLootVariant( "ASVAL", att_ACOGOptic, (decimal)0.1 ),
            new ExpansionLootVariant( "ASVAL", att_M68Optic, (decimal)0.15 ),
            new ExpansionLootVariant( "ASVAL", att_M4_T3NRDSOptic, (decimal)0.1 )
        };

            BindingList<ExpansionLootVariant> svdVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "SVD", null, (decimal)0.642857 ),
            new ExpansionLootVariant( "SVD", att_KazuarOptic, (decimal)0.082857 )
        };

            BindingList<ExpansionLootVariant> saigaVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "Saiga", saiga_2, (decimal)0.545454 )
        };

            Loot.Add(new ExpansionLoot("SKS", att_PUScopeOptic, (decimal)0.5, -1, sksVariants));
            Loot.Add(new ExpansionLoot("UMP45", att_UniversalLight, (decimal)0.25, -1, umpVariants));
            Loot.Add(new ExpansionLoot("Mosin9130", att_PUScopeOptic, (decimal)0.4, -1, mosinVariants));
            Loot.Add(new ExpansionLoot("B95", att_HuntingOptic, (decimal)0.3, -1, b95Variants));
            Loot.Add(new ExpansionLoot("CZ527", att_HuntingOptic, (decimal)0.5, -1, cz527Variants));
            Loot.Add(new ExpansionLoot("CZ75", att_TLRLight, (decimal)0.2, -1, cz75Variants));
            Loot.Add(new ExpansionLoot("FNX45", att_TLRLight, (decimal)0.4, -1, fnxVariants));
            Loot.Add(new ExpansionLoot("Glock19", att_TLRLight, (decimal)0.4, -1, fnxVariants));
            Loot.Add(new ExpansionLoot("MKII", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Magnum", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("Deagle", null, (decimal)0.125));
            Loot.Add(new ExpansionLoot("P1", null, (decimal)0.175));
            Loot.Add(new ExpansionLoot("Longhorn", null, (decimal)0.125));
            Loot.Add(new ExpansionLoot("Mp133Shotgun", null, (decimal)0.8));
            Loot.Add(new ExpansionLoot("Winchester70", winchester70_1, (decimal)0.5, -1, winchester70Variants));
            Loot.Add(new ExpansionLoot("AK101", att_AKPlastic, (decimal)0.12, -1, ak101Variants));
            Loot.Add(new ExpansionLoot("AK74", att_AK74, (decimal)0.495, -1, ak74Variants));
            Loot.Add(new ExpansionLoot("AKS74U", ak74u_1, (decimal)0.55, -1, ak74uVariants));
            Loot.Add(new ExpansionLoot("AKM", att_AKM, (decimal)1.3, -1, akmVariants));
            Loot.Add(new ExpansionLoot("M4A1", m4a1_1, (decimal)0.59, -1, m4a1Variants));
            Loot.Add(new ExpansionLoot("FAL", fal_1, (decimal)0.24, -1, falVariants));
            Loot.Add(new ExpansionLoot("SVD", att_PSO1Optic, (decimal)0.28, -1, svdVariants));
            Loot.Add(new ExpansionLoot("VSS", att_PSO11Optic, (decimal)0.10, -1, vssVariants));
            Loot.Add(new ExpansionLoot("ASVAL", att_ACOGOptic_6x, (decimal)0.05, -1, asvalVariants));
            Loot.Add(new ExpansionLoot("Saiga", saiga_1, (decimal)0.22, -1, saigaVariants));
            Loot.Add(new ExpansionLoot("M79", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M16A2", null, (decimal)0.60));
            Loot.Add(new ExpansionLoot("FAMAS", null, (decimal)0.55));
            Loot.Add(new ExpansionLoot("Aug", null, (decimal)0.60));
            Loot.Add(new ExpansionLoot("AugShort", null, (decimal)0.45));
            Loot.Add(new ExpansionLoot("Scout", att_ACOGOptic_6x, (decimal)0.05, -1, scoutVariants));

            Loot.Add(new ExpansionLoot("RGD5Grenade", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("M67Grenade", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_Red", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_Green", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_Yellow", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_Purple", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M18SmokeGrenade_White", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("RDG2SmokeGrenade_Black", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("RDG2SmokeGrenade_White", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("LandMineTrap", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("Mag_AK101_30Rnd", null, (decimal)0.10));
            Loot.Add(new ExpansionLoot("Mag_AK74_30Rnd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Mag_AKM_30Rnd", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("Mag_AKM_Drum75Rnd", null, (decimal)0.06));
            Loot.Add(new ExpansionLoot("Mag_AKM_Palm30Rnd", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("Mag_CMAG_20Rnd", null, (decimal)0.3));
            Loot.Add(new ExpansionLoot("Mag_CMAG_30Rnd", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("Mag_CMAG_40Rnd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Mag_FAL_20Rnd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Mag_STANAGCoupled_30Rnd", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("Mag_STANAG_30Rnd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Mag_SVD_10Rnd", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("Mag_Saiga_5Rnd", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("Mag_Saiga_8Rnd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Mag_Saiga_Drum20Rnd", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("Mag_CZ527_5rnd", null, (decimal)0.9));
            Loot.Add(new ExpansionLoot("Mag_Deagle_9Rnd", null, 1));
            Loot.Add(new ExpansionLoot("Mag_FNX45_15Rnd", null, 1));
            Loot.Add(new ExpansionLoot("Mag_UMP_25Rnd", null, (decimal)0.5));
        }
        static void Ammo_Civilian(BindingList<ExpansionLoot> Loot)
        {
            Loot.Add(new ExpansionLoot("AmmoBox_9x39_20Rnd", null, (decimal)0.5));
            Loot.Add(new ExpansionLoot("AmmoBox_9x19_25Rnd", null, 1));
            Loot.Add(new ExpansionLoot("AmmoBox_762x39_20Rnd", null, 1));
            Loot.Add(new ExpansionLoot("AmmoBox_45ACP_25Rnd", null, 1));
            Loot.Add(new ExpansionLoot("AmmoBox_308Win_20Rnd", null, 1));
            Loot.Add(new ExpansionLoot("AmmoBox_12gaSlug_10Rnd", null, 1));
            Loot.Add(new ExpansionLoot("Ammo_12gaPellets", null, 1));

            Loot.Add(new ExpansionLoot("Ammo_9x39", null, (decimal)0.5));
            Loot.Add(new ExpansionLoot("Ammo_762x39", null, 1));
            Loot.Add(new ExpansionLoot("Ammo_45ACP", null, 1));
            Loot.Add(new ExpansionLoot("Ammo_308Win", null, 1));
            Loot.Add(new ExpansionLoot("Ammo_12gaSlug", null, 1));
            Loot.Add(new ExpansionLoot("Ammo_12gaRubberSlug", null, 1));
            Loot.Add(new ExpansionLoot("Ammo_12gaPellets", null, (decimal)0.5));
        }
        static void Ammo_Military(BindingList<ExpansionLoot> Loot)
        {
            Loot.Add(new ExpansionLoot("AmmoBox_762x54Tracer_20Rnd", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("AmmoBox_762x54_20Rnd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("AmmoBox_762x39Tracer_20Rnd", null, (decimal)0.06));
            Loot.Add(new ExpansionLoot("AmmoBox_762x39_20Rnd", null, (decimal)0.09));
            Loot.Add(new ExpansionLoot("AmmoBox_556x45Tracer_20Rnd", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("AmmoBox_556x45_20Rnd", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("AmmoBox_545x39Tracer_20Rnd", null, (decimal)0.06));
            Loot.Add(new ExpansionLoot("AmmoBox_545x39_20Rnd", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("Ammo_762x54Tracer", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("Ammo_762x54", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Ammo_762x39Tracer", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Ammo_762x39", null, (decimal)0.12));
            Loot.Add(new ExpansionLoot("Ammo_556x45Tracer", null, (decimal)0.07));
            Loot.Add(new ExpansionLoot("Ammo_556x45", null, (decimal)0.11));
            Loot.Add(new ExpansionLoot("Ammo_545x39Tracer", null, (decimal)0.07));
            Loot.Add(new ExpansionLoot("Ammo_545x39", null, (decimal)0.10));

        }
        static void Deployable_BaseBuilding(BindingList<ExpansionLoot> Loot)
        {

            Loot.Add(new ExpansionLoot("MediumTent", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("LargeTent", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("CarTent", null, (decimal)0.15));
        }
        static void Gear_BaseBuilding(BindingList<ExpansionLoot> Loot)
        {
            BindingList<ExpansionLootVariant> combinationLockVariants = new BindingList<ExpansionLootVariant>() {
                new ExpansionLootVariant( "CombinationLock4" )
            };

            BindingList<ExpansionLootVariant> gasCanisterVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "MediumGasCanister" ),
                new ExpansionLootVariant( "LargeGasCanister" )
            };

            BindingList<ExpansionLootVariant> att_smallGasCanister = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("SmallGasCanister") };

            Loot.Add(new ExpansionLoot("NailBox", null, (decimal)0.8));
            Loot.Add(new ExpansionLoot("DuctTape", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("MetalPlate", null, (decimal)0.25));
            Loot.Add(new ExpansionLoot("WoodenPlank", null, (decimal)0.5));
            Loot.Add(new ExpansionLoot("Fabric", null, (decimal)0.5));
            Loot.Add(new ExpansionLoot("Hammer", null, (decimal)0.4));
            Loot.Add(new ExpansionLoot("Shovel", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Pliers", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("WoodAxe", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("Crowbar", null, (decimal)0.3));
            Loot.Add(new ExpansionLoot("Hacksaw", null, (decimal)0.3));
            Loot.Add(new ExpansionLoot("Handsaw", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("SledgeHammer", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("Hatchet", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("BarbedWire", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("MetalWire", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("CombinationLock", null, (decimal)0.1, -1, combinationLockVariants));
            Loot.Add(new ExpansionLoot("CamoNet", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Rope", null, (decimal)0.3));
            Loot.Add(new ExpansionLoot("Blowtorch", att_smallGasCanister, (decimal)0.15));
            Loot.Add(new ExpansionLoot("ExpansionPropaneTorch", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("ExpansionBoltCutters", null, (decimal)0.175));
            Loot.Add(new ExpansionLoot("SmallGasCanister", null, (decimal)0.225, -1, gasCanisterVariants));
            Loot.Add(new ExpansionLoot("EpoxyPutty", null, (decimal)0.2));
        }
        static void Gear_Mechanic(BindingList<ExpansionLoot> Loot)
        {
            BindingList<ExpansionLootVariant> att_smallGasCanister = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("SmallGasCanister") };
            Loot.Add(new ExpansionLoot("Blowtorch", att_smallGasCanister, (decimal)0.15));
            Loot.Add(new ExpansionLoot("EpoxyPutty", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("Screwdriver", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("Wrench", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("Pliers", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("LugWrench", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("Crowbar", null, (decimal)0.2));
        }
        static void Attachments(BindingList<ExpansionLoot> Loot)
        {
            BindingList<ExpansionLootVariant> battery = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Battery9V") };

            Loot.Add(new ExpansionLoot("AK_RailHndgrd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("AK_Bayonet", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("KobraOptic", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("UniversalLight", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("KobraOptic", battery, (decimal)0.08));
            Loot.Add(new ExpansionLoot("UniversalLight", battery, (decimal)0.08));
            Loot.Add(new ExpansionLoot("ACOGOptic", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("ACOGOptic_6x", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("M4_RisHndgrd", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M9A1_Bayonet", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Mosin_Bayonet", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("PUScopeOptic", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("TLRLight", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("TLRLight", battery, (decimal)0.08));
            Loot.Add(new ExpansionLoot("SKS_Bayonet", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("M68Optic", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M4_T3NRDSOptic", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("FNP45_MRDSOptic", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("PSO1Optic", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("PSO11Optic", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("M68Optic", battery, (decimal)0.08));
            Loot.Add(new ExpansionLoot("M4_T3NRDSOptic", battery, (decimal)0.08));
            Loot.Add(new ExpansionLoot("PSO1Optic", battery, (decimal)0.08));
            Loot.Add(new ExpansionLoot("PSO11Optic", battery, (decimal)0.08));

        }
        static void Food(BindingList<ExpansionLoot> Loot)
        {
            Loot.Add(new ExpansionLoot("BoxCerealCrunchin", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("PeachesCan", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("BakedBeansCan", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("SpaghettiCan", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("SardinesCan", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("TunaCan", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("WaterBottle", null, (decimal)0.5));

            Loot.Add(new ExpansionLoot("TacticalBaconCan", null, (decimal)0.025));

            Loot.Add(new ExpansionLoot("CanOpener", null, (decimal)0.5));
            Loot.Add(new ExpansionLoot("KitchenKnife", null, (decimal)0.3));
        }
        static void NBC(BindingList<ExpansionLoot> Loot)
        {
            BindingList<ExpansionLootVariant> att_gasmask = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("GasMask_Filter") };

            BindingList<ExpansionLootVariant> airborneMaskVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "AirborneMask", null, (decimal)0.80 )
            };

            BindingList<ExpansionLootVariant> gp5GasMaskVariants = new BindingList<ExpansionLootVariant>(){
                new ExpansionLootVariant( "GP5GasMask", null, (decimal)0.80 )
            };

            Loot.Add(new ExpansionLoot("NBCBootsGray", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("NBCBootsYellow", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("NBCGlovesGray", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("NBCGlovesYellow", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("NBCHoodGray", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("NBCHoodYellow", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("NBCJacketGray", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("NBCJacketYellow", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("NBCPantsGray", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("NBCPantsYellow", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("AirborneMask", att_gasmask, (decimal)0.1, -1, airborneMaskVariants));
            Loot.Add(new ExpansionLoot("GP5GasMask", att_gasmask, (decimal)0.1, -1, gp5GasMaskVariants));
            Loot.Add(new ExpansionLoot("GasMask", null, (decimal)0.05));
        }
        static void Gear_Medical(BindingList<ExpansionLoot> Loot)
        {
            BindingList<ExpansionLootVariant> firstaidkit_1 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("BandageDressing"), new ExpansionLootVariant("BandageDressing") };
            BindingList<ExpansionLootVariant> firstaidkit_2 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("BandageDressing"), new ExpansionLootVariant("BandageDressing"), new ExpansionLootVariant("BandageDressing"), new ExpansionLootVariant("BandageDressing") };
            BindingList<ExpansionLootVariant> firstaidkit_3 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Morphine"), new ExpansionLootVariant("BandageDressing") };
            BindingList<ExpansionLootVariant> firstaidkit_4 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("BandageDressing"), new ExpansionLootVariant("SalineBagIV") };
            BindingList<ExpansionLootVariant> firstaidkit_5 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PainkillerTablets"), new ExpansionLootVariant("DisinfectantAlcohol") };
            BindingList<ExpansionLootVariant> firstaidkit_6 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("CharcoalTablets"), new ExpansionLootVariant("Morphine") };
            BindingList<ExpansionLootVariant> firstaidkit_7 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Epinephrine"), new ExpansionLootVariant("StartKitIV") };
            BindingList<ExpansionLootVariant> firstaidkit_8 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("TetracyclineAntiBiotics"), new ExpansionLootVariant("VitaminBottle") };
            BindingList<ExpansionLootVariant> firstaidkit_9 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Morphine"), new ExpansionLootVariant("SalineBagIV") };
            BindingList<ExpansionLootVariant> firstaidkit_10 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PainkillerTablets"), new ExpansionLootVariant("Epinephrine") };
            BindingList<ExpansionLootVariant> firstaidkit_11 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("TetracyclineAntiBiotics"), new ExpansionLootVariant("Morphine") };
            BindingList<ExpansionLootVariant> firstaidkit_12 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("VitaminBottle"), new ExpansionLootVariant("SurgicalGloves_Blue") };
            BindingList<ExpansionLootVariant> firstaidkit_13 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("BandageDressing"), new ExpansionLootVariant("VitaminBottle") };
            BindingList<ExpansionLootVariant> firstaidkit_14 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("CharcoalTablets"), new ExpansionLootVariant("DisinfectantAlcohol") };
            BindingList<ExpansionLootVariant> firstaidkit_15 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("IodineTincture"), new ExpansionLootVariant("PainkillerTablets") };
            BindingList<ExpansionLootVariant> firstaidkit_16 = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("CharcoalTablets"), new ExpansionLootVariant("AntiChemInjector"), new ExpansionLootVariant("BandageDressing") };
            BindingList<ExpansionLootVariant> firstaidkitVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_2, (decimal)0.040984 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_3, (decimal)0.073770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_4, (decimal)0.073770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_5, (decimal)0.073770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_6, (decimal)0.073770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_7, (decimal)0.073770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_8, (decimal)0.073770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_9, (decimal)0.063770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_10, (decimal)0.063770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_11, (decimal)0.063770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_12, (decimal)0.063770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_13, (decimal)0.063770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_14, (decimal)0.063770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_15, (decimal)0.033770 ),
            new ExpansionLootVariant( "FirstAidKit", firstaidkit_16, (decimal)0.023770 )
        };

            Loot.Add(new ExpansionLoot("FirstAidKit", firstaidkit_1, (decimal)2.44, -1, firstaidkitVariants));

            Loot.Add(new ExpansionLoot("AntiChemInjector", null, (decimal)0.02));

            Loot.Add(new ExpansionLoot("BandageDressing", null, (decimal)0.15));

            Loot.Add(new ExpansionLoot("Morphine", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("Epinephrine", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("PainkillerTablets", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("CharcoalTablets", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("TetracyclineAntiBiotics", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("DisinfectantSpray", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("DisinfectantAlcohol", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("VitaminBottle", null, (decimal)0.2));

            Loot.Add(new ExpansionLoot("StartKitIV", null, (decimal)0.3));
            Loot.Add(new ExpansionLoot("SalineBagIV", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("SalineBag", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("BloodBagEmpty", null, (decimal)0.15));
            Loot.Add(new ExpansionLoot("BloodBagIV", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("BloodTestKit", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("BloodTestKit", null, (decimal)0.2));

            Loot.Add(new ExpansionLoot("IodineTincture", null, (decimal)0.2));
        }
        static void Clothing_Medical(BindingList<ExpansionLoot> Loot)
        {
            BindingList<ExpansionLootVariant> medicalScrubsHatsVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "MedicalScrubsHat_White" ),
            new ExpansionLootVariant( "MedicalScrubsHat_Green" )
        };

            BindingList<ExpansionLootVariant> medicalScrubsPantsVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "MedicalScrubsPants_White" ),
            new ExpansionLootVariant( "MedicalScrubsPants_Green" )
        };

            BindingList<ExpansionLootVariant> medicalScrubsShirtsVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "MedicalScrubsShirt_White" ),
            new ExpansionLootVariant( "MedicalScrubsShirt_Green" )
        };

            BindingList<ExpansionLootVariant> surgicalGlovesVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "SurgicalGloves_LightBlue" ),
            new ExpansionLootVariant( "SurgicalGloves_Green" )
        };

            Loot.Add(new ExpansionLoot("MedicalScrubsHat_Blue", null, (decimal)0.24, -1, medicalScrubsHatsVariants));
            Loot.Add(new ExpansionLoot("MedicalScrubsPants_Blue", null, (decimal)0.24, -1, medicalScrubsPantsVariants));
            Loot.Add(new ExpansionLoot("MedicalScrubsShirt_Blue", null, (decimal)0.24, -1, medicalScrubsShirtsVariants));

            Loot.Add(new ExpansionLoot("NioshFaceMask", null, (decimal)0.08));

            Loot.Add(new ExpansionLoot("SurgicalGloves_Blue", null, (decimal)0.24, -1, surgicalGlovesVariants));
        }
        static void Clothing_Civilian(BindingList<ExpansionLoot> Loot)
        {
            BindingList<ExpansionLootVariant> visor = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("DirtBikeHelmet_Visor") };

            Loot.Add(new ExpansionLoot("DirtBikeHelmet_Chernarus", visor, (decimal)0.3));
        }
        static void Clothing_Worker(BindingList<ExpansionLoot> Loot)
        {

        }
        static void Clothing_Military(BindingList<ExpansionLoot> Loot)
        {
            BindingList<ExpansionLootVariant> vest = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("PlateCarrierHolster"), new ExpansionLootVariant("PlateCarrierPouches") };

            Loot.Add(new ExpansionLoot("UKAssVest_Black", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("UKAssVest_Camo", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("UKAssVest_Khaki", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("UKAssVest_Olive", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("HighCapacityVest_Black", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("HighCapacityVest_Olive", null, (decimal)0.1));

            Loot.Add(new ExpansionLoot("PlateCarrierVest", null, (decimal)0.08));
            Loot.Add(new ExpansionLoot("PlateCarrierHolster", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("PlateCarrierPouches", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("PlateCarrierVest", vest, (decimal)0.05));

            Loot.Add(new ExpansionLoot("GhillieAtt_Mossy", null, (decimal)0.2));
            Loot.Add(new ExpansionLoot("GhillieHood_Mossy", null, (decimal)0.1));
            Loot.Add(new ExpansionLoot("GhillieBushrag_Mossy", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("GhillieSuit_Mossy", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("BallisticHelmet_UN", null, (decimal)0.08));
        }

        internal static BindingList<ExpansionLoot> Airdrop_Regular()
        {
            BindingList<ExpansionLoot> Loot = new BindingList<ExpansionLoot>();
            BindingList<ExpansionLootVariant> battery = new BindingList<ExpansionLootVariant>() { new ExpansionLootVariant("Battery9V") };

            Loot.Add(new ExpansionLoot("SewingKit", null, (decimal)0.25));
            Loot.Add(new ExpansionLoot("LeatherSewingKit", null, (decimal)0.25));
            Loot.Add(new ExpansionLoot("WeaponCleaningKit", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("Lockpick", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("Binoculars", null, (decimal)0.3));
            Loot.Add(new ExpansionLoot("Rangefinder", battery, (decimal)0.05));
            Loot.Add(new ExpansionLoot("WeaponCleaningKit", null, (decimal)0.05));
            Weapons_Civilian(Loot);
            Ammo_Civilian(Loot);
            Gear_BaseBuilding(Loot);
            Gear_Medical(Loot);
            Clothing_Civilian(Loot);
            Clothing_Worker(Loot);
            Clothing_Military(Loot);
            Food(Loot);

            return Loot;
        }
        internal static BindingList<ExpansionLoot> Airdrop_Medical()
        {
            BindingList<ExpansionLoot> Loot = new BindingList<ExpansionLoot>();

            Loot.Add(new ExpansionLoot("Bear_Pink", null, (decimal)0.1));

            Gear_Medical(Loot);
            Clothing_Medical(Loot);

            return Loot;
        }
        internal static BindingList<ExpansionLoot> Airdrop_BaseBuilding()
        {
            BindingList<ExpansionLoot> Loot = new BindingList<ExpansionLoot>();

            Deployable_BaseBuilding(Loot);
            Gear_BaseBuilding(Loot);

            return Loot;
        }
        internal static BindingList<ExpansionLoot> Airdrop_Military()
        {
            BindingList<ExpansionLoot> Loot = new BindingList<ExpansionLoot>();

            BindingList<ExpansionLootVariant> battery = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("Battery9V") };

            Loot.Add(new ExpansionLoot("Binoculars", null, (decimal)0.3));
            Loot.Add(new ExpansionLoot("Rangefinder", battery, (decimal)0.05));
            Loot.Add(new ExpansionLoot("GhillieAtt_Mossy", null, (decimal)0.05));
            Loot.Add(new ExpansionLoot("WeaponCleaningKit", null, (decimal)0.05));

            Weapons_Military(Loot);
            Ammo_Military(Loot);
            Attachments(Loot);
            Clothing_Military(Loot);
            Food(Loot);

            return Loot;
        }
        internal static BindingList<ExpansionLoot> Airdrop_Vehicle()
        {
            BindingList<ExpansionLoot> Loot = new BindingList<ExpansionLoot>();

            BindingList<ExpansionLootVariant> att_Hatchback_02 = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("Hatchback_02_Wheel"), new ExpansionLootVariant("Hatchback_02_Wheel"), new ExpansionLootVariant("Hatchback_02_Wheel"), new ExpansionLootVariant("Hatchback_02_Wheel"), new ExpansionLootVariant("Hatchback_02_Wheel"), new ExpansionLootVariant("Hatchback_02_Door_1_1"), new ExpansionLootVariant("Hatchback_02_Door_2_1"), new ExpansionLootVariant("Hatchback_02_Door_1_2"), new ExpansionLootVariant("Hatchback_02_Door_2_2"), new ExpansionLootVariant("Hatchback_02_Hood"), new ExpansionLootVariant("Hatchback_02_Trunk"), new ExpansionLootVariant("CarBattery"), new ExpansionLootVariant("CarRadiator"), new ExpansionLootVariant("SparkPlug"), new ExpansionLootVariant("HeadlightH7"), new ExpansionLootVariant("HeadlightH7") };
            BindingList<ExpansionLootVariant> att_Sedan_02 = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("Sedan_02_Wheel"), new ExpansionLootVariant("Sedan_02_Wheel"), new ExpansionLootVariant("Sedan_02_Wheel"), new ExpansionLootVariant("Sedan_02_Wheel"), new ExpansionLootVariant("Sedan_02_Wheel"), new ExpansionLootVariant("CarBattery"), new ExpansionLootVariant("CarRadiator"), new ExpansionLootVariant("SparkPlug"), new ExpansionLootVariant("Sedan_02_Hood"), new ExpansionLootVariant("Sedan_02_Trunk"), new ExpansionLootVariant("Sedan_02_Door_1_1"), new ExpansionLootVariant("Sedan_02_Door_2_1"), new ExpansionLootVariant("Sedan_02_Door_1_2"), new ExpansionLootVariant("Sedan_02_Door_2_2"), new ExpansionLootVariant("HeadlightH7"), new ExpansionLootVariant("HeadlightH7") };
            BindingList<ExpansionLootVariant> att_CivilianSedan = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("CivSedanWheel"), new ExpansionLootVariant("CivSedanWheel"), new ExpansionLootVariant("CivSedanWheel"), new ExpansionLootVariant("CivSedanWheel"), new ExpansionLootVariant("CivSedanWheel"), new ExpansionLootVariant("CarBattery"), new ExpansionLootVariant("CarRadiator"), new ExpansionLootVariant("SparkPlug"), new ExpansionLootVariant("CivSedanHood"), new ExpansionLootVariant("CivSedanTrunk"), new ExpansionLootVariant("CivSedanDoors_Driver"), new ExpansionLootVariant("CivSedanDoors_CoDriver"), new ExpansionLootVariant("CivSedanDoors_BackLeft"), new ExpansionLootVariant("CivSedanDoors_BackRight"), new ExpansionLootVariant("HeadlightH7"), new ExpansionLootVariant("HeadlightH7") };
            BindingList<ExpansionLootVariant> att_Truck_01_Covered = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("Truck_01_Wheel"), new ExpansionLootVariant("Truck_01_Wheel"), new ExpansionLootVariant("Truck_01_Wheel"), new ExpansionLootVariant("Truck_01_Wheel"), new ExpansionLootVariant("Truck_01_WheelDouble"), new ExpansionLootVariant("Truck_01_WheelDouble"), new ExpansionLootVariant("Truck_01_WheelDouble"), new ExpansionLootVariant("Truck_01_WheelDouble"), new ExpansionLootVariant("TruckBattery"), new ExpansionLootVariant("Truck_01_Hood"), new ExpansionLootVariant("Truck_01_Door_1_1"), new ExpansionLootVariant("Truck_01_Door_2_1"), new ExpansionLootVariant("HeadlightH7"), new ExpansionLootVariant("HeadlightH7") };
            BindingList<ExpansionLootVariant> att_Offroad_02 = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("Offroad_02_Wheel"), new ExpansionLootVariant("Offroad_02_Wheel"), new ExpansionLootVariant("Offroad_02_Wheel"), new ExpansionLootVariant("Offroad_02_Wheel"), new ExpansionLootVariant("Offroad_02_Wheel"), new ExpansionLootVariant("Offroad_02_Door_1_1"), new ExpansionLootVariant("Offroad_02_Door_2_1"), new ExpansionLootVariant("Offroad_02_Door_1_2"), new ExpansionLootVariant("Offroad_02_Door_2_2"), new ExpansionLootVariant("Offroad_02_Hood"), new ExpansionLootVariant("Offroad_02_Trunk"), new ExpansionLootVariant("CarBattery"), new ExpansionLootVariant("GlowPlug"), new ExpansionLootVariant("HeadlightH7"), new ExpansionLootVariant("HeadlightH7") };

            BindingList<ExpansionLootVariant> att_OffroadHatchback = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("CarBattery"), new ExpansionLootVariant("CarRadiator"), new ExpansionLootVariant("SparkPlug"), new ExpansionLootVariant("HatchbackHood"), new ExpansionLootVariant("HatchbackTrunk"), new ExpansionLootVariant("HatchbackDoors_Driver"), new ExpansionLootVariant("HatchbackDoors_CoDriver"), new ExpansionLootVariant("HeadlightH7"), new ExpansionLootVariant("HeadlightH7") };
            BindingList<ExpansionLootVariant> att_OffroadHatchback_White = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("CarBattery"), new ExpansionLootVariant("CarRadiator"), new ExpansionLootVariant("SparkPlug"), new ExpansionLootVariant("HatchbackHood"), new ExpansionLootVariant("HatchbackTrunk"), new ExpansionLootVariant("HatchbackDoors_Driver"), new ExpansionLootVariant("HatchbackDoors_CoDriver"), new ExpansionLootVariant("HeadlightH7"), new ExpansionLootVariant("HeadlightH7") };
            BindingList<ExpansionLootVariant> att_OffroadHatchback_Blue = new BindingList<ExpansionLootVariant>(){ new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("HatchbackWheel"), new ExpansionLootVariant("CarBattery"), new ExpansionLootVariant("CarRadiator"), new ExpansionLootVariant("SparkPlug"), new ExpansionLootVariant("HatchbackHood"), new ExpansionLootVariant("HatchbackTrunk"), new ExpansionLootVariant("HatchbackDoors_Driver"), new ExpansionLootVariant("HatchbackDoors_CoDriver"), new ExpansionLootVariant("HeadlightH7"), new ExpansionLootVariant("HeadlightH7") };
            BindingList<ExpansionLootVariant> offroadHatchbackVariants = new BindingList<ExpansionLootVariant>(){
            new ExpansionLootVariant( "OffroadHatchback_White", att_OffroadHatchback_White, (decimal)0.80 ),
            new ExpansionLootVariant( "OffroadHatchback_Blue", att_OffroadHatchback_Blue, (decimal)0.80 )
        };

            Loot.Add(new ExpansionLoot("OffroadHatchback", att_OffroadHatchback, (decimal)0.65, -1, offroadHatchbackVariants));
            Loot.Add(new ExpansionLoot("Hatchback_02", att_Hatchback_02, (decimal)0.575));// -1, hatchback_02Variants ));
            Loot.Add(new ExpansionLoot("Sedan_02", att_Sedan_02, (decimal)0.5));// -1, sedan_02Variants ));
            Loot.Add(new ExpansionLoot("CivilianSedan", att_CivilianSedan, (decimal)0.45));// -1, civilianSedanVariants ));
            Loot.Add(new ExpansionLoot("Truck_01_Covered", att_Truck_01_Covered, (decimal)0.35));// -1, truck_01_CoveredVariants ));
            Loot.Add(new ExpansionLoot("Offroad_02", att_Offroad_02, (decimal)0.125));

            return Loot;
        }
    }

}
