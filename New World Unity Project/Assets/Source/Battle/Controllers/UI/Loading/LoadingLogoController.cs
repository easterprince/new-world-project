using NewWorld.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battle.Controllers.UI.Loading {

    public class LoadingLogoController : MonoBehaviour {

        // Enumerator.

        public enum Condition {
            Waiting,
            Loading,
            Ready
        }


        // Fields.

        private Condition currentCondition = Condition.Waiting;
        private float currentAngle = 0;
        [SerializeField]
        private float rotationFrequency = 1;

        // Components.
        private Image image;

        // Sprites.
        [SerializeField]
        private Sprite waitingSprite;
        [SerializeField]
        private Sprite loadingSprite;
        [SerializeField]
        private Sprite readySprite;


        // Properties.

        public Condition CurrentCondition {
            get => currentCondition;
            set {
                currentCondition = value;
                UpdateSprite();
            }
        }

        public float RotationFrequency {
            get => rotationFrequency;
            set => rotationFrequency = value;
        }

        public Sprite WaitingSprite {
            get => waitingSprite;
            set {
                waitingSprite = value;
                UpdateSprite();
            }
        }

        public Sprite LoadingSprite {
            get => loadingSprite;
            set {
                loadingSprite = value;
                UpdateSprite();
            }
        }

        public Sprite ReadySprite {
            get => readySprite;
            set {
                readySprite = value;
                UpdateSprite();
            }
        }


        // Life cycle.

        private void Awake() {
            image = GetComponent<Image>();
            GameObjects.ValidateComponent(image);
        }

        private void LateUpdate() {
            if (currentCondition == Condition.Loading) {
                currentAngle = (currentAngle + 360 * rotationFrequency * Time.deltaTime) % 360;
                Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
                transform.localRotation = rotation;
            }
        }


        // Support methods.

        private void UpdateSprite() {
            switch (currentCondition) {
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

}
