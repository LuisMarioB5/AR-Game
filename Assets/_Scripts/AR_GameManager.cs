using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections; // Necesario para las corrutinas (tiempos de espera)

public class AR_GameManager : MonoBehaviour
{
    public static AR_GameManager instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI mensajeFinalText; // Texto para "¡GANASTE!"

    [Header("Cámara y Efectos")]
    public Camera arCamera;
    public GameObject hitParticles; // Tus chispas
    public AudioSource audioSource;
    public AudioClip hitSound;

    [Header("Fases del Juego")]
    public GameObject robotObject; // Arrastra el Robot aquí
    public GameObject golemObject; // Arrastra el Golem aquí

    private int score = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Configuración Inicial: Robot activo, Golem apagado
        if (robotObject != null) robotObject.SetActive(true);
        if (golemObject != null) golemObject.SetActive(false);
        if (mensajeFinalText != null) mensajeFinalText.text = "";
    }

    void Update()
    {
        // Detectar Clic (Nuevo Input System)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ProcesarClic(Mouse.current.position.ReadValue());
        }
        // Detectar Toque (Móvil)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            ProcesarClic(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    void ProcesarClic(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Buscamos si el objeto tiene vida
            AR_EnemyHealth enemigo = hit.transform.GetComponent<AR_EnemyHealth>();

            if (enemigo != null)
            {
                // 1. Hacer Daño
                enemigo.RecibirDaño();

                // 2. Efectos (Sonido y Partículas)
                if (audioSource && hitSound) audioSource.PlayOneShot(hitSound);
                
                if (hitParticles != null)
                {
                    Instantiate(hitParticles, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }

    public void SumarPuntos(int cantidad)
    {
        score += cantidad;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    // Esta función se llama automáticamente desde AR_EnemyHealth cuando alguien muere
    public void EnemigoDerrotado(AR_EnemyHealth enemigoMuerto)
    {
        StartCoroutine(ManejarFase(enemigoMuerto));
    }

    // Rutina para esperar un momento y cambiar de enemigo
    IEnumerator ManejarFase(AR_EnemyHealth enemigo)
    {
        // Esperamos 2 segundos para ver la animación de muerte
        yield return new WaitForSeconds(2f);

        // Desactivamos el enemigo muerto
        enemigo.gameObject.SetActive(false);

        if (enemigo.esElJefeFinal)
        {
            // SI MURIO EL GOLEM -> FIN DEL JUEGO
            if (mensajeFinalText != null) mensajeFinalText.text = "¡MISIÓN COMPLETADA!";
        }
        else
        {
            // SI MURIO EL ROBOT -> SALE EL GOLEM
            if (golemObject != null)
            {
                golemObject.SetActive(true);
                // Opcional: Sonido de aparición del jefe
            }
        }
    }
}