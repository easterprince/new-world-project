using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities {

    public static class Enumerables {

        public static IEnumerable<T> GetItself<T>(T element) {
            yield return element;
        }

        public static IEnumerable<Vector2Int> InSegment2(int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = 0; vector.x <= finish; ++vector.x) {
                for (vector.y = 0; vector.y <= finish; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InSegment2(int start, int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = start; vector.x <= finish; ++vector.x) {
                for (vector.y = start; vector.y <= finish; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InSegment2(Vector2Int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = 0; vector.x <= finish.x; ++vector.x) {
                for (vector.y = 0; vector.y <= finish.y; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InSegment2(Vector2Int start, Vector2Int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = start.x; vector.x <= finish.x; ++vector.x) {
                for (vector.y = start.y; vector.y <= finish.y; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InRange2(int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = 0; vector.x < finish; ++vector.x) {
                for (vector.y = 0; vector.y < finish; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InRange2(int start, int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = start; vector.x < finish; ++vector.x) {
                for (vector.y = start; vector.y < finish; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InRange2(Vector2Int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = 0; vector.x < finish.x; ++vector.x) {
                for (vector.y = 0; vector.y < finish.y; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InRange2(Vector2Int start, Vector2Int finish) {
            Vector2Int vector = Vector2Int.zero;
            for (vector.x = start.x; vector.x < finish.x; ++vector.x) {
                for (vector.y = start.y; vector.y < finish.y; ++vector.y) {
                    yield return vector;
                }
            }
        }


    }

}
