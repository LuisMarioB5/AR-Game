using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // <-- 1. ¡IMPORTANTE! Añadimos esta librería

public class ARGameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Camera arCamera; 
    
    private int score = 0;

    void Update()
    {
        // 2. CORRECCIÓN: Usamos Mouse.current en lugar de Input.GetMouseButtonDown
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckClick();
        }
        
        // (Opcional) Si luego lo pruebas en móvil (Android), esto activaría el toque:
        // else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        // {
        //     CheckClick();
        // }
    }

    void CheckClick()
    {
        // 3. CORRECCIÓN: Obtenemos la posición desde el nuevo sistema
        Vector2 clickPosition = Vector2.zero;

        if (Mouse.current != null)
        {
            clickPosition = Mouse.current.position.ReadValue();
        }
        // else if (Touchscreen.current != null) { clickPosition = Touchscreen.current.primaryTouch.position.ReadValue(); }

        // Lanzar el rayo desde esa posición
        Ray ray = arCamera.ScreenPointToRay(clickPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Si golpeamos al Robot...
            if (hit.transform.CompareTag("Enemy"))
            {
                AddScore();
                Debug.Log("¡Robot tocado!");
            }
        }
    }

    void AddScore()
    {
        score++;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}