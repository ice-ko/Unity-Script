#define SILENT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace V_AnimationSystem {

    public class UnitAnimType {

        public const string VALID_NAME_DIC = "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ0123456789_";

        // Anim Type, contains directional anims
        private string name;
        private bool disableOverwrite; // Cannot overwrite default animations
        private Dictionary<UnitAnim.AnimDir, UnitAnim> singleAnimDic;

        private UnitAnimType(string name) {
            this.name = name;
            singleAnimDic = new Dictionary<UnitAnim.AnimDir, UnitAnim>();
            SetAnimAll(null);
        }
        public bool CanOverwrite() {
            return !disableOverwrite;
        }
        public void SetName(string name) {
            if (!CanOverwrite()) {
                // Cannot overwrite! Cannot change name!
                return;
            }
            this.name = name;
        }
        public string GetName() {
            return name;
        }
        public List<string> GetTriggerList() {
            List<string> ret = new List<string>();
            foreach (UnitAnim.AnimDir animDir in System.Enum.GetValues(typeof(UnitAnim.AnimDir))) {
                UnitAnim unitAnim = singleAnimDic[animDir];
                ret.AddRange(unitAnim.GetTriggerList());
            }
            return ret;
        }
        public void TryFillDirections() {
            foreach (UnitAnim.AnimDir animDir in System.Enum.GetValues(typeof(UnitAnim.AnimDir))) {
                UnitAnim unitAnim = UnitAnim.GetUnitAnim(name + animDir);
                if (unitAnim != null) {
                    SetAnim(animDir, unitAnim);
                }
            }
            if (singleAnimDic[UnitAnim.AnimDir.Down] != null) {
                if (singleAnimDic[UnitAnim.AnimDir.DownLeft] == null) {
                    singleAnimDic[UnitAnim.AnimDir.DownLeft] = singleAnimDic[UnitAnim.AnimDir.Down];
                }
                if (singleAnimDic[UnitAnim.AnimDir.DownRight] == null) {
                    singleAnimDic[UnitAnim.AnimDir.DownRight] = singleAnimDic[UnitAnim.AnimDir.Down];
                }
            }
            if (singleAnimDic[UnitAnim.AnimDir.Up] != null) {
                if (singleAnimDic[UnitAnim.AnimDir.UpLeft] == null) {
                    singleAnimDic[UnitAnim.AnimDir.UpLeft] = singleAnimDic[UnitAnim.AnimDir.Up];
                }
                if (singleAnimDic[UnitAnim.AnimDir.UpRight] == null) {
                    singleAnimDic[UnitAnim.AnimDir.UpRight] = singleAnimDic[UnitAnim.AnimDir.Up];
                }
            }

            {
                UnitAnim unitAnim = UnitAnim.GetUnitAnim(name);
                if (unitAnim != null) {
                    foreach (UnitAnim.AnimDir animDir in System.Enum.GetValues(typeof(UnitAnim.AnimDir))) {
                        SetAnim(animDir, unitAnim);
                    }
                }
            }
        }
        public void SetAnims(UnitAnim animDown) {
            SetAnims(animDown, animDown, animDown, animDown, animDown, animDown, animDown, animDown);
        }
        public void SetAnims(UnitAnim animDown, UnitAnim animUp, UnitAnim animLeft, UnitAnim animRight) {
            SetAnims(animDown, animUp, animLeft, animRight, animDown, animDown, animUp, animUp);
        }
        public void SetAnims(UnitAnim animDown, UnitAnim animUp, UnitAnim animLeft, UnitAnim animRight, UnitAnim animDownLeft, UnitAnim animDownRight, UnitAnim animUpLeft, UnitAnim animUpRight) {
            singleAnimDic[UnitAnim.AnimDir.Down] = animDown;
            singleAnimDic[UnitAnim.AnimDir.Up] = animUp;
            singleAnimDic[UnitAnim.AnimDir.Left] = animLeft;
            singleAnimDic[UnitAnim.AnimDir.Right] = animRight;
            singleAnimDic[UnitAnim.AnimDir.DownLeft] = animDownLeft;
            singleAnimDic[UnitAnim.AnimDir.DownRight] = animDownRight;
            singleAnimDic[UnitAnim.AnimDir.UpLeft] = animUpLeft;
            singleAnimDic[UnitAnim.AnimDir.UpRight] = animUpRight;
        }
        public void SetAnim(UnitAnim.AnimDir animDir, UnitAnim unitAnim) {
            singleAnimDic[animDir] = unitAnim;
        }
        public void SetAnimAll(UnitAnim unitAnim) {
            foreach (UnitAnim.AnimDir animDir in System.Enum.GetValues(typeof(UnitAnim.AnimDir))) {
                singleAnimDic[animDir] = unitAnim;
            }
        }
        public void SetAnimAllNull(UnitAnim unitAnim) {
            foreach (UnitAnim.AnimDir animDir in System.Enum.GetValues(typeof(UnitAnim.AnimDir))) {
                if (singleAnimDic[animDir] == null) {
                    singleAnimDic[animDir] = unitAnim;
                }
            }
        }
        public UnitAnim GetUnitAnim(Vector3 dir) {
            return GetUnitAnim(V_UnitAnimation.GetAngleFromVector(dir));
        }
        public UnitAnim GetUnitAnim(UnitAnim.AnimDir animDir) {
            return singleAnimDic[animDir];
        }
        public UnitAnim GetUnitAnim(int angle) {
            return singleAnimDic[UnitAnim.GetAnimDirFromAngle(angle)];
        }
        public override string ToString() {
            string anims = "";

            foreach (UnitAnim.AnimDir animDir in System.Enum.GetValues(typeof(UnitAnim.AnimDir))) {
                string animName = singleAnimDic[animDir] == null ? "null" : singleAnimDic[animDir].ToString();
                anims += animName + "; ";
            }

            return name + ": " + anims;
        }
        public string Save_Linked() {
            //Returns a string to be used in savefiles
            string[] content = new string[]{
            name,
            ""+singleAnimDic[UnitAnim.AnimDir.Down],
            ""+singleAnimDic[UnitAnim.AnimDir.Up],
            ""+singleAnimDic[UnitAnim.AnimDir.Left],
            ""+singleAnimDic[UnitAnim.AnimDir.Right],
            ""+singleAnimDic[UnitAnim.AnimDir.DownLeft],
            ""+singleAnimDic[UnitAnim.AnimDir.DownRight],
            ""+singleAnimDic[UnitAnim.AnimDir.UpLeft],
            ""+singleAnimDic[UnitAnim.AnimDir.UpRight],
        };
            return string.Join("#UNITANIMTYPE#", content);
        }
        public static UnitAnimType Load_Linked(string save) {
            string[] content = V_Animation.SplitString(save, "#UNITANIMTYPE#");

            UnitAnimType unitAnimType = new UnitAnimType(content[0]);
            unitAnimType.SetAnims(
                UnitAnim.GetUnitAnim(content[1]),
                UnitAnim.GetUnitAnim(content[2]),
                UnitAnim.GetUnitAnim(content[3]),
                UnitAnim.GetUnitAnim(content[4]),
                UnitAnim.GetUnitAnim(content[5]),
                UnitAnim.GetUnitAnim(content[6]),
                UnitAnim.GetUnitAnim(content[7]),
                UnitAnim.GetUnitAnim(content[8])
            );

            return unitAnimType;
        }






        public string Save() {
            //Returns a string to be used in savefiles
            string[] content = new string[]{
            name,
            singleAnimDic[UnitAnim.AnimDir.Down].Save(),
            singleAnimDic[UnitAnim.AnimDir.Up].Save(),
            singleAnimDic[UnitAnim.AnimDir.Left].Save(),
            singleAnimDic[UnitAnim.AnimDir.Right].Save(),
            singleAnimDic[UnitAnim.AnimDir.DownLeft].Save(),
            singleAnimDic[UnitAnim.AnimDir.DownRight].Save(),
            singleAnimDic[UnitAnim.AnimDir.UpLeft].Save(),
            singleAnimDic[UnitAnim.AnimDir.UpRight].Save(),
        };
            return string.Join("#UNITANIMTYPE#", content);
        }
        public static UnitAnimType Load(string save) {
            string[] content = V_Animation.SplitString(save, "#UNITANIMTYPE#");

            UnitAnimType unitAnimType = new UnitAnimType(content[0]);
            unitAnimType.SetAnims(
                UnitAnim.Load(content[1]),
                UnitAnim.Load(content[2]),
                UnitAnim.Load(content[3]),
                UnitAnim.Load(content[4]),
                UnitAnim.Load(content[5]),
                UnitAnim.Load(content[6]),
                UnitAnim.Load(content[7]),
                UnitAnim.Load(content[8])
            );

            return unitAnimType;
        }




        public static UnitAnimType CreateNew(string name) {
            return new UnitAnimType(name);
        }

        private static List<UnitAnimType> unitAnimTypeList;
        private static bool isDirty;
        public static void SetDirty() {
            isDirty = true;
        }
        public static void CheckDirty() {
            if (isDirty) {
                ReInit();
            }
        }
        public static void ReInit() {
            unitAnimTypeList = null;
            Init();
        }
        public static void Init() {
            if (unitAnimTypeList != null) return;
            isDirty = false;
            // Load all anim types
            // Called after UnitAnim.Init();
            unitAnimTypeList = new List<UnitAnimType>();

            if (V_Animation.DATA_LOCATION == V_Animation.DataLocation.Assets) {
                LoadFromDataFolder();
            } else {
                LoadFromResources();
            }

#if !SILENT
            Debug.Log("Loaded Animation Types: "+unitAnimTypeList.Count);
#endif
        }

        private static void LoadFromDataFolder() {
            // Load Default Animations
            DirectoryInfo defaultDir = new DirectoryInfo(V_Animation.LOC_DEFAULT_ANIMATIONTYPES);
            List<FileInfo> defaultFileInfoList = new List<FileInfo>(defaultDir.GetFiles("*." + V_Animation.fileExtention_AnimationType));
            foreach (FileInfo fileInfo in defaultFileInfoList) {
                string readAllText;
                SaveSystem.FileData fileData;
                if (SaveSystem.Load(V_Animation.LOC_DEFAULT_ANIMATIONTYPES, fileInfo.Name, out fileData)) {
                    // Loaded!
                    readAllText = fileData.save;
                } else {
                    // Load failed!
                    continue;
                }
                UnitAnimType unitAnimType = UnitAnimType.Load_Linked(readAllText);
                unitAnimType.disableOverwrite = true;
                unitAnimTypeList.Add(unitAnimType);
            }

            DirectoryInfo dir = new DirectoryInfo(V_Animation.LOC_ANIMATIONTYPES);
            List<FileInfo> fileInfoList = new List<FileInfo>(dir.GetFiles("*." + V_Animation.fileExtention_AnimationType));

            Debug.Log("ANIMATION TYPES FOUND: " + fileInfoList.Count);

            foreach (FileInfo fileInfo in fileInfoList) {
                string readAllText;
                SaveSystem.FileData fileData;
                if (SaveSystem.Load(V_Animation.LOC_ANIMATIONTYPES, fileInfo.Name, out fileData)) {
                    // Loaded!
                    readAllText = fileData.save;
                } else {
                    // Load failed!
                    continue;
                }
                UnitAnimType unitAnimType = UnitAnimType.Load_Linked(readAllText);
                unitAnimTypeList.Add(unitAnimType);
            }
        }
        
        private static void LoadFromResources() {
            //TextAsset animationResourceTreeTextAsset = Resources.Load<TextAsset>("AnimationData/animationResourceTreeTextAsset");
            //string folderCSV = animationResourceTreeTextAsset.text;
            string folderCSV = "_Default/AnimationTypes,AnimationTypes";

            string[] folderArr = V_Animation.SplitString(folderCSV, ",");
            foreach (string folder in folderArr) {
                if (folder != "") {
                    // Load resources in folder
                    TextAsset[] textAssetArr = Resources.LoadAll<TextAsset>("AnimationData/" + folder);
                    //Debug.Log("AnimationTypes folder: "+folder+"; Found AnimationTypes: "+ textAssetArr.Length);
                    foreach (TextAsset textAsset in textAssetArr) {
                        //Debug.Log("Loading: "+textAsset.name);
                        byte[] byteArr = textAsset.bytes;

                        string readAllText;
                        SaveSystem.FileData fileData;
                        if (SaveSystem.Load(byteArr, out fileData)) {
                            // Loaded!
                            readAllText = fileData.save;
                        } else {
                            // Load failed!
                            readAllText = null;
                        }

                        UnitAnimType unitAnimType = UnitAnimType.Load_Linked(readAllText);
                        unitAnimTypeList.Add(unitAnimType);
                    }
                }
            }
        }






        public static string LoadAnimTypeString(string fileNameWithExtension) {
            string readAllText;
            SaveSystem.FileData fileData;
            if (SaveSystem.Load(V_Animation.LOC_ANIMATIONTYPES, fileNameWithExtension, out fileData)) {
                // Loaded!
                readAllText = fileData.save;
            } else {
                // Load failed!
                return null;
            }
            return readAllText;
        }




        public static UnitAnimType GetUnitAnimType(string unitAnimTypeName) {
            V_Animation.Init();
            foreach (UnitAnimType unitAnimType in unitAnimTypeList) {
                if (unitAnimType.name == unitAnimTypeName) {
                    return unitAnimType;
                }
            }
            Debug.LogWarning("#### UNIT ANIM TYPE NOT FOUND: " + unitAnimTypeName);
            return null;
        }
        public static List<UnitAnimType> GetUnitAnimTypeList() {
            return unitAnimTypeList;
        }





        /*
         * Helper class for making fake enums that get auto converted to UnitAnimType
         * Usage: PlayAnim(UnitAnimTypeEnum.IdleDown);
         */
        public class BaseUnitAnimTypeEnum {

            // Add extra fake enums here
            public static BaseUnitAnimTypeEnum IdleDown { get { return new BaseUnitAnimTypeEnum(MethodBase.GetCurrentMethod()); } }
            public static UnitAnimType _Test2 = new BaseUnitAnimTypeEnum(MethodBase.GetCurrentMethod());


            private string str;

            protected BaseUnitAnimTypeEnum(MethodBase methodBase) {
                str = methodBase.Name;
            }

            public static implicit operator UnitAnimType(BaseUnitAnimTypeEnum unitAnimTypeEnum) { return UnitAnimType.GetUnitAnimType(unitAnimTypeEnum.str); }
        }


        
        private class UnitAnimTypeEnum {

            static UnitAnimTypeEnum() {
                FieldInfo[] fieldInfoArr = typeof(UnitAnimTypeEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo fieldInfo in fieldInfoArr) {
                    if (fieldInfo != null) {
                        fieldInfo.SetValue(null, UnitAnimType.GetUnitAnimType(fieldInfo.Name));
                    }
                }
            }

            //public static UnitAnimType ArrowAttack;

        }


    }


}