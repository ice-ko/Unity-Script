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
using CodeMonkey.Utils;

namespace CodeMonkey {

    /*
     * Debug Class with various helper functions to quickly create buttons, text, etc
     * */
    public static class CMDebug {

        // Creates a Button in the World
        public static World_Sprite Button(Transform parent, Vector3 localPosition, string text, System.Action ClickFunc, int fontSize = 30, float paddingX = 5, float paddingY = 5) {
            return World_Sprite.CreateDebugButton(parent, localPosition, text, ClickFunc, fontSize, paddingX, paddingY);
        }

        // Creates a Button in the UI
        public static UI_Sprite ButtonUI(Vector2 anchoredPosition, string text, Action ClickFunc) {
            return UI_Sprite.CreateDebugButton(anchoredPosition, text, ClickFunc);
        }

        // Creates a World Text object at the world position
        public static void Text(string text, Vector3 localPosition = default(Vector3), Transform parent = null, int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = UtilsClass.sortingOrderDefault) {
            UtilsClass.CreateWorldText(text, parent, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder);
        }
        
        // World text pop up at mouse position
        public static void TextPopupMouse(string text) {
            UtilsClass.CreateWorldTextPopup(text, UtilsClass.GetMouseWorldPosition());
        }

        // Creates a Text pop up at the world position
        public static void TextPopup(string text, Vector3 position) {
            UtilsClass.CreateWorldTextPopup(text, position);
        }

        // Text Updater in World, (parent == null) = world position
        public static FunctionUpdater TextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null) {
            return UtilsClass.CreateWorldTextUpdater(GetTextFunc, localPosition, parent);
        }

        // Text Updater in UI
        public static FunctionUpdater TextUpdaterUI(Func<string> GetTextFunc, Vector2 anchoredPosition) {
            return UtilsClass.CreateUITextUpdater(GetTextFunc, anchoredPosition);
        }

        // Text Updater always following mouse
        public static void MouseTextUpdater(Func<string> GetTextFunc, Vector3 positionOffset = default(Vector3)) {
            GameObject gameObject = new GameObject();
            FunctionUpdater.Create(() => {
                gameObject.transform.position = UtilsClass.GetMouseWorldPosition() + positionOffset;
                return false;
            });
            TextUpdater(GetTextFunc, Vector3.zero, gameObject.transform);
        }

        // Trigger Action on Key
        public static FunctionUpdater KeyCodeAction(KeyCode keyCode, Action onKeyDown) {
            return UtilsClass.CreateKeyCodeAction(keyCode, onKeyDown);
        }
        


        // Debug DrawLine to draw a projectile, turn Gizmos On
        public static void DebugProjectile(Vector3 from, Vector3 to, float speed, float projectileSize) {
            Vector3 dir = (to - from).normalized;
            Vector3 pos = from;
            FunctionUpdater.Create(() => {
                Debug.DrawLine(pos, pos + dir * projectileSize);
                float distanceBefore = Vector3.Distance(pos, to);
                pos += dir * speed * Time.deltaTime;
                float distanceAfter = Vector3.Distance(pos, to);
                if (distanceBefore < distanceAfter) {
                    return true;
                }
                return false;
            });
        }


    }

}