using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_AnimationSystem {

    public class UVType {

        public int id;
        //public int preset;
        public string customName;
        public string textureName;
        public int textureWidth;
        public int textureHeight;
        public Vector2[] uvs;
        /*
        public UVType(int preset) {
            this.preset = preset;
            if (preset == Custom) {
                float x0, x1, y0, y1;
                x0 = y0 = 0f;
                x1 = y1 = 1f;
                uvs = new Vector2[] {
                    new Vector2(x0, y1),
                    new Vector2(x1, y1),
                    new Vector2(x0, y0),
                    new Vector2(x1, y0)
                };
            } else {
                uvs = null;
            }
            customName = ((Old_UVType)preset).ToString();
            id = Random.Range(10000, 99999);
        }
        public UVType(Vector2[] uvs) {
            this.uvs = uvs;
            preset = Custom;
            customName = ((Old_UVType)preset).ToString();
            id = Random.Range(10000, 99999);
        }*/
        public UVType(string customName, float x0, float y0, float x1, float y1, string textureName, int textureWidth, int textureHeight) {
            //preset = Custom;
            uvs = new Vector2[] {
            new Vector2(x0, y1),
            new Vector2(x1, y1),
            new Vector2(x0, y0),
            new Vector2(x1, y0)
        };
            this.customName = customName;
            this.textureName = textureName;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            id = Random.Range(10000, 99999);
        }
        public UVType(Vector2[] uvs, string customName, int id, string textureName, int textureWidth, int textureHeight) {
            //this.preset = preset;
            this.uvs = uvs;
            this.customName = customName;
            this.id = id;
            this.textureName = textureName;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
        }
        public override string ToString() {
            return customName;
            //return id + " " + customName;
        }
        public void GetUVCoords(out Vector2 v00, out Vector2 v11) {
            /*
            if (uvs == null || uvs.Length < 2) {
                Vector2[] tmpUVs = V_UnitSkeleton.GetUV_Type(this);
                /*
                Sprite_Tiles sprite_Tiles = new Sprite_Tiles(0);
                Vector2[] tmpUVs = null;
                switch (preset) {
                case UVType.Head_Down:      tmpUVs = sprite_Tiles.uvHead_Down;      break;
                case UVType.Head_Up:        tmpUVs = sprite_Tiles.uvHead_Up;        break;
                case UVType.Head_Left:      tmpUVs = sprite_Tiles.uvHead_Left;      break;
                case UVType.Head_Right:     tmpUVs = sprite_Tiles.uvHead_Right;     break;

                case UVType.Body_Down:      tmpUVs = sprite_Tiles.uvBody_Down;      break;
                case UVType.Body_Up:        tmpUVs = sprite_Tiles.uvBody_Up;        break;
                case UVType.Body_Left:      tmpUVs = sprite_Tiles.uvBody_Left;      break;
                case UVType.Body_Right:     tmpUVs = sprite_Tiles.uvBody_Right;     break;

                case UVType.Hand:
                case UVType.LHand:
                case UVType.RHand:
                    tmpUVs = sprite_Tiles.uvLHand;
                    break;

                case UVType.Foot:
                case UVType.Foot_Up:
                case UVType.Foot_Down:
                case UVType.Foot_Left:
                case UVType.Foot_Right:
                    tmpUVs = sprite_Tiles.uvFoot_Down;
                    break;

                case UVType.Hat:                tmpUVs = sprite_Tiles.uvFoot_Down;          break;
                case UVType.Sword:              tmpUVs = sprite_Tiles.uvSword;              break;
                case UVType.Sword_InvertH:      tmpUVs = sprite_Tiles.uvSword_InvertH;      break;
                case UVType.WeaponSecondary:    tmpUVs = sprite_Tiles.uvWeaponSecondary;    break;
                case UVType.Bow:                tmpUVs = sprite_Tiles.uvBow;                break;
                case UVType.Arrow:              tmpUVs = sprite_Tiles.uvArrow;              break;
                case UVType.StunRock:           tmpUVs = sprite_Tiles.uvStunRock;           break;
                case UVType.Shield:             tmpUVs = sprite_Tiles.uvShield;             break;
                case UVType.Trap:               tmpUVs = sprite_Tiles.uvTrap;               break;
                case UVType.Trap_InvertH:       tmpUVs = sprite_Tiles.uvTrap_InvertH;       break;

                case UVType.HairHat_Down:   tmpUVs = sprite_Tiles.uvHairHat_Down;   break;
                case UVType.HairHat_Up:     tmpUVs = sprite_Tiles.uvHairHat_Up;     break;
                case UVType.HairHat_Left:   tmpUVs = sprite_Tiles.uvHairHat_Left;   break;
                case UVType.HairHat_Right:  tmpUVs = sprite_Tiles.uvHairHat_Right;  break;
                }*
                if (tmpUVs != null) {
                    v00 = tmpUVs[2];
                    v11 = tmpUVs[1];
                    return;
                }
                v00 = new Vector2(0,0);
                v11 = new Vector2(1,1);
                return;
            }*/
            v00 = uvs[2];
            v11 = uvs[1];
        }
        public string GetUVCoordsString() {
            Vector2 v00 = uvs[2];
            Vector2 v11 = uvs[1];
            return "[" + v00.x + "," + v00.y + "]-[" + v11.x + "," + v11.y + "]";
        }
        public void SetTextureName(string textureName) {
            this.textureName = textureName;
        }
        public void SetTextureSize(int textureWidth, int textureHeight) {
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
        }
        public void SetCustomName(string customName) {
            this.customName = customName;
        }
        public void SetUVCoords(float x0, float y0, float x1, float y1) {
            uvs = new Vector2[] {
            new Vector2(x0, y1),
            new Vector2(x1, y1),
            new Vector2(x0, y0),
            new Vector2(x1, y0)
        };
        }
        public void GetUVCoords(out float x0, out float y0, out float x1, out float y1) {
            Vector2 v00 = uvs[2];
            Vector2 v11 = uvs[1];
            x0 = v00.x;
            y0 = v00.y;
            x1 = v11.x;
            y1 = v11.y;
        }
        public float GetHeightRatio() {
            if (uvs == null || uvs.Length < 4) return 1f;
            Vector2 v00 = uvs[2];
            Vector2 v11 = uvs[1];
            float width = Mathf.Abs(v11.x - v00.x);
            float height = Mathf.Abs(v11.y - v00.y);
            float ratio = height / width;
            float textureRatio = textureHeight * 1f / textureWidth;
            return ratio * textureRatio;
        }


        public static string Save_Static(UVType single) {
            return single.Save();
        }
        public string Save() {
            //Returns a string to be used in savefiles
            string[] content = new string[]{
            ""+99,
            V_Animation.Save_Array<Vector2>(uvs, delegate (Vector2 vec) { return V_Animation.Save_Vector2(vec); }, "#VECTOR2ARR#"),
            ""+customName,
            ""+id,
            ""+textureName,
            ""+textureWidth,
            ""+textureHeight,
        };
            return string.Join("#UVTYPE#", content);
        }

        public static UVType Load(string save) {
            if (!save.Contains("#UVTYPE#")) {
                // OLD Enum based UVType
                //Debug.LogError("OLD Enum based UVType");

                //Old_UVType oldUvType = MyUtils.GetEnumFromString<Old_UVType>(save);
                //return new UVType((int)oldUvType);
                
                switch (save) {
                case "Foot":        return UVType.dFoot;
                case "Body_Down":   return UVType.dBodyDown;
                case "Body_Up":     return UVType.dBodyUp;
                case "Body_Left":   return UVType.dBodyLeft;
                case "Body_Right":  return UVType.dBodyRight;
                case "Head_Down":   return UVType.dHeadDown;
                case "Head_Up":     return UVType.dHeadUp;
                case "Head_Left":   return UVType.dHeadLeft;
                case "Head_Right":  return UVType.dHeadRight;
                case "Hand":        return UVType.dHand;
                case "Sword":       return UVType.dWeapon;
                case "Sword_InvertH":   return UVType.dWeapon_InvertH;
                }

                return new UVType(save, 0f, 0f, 1f, 1f, "defaultSpriteSheet", 512, 512);
            }
            string[] content = V_Animation.SplitString(save, "#UVTYPE#");

            int preset = V_Animation.Parse_Int(content[0]);
            Vector2[] uvs = V_Animation.Load_Array<Vector2>(content[1], delegate (string str) { return V_Animation.Load_Vector2(str); }, "#VECTOR2ARR#");
            //MyUtils.StringArrPushIfIndex(2, ref content, ((Old_UVType)preset).ToString());
            V_Animation.StringArrPushIfIndex(2, ref content, "Head_Down");
            string customName = content[2];
            V_Animation.StringArrPushIfIndex(3, ref content, "0");
            int id = V_Animation.Parse_Int(content[3]);
            V_Animation.StringArrPushIfIndex(4, ref content, "");
            string textureName = content[4];
            V_Animation.StringArrPushIfIndex(5, ref content, "1024");
            int textureWidth = V_Animation.Parse_Int(content[5]);
            V_Animation.StringArrPushIfIndex(6, ref content, "1024");
            int textureHeight = V_Animation.Parse_Int(content[6]);

            return new UVType(uvs, customName, id, textureName, textureWidth, textureHeight);
        }





        public static void Init() {
            float defaultTextureWidth = 512;
            float defaultTextureHeight = 512;
            int x0, x1, y0, y1;

            x0 = 0; x1 = 128; y0 = 384; y1 = 512;
            dHeadDown = new UVType("Default Head Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 100 };

            x0 = 256; x1 = 384; y0 = 384; y1 = 512;
            dHeadUp = new UVType("Default Head Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 101 };

            x0 = 256; x1 = 128; y0 = 384; y1 = 512;
            dHeadLeft = new UVType("Default Head Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 102 };

            x0 = 128; x1 = 256; y0 = 384; y1 = 512;
            dHeadRight = new UVType("Default Head Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 103 };

            x0 = 0; x1 = 128; y0 = 256; y1 = 384;
            dBodyDown = new UVType("Default Body Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 104 };

            x0 = 256; x1 = 384; y0 = 256; y1 = 384;
            dBodyUp = new UVType("Default Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 105 };

            x0 = 256; x1 = 128; y0 = 256; y1 = 384;
            dBodyLeft = new UVType("Default Body Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 106 };

            x0 = 128; x1 = 256; y0 = 256; y1 = 384;
            dBodyRight = new UVType("Default Body Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 107 };

            x0 = 384; x1 = 448; y0 = 448; y1 = 512;
            dHand = new UVType("Default Hand", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 108 };

            x0 = 448; x1 = 512; y0 = 448; y1 = 512;
            dFoot = new UVType("Default Foot", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 109 };

            x0 = 0; x1 = 128; y0 = 128; y1 = 256;
            dWeapon = new UVType("Default Weapon", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 110 };

            x0 = 128; x1 = 256; y0 = 128; y1 = 256;
            dSecondary = new UVType("Default Secondary", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 111 };

            x0 = 128; x1 = 512; y0 = 0; y1 = 128;
            dRifle = new UVType("Default Rifle", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 112 };

            x0 = 512; x1 = 128; y0 = 0; y1 = 128;
            dRifle_InvertH = new UVType("Default Rifle Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 113 };

            x0 = 384; x1 = 512; y0 = 256; y1 = 384;
            dMuzzleFlash = new UVType("Default Muzzle Flash", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 114 };

            x0 = 384; x1 = 512; y0 = 0; y1 = 256;
            dAxe = new UVType("Default Axe", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 115 };

            x0 = 512; x1 = 384; y0 = 0; y1 = 256;
            dAxe_InvertH = new UVType("Default Axe Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 116 };

            x0 = 128; x1 = 0; y0 = 128; y1 = 256;
            dWeapon_InvertH = new UVType("Default Weapon Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 117 };
            
            x0 = 128; x1 = 512; y0 = 128; y1 = 0;
            dRifle_InvertV = new UVType("Default Rifle Invert V", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 118 };

            Init_Spartan();
            Init_Persian();
            Init_Ogre();
            Init_Marine();
            Init_Zombie();
            Init_Chicken();
        }

        public static UVType dHeadDown;
        public static UVType dHeadUp;
        public static UVType dHeadLeft;
        public static UVType dHeadRight;

        public static UVType dBodyDown;
        public static UVType dBodyUp;
        public static UVType dBodyLeft;
        public static UVType dBodyRight;

        public static UVType dHand;
        public static UVType dFoot;

        public static UVType dWeapon;
        public static UVType dWeapon_InvertH;
        public static UVType dSecondary;

        public static UVType dRifle;
        public static UVType dRifle_InvertH;
        public static UVType dRifle_InvertV;
        public static UVType dMuzzleFlash;
        public static UVType dAxe;
        public static UVType dAxe_InvertH;




        private static void Init_Spartan() {
            float defaultTextureWidth = 1024;
            float defaultTextureHeight = 512;
            int x0, x1, y0, y1;

            x0 = 279; x1 = 376; y0 = 316; y1 = 467;
            dSpartan_HeadDown = new UVType("Default Spartan Head Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 200 };

            x0 = 610; x1 = 712; y0 = 316; y1 = 467;
            dSpartan_HeadUp = new UVType("Default Spartan Head Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 201 };

            x0 = 438; x1 = 556; y0 = 316; y1 = 467;
            dSpartan_HeadRight = new UVType("Default Spartan Head Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 202 };

            x0 = 556; x1 = 438; y0 = 316; y1 = 467;
            dSpartan_HeadLeft = new UVType("Default Spartan Head Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 203 };

            x0 = 247; x1 = 407; y0 = 54; y1 = 299;
            dSpartan_BodyDown = new UVType("Default Spartan Body Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 204 };

            x0 = 574; x1 = 743; y0 = 54; y1 = 299;
            dSpartan_BodyUp = new UVType("Default Spartan Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 205 };

            x0 = 414; x1 = 552; y0 = 54; y1 = 299;
            dSpartan_BodyRight = new UVType("Default Spartan Body Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 206 };

            x0 = 552; x1 = 414; y0 = 54; y1 = 299;
            dSpartan_BodyLeft = new UVType("Default Spartan Body Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 207 };

            x0 = 886; x1 = 949; y0 = 389; y1 = 461;
            dSpartan_Hand = new UVType("Default Spartan Hand", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 208 };

            x0 = 886; x1 = 959; y0 = 258; y1 = 338;
            dSpartan_Foot = new UVType("Default Spartan Foot", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 209 };

            x0 = 775; x1 = 836; y0 = 44; y1 = 485;
            dSpartan_Spear = new UVType("Default Spartan Spear", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 210 };

            x0 = 31; x1 = 238; y0 = 168; y1 = 379;
            dSpartan_Shield = new UVType("Default Spartan Shield", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 211 };
        }
        public static UVType dSpartan_HeadDown;
        public static UVType dSpartan_HeadUp;
        public static UVType dSpartan_HeadLeft;
        public static UVType dSpartan_HeadRight;

        public static UVType dSpartan_BodyDown;
        public static UVType dSpartan_BodyUp;
        public static UVType dSpartan_BodyLeft;
        public static UVType dSpartan_BodyRight;

        public static UVType dSpartan_Hand;
        public static UVType dSpartan_Foot;
        public static UVType dSpartan_Spear;
        public static UVType dSpartan_Shield;



        private static void Init_Persian() {
            float defaultTextureWidth = 1024;
            float defaultTextureHeight = 512;
            int x0, x1, y0, y1;

            x0 = 266; x1 = 391; y0 = 338; y1 = 485;
            dPersian_HeadDown = new UVType("Default Persian Head Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 300 };

            x0 = 601; x1 = 720; y0 = 338; y1 = 485;
            dPersian_HeadUp = new UVType("Default Persian Head Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 301 };

            x0 = 432; x1 = 553; y0 = 338; y1 = 485;
            dPersian_HeadRight = new UVType("Default Persian Head Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 302 };

            x0 = 553; x1 = 432; y0 = 338; y1 = 485;
            dPersian_HeadLeft = new UVType("Default Persian Head Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 303 };

            x0 = 234; x1 = 421; y0 = 34; y1 = 298;
            dPersian_BodyDown = new UVType("Default Persian Body Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 304 };

            x0 = 566; x1 = 754; y0 = 34; y1 = 298;
            dPersian_BodyUp = new UVType("Default Persian Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 305 };

            x0 = 421; x1 = 555; y0 = 34; y1 = 298;
            dPersian_BodyRight = new UVType("Default Persian Body Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 306 };

            x0 = 555; x1 = 421; y0 = 34; y1 = 298;
            dPersian_BodyLeft = new UVType("Default Persian Body Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 307 };

            x0 = 876; x1 = 962; y0 = 377; y1 = 470;
            dPersian_Hand = new UVType("Default Persian Hand", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 308 };

            x0 = 884; x1 = 963; y0 = 257; y1 = 339;
            dPersian_Foot = new UVType("Default Persian Foot", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 309 };

            x0 = 770; x1 = 859; y0 = 158; y1 = 481;
            dPersian_Sword = new UVType("Default Persian Sword", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 310 };

            x0 = 14; x1 = 227; y0 = 145; y1 = 417;
            dPersian_Shield = new UVType("Default Persian Shield", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 311 };

            x0 = 859; x1 = 770; y0 = 158; y1 = 481;
            dPersian_Sword_InvertH = new UVType("Default Persian Sword Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 312 };
        }
        public static UVType dPersian_HeadDown;
        public static UVType dPersian_HeadUp;
        public static UVType dPersian_HeadLeft;
        public static UVType dPersian_HeadRight;

        public static UVType dPersian_BodyDown;
        public static UVType dPersian_BodyUp;
        public static UVType dPersian_BodyLeft;
        public static UVType dPersian_BodyRight;

        public static UVType dPersian_Hand;
        public static UVType dPersian_Foot;
        public static UVType dPersian_Sword;
        public static UVType dPersian_Sword_InvertH;
        public static UVType dPersian_Shield;



        private static void Init_Ogre() {
            float defaultTextureWidth = 1024;
            float defaultTextureHeight = 512;
            int x0, x1, y0, y1;

            x0 = 191; x1 = 325; y0 = 311; y1 = 467;
            dOgre_HeadDown = new UVType("Default Ogre Head Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 400 };

            x0 = 666; x1 = 803; y0 = 311; y1 = 467;
            dOgre_HeadUp = new UVType("Default Ogre Head Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 401 };

            x0 = 446; x1 = 571; y0 = 311; y1 = 467;
            dOgre_HeadRight = new UVType("Default Ogre Head Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 402 };

            x0 = 571; x1 = 446; y0 = 311; y1 = 467;
            dOgre_HeadLeft = new UVType("Default Ogre Head Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 403 };

            x0 = 157; x1 = 361; y0 = 0; y1 = 306;
            dOgre_BodyDown = new UVType("Default Ogre Body Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 404 };

            x0 = 630; x1 = 839; y0 = 0; y1 = 306;
            dOgre_BodyUp = new UVType("Default Ogre Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 405 };

            x0 = 392; x1 = 579; y0 = 0; y1 = 306;
            dOgre_BodyRight = new UVType("Default Ogre Body Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 406 };

            x0 = 579; x1 = 392; y0 = 0; y1 = 306;
            dOgre_BodyLeft = new UVType("Default Ogre Body Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 407 };

            x0 = 877; x1 = 968; y0 = 309; y1 = 409;
            dOgre_Hand = new UVType("Default Ogre Hand", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 408 };

            x0 = 880; x1 = 967; y0 = 202; y1 = 288;
            dOgre_Foot = new UVType("Default Ogre Foot", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 409 };

            x0 = 40; x1 = 145; y0 = 120; y1 = 412;
            dOgre_Club = new UVType("Default Ogre Club", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 410 };
        }
        public static UVType dOgre_HeadDown;
        public static UVType dOgre_HeadUp;
        public static UVType dOgre_HeadLeft;
        public static UVType dOgre_HeadRight;

        public static UVType dOgre_BodyDown;
        public static UVType dOgre_BodyUp;
        public static UVType dOgre_BodyLeft;
        public static UVType dOgre_BodyRight;

        public static UVType dOgre_Hand;
        public static UVType dOgre_Foot;
        public static UVType dOgre_Club;



        private static void Init_Marine() {
            float defaultTextureWidth = 1024;
            float defaultTextureHeight = 512;
            int x0, x1, y0, y1;

            x0 = 275; x1 = 387; y0 = 276; y1 = 425;
            dMarine_HeadDown = new UVType("Default Marine Head Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 500 };

            x0 = 608; x1 = 716; y0 = 276; y1 = 425;
            dMarine_HeadUp = new UVType("Default Marine Head Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 501 };

            x0 = 430; x1 = 553; y0 = 276; y1 = 425;
            dMarine_HeadRight = new UVType("Default Marine Head Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 502 };

            x0 = 553; x1 = 430; y0 = 276; y1 = 425;
            dMarine_HeadLeft = new UVType("Default Marine Head Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 503 };

            x0 = 241; x1 = 412; y0 = 57; y1 = 270;
            dMarine_BodyDown = new UVType("Default Marine Body Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 504 };

            x0 = 576; x1 = 743; y0 = 57; y1 = 270;
            dMarine_BodyUp = new UVType("Default Marine Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 505 };

            x0 = 420; x1 = 557; y0 = 57; y1 = 270;
            dMarine_BodyRight = new UVType("Default Marine Body Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 506 };

            x0 = 557; x1 = 420; y0 = 57; y1 = 270;
            dMarine_BodyLeft = new UVType("Default Marine Body Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 507 };

            x0 = 888; x1 = 949; y0 = 331; y1 = 395;
            dMarine_Hand = new UVType("Default Marine Hand", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 508 };

            x0 = 888; x1 = 954; y0 = 198; y1 = 272;
            dMarine_Foot = new UVType("Default Marine Foot", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 509 };

            x0 = 70; x1 = 192; y0 = 90; y1 = 450;
            dMarine_Gun = new UVType("Default Marine Gun", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 510 };

            x0 = 192; x1 = 70; y0 = 90; y1 = 450;
            dMarine_Gun_InvertH = new UVType("Default Marine Gun Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 511 };

            x0 = 876; x1 = 970; y0 = 26; y1 = 158;
            dMarine_MuzzleFlash = new UVType("Default Marine Muzzle Flash", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 512 };
        }
        public static UVType dMarine_HeadDown;
        public static UVType dMarine_HeadUp;
        public static UVType dMarine_HeadLeft;
        public static UVType dMarine_HeadRight;

        public static UVType dMarine_BodyDown;
        public static UVType dMarine_BodyUp;
        public static UVType dMarine_BodyLeft;
        public static UVType dMarine_BodyRight;

        public static UVType dMarine_Hand;
        public static UVType dMarine_Foot;
        public static UVType dMarine_Gun;
        public static UVType dMarine_Gun_InvertH;
        public static UVType dMarine_MuzzleFlash;



        private static void Init_Zombie() {
            float defaultTextureWidth = 1024;
            float defaultTextureHeight = 1024;
            int x0, x1, y0, y1;

            x0 = 273; x1 = 387; y0 = 797; y1 = 940;
            dZombie_HeadDown = new UVType("Default Zombie Head Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 600 };

            x0 = 603; x1 = 715; y0 = 797; y1 = 940;
            dZombie_HeadUp = new UVType("Default v Head Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 601 };

            x0 = 432; x1 = 546; y0 = 797; y1 = 940;
            dZombie_HeadRight = new UVType("Default Zombie Head Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 602 };

            x0 = 546; x1 = 432; y0 = 797; y1 = 940;
            dZombie_HeadLeft = new UVType("Default Zombie Head Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 603 };

            x0 = 252; x1 = 403; y0 = 588; y1 = 783;
            dZombie_BodyDown = new UVType("Default Zombie Body Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 604 };

            x0 = 580; x1 = 735; y0 = 588; y1 = 783;
            dZombie_BodyUp = new UVType("Default Zombie Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 605 };

            x0 = 428; x1 = 547; y0 = 588; y1 = 783;
            dZombie_BodyRight = new UVType("Default Zombie Body Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 606 };

            x0 = 547; x1 = 428; y0 = 588; y1 = 783;
            dZombie_BodyLeft = new UVType("Default Zombie Body Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 607 };

            x0 = 888; x1 = 947; y0 = 843; y1 = 904;
            dZombie_Hand = new UVType("Default Zombie Hand", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 608 };

            x0 = 888; x1 = 956; y0 = 713; y1 = 785;
            dZombie_Foot = new UVType("Default Zombie Foot", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 609 };

            x0 = 775; x1 = 877; y0 = 618; y1 = 931;
            dZombie_AxeBloody = new UVType("Default Zombie Axe Bloody", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 610 };

            x0 = 877; x1 = 775; y0 = 618; y1 = 931;
            dZombie_AxeBloody_InvertH = new UVType("Default Zombie Axe Bloody Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 611 };

            x0 = 775; x1 = 877; y0 = 240; y1 = 549;
            dZombie_Axe = new UVType("Default Zombie Axe", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 612 };

            x0 = 877; x1 = 775; y0 = 240; y1 = 549;
            dZombie_Axe_InvertH = new UVType("Default Zombie Axe Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 613 };

            x0 = 33; x1 = 211; y0 = 653; y1 = 836;
            dZombie_ShieldBloody = new UVType("Default Zombie Shield Bloody", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 614 };

            x0 = 33; x1 = 211; y0 = 278; y1 = 458;
            dZombie_Shield = new UVType("Default Zombie Shield", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 615 };
        }
        public static UVType dZombie_HeadDown;
        public static UVType dZombie_HeadUp;
        public static UVType dZombie_HeadLeft;
        public static UVType dZombie_HeadRight;

        public static UVType dZombie_BodyDown;
        public static UVType dZombie_BodyUp;
        public static UVType dZombie_BodyLeft;
        public static UVType dZombie_BodyRight;

        public static UVType dZombie_Hand;
        public static UVType dZombie_Foot;
        public static UVType dZombie_AxeBloody;
        public static UVType dZombie_AxeBloody_InvertH;
        public static UVType dZombie_Axe;
        public static UVType dZombie_Axe_InvertH;
        public static UVType dZombie_ShieldBloody;
        public static UVType dZombie_Shield;



        private static void Init_Chicken() {
            float defaultTextureWidth = 1024;
            float defaultTextureHeight = 256;
            int x0, x1, y0, y1;

            x0 = 74; x1 = 210; y0 = 69; y1 = 219;
            dChicken_BodyDown = new UVType("Default Chicken Body Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 700 };

            x0 = 465; x1 = 595; y0 = 69; y1 = 219;
            dChicken_BodyUp = new UVType("Default Chicken Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 701 };

            x0 = 261; x1 = 403; y0 = 69; y1 = 219;
            dChicken_BodyRight = new UVType("Default Chicken Body Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 702 };

            x0 = 403; x1 = 261; y0 = 69; y1 = 219;
            dChicken_BodyLeft = new UVType("Default Chicken Body Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 703 };
        }

        public static UVType dChicken_BodyDown;
        public static UVType dChicken_BodyUp;
        public static UVType dChicken_BodyLeft;
        public static UVType dChicken_BodyRight;









        /*
        public const int Head_Down = 0;
        public const int Head_Up = 1;
        public const int Head_Left = 2;
        public const int Head_Right = 3;

        public const int Body_Down = 4;
        public const int Body_Up = 5;
        public const int Body_Left = 6;
        public const int Body_Right = 7;

        public const int Hand = 8;
        public const int LHand = 9;
        public const int RHand = 10;

        public const int Foot = 11;
        public const int Foot_Up = 12;
        public const int Foot_Down = 13;
        public const int Foot_Left = 14;
        public const int Foot_Right = 15;

        public const int Hat = 16;
        public const int Sword = 17;
        public const int Sword_InvertH = 18;
        public const int Bow = 19;
        public const int Arrow = 20;
        public const int Shield = 21;
        public const int Trap = 22;
        public const int Trap_InvertH = 23;

        public const int StunRock = 24;

        public const int WeaponSecondary = 25;

        public const int HairHat_Down = 26;
        public const int HairHat_Up = 27;
        public const int HairHat_Left = 28;
        public const int HairHat_Right = 29;

        public const int Custom = 99;
        */
    }
    /*
    public enum Old_UVType {
        Head_Down,
        Head_Up,
        Head_Left,
        Head_Right,

        Body_Down,
        Body_Up,
        Body_Left,
        Body_Right,

        Hand,
        LHand,
        RHand,

        Foot,
        Foot_Up,
        Foot_Down,
        Foot_Left,
        Foot_Right,

        Hat,
        Sword,
        Sword_InvertH,
        Bow,
        Arrow,
        Shield,
        Trap,
        Trap_InvertH,

        StunRock,

        WeaponSecondary,

        HairHat_Down,
        HairHat_Up,
        HairHat_Left,
        HairHat_Right,

        Custom,
    }*/

}