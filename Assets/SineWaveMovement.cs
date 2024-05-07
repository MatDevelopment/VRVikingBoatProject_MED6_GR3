using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveMovement : MonoBehaviour
{
    public float amplitude = 1f; // The amplitude of the sine wave
    public float frequency = 1f; // The frequency of the sine wave
    public float movementSpeed = 1f; // The speed of the movement
    public float updateInterval = 0.3f; // Time interval between updates

    private Vector3 startPosition;

    [SerializeField]
    [Header("gives random start value so the boats are not in sync")]
    private float randomStartValue;
    void Start()
    {
        randomStartValue = Random.Range(0, 360);
        startPosition = transform.position;
        StartCoroutine(UpdatePositionCoroutine());
    }

    private IEnumerator UpdatePositionCoroutine()
    {
        while (true)
        {
            // Calculate the new position using a sine wave
            float newYPos = startPosition.y + Mathf.Sin((Time.time + randomStartValue) * frequency) * amplitude;
            Vector3 newPosition = new Vector3(startPosition.x, newYPos, startPosition.z);

            // Move the object towards the new position
            transform.position = Vector3.MoveTowards(transform.position, newPosition, movementSpeed * Time.deltaTime);

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
