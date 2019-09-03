using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace V_AnimationSystem {

    /*
     * Contains the Frames for an animation for a Body Part
     * */
    public class V_Skeleton_Anim {

        public V_Skeleton_Frame[] frames;
        private int framesLength;

        private V_Skeleton_Frame currentAnimFrame;
        private bool canLoop = true;
        public bool looped = false;
        public BodyPart bodyPart;

        private int currentFrame = 0;
        private bool[] hasAnimFrameTrigger;

        private float frameTimer = 0f;
        private float frameRate;
        private float frameRateOriginal;
        private bool useUnscaledTime = false;

        public bool defaultHasVariableSortingOrder = false;

        public V_UnitSkeleton.OnAnimTrigger onAnimTrigger;
        public Action<V_Skeleton_Anim> onAnimComplete;

        public V_Skeleton_Anim(V_Skeleton_Frame[] _frames, BodyPart _bodyPart, float _frameRate) {
            SetFrames(_frames);
            currentAnimFrame = frames[0];
            bodyPart = _bodyPart;
            frameRate = _frameRate;
            frameRateOriginal = frameRate;
        }

        public void Update(float deltaTime) {
            frameTimer = frameTimer - deltaTime;
            while (frameTimer < 0 && (canLoop || !looped)) {
                frameTimer = frameTimer + frameRate;
                currentFrame = currentFrame + 1;
                if (currentFrame >= framesLength) {
                    currentFrame = 0;
                    looped = true;
                }

                currentAnimFrame = frames[currentFrame];
                if (hasAnimFrameTrigger[currentFrame]) {
                    if (onAnimTrigger != null) onAnimTrigger(currentAnimFrame.trigger);
                }

                if (currentFrame >= framesLength - 1) {
                    // Last frame
                    if (!canLoop) {
                        looped = true;
                        if (onAnimComplete != null) onAnimComplete(this);
                        break;
                    } else {
                        if (onAnimComplete != null) onAnimComplete(this);
                    }
                }
            }
        }

        public void UpdateNextFrame() {
            Update(frameRate);
        }

        public V_Skeleton_Frame GetFirstFrame() {
            return frames[0];
        }

        public V_Skeleton_Frame GetCurrentAnimFrame() {
            return currentAnimFrame;
        }

        public V_Skeleton_Frame[] GetFrames() {
            return frames;
        }

        private void SetFrames(V_Skeleton_Frame[] frames) {
            this.frames = frames;
            framesLength = frames.Length;
            hasAnimFrameTrigger = new bool[framesLength];
            for (int i = 0; i < frames.Length; i++) {
                hasAnimFrameTrigger[i] = !string.IsNullOrEmpty(frames[i].trigger);
            }
        }
        public List<string> GetTriggerList() {
            List<string> ret = new List<string>();
            foreach (V_Skeleton_Frame frame in frames) {
                if (!string.IsNullOrEmpty(frame.trigger)) {
                    // Has trigger
                    ret.Add(frame.trigger);
                }
            }
            return ret;
        }
        public List<UVType> GetFrameUVTypeList() {
            List<UVType> ret = new List<UVType>();
            foreach (V_Skeleton_Frame frame in frames) {
                ret.Add(frame.GetUVType());
            }
            return ret;
        }

        public float GetAnimTimer() {
            return framesLength * frameRate;
        }
        public float GetFrameRateOriginal() {
            return frameRateOriginal;
        }
        public void SetFrameRateOriginal(float rate) {
            frameRateOriginal = rate;
            frameRate = rate;
        }
        public void TestFirstFrameTrigger() {
            if (!string.IsNullOrEmpty(frames[currentFrame].trigger)) {
                if (onAnimTrigger != null) onAnimTrigger(frames[currentFrame].trigger);
            }
        }
        public bool IsBodyPartPointer() {
            return bodyPart.preset == BodyPart.Pointer_1 || bodyPart.preset == BodyPart.Pointer_2 || bodyPart.preset == BodyPart.Pointer_3;
        }
        public bool HasVariableSortingOrder() {
            if (defaultHasVariableSortingOrder) {
                return true;
            }
            int initSortingOrder = frames[0].sortingOrder;
            foreach (V_Skeleton_Frame frame in frames) {
                if (frame.sortingOrder != initSortingOrder) {
                    // Different sorting order
                    return true;
                }
            }
            return false;
        }
        public void Reset() {
            currentFrame = 0;
            currentAnimFrame = frames[currentFrame];
            looped = false;
        }
        public void SetAllFramesSortingOrder(int sortingOrder) {
            foreach (V_Skeleton_Frame frame in frames) {
                frame.sortingOrder = sortingOrder;
            }
        }
        public void SetUseUnscaledTime(bool useUnscaledTime) {
            this.useUnscaledTime = useUnscaledTime;
        }
        public void SetFrameRateMod(float frameRateMod) {
            frameRate /= frameRateMod;
            if (frameRateMod > 0 && (frameRate <= 0.00001f || float.IsNaN(frameRate) || float.IsInfinity(frameRate))) {
                frameRate = 0.00001f;
            }
        }
        public void SetFrameRateMod_DontStack(float frameRateMod) {
            frameRate = frameRateOriginal / frameRateMod;
        }
        public void SetCurrentFrame(int currentFrame) {
            this.currentFrame = currentFrame;
        }
        public V_Skeleton_Frame GetCurrentFrame() {
            return currentAnimFrame;
        }
        public int GetCurrentFrameNumberIndex() {
            return currentFrame;
        }
        public int GetCurrentFrameNumber() {
            return currentFrame + 1;
        }
        public int GetTotalFrameCount() {
            return frames.Length;
        }
        public void ModifyCurrentFrameScaleX(float scaleX) {
            currentAnimFrame.SetScaleX(scaleX);
        }
        public void ModifyCurrentFrameScaleY(float scaleY) {
            currentAnimFrame.SetScaleY(scaleY);
        }
        public void ModifyCurrentFramePos(Vector3 newPos) {
            currentAnimFrame.SetNewPos(newPos);
        }
        public void SetCurrentFrameSize(float newSize) {
            currentAnimFrame.SetNewSize(newSize);
        }
        public void SetCurrentFrameRotation(float newRot) {
            currentAnimFrame.SetNewRotation(newRot);
        }

        public void AddKeyframeFirstToEnd() {
            List<V_Skeleton_Frame> newFrames = new List<V_Skeleton_Frame>(frames);

            List<V_Skeleton_Frame> keyframes = GetKeyframes();
            V_Skeleton_Frame firstKeyframe = keyframes[0];
            V_Skeleton_Frame cloned = firstKeyframe.CloneNew();
            newFrames.Add(cloned);
            SetFrames(newFrames.ToArray());
            currentFrame = System.Array.IndexOf(frames, cloned);
            currentAnimFrame = newFrames[currentFrame];

            RemakeTween();
        }
        public void AddKeyframe() {
            List<V_Skeleton_Frame> newFrames = new List<V_Skeleton_Frame>(frames);
            if (currentAnimFrame.frameCount != -1) {
                //Is keyframe, duplicate this one
                V_Skeleton_Frame cloned = currentAnimFrame.CloneNew();
                newFrames.Insert(newFrames.IndexOf(currentAnimFrame) + 1, cloned);
                SetFrames(newFrames.ToArray());
                currentFrame = System.Array.IndexOf(frames, cloned);
                currentAnimFrame = newFrames[currentFrame];
            } else {
                //Not keyframe, clone last keyframe
                List<V_Skeleton_Frame> keyframes = GetKeyframes();
                V_Skeleton_Frame cloned = keyframes[keyframes.Count - 1].CloneNew();
                newFrames.Add(cloned);
                SetFrames(newFrames.ToArray());
                currentFrame = System.Array.IndexOf(frames, cloned);
                currentAnimFrame = newFrames[currentFrame];
            }
            RemakeTween();
        }
        public void DeleteKeyframe() {
            List<V_Skeleton_Frame> keyframes = GetKeyframes();
            if (currentAnimFrame.frameCount != -1) {
                //Is keyframe, delete
                keyframes.Remove(currentAnimFrame);
                SetFrames(keyframes.ToArray());
                RemakeTween();
            }
        }
        public void ModifyFrameCount(int frameCount) {
            currentAnimFrame.frameCount = frameCount;
            RemakeTween();
        }
        public void SelectFrame(int frameIndex) {
            currentFrame = frameIndex;
            currentAnimFrame = frames[currentFrame];
        }
        public void SelectKeyframeRight() {
            //Look for next keyframe
            if (GetKeyframes().Count > 1) {
                //Theres more than one Keyframe
                int newFrameIndex = (currentFrame + 1) % frames.Length;
                while (newFrameIndex != currentFrame) {
                    V_Skeleton_Frame frame = frames[newFrameIndex];
                    if (frame.frameCount != -1) {
                        //Is keyframe, select this one
                        currentFrame = newFrameIndex;
                        currentAnimFrame = frames[currentFrame];
                        break;
                    }
                    newFrameIndex = (newFrameIndex + 1) % frames.Length;
                }
            }
        }
        public void SelectKeyframeLeft() {
            //Look for next keyframe
            if (GetKeyframes().Count > 1) {
                //Theres more than one Keyframe
                int newFrameIndex = currentFrame - 1;
                if (newFrameIndex < 0) newFrameIndex = frames.Length - 1;
                while (newFrameIndex != currentFrame) {
                    V_Skeleton_Frame frame = frames[newFrameIndex];
                    if (frame.frameCount != -1) {
                        //Is keyframe, select this one
                        currentFrame = newFrameIndex;
                        currentAnimFrame = frames[currentFrame];
                        break;
                    }
                    newFrameIndex--;
                    if (newFrameIndex < 0) newFrameIndex = frames.Length - 1;
                }
            }
        }
        public V_Skeleton_Anim CloneDeep() {
            // Deep copy, clones every individual frame
            List<V_Skeleton_Frame> framesList = new List<V_Skeleton_Frame>();
            foreach (V_Skeleton_Frame frame in frames)
                framesList.Add(frame.Clone());
            return new V_Skeleton_Anim(framesList.ToArray(), bodyPart, frameRate);
        }
        public V_Skeleton_Anim Clone() {
            // Shallow Copy, does not clone frames
            return new V_Skeleton_Anim(frames, bodyPart, frameRate);
        }
        public V_Skeleton_Anim CloneOnlyKeyframes() {
            return new V_Skeleton_Anim(CloneKeyframes().ToArray(), bodyPart, frameRate);
        }
        public List<V_Skeleton_Frame> CloneKeyframes() {
            List<V_Skeleton_Frame> ret = new List<V_Skeleton_Frame>();
            List<V_Skeleton_Frame> keyframes = GetKeyframes();
            foreach (V_Skeleton_Frame frame in keyframes) {
                ret.Add(frame.Clone());
            }
            return ret;
        }
        public List<V_Skeleton_Frame> GetKeyframes() {
            List<V_Skeleton_Frame> keyframes = new List<V_Skeleton_Frame>();
            foreach (V_Skeleton_Frame frame in frames) {
                if (frame.frameCount != -1) {
                    keyframes.Add(frame);
                }
            }
            return keyframes;
        }
        public void RemakeTween() {
            List<V_Skeleton_Frame> keyframes = GetKeyframes();
            foreach (V_Skeleton_Frame keyframe in keyframes) keyframe.RefreshVertices();
            SetFrames(V_Skeleton_Frame.Smooth(keyframes.ToArray()));
            if (keyframes.IndexOf(currentAnimFrame) != -1) {
                //Currently selected keyframe, keep selection
                currentFrame = System.Array.IndexOf(frames, currentAnimFrame);
            } else {
                currentFrame = 0;
            }
            currentAnimFrame = frames[currentFrame];
        }
        public void MoveKeyframeLeft() {
            List<V_Skeleton_Frame> keyframes = GetKeyframes();
            int changeIndex = keyframes.IndexOf(currentAnimFrame);
            if (changeIndex > 0) {
                keyframes[changeIndex] = keyframes[changeIndex - 1];
                keyframes[changeIndex - 1] = currentAnimFrame;
            }
            SetFrames(keyframes.ToArray());
            RemakeTween();
        }
        public void MoveKeyframeRight() {
            List<V_Skeleton_Frame> keyframes = GetKeyframes();
            int changeIndex = keyframes.IndexOf(currentAnimFrame);
            if (changeIndex < keyframes.Count - 1) {
                keyframes[changeIndex] = keyframes[changeIndex + 1];
                keyframes[changeIndex + 1] = currentAnimFrame;
            }
            SetFrames(keyframes.ToArray());
            RemakeTween();
        }
        public void RemoveKeyframesExceptFirst() {
            List<V_Skeleton_Frame> keyframes = GetKeyframes();
            SetFrames(new V_Skeleton_Frame[] { keyframes[0] });
            RemakeTween();
        }







        public static string Save_Static(V_Skeleton_Anim single) {
            return single.Save();
        }
        public string Save() {
            // Returns a string to be used in savefiles
            string[] content = new string[]{
            ""+bodyPart.Save(),
            "",
            ""+frameRate,
            V_Animation.Save_Array<V_Skeleton_Frame>(frames, V_Skeleton_Frame.Save_Static, "#SKELETONFRAMELIST#")
        };
            return string.Join("#SKELETONANIM#", content);
        }
        public static V_Skeleton_Anim Load(string save) {
            string[] content = V_Animation.SplitString(save, "#SKELETONANIM#");
            BodyPart bodyPart = BodyPart.Load(content[0]);
            float frameRate = float.Parse(content[2]);
            V_Skeleton_Frame[] frames = V_Animation.Load_Array<V_Skeleton_Frame>(content[3], V_Skeleton_Frame.Load, "#SKELETONFRAMELIST#");

            return new V_Skeleton_Anim(frames, bodyPart, frameRate);
        }

    }

}