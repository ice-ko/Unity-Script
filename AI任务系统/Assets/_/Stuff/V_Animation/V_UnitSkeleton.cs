using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace V_AnimationSystem {

    public class V_UnitSkeleton {

        private int id = UnityEngine.Random.Range(10000, 99999);

        private Mesh mesh;
        private bool hasVariableSortingOrder;
        private bool alreadyLooped;


        private UnitAnim lastUnitAnim = UnitAnim.None;

        private float frameRateMod = 1f;

        private float refreshTimer;
        private float refreshTimerMax = .016f;

        private V_ISkeleton_Updater skeletonUpdater;

        public delegate void OnAnimComplete(UnitAnim unitAnim);
        private OnAnimComplete onAnimComplete;
        public delegate void OnAnimInterrupted(UnitAnim interruptedUnitAnim, UnitAnim unitAnim, bool alreadyLooped);
        private OnAnimInterrupted onAnimInterrupted;
        public delegate void OnAnimTrigger(string trigger);
        private OnAnimTrigger onAnimTrigger;
        public delegate Vector3 DelConvertLocalPositionToWorldPosition(Vector3 position);
        private DelConvertLocalPositionToWorldPosition ConvertLocalPositionToWorldPosition;

        public delegate void OnPointerMove(Vector3 pointer, int rot);
        private OnPointerMove onPointerMove_1, onPointerMove_2, onPointerMove_3;

        private Vector3 animPointer_1, animPointer_2, animPointer_3;

        private List<V_Skeleton_Anim> tmpAnims = new List<V_Skeleton_Anim>();

        public event OnAnimComplete OnAnyAnimComplete;
        public event OnAnimInterrupted OnAnyAnimInterrupted;
        public event OnAnimTrigger OnAnyAnimTrigger;
        public event Action<UnitAnim> OnAnyPlayAnim;

        public V_UnitSkeleton(float frameRateMod, DelConvertLocalPositionToWorldPosition ConvertLocalPositionToWorldPosition, Action<Mesh> SetMesh) {
            V_Animation.Init();
            this.frameRateMod = frameRateMod;
            this.ConvertLocalPositionToWorldPosition = ConvertLocalPositionToWorldPosition;
            mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();

            mesh.triangles = null;
            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            SetMesh(mesh);

            PlayAnim(UnitAnim.DefaultAnimation, 1f, null, null, null);
        }
        public void DestroySelf() {
            UnityEngine.Object.Destroy(mesh);
        }
        public V_Skeleton_Anim[] GetAnims() {
            return skeletonUpdater.GetAnims();
        }
        public V_ISkeleton_Updater GetSkeletonUpdater() {
            return skeletonUpdater;
        }
        public Vector3 GetBodyPartPosition(string bodyPartName) {
            V_Skeleton_Anim bodyPartAnim = GetSkeletonUpdater().GetAnimWithBodyPartName(bodyPartName);
            if (bodyPartAnim != null) {
                Vector3 pos = bodyPartAnim.GetCurrentAnimFrame().pos;
                return ConvertLocalPositionToWorldPosition(pos);
            } else {
                return ConvertLocalPositionToWorldPosition(Vector3.zero);
            }
        }

        // All body parts in this UnitAnim get replaced in the current anim, if one doesnt exist it is added
        public void ReplaceAllBodyPartsInAnimation(UnitAnim unitAnim) {
            foreach (V_Skeleton_Anim skeletonAnim in unitAnim.GetAnims()) {
                bool replaced = TryReplaceBodyPartSkeletonAnim(skeletonAnim);
                if (!replaced) {
                    // Didn't replace, body part doesn't exist, add
                    GetSkeletonUpdater().AddAnim(skeletonAnim);
                }
            }
        }

        public void ReplaceBodyPartSkeletonAnim(UnitAnim unitAnim, params string[] bodyPartNameArr) {
            foreach (string bodyPartName in bodyPartNameArr) {
                TryReplaceBodyPartSkeletonAnim(bodyPartName, unitAnim.GetSkeletonAnim_BodyPartCustom(bodyPartName));
            }
        }
        public bool TryReplaceBodyPartSkeletonAnim(string bodyPartName, UnitAnim unitAnim) {
            return TryReplaceBodyPartSkeletonAnim(bodyPartName, unitAnim.GetSkeletonAnim_BodyPartCustom(bodyPartName));
        }
        public bool TryReplaceBodyPartSkeletonAnim(string bodyPartName, V_Skeleton_Anim anim) {
            return GetSkeletonUpdater().TryReplaceAnimOnBodyPart(bodyPartName, anim);
        }
        public bool TryReplaceBodyPartSkeletonAnim(V_Skeleton_Anim anim) {
            return GetSkeletonUpdater().TryReplaceAnimOnBodyPart(anim.bodyPart.customName, anim);
        }

        public UnitAnim GetActiveUnitAnim() {
            return lastUnitAnim;
        }
        public void SetHasVariableSortingOrder(bool set) {
            hasVariableSortingOrder = set;
            skeletonUpdater.SetHasVariableSortingOrder(set);
        }
        public void Update(float deltaTime) {
            V_ISkeleton_Updater activeSkeletonUpdater = skeletonUpdater;
            bool allLooped = skeletonUpdater.Update(deltaTime);

            if (activeSkeletonUpdater != skeletonUpdater) {
                // Skeleton Updater changed during update
                return;
            }

            refreshTimer -= deltaTime;
            if (refreshTimer <= 0f) {
                refreshTimer = refreshTimerMax;
                skeletonUpdater.Refresh();
            }

            if (allLooped) {
                alreadyLooped = true;
            }
            if (allLooped && onAnimComplete != null) {
                OnAnimComplete backOnAnimComplete = onAnimComplete;
                onAnimComplete = null;
                backOnAnimComplete(lastUnitAnim);
                if (OnAnyAnimComplete != null) OnAnyAnimComplete(lastUnitAnim);
            } else {
                //UpdatePlay();
                //Play();
            }
        }
        public void SkeletonRefresh() {
            skeletonUpdater.Refresh();
        }



        public Vector3 GetAnimPointer(int pointer) {
            switch (pointer) {
            default:
            case 1: return ConvertLocalPositionToWorldPosition(animPointer_1);
            case 2: return ConvertLocalPositionToWorldPosition(animPointer_2);
            case 3: return ConvertLocalPositionToWorldPosition(animPointer_3);
            }
        }

        /*public void SetFrameRateMod(float frameRateMod) {
            this.frameRateMod = frameRateMod;

            if (anims != null) {
                foreach (V_Skeleton_Anim anim in anims) {
                    anim.SetFrameRateMod(frameRateMod);
                }
            }
        }
        public void SetFrameRateMod_DontStack(float frameRateMod) {
            this.frameRateMod = frameRateMod;

            if (anims != null) {
                foreach (V_Skeleton_Anim anim in anims) {
                    anim.SetFrameRateMod_DontStack(frameRateMod);
                }
            }
        }*/
        public bool ClearOnAnimInterruptedIfMatches(OnAnimInterrupted onAnimInterrupted) {
            if (this.onAnimInterrupted == onAnimInterrupted) {
                // Matches, clear!
                this.onAnimInterrupted = null;
                return true;
            }
            return false;
        }
        public bool PlayAnimIfOnCompleteMatches(UnitAnim unitAnim, OnAnimComplete onAnimComplete) {
            if (this.onAnimComplete == onAnimComplete) {
                // Matches!, Play anim
                PlayAnim(unitAnim, 1f, null, null, null);
                return true;
            }
            return false;
        }
        public void SetAnimsArr(V_Skeleton_Anim[] anims) {
            (skeletonUpdater as V_Skeleton_Updater).SetAnimsArr(anims);
        }
        public void PlayAnim(UnitAnim unitAnim, float frameRateModModifier, OnAnimComplete onAnimComplete, OnAnimTrigger onAnimTrigger, OnAnimInterrupted onAnimInterrupted) {
            if (this.onAnimInterrupted != null) {
                OnAnimInterrupted tmpAnimInterrupted = this.onAnimInterrupted;
                this.onAnimInterrupted = null;
                tmpAnimInterrupted(lastUnitAnim, unitAnim, alreadyLooped);
                if (OnAnyAnimInterrupted != null) OnAnyAnimInterrupted(lastUnitAnim, unitAnim, alreadyLooped);
            }

            this.onAnimComplete = onAnimComplete;
            this.onAnimTrigger = onAnimTrigger;
            this.onAnimInterrupted = onAnimInterrupted;

            lastUnitAnim = unitAnim;

            alreadyLooped = false;

            // Dynamic anim
            V_Skeleton_Updater newSkeletonUpdater = new V_Skeleton_Updater(mesh, unitAnim.GetAnims(), frameRateMod, frameRateModModifier, onAnimTrigger, OnAnyAnimTrigger);
            newSkeletonUpdater.TestFirstFrameTrigger();
            skeletonUpdater = newSkeletonUpdater;


            if (OnAnyPlayAnim != null) OnAnyPlayAnim(unitAnim);
        }
        public void PlayAnimContinueFrames(UnitAnim unitAnim, float frameRateModModifier, OnAnimComplete onAnimComplete, OnAnimTrigger onAnimTrigger, OnAnimInterrupted onAnimInterrupted) {
            //Play new anim starting from same frames
            if (this.onAnimInterrupted != null) {
                OnAnimInterrupted tmpAnimInterrupted = this.onAnimInterrupted;
                this.onAnimInterrupted = null;
                tmpAnimInterrupted(lastUnitAnim, unitAnim, alreadyLooped);
                if (OnAnyAnimInterrupted != null) OnAnyAnimInterrupted(lastUnitAnim, unitAnim, alreadyLooped);
            }

            this.onAnimComplete = onAnimComplete;
            this.onAnimTrigger = onAnimTrigger;
            this.onAnimInterrupted = onAnimInterrupted;

            lastUnitAnim = unitAnim;

            alreadyLooped = false;

            // Dynamic anim
            if (skeletonUpdater is V_Skeleton_Updater) {
                // Continuing from normal updater
                V_Skeleton_Updater newSkeletonUpdater = new V_Skeleton_Updater(mesh, unitAnim.GetAnims(), frameRateMod, frameRateModModifier, onAnimTrigger, OnAnyAnimTrigger);
                newSkeletonUpdater.SetFramesToSame((skeletonUpdater as V_Skeleton_Updater).GetAnims());
                skeletonUpdater = newSkeletonUpdater;
            } else {
                // Previous was cached updater, ignore continue frames
                skeletonUpdater = new V_Skeleton_Updater(mesh, unitAnim.GetAnims(), frameRateMod, frameRateModModifier, onAnimTrigger, OnAnyAnimTrigger);
            }
        }
        public void PlayAnimContinueFrames(UnitAnim unitAnim, float frameRateModModifier) {
            //Play new anim starting from same frames
            lastUnitAnim = unitAnim;

            alreadyLooped = false;

            // Dynamic anim
            if (skeletonUpdater is V_Skeleton_Updater) {
                // Continuing from normal updater
                V_Skeleton_Updater newSkeletonUpdater = new V_Skeleton_Updater(mesh, unitAnim.GetAnims(), frameRateMod, frameRateModModifier, onAnimTrigger, OnAnyAnimTrigger);
                newSkeletonUpdater.SetFramesToSame((skeletonUpdater as V_Skeleton_Updater).GetAnims());
                skeletonUpdater = newSkeletonUpdater;
            } else {
                // Previous was cached updater, ignore continue frames
                skeletonUpdater = new V_Skeleton_Updater(mesh, unitAnim.GetAnims(), frameRateMod, frameRateModModifier, onAnimTrigger, OnAnyAnimTrigger);
            }
        }
        public static void AddSquare(int verticesIndex, Vector3[] verticesArr, int uvsIndex, Vector2[] uvsArr, int trianglesIndex, int[] trianglesArr, Vector3 localPos, Vector3 v00, Vector3 v01, Vector3 v10, Vector3 v11, Vector2[] squareUV) {
            /* 01
             * 11
             * 00
             * 10 */
            verticesArr[verticesIndex] = localPos + v01;
            verticesArr[verticesIndex + 1] = localPos + v11;
            verticesArr[verticesIndex + 2] = localPos + v00;
            verticesArr[verticesIndex + 3] = localPos + v10;
            /* 0
             * 1
             * 2
             * 2
             * 1
             * 3 */
            trianglesArr[trianglesIndex] = verticesIndex;
            trianglesArr[trianglesIndex + 1] = verticesIndex + 1;
            trianglesArr[trianglesIndex + 2] = verticesIndex + 2;
            trianglesArr[trianglesIndex + 3] = verticesIndex + 2;
            trianglesArr[trianglesIndex + 4] = verticesIndex + 1;
            trianglesArr[trianglesIndex + 5] = verticesIndex + 3;

            for (int i = 0; i < squareUV.Length; i++) {
                uvsArr[uvsIndex + i] = squareUV[i];
            }
        }
        public static void AddSquare(List<Vector3> verticesList, List<Vector2> uvsList, List<int> trianglesList, Vector3 localPos, Vector3 v00, Vector3 v01, Vector3 v10, Vector3 v11, Vector2[] squareUV) {
            /* 01
             * 11
             * 00
             * 10 */
            int verticesIndex = verticesList.Count;
            verticesList.Add(localPos + v01);
            verticesList.Add(localPos + v11);
            verticesList.Add(localPos + v00);
            verticesList.Add(localPos + v10);
            /* 0
             * 1
             * 2
             * 2
             * 1
             * 3 */
            trianglesList.Add(verticesIndex);
            trianglesList.Add(verticesIndex + 1);
            trianglesList.Add(verticesIndex + 2);
            trianglesList.Add(verticesIndex + 2);
            trianglesList.Add(verticesIndex + 1);
            trianglesList.Add(verticesIndex + 3);

            for (int i = 0; i < squareUV.Length; i++) {
                uvsList.Add(squareUV[i]);
            }
        }

        public static Vector2[] GetUV_Type(UVType uvType) {
            return uvType.uvs;
            /*
            Vector2[] 
            uvHead_Up, uvHead_Down, uvHead_Left, uvHead_Right, 
            uvBody_Up, uvBody_Down, uvBody_Left, uvBody_Right,
            uvLHand, uvRHand,
            uvFoot_Up, uvFoot_Down, uvFoot_Left, uvFoot_Right,
            uvHat, uvSword, uvSword_InvertH, uvBow, uvArrow, uvShield, uvShieldBroken, uvTrap, uvTrap_InvertH,
            uvWeaponSecondary, uvStunRock,

            uvHairHat_Down, uvHairHat_Left, uvHairHat_Right, uvHairHat_Up;

            uvHead_Right = V_UnitSkeleton.GetUV(0, 128, 512, 640);
            uvBody_Right = V_UnitSkeleton.GetUV(128, 384, 256, 512);
            uvLHand = V_UnitSkeleton.GetUV(0, 128, 256, 384);

            uvHead_Left = V_UnitSkeleton.GetUV(128, 0, 512, 640);
            uvBody_Left = V_UnitSkeleton.GetUV(384, 128, 256, 512);

            uvHead_Down = V_UnitSkeleton.GetUV(0,128, 640, 768);
            uvBody_Down = V_UnitSkeleton.GetUV(128,384, 512, 768);
            uvFoot_Down = V_UnitSkeleton.GetUV(0, 128, 128, 256);

            uvHead_Up = V_UnitSkeleton.GetUV(0,128, 384, 512);
            uvBody_Up = V_UnitSkeleton.GetUV(128,384, 0, 256);

            uvSword = V_UnitSkeleton.GetUV(0,256, 768, 1024);
            uvSword_InvertH = V_UnitSkeleton.GetUV(256,0, 768, 1024);

            uvWeaponSecondary = V_UnitSkeleton.GetUV(256,512, 768, 1024);

            uvBow = V_UnitSkeleton.GetUV(0,256, 768, 1024);
            uvArrow = V_UnitSkeleton.GetUV(256,512, 768, 1024);

            uvShield = V_UnitSkeleton.GetUV(256,512, 768, 1024);


            uvHairHat_Down = V_UnitSkeleton.GetUV(384,640, 512, 768);
            uvHairHat_Right = V_UnitSkeleton.GetUV(384,640, 256, 512);
            uvHairHat_Left = V_UnitSkeleton.GetUV(640,384, 256, 512);
            uvHairHat_Up = V_UnitSkeleton.GetUV(384,640, 0, 256);

            switch (uvType.preset) {
            default:
            case UVType.Head_Down:
                return uvHead_Down;
            case UVType.Head_Up:
                return uvHead_Up;
            case UVType.Head_Left:
                return uvHead_Left;
            case UVType.Head_Right:
                return uvHead_Right;

            case UVType.Body_Down:
                return uvBody_Down;
            case UVType.Body_Up:
                return uvBody_Up;
            case UVType.Body_Left:
                return uvBody_Left;
            case UVType.Body_Right:
                return uvBody_Right;

            case UVType.Hand:
            case UVType.LHand:
            case UVType.RHand:
                return uvLHand;

            case UVType.Foot:
            case UVType.Foot_Up:
            case UVType.Foot_Down:
            case UVType.Foot_Left:
            case UVType.Foot_Right:
                return uvFoot_Down;

            /*case UVType.Hat:
                return uvHat;*

            case UVType.Sword:
                return uvSword;
            case UVType.Sword_InvertH:
                return uvSword_InvertH;

            case UVType.WeaponSecondary:
                return uvWeaponSecondary;

            case UVType.Bow:
                return uvBow;
            case UVType.Arrow:
                return uvArrow;

            case UVType.Shield:
                return uvShield;


            case UVType.HairHat_Down:
                return uvHairHat_Down;
            case UVType.HairHat_Up:
                return uvHairHat_Up;
            case UVType.HairHat_Left:
                return uvHairHat_Left;
            case UVType.HairHat_Right:
                return uvHairHat_Right;


            case UVType.Custom:
                return uvType.uvs;
            }*/
        }
        public static Vector2[] GetUV(float x0, float x1, float y0, float y1) {
            x0 /= 1024f;
            x1 /= 1024f;
            y0 /= 1024;
            y1 /= 1024;

            return new Vector2[]{
            new Vector2(x0,y1),
            new Vector2(x1,y1),
            new Vector2(x0,y0),
            new Vector2(x1,y0),
        };
        }








        /*
        public struct CachedAnimationMesh {
            // Stores mesh data per frame
            //public Old_UnitAnim unitAnim;
            public Vector3[][] verticesArrFrames;
            public Vector2[][] uvsArrFrames;
            public int[][] trianglesArrFrames;
            public bool uvsChange;
            public bool trianglesChange;
            public string[] triggerFrames;
            public bool[] hasTriggerFrames;
        }

        private static bool CanCacheAnimation(Old_UnitAnim unitAnim) {
            // Test if anim has no triggers
            V_Skeleton_Anim[] anims = Animations[unitAnim];
            for (int i=0; i<anims.Length; i++) {
                anims[i] = anims[i].Clone();
            }

            int animsLength = anims.Length;

            bool foundTrigger = false;
            UnitSkeleton.OnAnimTrigger onAnimTrigger = delegate (string trigger) { foundTrigger = true; };

            for (int i=0; i<animsLength; i++) {
                V_Skeleton_Anim anim = anims[i];
                anim.onAnimTrigger = onAnimTrigger;
            }

            while (true) {
                bool allLooped = true;

                for (int i=0; i<animsLength; i++) {
                    V_Skeleton_Anim anim = anims[i];
                    anim.UpdateNextFrame();
                    allLooped = allLooped && anim.looped;
                }

                if (allLooped || foundTrigger) {
                    break;
                }
            }
            // Can cache if didn't find trigger
            return !foundTrigger;
        }
        private static CachedAnimationMesh CacheAnimation(Old_UnitAnim unitAnim) {
            V_Skeleton_Anim[] anims = Animations[unitAnim];
            for (int i=0; i<anims.Length; i++) {
                anims[i] = anims[i].Clone();
            }

            Sprite_Tiles spriteTiles = new Sprite_Tiles(0);
            List<Vector3[]> verticesList = new List<Vector3[]>();
            List<Vector2[]> uvsList = new List<Vector2[]>();
            List<int[]> trianglesList = new List<int[]>();
            bool uvsChange = false;
            bool trianglesChange = false;
            List<string> triggerList = new List<string>();
            List<bool> hasTriggerList = new List<bool>();

            int animsLength = anims.Length;
            Vector2[] previousUV = null;
            int[] previousTriangles = null;
            while (true) {
                for (int i=0; i<animsLength; i++) {
                    for (int j=i+1; j<animsLength; j++) {
                        if (anims[j].GetCurrentFrame().sortingOrder < anims[i].GetCurrentFrame().sortingOrder) {
                            V_Skeleton_Anim tmp = anims[j];
                            anims[j] = anims[i];
                            anims[i] = tmp;
                        }
                    }
                }

                bool allLooped = true;

                for (int i=0; i<animsLength; i++) {
                    V_Skeleton_Anim anim = anims[i];
                    anim.UpdateNextFrame();
                    allLooped = allLooped && anim.looped;
                }

                // Save animation frame
                List<Vector3> vertices = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();
                List<int> triangles = new List<int>();

                string trigger = null;
                for (int i=0; i<animsLength; i++) {
                    V_Skeleton_Anim anim = anims[i];
                    V_Skeleton_Frame frame = anim.currentAnimFrame;
                    AddSquare(vertices, uvs, triangles, frame.pos, frame.v00+frame.v00offset, frame.v01+frame.v01offset, frame.v10+frame.v10offset, frame.v11+frame.v11offset, GetUV_Type(frame.uvType, spriteTiles));
                    if (!string.IsNullOrEmpty(frame.GetTrigger())) {
                        // Has trigger
                        trigger = frame.GetTrigger();
                    }
                }
                triggerList.Add(trigger);
                hasTriggerList.Add(!string.IsNullOrEmpty(trigger));

                // Check if UVs changed
                if (!uvsChange && previousUV != null) {
                    // UVs havent changed yet, check if different
                    if (previousUV.Length != uvs.Count) {
                        // Different size
                        uvsChange = true;
                    } else {
                        // Same size
                        for (int i=0; i<previousUV.Length; i++) {
                            if (previousUV[i] != uvs[i]) {
                                // Different
                                uvsChange = true;
                                break;
                            }
                        }
                    }
                }
                previousUV = uvs.ToArray();

                // Check if Triangles changed
                if (!trianglesChange && previousTriangles != null) {
                    // Triangles havent changed yet, check if different
                    if (previousTriangles.Length != triangles.Count) {
                        // Different size
                        trianglesChange = true;
                    } else {
                        // Same size
                        for (int i=0; i<previousTriangles.Length; i++) {
                            if (previousTriangles[i] != triangles[i]) {
                                // Different
                                trianglesChange = true;
                                break;
                            }
                        }
                    }
                }
                previousTriangles = triangles.ToArray();

                verticesList.Add(vertices.ToArray());
                uvsList.Add(uvs.ToArray());
                trianglesList.Add(triangles.ToArray());

                if (allLooped) {
                    break;
                }
            }

            if (!uvsChange) {
                // UVs dont change, cache only once
                uvsList = new List<Vector2[]>() { uvsList[0] };
            }
            if (!trianglesChange) {
                // Triangles dont change, cache only once
                trianglesList = new List<int[]>() { trianglesList[0] };
            }
            //Debug.Log("Caching Animation: " + unitAnim + "; Frames:" + verticesList.Count + "; UVsChange:" + uvsChange + "; TrianglesChange:" + trianglesChange + "; Trigger:"+hasTrigger);
            return new CachedAnimationMesh {
                unitAnim = unitAnim,
                verticesArrFrames = verticesList.ToArray(),
                uvsArrFrames = uvsList.ToArray(),
                trianglesArrFrames = trianglesList.ToArray(),
                uvsChange = uvsChange,
                trianglesChange = trianglesChange,
                triggerFrames = triggerList.ToArray(),
                hasTriggerFrames = hasTriggerList.ToArray()
            };
        }
        */
    }

}