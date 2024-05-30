using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace {

    public class CameraDrift : MonoBehaviour {

        [SerializeField] private Vector2 _min;
        [SerializeField] private Vector2 _max;
        [SerializeField] private Vector2 _yRotationRange;
        [SerializeField] private float _lerpSpeed = 0.05f;
        
        private Vector3 _newPosition;
        private Quaternion _newRotation;

        private void Awake() {

            var cameraTransform = transform;
            _newPosition = cameraTransform.position;
            _newRotation = cameraTransform.rotation;
        }

        private void Update() {

            Transform cameraTransform;
            (cameraTransform = transform).position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * _lerpSpeed);
            transform.rotation = Quaternion.Lerp(cameraTransform.rotation, _newRotation, Time.deltaTime * _lerpSpeed);

            if (Vector3.Distance(transform.position, _newPosition) < 1f) {

                GetNewPosition();
            }
        }

        private void GetNewPosition() {

            var xPos = Random.Range(_min.x, _max.x);
            var zPos = Random.Range(_min.y, _max.y);
            _newRotation = Quaternion.Euler(0, Random.Range(_yRotationRange.x, _yRotationRange.y), 0);
            _newPosition = new Vector3(xPos, 0, zPos);
        }

    }

}
