using System.Collections;
using System.Collections.Generic;

namespace NewWorld.Utilities {

    public class SingleElementEnumerable<T> : IEnumerable<T> {

        // Fields.

        private T element;


        // Properties.

        public T Element {
            get => element;
            set => element = value;
        }


        // Constructor.

        public SingleElementEnumerable(T element) {
            this.element = element;
        }


        // Enumeration.

        public IEnumerator<T> GetEnumerator() {
            yield return element;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            yield return element;
        }


    }

}
