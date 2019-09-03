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
     * Mesh in the World
     * */
    public class World_Mesh {
        
        private const int sortingOrderDefault = 5000;

        public GameObject gameObject;
        public Transform transform;
        private Material material;
        private Vector3[] vertices;
        private Vector2[] uv;
        private int[] triangles;
        private Mesh mesh;


        public static World_Mesh Create(Vector3 position, float eulerZ, float meshWidth, float meshHeight, Material material, UVCoords uvCoords, int sortingOrderOffset = 0) {
            return new World_Mesh(null, position, Vector3.one, eulerZ, meshWidth, meshHeight, material, uvCoords, sortingOrderOffset);
        }

        public static World_Mesh Create(Vector3 lowerLeftCorner, float width, float height, Material material, UVCoords uvCoords, int sortingOrderOffset = 0) {
            return Create(lowerLeftCorner, lowerLeftCorner + new Vector3(width, height), material, uvCoords, sortingOrderOffset);
        }

        public static World_Mesh Create(Vector3 lowerLeftCorner, Vector3 upperRightCorner, Material material, UVCoords uvCoords, int sortingOrderOffset = 0) {
            float width = upperRightCorner.x - lowerLeftCorner.x;
            float height = upperRightCorner.y - lowerLeftCorner.y;
            Vector3 localScale = upperRightCorner - lowerLeftCorner;
            Vector3 position = lowerLeftCorner + localScale * .5f;
            return new World_Mesh(null, position, Vector3.one, 0f, width, height, material, uvCoords, sortingOrderOffset);
        }


        private static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault) {
            return (int)(baseSortingOrder - position.y) + offset;
        }


        public class UVCoords {
            public int x, y, width, height;
            public UVCoords(int x, int y, int width, int height) {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
            }
        }


        public World_Mesh(Transform parent, Vector3 localPosition, Vector3 localScale, float eulerZ, float meshWidth, float meshHeight, Material material, UVCoords uvCoords, int sortingOrderOffset) {
            this.material = material;

            vertices = new Vector3[4];
            uv = new Vector2[4];
            triangles = new int[6];

            /* 0,1
             * 1,1
             * 0,0
             * 1,0
             */
            
            float meshWidthHalf  = meshWidth  / 2f;
            float meshHeightHalf = meshHeight / 2f;

            vertices[0] = new Vector3(-meshWidthHalf,  meshHeightHalf);
            vertices[1] = new Vector3( meshWidthHalf,  meshHeightHalf);
            vertices[2] = new Vector3(-meshWidthHalf, -meshHeightHalf);
            vertices[3] = new Vector3( meshWidthHalf, -meshHeightHalf);
            
            if (uvCoords == null) {
                uvCoords = new UVCoords(0, 0, material.mainTexture.width, material.mainTexture.height);
            }

            Vector2[] uvArray = GetUVRectangleFromPixels(uvCoords.x, uvCoords.y, uvCoords.width, uvCoords.height, material.mainTexture.width, material.mainTexture.height);

            ApplyUVToUVArray(uvArray, ref uv);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 2;
            triangles[4] = 1;
            triangles[5] = 3;

            mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = localPosition;
            gameObject.transform.localScale = localScale;
            gameObject.transform.localEulerAngles = new Vector3(0, 0, eulerZ);

            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshRenderer>().material = material;

            transform = gameObject.transform;

            SetSortingOrderOffset(sortingOrderOffset);
        }

        private Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight) {
            return new Vector2((float)x / textureWidth, (float)y / textureHeight);
        }

        private Vector2[] GetUVRectangleFromPixels(int x, int y, int width, int height, int textureWidth, int textureHeight) {
            /* 0, 1
             * 1, 1
             * 0, 0
             * 1, 0
             * */
            return new Vector2[] { 
                ConvertPixelsToUVCoordinates(x, y + height, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x + width, y + height, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x, y, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x + width, y, textureWidth, textureHeight)
            };
        }

        private void ApplyUVToUVArray(Vector2[] uv, ref Vector2[] mainUV) {
            if (uv == null || uv.Length < 4 || mainUV == null || mainUV.Length < 4) throw new System.Exception();
            mainUV[0] = uv[0];
            mainUV[1] = uv[1];
            mainUV[2] = uv[2];
            mainUV[3] = uv[3];
        }

        public void SetUVCoords(UVCoords uvCoords) {
            Vector2[] uvArray = GetUVRectangleFromPixels(uvCoords.x, uvCoords.y, uvCoords.width, uvCoords.height, material.mainTexture.width, material.mainTexture.height);
            ApplyUVToUVArray(uvArray, ref uv);
            mesh.uv = uv;
        }

        public void SetSortingOrderOffset(int sortingOrderOffset) {
            SetSortingOrder(GetSortingOrder(gameObject.transform.position, sortingOrderOffset));
        }

        public void SetSortingOrder(int sortingOrder) {
            gameObject.GetComponent<Renderer>().sortingOrder = sortingOrder;
        }

        public void SetLocalScale(Vector3 localScale) {
            transform.localScale = localScale;
        }

        public void SetPosition(Vector3 localPosition) {
            transform.localPosition = localPosition;
        }

        public void AddPosition(Vector3 addPosition) {
            transform.localPosition += addPosition;
        }

        public Vector3 GetPosition() {
            return transform.localPosition;
        }

        public int GetSortingOrder() {
            return gameObject.GetComponent<Renderer>().sortingOrder;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void DestroySelf() {
            Object.Destroy(gameObject);
        }

    }
}