using UnityEngine;
using System.Collections;

public class SpikePressTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private float pressDelay = 2f;
    [SerializeField] private float pressSpeed = 5f;
    [SerializeField] private float returnSpeed = 3f;
    [SerializeField] private Vector3 pressOffset = new Vector3(0, -1, 0);

    private Vector3 initialPosition;
    private Vector3 pressPosition;
    private bool isPressingDown = true;

    private void Start()
    {
        initialPosition = transform.position;
        pressPosition = initialPosition + pressOffset;

        StartCoroutine(PressTrapCycle());
    }

    private IEnumerator PressTrapCycle()
    {
        while (true)
        {
            if (isPressingDown)
            {
                yield return MoveTrap(pressPosition, pressSpeed);
                yield return new WaitForSeconds(pressDelay);
                isPressingDown = false;
            }
            else
            {
                yield return MoveTrap(initialPosition, returnSpeed);
                yield return new WaitForSeconds(pressDelay);
                isPressingDown = true;
            }
        }
    }

    private IEnumerator MoveTrap(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = target; // Snap to target after movement
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by spike press trap!");
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}