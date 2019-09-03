/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using UnityEngine;
using System.Reflection;
using V_AnimationSystem;

public class GameAssets : MonoBehaviour {

    private static GameAssets _i;

    public static GameAssets i {
        get {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }


    
    public Material m_MarineSpriteSheet;

    public Sprite goldNodeDepletedSprite;







    
    public static class UnitAnimTypeEnum {

        static UnitAnimTypeEnum() {
            V_Animation.Init();
            FieldInfo[] fieldInfoArr = typeof(UnitAnimTypeEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fieldInfoArr) {
                if (fieldInfo != null) {
                    fieldInfo.SetValue(null, UnitAnimType.GetUnitAnimType(fieldInfo.Name));
                }
            }
        }

        public static UnitAnimType dSwordTwoHandedBack_Idle;
        public static UnitAnimType dSwordTwoHandedBack_Walk;
        public static UnitAnimType dSwordTwoHandedBack_Sword;
        public static UnitAnimType dSwordTwoHandedBack_Sword2;

        public static UnitAnimType dMinion_Idle;
        public static UnitAnimType dMinion_Walk;
        public static UnitAnimType dMinion_Attack;

        public static UnitAnimType dShielder_Idle;
        public static UnitAnimType dShielder_Walk;

        public static UnitAnimType dSwordShield_Idle;
        public static UnitAnimType dSwordShield_Walk;

        public static UnitAnimType dMarine_Idle;
        public static UnitAnimType dMarine_Walk;
        public static UnitAnimType dMarine_Attack;

        public static UnitAnimType dBareHands_Idle;
        public static UnitAnimType dBareHands_Walk;
        

    }




    public static class UnitAnimEnum {

        static UnitAnimEnum() {
            V_Animation.Init();
            FieldInfo[] fieldInfoArr = typeof(UnitAnimEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fieldInfoArr) {
                if (fieldInfo != null) {
                    fieldInfo.SetValue(null, UnitAnim.GetUnitAnim(fieldInfo.Name));
                }
            }
        }
        
        public static UnitAnim dMarine_AimWeaponRight;
        public static UnitAnim dMarine_AimWeaponRightInvertV;
        public static UnitAnim dMarine_ShootWeaponRight;
        public static UnitAnim dMarine_ShootWeaponRightInvertV;
        
    }

}
