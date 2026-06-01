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




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

        if (isHolding)
        {
            Hold();
            
        }
    }

     private void OnMouseDown()
     {
         // pickup
         if(tempParent != null)
         {
             maxDistance = Vector3.Distance(this.transform.position, tempParent.transform.position);
             if (distance <= maxDistance)
             {
                 isHolding = true;
                 rb.useGravity = false;
                 rb.detectCollisions = true;

                 this.transform.SetParent(tempParent.transform);
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

       private void OnMouseExit()
       {
           Drop();
       }

     private void Hold()
     {
         rb.linearVelocity = Vector3.zero;
         rb.angularVelocity = Vector3.zero;

        if(distance >= maxDistance)
         {
             Drop();
         }
     }

     private void Drop()
     {
         if (isHolding)
         {
             isHolding = false;
             objectPos = this.transform.position;
             this.transform.position = objectPos;
             this.transform.SetParent(null);
             rb.useGravity = true;
         }
     }
   



}
