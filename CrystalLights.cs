using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ItemManager;
using PieceManager;
using ServerSync;
using UnityEngine;
using LocalizationManager;


namespace CrystalLights
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInDependency("org.bepinex.plugins.jewelcrafting")]
    public class CrystalLights : BaseUnityPlugin
    {
        private const string ModName = "CrystalLights";
        private const string ModVersion = "1.0.0";
        private const string ModGUID = "org.bepinex.plugins.crystallights";


        private static readonly ConfigSync configSync = new(ModName) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        private static ConfigEntry<Toggle> serverConfigLocked = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        private enum Toggle
        {
            On = 1,
            Off = 0
        }


        public void Awake()
        {

            Localizer.Load();

            CrystalLightRange = config("Crystal Light Range", "Effect Range", 12f, new ConfigDescription("Set the range of Crystal Lights.", new AcceptableValueRange<float>(1f, 50f)));

            serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
            configSync.AddLockingConfigEntry(serverConfigLocked);


            Item CL_Crystal_Hammer = new("crystallights", "CL_Crystal_Hammer");  //assetbundle name, Asset Name
            CL_Crystal_Hammer.Crafting.Add(ItemManager.CraftingTable.Workbench, 1);
            CL_Crystal_Hammer.RequiredItems.Add("Uncut_Black_Stone", 1);
            CL_Crystal_Hammer.CraftAmount = 1;

            #region pieces 

            BuildPiece CL_Large_Wall_Black = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Large_Wall_Black", true, "CL_Crystal_Hammer");
            CL_Large_Wall_Black.RequiredItems.Add("Uncut_Black_Stone", 10, true);
            CL_Large_Wall_Black.RequiredItems.Add("Iron", 2, true);
            CL_Large_Wall_Black.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Large_Wall_Black.Prefab);

            BuildPiece CL_Large_Wall_Blue = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Large_Wall_Blue", true, "CL_Crystal_Hammer");
            CL_Large_Wall_Blue.RequiredItems.Add("Uncut_Blue_Stone", 10, true);
            CL_Large_Wall_Blue.RequiredItems.Add("Iron", 2, true);
            CL_Large_Wall_Blue.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Large_Wall_Blue.Prefab);

            BuildPiece CL_Large_Wall_Green = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Large_Wall_Green", true, "CL_Crystal_Hammer");
            CL_Large_Wall_Green.RequiredItems.Add("Uncut_Green_Stone", 10, true);
            CL_Large_Wall_Green.RequiredItems.Add("Iron", 2, true);
            CL_Large_Wall_Green.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Large_Wall_Green.Prefab);

            BuildPiece CL_Large_Wall_Orange = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Large_Wall_Orange", true, "CL_Crystal_Hammer");
            CL_Large_Wall_Orange.RequiredItems.Add("Uncut_Orange_Stone", 10, true);
            CL_Large_Wall_Orange.RequiredItems.Add("Iron", 2, true);
            CL_Large_Wall_Orange.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Large_Wall_Orange.Prefab);

            BuildPiece CL_Large_Wall_Purple = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Large_Wall_Purple", true, "CL_Crystal_Hammer");
            CL_Large_Wall_Purple.RequiredItems.Add("Uncut_Purple_Stone", 10, true);
            CL_Large_Wall_Purple.RequiredItems.Add("Iron", 2, true);
            CL_Large_Wall_Purple.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Large_Wall_Purple.Prefab);

            BuildPiece CL_Large_Wall_Red = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Large_Wall_Red", true, "CL_Crystal_Hammer");
            CL_Large_Wall_Red.RequiredItems.Add("Uncut_Red_Stone", 10, true);
            CL_Large_Wall_Red.RequiredItems.Add("Iron", 2, true);
            CL_Large_Wall_Red.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Large_Wall_Red.Prefab);

            BuildPiece CL_Large_Wall_Yellow = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Large_Wall_Yellow", true, "CL_Crystal_Hammer");
            CL_Large_Wall_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 10, true);
            CL_Large_Wall_Yellow.RequiredItems.Add("Iron", 2, true);
            CL_Large_Wall_Yellow.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Large_Wall_Yellow.Prefab);

            BuildPiece CL_Small_Wall_Black = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Small_Wall_Black", true, "CL_Crystal_Hammer");
            CL_Small_Wall_Black.RequiredItems.Add("Uncut_Black_Stone", 3, true);
            CL_Small_Wall_Black.RequiredItems.Add("Iron", 1, true);
            CL_Small_Wall_Black.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Small_Wall_Black.Prefab);

            BuildPiece CL_Small_Wall_Blue = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Small_Wall_Blue", true, "CL_Crystal_Hammer");
            CL_Small_Wall_Blue.RequiredItems.Add("Uncut_Blue_Stone", 3, true);
            CL_Small_Wall_Blue.RequiredItems.Add("Iron", 1, true);
            CL_Small_Wall_Blue.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Small_Wall_Blue.Prefab);

            BuildPiece CL_Small_Wall_Green = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Small_Wall_Green", true, "CL_Crystal_Hammer");
            CL_Small_Wall_Green.RequiredItems.Add("Uncut_Green_Stone", 3, true);
            CL_Small_Wall_Green.RequiredItems.Add("Iron", 1, true);
            CL_Small_Wall_Green.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Small_Wall_Green.Prefab);

            BuildPiece CL_Small_Wall_Orange = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Small_Wall_Orange", true, "CL_Crystal_Hammer");
            CL_Small_Wall_Orange.RequiredItems.Add("Uncut_Orange_Stone", 3, true);
            CL_Small_Wall_Orange.RequiredItems.Add("Iron", 1, true);
            CL_Small_Wall_Orange.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Small_Wall_Orange.Prefab);

            BuildPiece CL_Small_Wall_Purple = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Small_Wall_Purple", true, "CL_Crystal_Hammer");
            CL_Small_Wall_Purple.RequiredItems.Add("Uncut_Purple_Stone", 3, true);
            CL_Small_Wall_Purple.RequiredItems.Add("Iron", 1, true);
            CL_Small_Wall_Purple.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Small_Wall_Purple.Prefab);

            BuildPiece CL_Small_Wall_Red = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Small_Wall_Red", true, "CL_Crystal_Hammer");
            CL_Small_Wall_Red.RequiredItems.Add("Uncut_Red_Stone", 3, true);
            CL_Small_Wall_Red.RequiredItems.Add("Iron", 1, true);
            CL_Small_Wall_Red.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Small_Wall_Red.Prefab);

            BuildPiece CL_Small_Wall_Yellow = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Small_Wall_Yellow", true, "CL_Crystal_Hammer");
            CL_Small_Wall_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 3, true);
            CL_Small_Wall_Yellow.RequiredItems.Add("Iron", 1, true);
            CL_Small_Wall_Yellow.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Small_Wall_Yellow.Prefab);

            BuildPiece CL_Standing_Lamp_Black = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Standing_Lamp_Black", true, "CL_Crystal_Hammer");
            CL_Standing_Lamp_Black.RequiredItems.Add("Uncut_Black_Stone", 1, true);
            CL_Standing_Lamp_Black.RequiredItems.Add("Iron", 1, true);
            CL_Standing_Lamp_Black.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Standing_Lamp_Black.Prefab);

            BuildPiece CL_Standing_Lamp_Blue = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Standing_Lamp_Blue", true, "CL_Crystal_Hammer");
            CL_Standing_Lamp_Blue.RequiredItems.Add("Uncut_Blue_Stone", 1, true);
            CL_Standing_Lamp_Blue.RequiredItems.Add("Iron", 1, true);
            CL_Standing_Lamp_Blue.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Standing_Lamp_Blue.Prefab);

            BuildPiece CL_Standing_Lamp_Green = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Standing_Lamp_Green", true, "CL_Crystal_Hammer");
            CL_Standing_Lamp_Green.RequiredItems.Add("Uncut_Green_Stone", 1, true);
            CL_Standing_Lamp_Green.RequiredItems.Add("Iron", 1, true);
            CL_Standing_Lamp_Green.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Standing_Lamp_Green.Prefab);

            BuildPiece CL_Standing_Lamp_Orange = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Standing_Lamp_Orange", true, "CL_Crystal_Hammer");
            CL_Standing_Lamp_Orange.RequiredItems.Add("Uncut_Orange_Stone", 1, true);
            CL_Standing_Lamp_Orange.RequiredItems.Add("Iron", 1, true);
            CL_Standing_Lamp_Orange.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Standing_Lamp_Orange.Prefab);

            BuildPiece CL_Standing_Lamp_Purple = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Standing_Lamp_Purple", true, "CL_Crystal_Hammer");
            CL_Standing_Lamp_Purple.RequiredItems.Add("Uncut_Purple_Stone", 1, true);
            CL_Standing_Lamp_Purple.RequiredItems.Add("Iron", 1, true);
            CL_Standing_Lamp_Purple.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Standing_Lamp_Purple.Prefab);

            BuildPiece CL_Standing_Lamp_Red = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Standing_Lamp_Red", true, "CL_Crystal_Hammer");
            CL_Standing_Lamp_Red.RequiredItems.Add("Uncut_Red_Stone", 1, true);
            CL_Standing_Lamp_Red.RequiredItems.Add("Iron", 1, true);
            CL_Standing_Lamp_Red.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Standing_Lamp_Red.Prefab);

            BuildPiece CL_Standing_Lamp_Yellow = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Standing_Lamp_Yellow", true, "CL_Crystal_Hammer");
            CL_Standing_Lamp_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 1, true);
            CL_Standing_Lamp_Yellow.RequiredItems.Add("Iron", 1, true);
            CL_Standing_Lamp_Yellow.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Standing_Lamp_Yellow.Prefab);

            BuildPiece CL_Chandelier_Black = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Chandelier_Black", true, "CL_Crystal_Hammer");
            CL_Chandelier_Black.RequiredItems.Add("Uncut_Black_Stone", 15, true);
            CL_Chandelier_Black.RequiredItems.Add("Iron", 5, true);
            CL_Chandelier_Black.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Chandelier_Black.Prefab);

            BuildPiece CL_Chandelier_Blue = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Chandelier_Blue", true, "CL_Crystal_Hammer");
            CL_Chandelier_Blue.RequiredItems.Add("Uncut_Blue_Stone", 15, true);
            CL_Chandelier_Blue.RequiredItems.Add("Iron", 5, true);
            CL_Chandelier_Blue.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Chandelier_Blue.Prefab);

            BuildPiece CL_Chandelier_Green = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Chandelier_Green", true, "CL_Crystal_Hammer");
            CL_Chandelier_Green.RequiredItems.Add("Uncut_Green_Stone", 15, true);
            CL_Chandelier_Green.RequiredItems.Add("Iron", 5, true);
            CL_Chandelier_Green.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Chandelier_Green.Prefab);

            BuildPiece CL_Chandelier_Orange = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Chandelier_Orange", true, "CL_Crystal_Hammer");
            CL_Chandelier_Orange.RequiredItems.Add("Uncut_Orange_Stone", 15, true);
            CL_Chandelier_Orange.RequiredItems.Add("Iron", 5, true);
            CL_Chandelier_Orange.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Chandelier_Orange.Prefab);

            BuildPiece CL_Chandelier_Yellow = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Chandelier_Yellow", true, "CL_Crystal_Hammer");
            CL_Chandelier_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 15, true);
            CL_Chandelier_Yellow.RequiredItems.Add("Iron", 5, true);
            CL_Chandelier_Yellow.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Chandelier_Yellow.Prefab);

            BuildPiece CL_Chandelier_Purple = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Chandelier_Purple", true, "CL_Crystal_Hammer");
            CL_Chandelier_Purple.RequiredItems.Add("Uncut_Purple_Stone", 15, true);
            CL_Chandelier_Purple.RequiredItems.Add("Iron", 5, true);
            CL_Chandelier_Purple.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Chandelier_Purple.Prefab);

            BuildPiece CL_Chandelier_Red = new(PiecePrefabManager.RegisterAssetBundle("crystallights"), "CL_Chandelier_Red", true, "CL_Crystal_Hammer");
            CL_Chandelier_Red.RequiredItems.Add("Uncut_Red_Stone", 15, true);
            CL_Chandelier_Red.RequiredItems.Add("Iron", 5, true);
            CL_Chandelier_Red.Category.Add(BuildPieceCategory.Misc);
            CrystalLightRange.SettingChanged += (_, _) => SetLightIntensity(CL_Chandelier_Red.Prefab);

            #endregion


            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony harmony = new(ModGUID);
            harmony.PatchAll(assembly);

        }

        private void SetLightIntensity(GameObject lightPrefab)
        {
            if (lightPrefab == null) return;
            var light = lightPrefab.GetComponentsInChildren<Light>();
            if (light is not { Length: > 0 }) return;
            foreach (Light lightComp in light)
            {
                lightComp.range = CrystalLightRange.Value;
            }
        }

        public static ConfigEntry<float> CrystalLightRange = null!;

    
    }
}
