namespace NewWorld.Utilities {

    public static class Floats {

        // Methods.

        public static float SetPositive(float value) {
            if (float.IsNaN(value) || value <= 0) {
                return 0;
            }
            return value;
        }

        public static float LimitPositive(float value, float limit) {
            if (float.IsNaN(value) || value <= 0) {
                return 0;
            }
            if (!float.IsNaN(limit) && value >= limit) {
                return limit;
            }
            return value;
        }

        public static float ClampPositive(float value, float lowerLimit, float upperLimit) {
            if (float.IsNaN(value) || value <= 0) {
                return 0;
            }
            if (!float.IsNaN(lowerLimit) && value <= lowerLimit) {
                return lowerLimit;
            }
            if (!float.IsNaN(upperLimit) && value >= upperLimit) {
                return upperLimit;
            }
            return value;
        }


    }

}
