﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RAGE;
using static RAGE.Game.Misc;

namespace ProjectClient
{
    public class Enums
    {
        public static uint[] ObjectsWeaponComponents = new uint[]
        {
            GetHashKey("w_ar_carbineriflemk2_mag1"), //COMPONENT_CARBINERIFLE_MK2_CLIP_01
            GetHashKey("w_ar_carbineriflemk2_mag2"), // COMPONENT_CARBINERIFLE_MK2_CLIP_02
            GetHashKey("w_ar_carbineriflemk2_mag_tr"), // COMPONENT_CARBINERIFLE_MK2_CLIP_TRACER
            /*GetHashKey("w_ar_carbineriflemk2_mag_inc"), // COMPONENT_CARBINERIFLE_MK2_CLIP_INCENDIARY
            GetHashKey("w_ar_carbineriflemk2_mag_ap"), // COMPONENT_CARBINERIFLE_MK2_CLIP_ARMORPIERCING
            GetHashKey("w_ar_carbineriflemk2_fmj"), // COMPONENT_CARBINERIFLE_MK2_CLIP_FMJ*/
            GetHashKey("w_at_afgrip_2"), // COMPONENT_AT_AR_AFGRIP_02
            GetHashKey("w_at_ar_flsh"), // COMPONENT_AT_AR_FLSH
            GetHashKey("w_at_sights_1"), // COMPONENT_AT_SIGHTS
            GetHashKey("w_at_scope_macro_2_mk2"), // COMPONENT_AT_SCOPE_MACRO_MK2
            GetHashKey("w_at_scope_medium_2"), // COMPONENT_AT_SCOPE_MEDIUM_MK2
            GetHashKey("w_at_ar_supp"), // COMPONENT_AT_AR_SUPP
            GetHashKey("w_at_muzzle_1"), // COMPONENT_AT_MUZZLE_01
            GetHashKey("w_at_muzzle_2"), // COMPONENT_AT_MUZZLE_02
            GetHashKey("w_at_muzzle_3"), // COMPONENT_AT_MUZZLE_03
            GetHashKey("w_at_muzzle_4"), // COMPONENT_AT_MUZZLE_04
            GetHashKey("w_at_muzzle_5"), // COMPONENT_AT_MUZZLE_05
            GetHashKey("w_at_muzzle_6"), // COMPONENT_AT_MUZZLE_06
            GetHashKey("w_at_muzzle_7"), // COMPONENT_AT_MUZZLE_07
            GetHashKey("w_at_cr_barrel_1"), // COMPONENT_AT_CR_BARREL_01
            GetHashKey("w_at_cr_barrel_2"), // COMPONENT_AT_CR_BARREL_02
            GetHashKey("w_ar_carbineriflemk2_camo1"), // COMPONENT_CARBINERIFLE_MK2_CAMO
            GetHashKey("w_ar_carbineriflemk2_camo2"), // COMPONENT_CARBINERIFLE_MK2_CAMO_02
            GetHashKey("w_ar_carbineriflemk2_camo3"), // COMPONENT_CARBINERIFLE_MK2_CAMO_03
            GetHashKey("w_ar_carbineriflemk2_camo4"), // COMPONENT_CARBINERIFLE_MK2_CAMO_04
            GetHashKey("w_ar_carbineriflemk2_camo5"), // COMPONENT_CARBINERIFLE_MK2_CAMO_05
            GetHashKey("w_ar_carbineriflemk2_camo6"), // COMPONENT_CARBINERIFLE_MK2_CAMO_06
            GetHashKey("w_ar_carbineriflemk2_camo7"), // COMPONENT_CARBINERIFLE_MK2_CAMO_07
            GetHashKey("w_ar_carbineriflemk2_camo8"), // COMPONENT_CARBINERIFLE_MK2_CAMO_08
            GetHashKey("w_ar_carbineriflemk2_camo9"), // COMPONENT_CARBINERIFLE_MK2_CAMO_09
            GetHashKey("w_ar_carbineriflemk2_camo10"), // COMPONENT_CARBINERIFLE_MK2_CAMO_10
            GetHashKey("w_ar_carbineriflemk2_camo_ind1") // COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01
        };

        public enum CarbinRifleMk2Components : uint
        {
            COMPONENT_CARBINERIFLE_MK2_CLIP_01 = 0x4C7A391E,
            COMPONENT_CARBINERIFLE_MK2_CLIP_02 = 0x5DD5DBD5,
            COMPONENT_CARBINERIFLE_MK2_CLIP_TRACER = 0x1757F566,
            COMPONENT_CARBINERIFLE_MK2_CLIP_INCENDIARY = 0x3D25C2A7,
            COMPONENT_CARBINERIFLE_MK2_CLIP_ARMORPIERCING = 0x255D5D57,
            COMPONENT_CARBINERIFLE_MK2_CLIP_FMJ = 0x44032F11,
            COMPONENT_AT_AR_AFGRIP_02 = 0x9D65907A,
            COMPONENT_AT_AR_FLSH = 0x7BC4CDDC,
            COMPONENT_AT_SIGHTS = 0x420FD713,
            COMPONENT_AT_SCOPE_MACRO_MK2 = 0x49B2945,
            COMPONENT_AT_SCOPE_MEDIUM_MK2 = 0xC66B6542,
            COMPONENT_AT_AR_SUPP = 0x837445AA,
            COMPONENT_AT_MUZZLE_01 = 0xB99402D4,
            COMPONENT_AT_MUZZLE_02 = 0xC867A07B,
            COMPONENT_AT_MUZZLE_03 = 0xDE11CBCF,
            COMPONENT_AT_MUZZLE_04 = 0xEC9068CC,
            COMPONENT_AT_MUZZLE_05 = 0x2E7957A,
            COMPONENT_AT_MUZZLE_06 = 0x347EF8AC,
            COMPONENT_AT_MUZZLE_07 = 0x4DB62ABE,
            COMPONENT_AT_CR_BARREL_01 = 0x833637FF,
            COMPONENT_AT_CR_BARREL_02 = 0x8B3C480B,
            COMPONENT_CARBINERIFLE_MK2_CAMO = 0x4BDD6F16,
            COMPONENT_CARBINERIFLE_MK2_CAMO_02 = 0x406A7908,
            COMPONENT_CARBINERIFLE_MK2_CAMO_03 = 0x2F3856A4,
            COMPONENT_CARBINERIFLE_MK2_CAMO_04 = 0xE50C424D,
            COMPONENT_CARBINERIFLE_MK2_CAMO_05 = 0xD37D1F2F,
            COMPONENT_CARBINERIFLE_MK2_CAMO_06 = 0x86268483,
            COMPONENT_CARBINERIFLE_MK2_CAMO_07 = 0xF420E076,
            COMPONENT_CARBINERIFLE_MK2_CAMO_08 = 0xAAE14DF8,
            COMPONENT_CARBINERIFLE_MK2_CAMO_09 = 0x9893A95D,
            COMPONENT_CARBINERIFLE_MK2_CAMO_10 = 0x6B13CD3E,
            COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01 = 0xDA55CD3F
        }

        public enum WeaponHash : uint
        {
            Sniperrifle = 100416529,
            Fireextinguisher = 101631238,
            Compactlauncher = 125959754,
            Snowball = 126349499,
            Vintagepistol = 137902532,
            Combatpdw = 171789620,
            Heavysniper_mk2 = 177293209,
            Heavysniper = 205991906,
            Autoshotgun = 317205821,
            Microsmg = 324215364,
            Wrench = 419712736,
            Pistol = 453432689,
            Pumpshotgun = 487013001,
            Appistol = 584646201,
            Ball = 600439132,
            Molotov = 615608432,
            CeramicPistol = 727643628,
            Smg = 736523883,
            Stickybomb = 741814745,
            Petrolcan = 883325847,
            Stungun = 911657153,
            Stone_hatchet = 940833800,
            Assaultrifle_mk2 = 961495388,
            Heavyshotgun = 984333226,
            Minigun = 1119849093,
            Golfclub = 1141786504,
            Raycarbine = 1198256469,
            Flaregun = 1198879012,
            Flare = 1233104067,
            Grenadelauncher_smoke = 1305664598,
            Hammer = 1317494643,
            Pumpshotgun_mk2 = 1432025498,
            Combatpistol = 1593441988,
            Gusenberg = 1627465347,
            Compactrifle = 1649403952,
            Hominglauncher = 1672152130,
            Nightstick = 1737195953,
            Marksmanrifle_mk2 = 1785463520,
            Railgun = 1834241177,
            Sawnoffshotgun = 2017895192,
            Smg_mk2 = 2024373456,
            Bullpuprifle = 2132975508,
            Firework = 2138347493,
            Combatmg = 2144741730,
            Carbinerifle = 2210333304,
            Crowbar = 2227010557,
            Bullpuprifle_mk2 = 2228681469,
            Snspistol_mk2 = 2285322324,
            Flashlight = 2343591895,
            //Proximine = 2381443905,
            NavyRevolver = 2441047180,
            Dagger = 2460120199,
            Grenade = 2481070269,
            Poolcue = 2484171525,
            Bat = 2508868239,
            Specialcarbine_mk2 = 2526821735,
            Doubleaction = 2548703416,
            Pistol50 = 2578377531,
            Knife = 2578778090,
            Mg = 2634544996,
            Bullpupshotgun = 2640438543,
            Bzgas = 2694266206,
            Unarmed = 2725352035,
            Grenadelauncher = 2726580491,
            Musket = 2828843422,
            Advancedrifle = 2937143193,
            Raypistol = 2939590305,
            Rpg = 2982836145,
            Rayminigun = 3056410471,
            Pipebomb = 3125143736,
            HazardCan = 3126027122,
            Minismg = 3173288789,
            Snspistol = 3218215474,
            Pistol_mk2 = 3219281620,
            Assaultrifle = 3220176749,
            Specialcarbine = 3231910285,
            Revolver = 3249783761,
            Marksmanrifle = 3342088282,
            Revolver_mk2 = 3415619887,
            Battleaxe = 3441901897,
            Heavypistol = 3523564046,
            Knuckle = 3638508604,
            Machinepistol = 3675956304,
            Combatmg_mk2 = 3686625920,
            Marksmanpistol = 3696079510,
            Machete = 3713923289,
            Switchblade = 3756226112,
            Assaultshotgun = 3800352039,
            Dbshotgun = 4019527611,
            Assaultsmg = 4024951519,
            Hatchet = 4191993645,
            Bottle = 4192643659,
            Carbinerifle_mk2 = 4208062921,
            Parachute = 4222310262/*,
            Smokegrenade = 4256991824*/
        }
    }
}
