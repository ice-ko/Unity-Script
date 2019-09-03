using System;
using UnityEngine;

namespace V_ObjectSystem {

    public class V_IHasWorldPosition_Class : V_IHasWorldPosition {

        private Func<Vector3> GetPositionFunc;

        public V_IHasWorldPosition_Class(Func<Vector3> GetPositionFunc) {
            this.GetPositionFunc = GetPositionFunc;
        }
        public Vector3 GetPosition() {
            return GetPositionFunc();
        }

    }
    public interface V_IHasWorldPosition {

        Vector3 GetPosition();

    }

}