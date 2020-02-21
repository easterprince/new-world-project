using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battlefield.UI {

    public class LoadingLogoController : MonoBehaviour {

        // Enumerator.

        public enum Condition {
            Inactive,
            Waiting,
            Loading,
            Ready
        }


        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private Sprite waitingSprite;

        [SerializeField]
        private Sprite loadingSprite;

        [SerializeField]
        private Sprite readySprite;

#pragma warning restore IDE0044, CS0414, CS0649

        private Image image;
        private Condition currentCondition;
        private int currentAngle = 0;


        // Properties.

        public Condition CurrentCondition {
            get => currentCondition;
            set {
                currentCondition = value;
                switch (value) {
                    case Condition.Inactive:
                    image.sprite = null;
                    break;
                    case Condition.Waiting:
                    image.sprite = waitingSprite;
                    break;
                    case Condition.Loading:
                    image.sprite = loadingSprite;
                    break;
                    case Condition.Ready:
                    image.sprite = readySprite;
                    break;
                }
            }
        }


        // Life cycle.

        private void Awake() {
            if (waitingSprite == null) {
                throw new MissingReferenceException($"Missing waiting sprite!");
            }
            if (loadingSprite == null) {
                throw new MissingReferenceException($"Missing loading sprite!");
            }
            if (readySprite == null) {
                throw new MissingReferenceException($"Missing ready sprite!");
            }
            image = GetComponent<Image>();
            if (image == null) {
                throw new MissingComponentException($"Missing Image component!");
            }
            CurrentCondition = Condition.Inactive;
        }

        private void Update() {
            if (currentCondition == Condition.Loading) {
                currentAngle = (currentAngle + 1) % 360;
                Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
                transform.localRotation = rotation;
            }
        }


    }

}
