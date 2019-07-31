/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils {
    
    /*
     * UI Container with multiple bars, useful for displaying one bar with multiple inner bars like success chance and failure chance
     * */
    public class UI_BarMultiple {
        
        private GameObject gameObject;
        private RectTransform rectTransform;
        private RectTransform[] barArr;
        private Image[] barImageArr;
        private Vector2 size;
        
        public class Outline {
            public float size = 1f;
            public Color color = Color.black;
            public Outline(float size, Color color) {
                this.size = size;
                this.color = color;
            }
        }

        public UI_BarMultiple(Transform parent, Vector2 anchoredPosition, Vector2 size, Color[] barColorArr, Outline outline) {
            this.size = size;
            SetupParent(parent, anchoredPosition, size);
            if (outline != null) SetupOutline(outline, size);
            List<RectTransform> barList = new List<RectTransform>();
            List<Image> barImageList = new List<Image>();
            List<float> defaultSizeList = new List<float>();
            foreach (Color color in barColorArr) {
                barList.Add(SetupBar(color));
                defaultSizeList.Add(1f / barColorArr.Length);
            }
            barArr = barList.ToArray();
            barImageArr = barImageList.ToArray();
            SetSizes(defaultSizeList.ToArray());
        }
        private void SetupParent(Transform parent, Vector2 anchoredPosition, Vector2 size) {
            gameObject = new GameObject("UI_BarMultiple", typeof(RectTransform));
            rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.sizeDelta = size;
            rectTransform.anchorMin = new Vector2(0, .5f);
            rectTransform.anchorMax = new Vector2(0, .5f);
            rectTransform.pivot = new Vector2(0, .5f);
            rectTransform.anchoredPosition = anchoredPosition;
        }
        private void SetupOutline(Outline outline, Vector2 size) {
            UtilsClass.DrawSprite(outline.color, gameObject.transform, Vector2.zero, size + new Vector2(outline.size, outline.size), "Outline");
        }
        private RectTransform SetupBar(Color barColor) {
            RectTransform bar = UtilsClass.DrawSprite(barColor, gameObject.transform, Vector2.zero, Vector2.zero, "Bar");
            bar.anchorMin = new Vector2(0,0);
            bar.anchorMax = new Vector2(0,1f);
            bar.pivot = new Vector2(0,.5f);
            return bar;
        }
        public void SetSizes(float[] sizeArr) {
            if (sizeArr.Length != barArr.Length) {
                throw new System.Exception("Length doesn't match!");
            }
            Vector2 pos = Vector2.zero;
            for (int i=0; i<sizeArr.Length; i++) {
                float scaledSize = sizeArr[i] * size.x;
                barArr[i].anchoredPosition = pos;
                barArr[i].sizeDelta = new Vector2(scaledSize, 0f);
                pos.x += scaledSize;
            }
        }
        public Vector2 GetSize() {
            return size;
        }
        public void DestroySelf() {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
