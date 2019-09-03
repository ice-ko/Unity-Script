using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_ObjectSystem {

    public interface V_IObjectTransformBody {

        Vector3 ConvertBodyLocalPositionToWorldPosition(Vector3 position);
        void SetBodyMesh(Mesh mesh);
        void SetBodyMaterial(Material material);
        Transform GetBodyTransform();
        Material GetBodyMaterial();

    }

}