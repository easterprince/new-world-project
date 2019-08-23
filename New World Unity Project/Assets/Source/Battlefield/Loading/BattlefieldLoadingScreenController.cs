using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battlefield.Loading {

    public class BattlefieldLoadingScreenController : MonoBehaviour {

        [SerializeField]
        public GameObject readyTextPanel;

        [SerializeField]
        public GameObject logo;

        [SerializeField]
        public Sprite rotatingSprite;

        [SerializeField]
        public Sprite stoppedSprite;

        private Image logoImage;
        private bool rotating;
        private int logoRotation = 0;

        public bool LoadingAnimation {
            set {
                rotating = value;
                readyTextPanel.SetActive(!value);
                logoImage.sprite = rotating ? rotatingSprite : stoppedSprite;
            }
            get => rotating;
        }


        // Life cycle.

        void Awake() {
            logoImage = logo.GetComponent<Image>();
            LoadingAnimation = true;
        }

        void Update() {
            if (rotating) {
                ++logoRotation;
                if (logoRotation == 360) {
                    logoRotation = 0;
                }
                logo.transform.rotation = new Quaternion() {
                    eulerAngles = new Vector3(0, 0, logoRotation)
                };
            }
        }

    }

}
