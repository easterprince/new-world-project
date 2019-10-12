using UnityEngine;

namespace NewWorld.Utilities {

    public static class MinimumMetric {
        
        public static float GetNorm(in Vector2 vector) {
            return Mathf.Min(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }


    }
}
