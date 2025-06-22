
using UnityEngine;
    public class RollingWheelSpikeTrap : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 100f;

        private void Update()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player hit by rolling wheel spike trap!");
                StartCoroutine(other.GetComponent<BikeController>().Explode());
            }
        }


    }