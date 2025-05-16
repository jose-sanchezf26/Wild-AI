using UnityEngine;

public class RegisteredIcon : MonoBehaviour
{
    public SpriteRenderer registeredIcon;

    // Update is called once per frame
    void Update()
    {
        // Detectar el clic derecho sobre el objeto
        if (Input.GetMouseButtonDown(2)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Si el clic es sobre este objeto
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // ChangeColor();
                registeredIcon.gameObject.SetActive(!registeredIcon.gameObject.activeSelf);
            }
        }
    }
    private void ChangeColor()
    {
        if (registeredIcon != null)
        {
            if( registeredIcon.color == Color.red)
            {
                registeredIcon.color = Color.green; // Cambia a blanco
            }
            else
            {
                registeredIcon.color = Color.red; // Cambia a rojo
            }
        }
    }
}
