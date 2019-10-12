using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities {

    public static class Enumerables {

        public static IEnumerable<T> GetItself<T>(T element) {
            yield return element;
        }

        public static IEnumerable<Vector2Int> GetAllVectorsInRectangle(Vector2Int start, Vector2Int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = start.x; vector.x < finish.x; ++vector.x) {
                for (vector.y = start.y; vector.y < finish.y; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> GetAllVectorsInRectangle(Vector2Int finish) {
            return GetAllVectorsInRectangle(Vector2Int.zero, finish);
        }


    }

}
