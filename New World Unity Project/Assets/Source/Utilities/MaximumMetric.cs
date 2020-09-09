using UnityEngine;

namespace NewWorld.Utilities {

    public static class MaximumMetric {

        public static float GetNorm(in Vector2 vector) {
            return Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }

        public static float GetNorm(in Vector3 vector) {
            return Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }


    }
}
