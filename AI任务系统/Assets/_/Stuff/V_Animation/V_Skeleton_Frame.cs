using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace V_AnimationSystem {

    public class V_Skeleton_Frame {

        public const string VALID_TRIGGER_DIC = "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ0123456789_";

        public int frameCount;
        public Vector3 pos;
        public Vector3 basePosition;
        public Vector2 pivot;
        public Vector3 v00, v01, v10, v11;
        public Vector3 v00offset, v01offset, v10offset, v11offset;
        public float rot;
        public float baseRotation;
        public string trigger;
        public UVType uvType;
        public float size;
        public float scaleX;
        public float scaleY;
        public int sortingOrder;
        public int baseSortingOrder;

        public V_Skeleton_Frame(int _frameCount, Vector3 _pos, float size, float _rot, string _trigger, UVType _uvType, float scaleX, float scaleY, int sortingOrder, Vector2 pivot,
                Vector3 v00offset, Vector3 v01offset, Vector3 v10offset, Vector3 v11offset) {
            frameCount = _frameCount;
            pos = _pos;
            basePosition = pos;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.sortingOrder = sortingOrder;
            baseSortingOrder = sortingOrder;
            this.pivot = pivot;
            this.v00offset = v00offset;
            this.v01offset = v01offset;
            this.v10offset = v10offset;
            this.v11offset = v11offset;
            SetNewSize(size);
            rot = _rot;
            baseRotation = rot;
            RefreshVertices();
            trigger = _trigger;
            uvType = _uvType;
        }
        public V_Skeleton_Frame(int _frameCount, Vector3 _pos, float _rot, string _trigger, UVType _uvType, float size, float scaleX, float scaleY, int sortingOrder, Vector2 pivot,
                Vector3 v00offset, Vector3 v01offset, Vector3 v10offset, Vector3 v11offset) {
            frameCount = _frameCount;
            pos = _pos;
            basePosition = pos;
            rot = _rot;
            baseRotation = rot;
            trigger = _trigger;
            uvType = _uvType;
            this.size = size;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.sortingOrder = sortingOrder;
            this.pivot = pivot;
            this.v00offset = v00offset;
            this.v01offset = v01offset;
            this.v10offset = v10offset;
            this.v11offset = v11offset;
            RefreshVertices();
        }
        public void SetNewPos(Vector3 newPos) {
            pos = newPos;
        }
        public float GetSize() {
            return size;
        }
        public void SetScaleX(float scaleX) {
            this.scaleX = scaleX;
            RefreshVertices();
        }
        public void SetScaleY(float scaleY) {
            this.scaleY = scaleY;
            RefreshVertices();
        }
        public void SetPivot(Vector2 pivot) {
            this.pivot = pivot;
            RefreshVertices();
        }
        public void SetNewSize(float size) {
            this.size = size;
            RefreshVertices();
        }
        public void SetNewRotation(float rot) {
            this.rot = rot;
            RefreshVertices();
        }
        public void ApplyBonusRotation(float bonusRotation) {
            SetNewRotation(baseRotation + bonusRotation);
        }
        public void ResetBonusRotation() {
            SetNewRotation(baseRotation);
        }
        public float GetBaseRotation() {
            return baseRotation;
        }
        
        public void ApplyTemporaryPosition(Vector3 temporaryPosition) {
            SetNewPos(temporaryPosition);
        }
        public void ApplyBonusPosition(Vector3 bonusPosition) {
            SetNewPos(basePosition + bonusPosition);
        }
        public void ResetBonusPosition() {
            SetNewPos(basePosition);
        }
        public Vector3 GetBasePosition() {
            return basePosition;
        }
        
        public void ApplyBonusSortingOrder(int bonusSortingOrder) {
            sortingOrder = baseSortingOrder + bonusSortingOrder;
        }

        public void RefreshVertices() {
            float widthHalf = size * scaleX;

            float uvHeightScale;
            if (uvType == null) {
                uvHeightScale = 1f;
            } else {
                uvHeightScale = uvType.GetHeightRatio();
            }
            float heightHalf = (size * uvHeightScale) * scaleY;

            Quaternion eulerQuaternion = Quaternion.Euler(0, 0, rot);
            v00 = eulerQuaternion * new Vector3(-size * scaleX - pivot.x * widthHalf, -(size * uvHeightScale) * scaleY - pivot.y * heightHalf);
            v01 = eulerQuaternion * new Vector3(-size * scaleX - pivot.x * widthHalf, +(size * uvHeightScale) * scaleY - pivot.y * heightHalf);
            v10 = eulerQuaternion * new Vector3(+size * scaleX - pivot.x * widthHalf, -(size * uvHeightScale) * scaleY - pivot.y * heightHalf);
            v11 = eulerQuaternion * new Vector3(+size * scaleX - pivot.x * widthHalf, +(size * uvHeightScale) * scaleY - pivot.y * heightHalf);
        }
        public string GetTrigger() {
            return trigger;
        }
        public void SetTrigger(string trigger) {
            this.trigger = trigger;
        }
        public UVType GetUVType() {
            return uvType;
        }
        public V_Skeleton_Frame CloneNew() {
            return new V_Skeleton_Frame(0, pos, rot, trigger, uvType, size, scaleX, scaleY, sortingOrder, pivot, v00offset, v01offset, v10offset, v11offset);
        }
        public V_Skeleton_Frame Clone() {
            return new V_Skeleton_Frame(frameCount, pos, rot, trigger, uvType, size, scaleX, scaleY, sortingOrder, pivot, v00offset, v01offset, v10offset, v11offset);
        }
        public bool IsKeyframe() {
            return frameCount != -1;
        }
        public void SetUVTypeCoords(Vector2 v00, Vector2 v01, Vector2 v10, Vector2 v11) {
            uvType.uvs = new Vector2[] {
                v01,
                v11,
                v00,
                v10
            };
        }
        public void SetUVTypeCoords(float x0, float x1, float y0, float y1) {
            uvType.uvs = new Vector2[] {
                new Vector2(x0, y1),
                new Vector2(x1, y1),
                new Vector2(x0, y0),
                new Vector2(x1, y0)
            };
        }


        public static V_Skeleton_Frame[] Smooth(V_Skeleton_Frame[] keyframes) {
            List<V_Skeleton_Frame> ret = new List<V_Skeleton_Frame>();

            V_Skeleton_Frame prevFrame = keyframes[0];
            ret.Add(prevFrame);
            for (int i = 1; i < keyframes.Length; i++) {
                V_Skeleton_Frame frame = keyframes[i];
                for (int j = 1; j < frame.frameCount - 1; j++) {
                    ret.Add(GetSmoothFrame(prevFrame, frame, j));
                }
                ret.Add(frame);
                prevFrame = frame;
            }

            return ret.ToArray();
        }
        public static V_Skeleton_Frame GetSmoothFrame(V_Skeleton_Frame prevFrame, V_Skeleton_Frame frame, int index) {
            Vector3 posInc = (frame.pos - prevFrame.pos) / frame.frameCount;
            float rotInc = ((frame.rot - prevFrame.rot) * 1f) / frame.frameCount;
            float sizeInc = ((frame.size - prevFrame.size) * 1f) / frame.frameCount;
            float scaleXInc = ((frame.scaleX - prevFrame.scaleX) * 1f) / frame.frameCount;
            float scaleYInc = ((frame.scaleY - prevFrame.scaleY) * 1f) / frame.frameCount;
            Vector3 v00offsetInc = (frame.v00offset - prevFrame.v00offset) / frame.frameCount;
            Vector3 v01offsetInc = (frame.v01offset - prevFrame.v01offset) / frame.frameCount;
            Vector3 v10offsetInc = (frame.v10offset - prevFrame.v10offset) / frame.frameCount;
            Vector3 v11offsetInc = (frame.v11offset - prevFrame.v11offset) / frame.frameCount;

            return new V_Skeleton_Frame(
                -1,
                prevFrame.pos + (posInc * index),
                (prevFrame.rot + (rotInc * index)),
                "",
                prevFrame.uvType,
                (prevFrame.size + (sizeInc * index)),
                (prevFrame.scaleX + (scaleXInc * index)),
                (prevFrame.scaleY + (scaleYInc * index)),
                prevFrame.sortingOrder,
                prevFrame.pivot,
                prevFrame.v00offset + (v00offsetInc * index),
                prevFrame.v01offset + (v01offsetInc * index),
                prevFrame.v10offset + (v10offsetInc * index),
                prevFrame.v11offset + (v11offsetInc * index)
            );
        }


        public static string Save_Static(V_Skeleton_Frame frame) {
            return frame.Save();
        }
        public string Save() {
            //Returns a string to be used in savefiles
            string[] content = new string[]{
            ""+frameCount,
            ""+V_Animation.Save_Vector3(pos),
            ""+size,
            ""+(int)rot,
            trigger,
            uvType.Save(),
            ""+scaleX,
            ""+scaleY,
            ""+sortingOrder,
            ""+V_Animation.Save_Vector2(pivot),
            ""+V_Animation.Save_Vector3(v00offset),
            ""+V_Animation.Save_Vector3(v01offset),
            ""+V_Animation.Save_Vector3(v10offset),
            ""+V_Animation.Save_Vector3(v11offset),
        };
            return string.Join("#SKELETONFRAME#", content);
        }

        public static V_Skeleton_Frame Load(string save) {
            string[] content = V_Animation.SplitString(save, "#SKELETONFRAME#");
            int frameCount = int.Parse(content[0]);
            Vector3 pos = V_Animation.Load_Vector3(content[1]);
            float size = float.Parse(content[2]);
            int rot = int.Parse(content[3]);
            V_Animation.StringArrPushIfIndex(4, ref content, "");
            string trigger = content[4];
            V_Animation.StringArrPushIfIndex(5, ref content, "");
            UVType uvType = UVType.Load(content[5]);

            V_Animation.StringArrPushIfIndex(6, ref content, "1");
            float scaleX = float.Parse(content[6]);
            V_Animation.StringArrPushIfIndex(7, ref content, "1");
            float scaleY = float.Parse(content[7]);
            V_Animation.StringArrPushIfIndex(8, ref content, "-100");
            int sortingOrder = int.Parse(content[8]);
            V_Animation.StringArrPushIfIndex(9, ref content, "0,0");
            Vector2 pivot = V_Animation.Load_Vector2(content[9]);

            V_Animation.StringArrPushIfIndex(10, ref content, "0,0");
            Vector2 v00offset = V_Animation.Load_Vector2(content[10]);
            V_Animation.StringArrPushIfIndex(11, ref content, "0,0");
            Vector2 v01offset = V_Animation.Load_Vector2(content[11]);
            V_Animation.StringArrPushIfIndex(12, ref content, "0,0");
            Vector2 v10offset = V_Animation.Load_Vector2(content[12]);
            V_Animation.StringArrPushIfIndex(13, ref content, "0,0");
            Vector2 v11offset = V_Animation.Load_Vector2(content[13]);

            return new V_Skeleton_Frame(frameCount, pos, size, rot, trigger, uvType, scaleX, scaleY, sortingOrder, pivot, v00offset, v01offset, v10offset, v11offset);
        }
    }

}