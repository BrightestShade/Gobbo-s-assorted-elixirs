using UnityEngine;

public class IngredientPickUp : MonoBehaviour
{
    bool isHolding = false;

    [SerializeField]
    float throwForce = 600f;

    [SerializeField]
    float maxDistance = 3f;

    float distance;

    TempParent tempParent;
    Rigidbody rb;

    Vector3 objectPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
    }

    void Update()
    {
        if (tempParent != null)
        {
            distance = Vector3.Distance(
                transform.position,
                tempParent.transform.position);
        }

        if (isHolding)
        {
            Hold();
        }
    }

    private void OnMouseDown()
    {
        if (tempParent != null)
        {
            if (distance <= maxDistance)
            {
                isHolding = true;

                rb.useGravity = false;
                rb.detectCollisions = true;

                transform.SetParent(tempParent.transform);

                // Snap to hold point
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            Debug.Log("Temp Parent item not found in scene");
        }
    }

    private void OnMouseUp()
    {
        Drop();
    }

   /* private void OnMouseExit()
    {
        Drop();
    }*/

    private void Hold()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (distance >= maxDistance)
        {
            Drop();
        }
    }

    private void Drop()
    {
        if (isHolding)
        {
            isHolding = false;
            objectPos = transform.position;
            transform.position = objectPos;
            transform.SetParent(null);
            rb.useGravity = true;
        }
    }
}