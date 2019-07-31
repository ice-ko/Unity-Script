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
     * Sprite in the UI
     * */
    public class UI_Sprite {

        private static Transform GetCanvasTransform() {
            return UtilsClass.GetCanvasTransform();
        }

        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc) {
            return CreateDebugButton(anchoredPosition, size, ClickFunc, Color.green);
        }
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color) {
            UI_Sprite uiSprite = new UI_Sprite(GetCanvasTransform(), Assets.i.s_White, anchoredPosition, size, color);
            uiSprite.AddButton(ClickFunc, null, null);
            return uiSprite;
        }
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, string text, Action ClickFunc) {
            return CreateDebugButton(anchoredPosition, text, ClickFunc, Color.green);
        }
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, string text, Action ClickFunc, Color color) {
            return CreateDebugButton(anchoredPosition, text, ClickFunc, color, new Vector2(30, 20));
        }
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, string text, Action ClickFunc, Color color, Vector2 padding) {
            UI_TextComplex uiTextComplex;
            UI_Sprite uiSprite = CreateDebugButton(anchoredPosition, Vector2.zero, ClickFunc, color, text, out uiTextComplex);
            uiSprite.SetSize(new Vector2(uiTextComplex.GetTotalWidth(), uiTextComplex.GetTotalHeight()) + padding);
            return uiSprite;
        }
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color, string text) {
            UI_TextComplex uiTextComplex;
            return CreateDebugButton(anchoredPosition, size, ClickFunc, color, text, out uiTextComplex);
        }
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color, string text, out UI_TextComplex uiTextComplex) {
            if (color.r >= 1f) color.r = .9f;
            if (color.g >= 1f) color.g = .9f;
            if (color.b >= 1f) color.b = .9f;
            Color colorOver = color * 1.1f; // button over color lighter
            UI_Sprite uiSprite = new UI_Sprite(GetCanvasTransform(), Assets.i.s_White, anchoredPosition, size, color);
            uiSprite.AddButton(ClickFunc, () => uiSprite.SetColor(colorOver), () => uiSprite.SetColor(color));
            uiTextComplex = new UI_TextComplex(uiSprite.gameObject.transform, Vector2.zero, 12, '#', text, null, null);
            uiTextComplex.SetTextColor(Color.black);
            uiTextComplex.SetAnchorMiddle();
            uiTextComplex.CenterOnPosition(Vector2.zero);
            return uiSprite;
        }
        


        public GameObject gameObject;
        public Image image;
        public RectTransform rectTransform;


        public UI_Sprite(Transform parent, Sprite sprite, Vector2 anchoredPosition, Vector2 size, Color color) {
            rectTransform = UtilsClass.DrawSprite(sprite, parent, anchoredPosition, size, "UI_Sprite");
            gameObject = rectTransform.gameObject;
            image = gameObject.GetComponent<Image>();
            image.color = color;
        }
        public void SetColor(Color color) {
            image.color = color;
        }
        public void SetSprite(Sprite sprite) {
            image.sprite = sprite;
        }
        public void SetSize(Vector2 size) {
            rectTransform.sizeDelta = size;
        }
        public void SetAnchoredPosition(Vector2 anchoredPosition) {
            rectTransform.anchoredPosition = anchoredPosition;
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
        public void DestroySelf() {
            UnityEngine.Object.Destroy(gameObject);
        }

    }
}