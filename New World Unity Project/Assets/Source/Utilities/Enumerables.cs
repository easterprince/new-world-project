using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities {

    public static class Enumerables {

        // Trivial.

        public static IEnumerable<T> GetNothing<T>() {
            yield break;
        }

        public static IEnumerable<T> GetSingle<T>(T element) {
            yield return element;
        }


        // Unions.

        public static IEnumerable<T> Unite<T>(T element1, T element2) {
            yield return element1;
            yield return element2;
        }

        public static IEnumerable<T> Unite<T>(params T[] elements) {
            foreach (T element in elements) {
                yield return element;
            }
        }

        public static IEnumerable<T> Unite<T>(IEnumerable<T> enumerable, T additional) {
            foreach (T element in enumerable) {
                yield return element;
            }
            yield return additional;
        }

        public static IEnumerable<T> Unite<T>(IEnumerable<T> enumerable, params T[] elements) {
            foreach (T element in enumerable) {
                yield return element;
            }
            foreach (T element in elements) {
                yield return element;
            }
        }

        public static IEnumerable<T> Unite<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2) {
            foreach (T element in enumerable1) {
                yield return element;
            }
            foreach (T element in enumerable2) {
                yield return element;
            }
        }

        public static IEnumerable<T> Unite<T>(params IEnumerable<T>[] enumerables) {
            foreach (IEnumerable<T> enumerable in enumerables) {
                foreach (T element in enumerable) {
                    yield return element;
                }
            }
        }


        // Vector2Int.

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
