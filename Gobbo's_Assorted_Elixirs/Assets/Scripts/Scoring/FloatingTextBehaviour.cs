using UnityEngine;

public class FloatingTextBehaviour : MonoBehaviour
{
    void LateUpdate() // Late update so it runs after every other update methods finish
    {
        transform.forward = Camera.main.transform.forward; // turns the text object to face the player
    }
}