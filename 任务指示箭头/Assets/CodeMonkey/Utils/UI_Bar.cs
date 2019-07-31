/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils {

    /*
     * Bar in the UI with scaleable Bar and Background
     * */
    public class UI_Bar {
        
        public GameObject gameObject;
        private RectTransform rectTransform;
        private RectTransform background;
        private RectTransform bar;
        private Vector2 size;
        
        /* 
         * Outline into for Bar
         * */
        public class Outline {
            public float size = 1f;
            public Color color = Color.black;
            public Outline(float size, Color color) {
                this.size = size;
                this.color = color;
            }
        }

        public UI_Bar(Transform parent, Vector2 anchoredPosition, Vector2 size, Color barColor, float sizeRatio) {
            SetupParent(parent, anchoredPosition, size);
            SetupBar(barColor);
            SetSize(sizeRatio);
        }
        public UI_Bar(Transform parent, Vector2 anchoredPosition, Vector2 size, Color barColor, float sizeRatio, Outline outline) {
            SetupParent(parent, anchoredPosition, size);
            if (outline != null) SetupOutline(outline, size);
            SetupBar(barColor);
            SetSize(sizeRatio);
        }
        public UI_Bar(Transform parent, Vector2 anchoredPosition, Vector2 size, Color backgroundColor, Color barColor, float sizeRatio) {
            SetupParent(parent, anchoredPosition, size);
            SetupBackground(backgroundColor);
            SetupBar(barColor);
            SetSize(sizeRatio);
        }
        public UI_Bar(Transform parent, Vector2 anchoredPosition, Vector2 size, Color backgroundColor, Color barColor, float sizeRatio, Outline outline) {
            SetupParent(parent, anchoredPosition, size);
            if (outline != null) SetupOutline(outline, size);
            SetupBackground(backgroundColor);
            SetupBar(barColor);
            SetSize(sizeRatio);
        }
        private void SetupParent(Transform parent, Vector2 anchoredPosition, Vector2 size) {
            this.size = size;
            gameObject = new GameObject("UI_Bar", typeof(RectTransform));
            rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.sizeDelta = size;
            rectTransform.anchorMin = new Vector2(0, .5f);
            rectTransform.anchorMax = new Vector2(0, .5f);
            rectTransform.pivot = new Vector2(0, .5f);
            rectTransform.anchoredPosition = anchoredPosition;
        }
        private RectTransform SetupOutline(Outline outline, Vector2 size) {
            return UtilsClass.DrawSprite(outline.color, gameObject.transform, Vector2.zero, size + new Vector2(outline.size, outline.size), "Outline");
        }
        private void SetupBackground(Color backgroundColor) {
            background = UtilsClass.DrawSprite(backgroundColor, gameObject.transform, Vector2.zero, Vector2.zero, "Background");
            background.anchorMin = new Vector2(0,0);
            background.anchorMax = new Vector2(1,1);
        }
        private void SetupBar(Color barColor) {
            bar = UtilsClass.DrawSprite(barColor, gameObject.transform, Vector2.zero, Vector2.zero, "Bar");
            bar.anchorMin = new Vector2(0,0);
            bar.anchorMax = new Vector2(0,1f);
            bar.pivot = new Vector2(0,.5f);
        }
        public void SetSize(float sizeRatio) {
            bar.sizeDelta = new Vector2(sizeRatio * size.x, 0);
        }
        public void SetColor(Color color) {
            bar.GetComponent<Image>().color = color;
        }
        public void SetActive(bool active) {
            gameObject.SetActive(active);
        }
        public void AddOutline(Outline outline) {
            RectTransform outlineRectTransform = SetupOutline(outline, size);
            outlineRectTransform.transform.SetAsFirstSibling();
        }
        public void SetRaycastTarget(bool set) {
            foreach (Transform trans in gameObject.transform) {
                if (trans.GetComponent<Image>() != null) {
                    trans.GetComponent<Image>().raycastTarget = set;
                }
            }
        }
        public void DestroySelf() {
            UnityEngine.Object.Destroy(gameObject);
        }
        public Button_UI AddButton() {
            return AddButton(null, null, null);
        }
        public Button_UI AddButton(Action ClickFunc, Action MouseOverOnceFunc, Action MouseOutOnceFunc) {
            Button_UI buttonUI = gameObject.AddComponent<Button_UI>();
            if (ClickFunc != null)
                buttonUI.ClickFunc = ClickFunc;
            if (MouseOverOnceFunc != null)
                buttonUI.MouseOverOnceFunc = MouseOverOnceFunc;
            if (MouseOutOnceFunc != null)
                buttonUI.MouseOutOnceFunc = MouseOutOnceFunc;
            return buttonUI;
        }

    }


}
