using System;
using UnityEngine;
using System.Collections;

namespace V_AnimationSystem {

    /*
     * Handles choosing the specific angle animation for a given UnitAnimType
     * */
    public class V_UnitAnimation {

        // Cached for speed
        private static readonly Vector3 vector3Down = new Vector3(0, -1);
        private static readonly Vector3 vector3Zero = new Vector3(0, 0);
        private const float MathfRad2Deg = Mathf.Rad2Deg;


        private V_UnitSkeleton unitSkeleton;

        private UnitAnim activeAnim;
        private UnitAnimType activeAnimType;
        private int activeAngle;

        public V_UnitAnimation(V_UnitSkeleton unitSkeleton) {
            this.unitSkeleton = unitSkeleton;
        }



        public UnitAnim GetActiveAnim() {
            return activeAnim;
        }
        public UnitAnimType GetActiveAnimType() {
            return activeAnimType;
        }


        public void ClearOnAnimInterruptedIfMatches(V_UnitSkeleton.OnAnimInterrupted OnAnimInterrupted) {
            unitSkeleton.ClearOnAnimInterruptedIfMatches(OnAnimInterrupted);
        }
        public void PlayAnimIdleIfOnCompleteMatches(Vector3 dir, V_UnitSkeleton.OnAnimComplete OnAnimComplete, UnitAnimType idleAnimType) {
            int angle = GetAngleFromVector(dir);
            UnitAnim unitAnim = idleAnimType.GetUnitAnim(angle);
            if (unitSkeleton.PlayAnimIfOnCompleteMatches(unitAnim, OnAnimComplete)) {
                // Playing anim
                activeAnimType = idleAnimType;
                activeAnim = unitAnim;
                activeAngle = angle;
            }
        }



        public void PlayAnim(UnitAnimType animType, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            PlayAnim(animType, activeAngle, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }
        public void PlayAnim(UnitAnimType animType, Vector3 dir, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            int angle = GetAngleFromVector(dir);
            PlayAnim(animType, angle, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }
        public void PlayAnim(UnitAnimType animType, int angle, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            // Ignores if same animType, same angle and same frameRateMod

            // 8 angles
            if (animType == activeAnimType && activeAngle == angle) {
                // Same anim, same angle
                return;
            } else {
                if (animType != activeAnimType) {
                    // Different anim, same angle
                } else {
                    // Different angle, same anim
                }
            }
            PlayAnimForced(animType, angle, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }
        public void PlayAnim(UnitAnim unitAnim) {
            PlayAnim(unitAnim, 1f, null, null, null);
        }
        public void PlayAnim(UnitAnim unitAnim, Action onAnimComplete) {
            PlayAnim(unitAnim, 1f, (UnitAnim u) => { onAnimComplete(); }, null, null);
        }
        public void PlayAnim(UnitAnim unitAnim, float frameRateMod, Action onAnimComplete) {
            PlayAnim(unitAnim, frameRateMod, (UnitAnim u) => { onAnimComplete(); }, null, null);
        }
        public void PlayAnim(UnitAnim unitAnim, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            // Ignores if same animType, same angle and same frameRateMod

            if (unitAnim == activeAnim) {
                // Same anim, same angle
                return;
            }
            PlayAnimForced(unitAnim, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }


        public void PlayAnimForced(UnitAnimType animType, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            PlayAnimForced(animType, activeAngle, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }
        public void PlayAnimForced(UnitAnimType animType, Vector3 dir, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            int angle = GetAngleFromVector(dir);
            PlayAnimForced(animType, angle, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }
        public void PlayAnimForced(UnitAnimType animType, int angle, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            // Forcefully play animation no matter what is currently playing
            activeAnimType = animType;
            activeAngle = angle;

            UnitAnim unitAnim = animType.GetUnitAnim(angle);
            activeAnim = unitAnim;

            unitSkeleton.PlayAnim(unitAnim, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }
        public void PlayAnimForced(UnitAnim unitAnim, float frameRateMod, Action onAnimComplete) {
            PlayAnimForced(unitAnim, frameRateMod, (UnitAnim u) => { if (onAnimComplete != null) onAnimComplete(); }, null, null);
        }
        public void PlayAnimForced(UnitAnim unitAnim, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            // Forcefully play animation no matter what is currently playing
            activeAnimType = unitAnim.GetUnitAnimType();
            activeAnim = unitAnim;

            unitSkeleton.PlayAnim(unitAnim, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }



        public void UpdateAnim(UnitAnimType animType, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            UpdateAnim(animType, activeAngle, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }
        public void UpdateAnim(UnitAnimType animType, Vector3 dir, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            int angle = GetAngleFromVector(dir);
            UpdateAnim(animType, angle, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
        }
        public void UpdateAnim(UnitAnimType animType, int angle, float frameRateMod, V_UnitSkeleton.OnAnimComplete onAnimComplete, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimInterrupted onAnimInterrupted) {
            // Update animation if different angle
            if (animType == activeAnimType) {
                // Same anim, check angle
                if (activeAngle == angle) {
                    // Same angle, ignore
                    return;
                } else {
                    // Different angle
                    activeAngle = angle;
                    UnitAnim unitAnim = activeAnimType.GetUnitAnim(activeAngle);
                    activeAnim = unitAnim;
                    unitSkeleton.PlayAnimContinueFrames(unitAnim, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
                }
            } else {
                // Different anim
                PlayAnim(animType, angle, frameRateMod, onAnimComplete, onAnimTrigger, onAnimInterrupted);
            }
        }


        public void UpdateAnim(UnitAnimType animType, Vector3 dir, float frameRateMod) {
            int angle = GetAngleFromVector(dir);
            UpdateAnim(animType, angle, frameRateMod);
        }
        public void UpdateAnim(UnitAnimType animType, int angle, float frameRateMod) {
            // Update animation if different angle
            if (animType == activeAnimType) {
                // Same anim, check angle
                if (activeAngle == angle) {
                    // Same angle, ignore
                    return;
                } else {
                    // Different angle
                    activeAngle = angle;
                    UnitAnim unitAnim = activeAnimType.GetUnitAnim(activeAngle);
                    activeAnim = unitAnim;
                    unitSkeleton.PlayAnimContinueFrames(unitAnim, frameRateMod);
                }
            } else {
                // Different anim
                PlayAnim(animType, angle, frameRateMod, null, null, null);
            }
        }














        public static int GetAngleFromVector(Vector3 dir) {
            if (dir.x == 0f && dir.y == 0f) dir = vector3Down;

            double n = Math.Atan2(dir.y, dir.x) * MathfRad2Deg;
            if (n < 0) n += 360;
            int angle = (int)Math.Round(n / 45);

            return angle;
        }

        public static int GetDeepAngleFromVector(Vector3 dir) {
            if (dir.x == 0f && dir.y == 0f) dir = vector3Down;

            double n = Math.Atan2(dir.y, dir.x) * MathfRad2Deg;
            if (n < 0) n += 360;
            int angle = (int)Math.Round(n / 22);
            //if (angle == 16) angle = 0;

            return angle;
        }

    }

}