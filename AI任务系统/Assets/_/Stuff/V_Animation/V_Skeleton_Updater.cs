using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;

namespace V_AnimationSystem {

    public class V_Skeleton_Updater : V_ISkeleton_Updater {

        private int id;

        //private List<V_Skeleton_Anim> tmpAnims;
        public Dictionary<string, V_Skeleton_Anim> bodyPartNameAnimDic;

        private Mesh mesh;
        private bool hasVariableSortingOrder;
        private bool alreadyLooped;

        private Vector3[] verticesArr;
        private Vector2[] uvsArr;
        private int[] trianglesArr;
        private V_Skeleton_Anim[] anims;
        private int animsLength;

        public V_Skeleton_Updater(Mesh mesh, V_Skeleton_Anim[] loadAnims, float frameRateMod, float frameRateModModifier, V_UnitSkeleton.OnAnimTrigger onAnimTrigger, V_UnitSkeleton.OnAnimTrigger OnAnyAnimTrigger, int id = -1) {
            this.mesh = mesh;
            this.id = id;

            List<V_Skeleton_Anim> tmpAnims = new List<V_Skeleton_Anim>();
            hasVariableSortingOrder = false;
            for (int i = 0; i < loadAnims.Length; i++) {
                V_Skeleton_Anim clone = loadAnims[i].Clone();
                if (!hasVariableSortingOrder && clone.HasVariableSortingOrder()) hasVariableSortingOrder = true;
                clone.SetFrameRateMod(frameRateMod * frameRateModModifier);
                clone.onAnimTrigger = onAnimTrigger;
                clone.onAnimTrigger += OnAnyAnimTrigger;
                tmpAnims.Add(clone);
            }
            bodyPartNameAnimDic = new Dictionary<string, V_Skeleton_Anim>();
            anims = tmpAnims.ToArray();
            foreach (V_Skeleton_Anim anim in anims) {
                bodyPartNameAnimDic[anim.bodyPart.customName] = anim;
            }
            animsLength = anims.Length;


            verticesArr = new Vector3[animsLength * 4];
            uvsArr = new Vector2[animsLength * 4];
            trianglesArr = new int[animsLength * 6];


            Play();
        }

        public void TestFirstFrameTrigger() {
            for (int j = 0; j < anims.Length; j++) {
                anims[j].TestFirstFrameTrigger();
            }
        }
        public void SetFramesToSame(V_Skeleton_Anim[] previousAnims) {
            //Set frames to same
            for (int j = 0; j < previousAnims.Length; j++) {
                V_Skeleton_Anim anim = previousAnims[j];
                for (int i = 0; i < previousAnims.Length; i++) {
                    V_Skeleton_Anim newAnim = anims[i];
                    if (anim.bodyPart.Equals(newAnim.bodyPart)) {
                        //Same bodyPart
                        newAnim.SetCurrentFrame(anim.GetCurrentFrameNumberIndex());
                    }
                }
            }
            Play();
        }
        public V_Skeleton_Anim GetAnimWithBodyPartName(string bodyPartName) {
            if (bodyPartNameAnimDic.ContainsKey(bodyPartName)) {
                return bodyPartNameAnimDic[bodyPartName];
            } else {
                return null;
            }
        }
        public V_Skeleton_Anim[] GetAnims() {
            return anims;
        }
        public bool TryReplaceAnimOnBodyPart(string bodyPartName, V_Skeleton_Anim anim) {
            bool ret = false;
            for (int i = 0; i < anims.Length; i++) {
                if (anims[i].bodyPart.customName == bodyPartName) {
                    anims[i] = anim;
                    bodyPartNameAnimDic[anim.bodyPart.customName] = anim;
                    ret = true;
                }
            }
            return ret;
        }
        public void RemoveAnimBodyPart(string bodyPartName) {
            List<V_Skeleton_Anim> animList = new List<V_Skeleton_Anim>(anims);
            for (int i = 0; i < animList.Count; i++) {
                if (animList[i].bodyPart.customName == bodyPartName) {
                    // Remove this anim
                    animList.RemoveAt(i);
                    i--;
                }
            }
            SetAnimsArr(animList.ToArray());
        }
        public void AddAnim(V_Skeleton_Anim skeletonAnim) {
            List<V_Skeleton_Anim> animList = new List<V_Skeleton_Anim>(anims);
            animList.Add(skeletonAnim);
            SetAnimsArr(animList.ToArray());
        }
        public void SetAnimsArr(V_Skeleton_Anim[] anims) {
            this.anims = anims;
            animsLength = anims.Length;
            bodyPartNameAnimDic = new Dictionary<string, V_Skeleton_Anim>();
            foreach (V_Skeleton_Anim anim in anims) {
                bodyPartNameAnimDic[anim.bodyPart.customName] = anim;
            }

            verticesArr = new Vector3[animsLength * 4];
            uvsArr = new Vector2[animsLength * 4];
            trianglesArr = new int[animsLength * 6];
            Play();
        }

        public void SetHasVariableSortingOrder(bool set) {
            hasVariableSortingOrder = set;
        }
        public bool Update(float deltaTime) {
            bool allLooped = true;

            if (hasVariableSortingOrder) {
                for (int i = 0; i < animsLength; i++) {
                    for (int j = i + 1; j < animsLength; j++) {
                        if (anims[j].GetCurrentFrame().sortingOrder < anims[i].GetCurrentFrame().sortingOrder) {
                            V_Skeleton_Anim tmp = anims[j];
                            anims[j] = anims[i];
                            anims[i] = tmp;
                        }
                    }
                }
            }


            for (int i = 0; i < animsLength; i++) {
                V_Skeleton_Anim anim = anims[i];
                anim.Update(deltaTime);
                allLooped = allLooped && anim.looped;
            }

            return allLooped;
        }
        public void Refresh() {
            Play();
        }
        private void Play() {
            int verticesIndex = 0;
            int uvsIndex = 0;
            int trianglesIndex = 0;
            for (int i = 0; i < animsLength; i++) {
                V_Skeleton_Anim anim = anims[i];
                V_Skeleton_Frame frame = anim.GetCurrentAnimFrame();

                V_UnitSkeleton.AddSquare(verticesIndex, verticesArr, uvsIndex, uvsArr, trianglesIndex, trianglesArr, frame.pos, frame.v00 + frame.v00offset, frame.v01 + frame.v01offset, frame.v10 + frame.v10offset, frame.v11 + frame.v11offset, V_UnitSkeleton.GetUV_Type(frame.uvType));
                verticesIndex = verticesIndex + 4;
                uvsIndex = uvsIndex + 4;
                trianglesIndex = trianglesIndex + 6;
            }

            //mesh.triangles = null;
            mesh.Clear();
            mesh.vertices = verticesArr;
            mesh.uv = uvsArr;
            mesh.triangles = trianglesArr;
        }
    }




    public interface V_ISkeleton_Updater {

        bool Update(float deltaTime);
        void Refresh();
        void SetHasVariableSortingOrder(bool set);
        V_Skeleton_Anim[] GetAnims();
        V_Skeleton_Anim GetAnimWithBodyPartName(string bodyPartName);
        bool TryReplaceAnimOnBodyPart(string bodyPartName, V_Skeleton_Anim anim);

        void RemoveAnimBodyPart(string bodyPartName);
        void AddAnim(V_Skeleton_Anim skeletonAnim);
    }

}