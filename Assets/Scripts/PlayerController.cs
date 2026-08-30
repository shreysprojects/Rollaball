using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb; 

    // Variable to keep track of collected "PickUp" objects.
    private int count;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;

    // UI text component to display count of "PickUp" objects collected.
    public TextMeshProUGUI countText;

    // UI object to display winning text.
    public GameObject winTextObject;

    // Array of colors for the pickups
    private Color[] pickupColors = new Color[]
    {
        Color.red,      // 1 point
        Color.blue,     // 2 points
        Color.green,    // 3 points
        Color.yellow,   // 4 points
        Color.cyan,     // 5 points
        Color.magenta,  // 6 points
        new Color(1f, 0.5f, 0f), // Orange - 7 points
        new Color(0.5f, 0f, 1f), // Purple - 8 points
        Color.white     // 4 points
    };

    // Point values for each pickup (totaling 30: 1+2+3+4+5+6+7+8+4 = 40... let me recalculate)
    // 1+2+3+4+5+6+7+8 = 36, so the 9th needs to be -6 to get 30
    // But let's use positive values: 1+2+3+4+5+6+7+1+1 = 30
    private int[] pointValues = new int[] { 1, 2, 3, 4, 5, 6, 7, 1, 1 }; // Totals to 30

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        // Initialize count to zero.
        count = 0;

        // Update the count display.
        SetCountText();

        // Initially set the win text to be inactive.
        winTextObject.SetActive(false);

        // Setup pickups with different colors and point values
        SetupPickups();
    }
 
    // Setup all pickups with different colors
    void SetupPickups()
    {
        // Find all objects with the "PickUp" tag
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");

        Debug.Log("Number of pickups found: " + pickups.Length);

        // Assign colors and point values to each pickup
        for (int i = 0; i < pickups.Length; i++)
        {
            // Use modulo to wrap around if there are more pickups than colors
            int colorIndex = i % pickupColors.Length;
            int pointIndex = i % pointValues.Length;

            // Get or add the Renderer component
            Renderer renderer = pickups[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = pickupColors[colorIndex];
            }

            // Add a PickupValue component to store the point value
            PickupValue pickupValue = pickups[i].GetComponent<PickupValue>();
            if (pickupValue == null)
            {
                pickupValue = pickups[i].AddComponent<PickupValue>();
            }
            pickupValue.points = pointValues[pointIndex];

            Debug.Log("Pickup " + i + " assigned " + pointValues[pointIndex] + " points");
        }
    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate() 
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed); 
    }

    void OnTriggerEnter(Collider other) 
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("PickUp")) 
        {
            // Get the point value from the pickup
            PickupValue pickupValue = other.gameObject.GetComponent<PickupValue>();
            int points = (pickupValue != null) ? pickupValue.points : 1;

            Debug.Log("Collected pickup worth " + points + " points. Total before: " + count);

            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);

            // Increment the count by the pickup's point value.
            count = count + points;

            Debug.Log("Total after: " + count);

            // Update the count display.
            SetCountText();
        }
    }

    // Function to update the displayed count of "PickUp" objects collected.
    void SetCountText() 
    {
        // Update the count text with the current count.
        countText.text = "Count: " + count.ToString();

        // Check if the count has reached the win condition.
        if (count >= 30)
        {
            // Display the win text.
            winTextObject.SetActive(true);

            // Destroy the enemy GameObject.
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));

            // Stop the game
            Time.timeScale = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the current object
            Destroy(gameObject); 
     
            // Update the winText to display "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";

            // Stop the game
            Time.timeScale = 0;
        }
    }
}

// Helper component to store point values for pickups
public class PickupValue : MonoBehaviour
{
    public int points = 1;
}