using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController: MonoBehaviour
{
    int score = 0;
    int targetScore = 50;
    string message = "";
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public bool isGrounded;
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        Vector3 move = new Vector3(horizontalInput, 0, verticalInput);
        rb.AddForce(move * moveSpeed);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    void OnTriggerEnter(Collider other)
     {
        if (other.CompareTag("Green"))
        {
            AddScore(10);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Red"))
        {
            AddScore(-5);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Finish"))
        {
            if (score >= targetScore)
            {
                RestartGame();
            }
            else
            {
                message = "Yeterli puan yok!";
            }
        }
    }
    void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puan: " + score);
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 32;
        style.normal.textColor = Color.white;
 
        GUI.Label(new Rect(20, 20, 600, 50), "Puan: " + score + " / " + targetScore, style);
 
        if (score >= targetScore)
        {
            GUI.Label(new Rect(20, 70, 800, 50), "Hedef tamam! Bitiş küpüne git!", style);
        }
 
        if (message != "")
        {
            GUI.Label(new Rect(20, 120, 800, 50), message, style);
        }
    }
}