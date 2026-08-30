using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float bounceHeight = 0.5f;
    public float bounceSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        float yOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }
}
