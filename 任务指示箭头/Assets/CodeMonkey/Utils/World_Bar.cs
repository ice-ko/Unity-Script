/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;

namespace CodeMonkey.Utils {

    /*
     * Bar in the World, great for quickly making a health bar
     * */
    public class World_Bar {
        
        private GameObject gameObject;
        private Transform transform;
        private Transform background;
        private Transform bar;

        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = 5000) {
            return (int)(baseSortingOrder - position.y) + offset;
        }

        public class Outline {
            public float size = 1f;
            public Color color = Color.black;
        }

        public World_Bar(Transform parent, Vector3 localPosition, Vector3 localScale, Color? backgroundColor, Color barColor, float sizeRatio, int sortingOrder, Outline outline = null) {
            SetupParent(parent, localPosition);
            if (outline != null) SetupOutline(outline, localScale, sortingOrder - 1);
            if (backgroundColor != null) SetupBackground((Color)backgroundColor, localScale, sortingOrder);
            SetupBar(barColor, localScale, sortingOrder + 1);
            SetSize(sizeRatio);
        }
        private void SetupParent(Transform parent, Vector3 localPosition) {
            gameObject = new GameObject("World_Bar");
            transform = gameObject.transform;
            transform.SetParent(parent);
            transform.localPosition = localPosition;
        }
        private void SetupOutline(Outline outline, Vector3 localScale, int sortingOrder) {
            UtilsClass.CreateWorldSprite(transform, "Outline", Assets.i.s_White, new Vector3(0,0), localScale + new Vector3(outline.size, outline.size), sortingOrder, outline.color);
        }
        private void SetupBackground(Color backgroundColor, Vector3 localScale, int sortingOrder) {
            background = UtilsClass.CreateWorldSprite(transform, "Background", Assets.i.s_White, new Vector3(0,0), localScale, sortingOrder, backgroundColor).transform;
        }
        private void SetupBar(Color barColor, Vector3 localScale, int sortingOrder) {
            GameObject barGO = new GameObject("Bar");
            bar = barGO.transform;
            bar.SetParent(transform);
            bar.localPosition = new Vector3(-localScale.x / 2f, 0, 0);
            bar.localScale = new Vector3(1,1,1);
            Transform barIn = UtilsClass.CreateWorldSprite(bar, "BarIn", Assets.i.s_White, new Vector3(localScale.x / 2f, 0), localScale, sortingOrder, barColor).transform;
        }
        public void SetRotation(float rotation) {
            transform.localEulerAngles = new Vector3(0, 0, rotation);
        }
        public void SetSize(float sizeRatio) {
            bar.localScale = new Vector3(sizeRatio, 1, 1);
        }
        public void SetColor(Color color) {
            bar.Find("BarIn").GetComponent<SpriteRenderer>().color = color;
        }
        public void Show() {
            gameObject.SetActive(true);
        }
        public void Hide() {
            gameObject.SetActive(false);
        }
        public Button_Sprite AddButton(System.Action ClickFunc, System.Action MouseOverOnceFunc, System.Action MouseOutOnceFunc) {
            Button_Sprite buttonSprite = gameObject.AddComponent<Button_Sprite>();
            if (ClickFunc != null)
                buttonSprite.ClickFunc = ClickFunc;
            if (MouseOverOnceFunc != null)
                buttonSprite.MouseOverOnceFunc = MouseOverOnceFunc;
            if (MouseOutOnceFunc != null)
                buttonSprite.MouseOutOnceFunc = MouseOutOnceFunc;
            return buttonSprite;
        }
        public void DestroySelf() {
            if (gameObject != null) {
                Object.Destroy(gameObject);
            }
        }

    }
    
}
