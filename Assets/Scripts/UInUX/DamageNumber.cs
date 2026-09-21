using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float floatSpeed = 1.5f;
    public float lifetime = 0.8f;
    private float timer;

    public void Setup(int amount, bool isCrit)
{
    textMesh.text = amount.ToString();
    textMesh.alignment = TextAlignmentOptions.Center;
    textMesh.color = isCrit ? Color.yellow : Color.white;
    textMesh.fontSize = isCrit ? 8 : 6;
}

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        transform.forward = Camera.main.transform.forward;

        timer += Time.deltaTime;
        Color c = textMesh.color;
        textMesh.color = new Color(c.r, c.g, c.b, 1f - (timer / lifetime));

        if (timer >= lifetime) Destroy(gameObject);
    }
}