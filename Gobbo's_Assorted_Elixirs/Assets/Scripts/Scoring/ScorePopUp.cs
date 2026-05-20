using UnityEngine;
using TMPro;

public class ScorePopUp : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lifeTime = 1f;

    private TextMeshPro textMesh;
    private Color textColor;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, Time.deltaTime / lifeTime);

        textColor.a -= Time.deltaTime / lifeTime;

        textMesh.color = textColor;
    }

    public void SetText(string text)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();

        textMesh.text = text;
    }
}