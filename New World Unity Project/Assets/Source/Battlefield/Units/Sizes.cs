using System;
using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public static class Sizes {

        // Exception builder.

        public static bool IsValidSize(float parameterValue) {
            return parameterValue >= 0;
        }

        public static ArgumentOutOfRangeException BuildInvalidSizeException(string parameterName, float parameterValue) {
            return new ArgumentOutOfRangeException(
                parameterName,
                parameterValue,
                $"Provided size is invalid. It must be non-negative."
            );
        }

    }

}
