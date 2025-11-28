using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class AR_GameManager : MonoBehaviour
{
    public static AR_GameManager instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI mensajeFinalText;

    [Header("Cámara y Efectos")]
    public Camera arCamera;
    public GameObject hitParticles;
    public AudioSource audioSource;
    public AudioClip hitSound;

    [Header("Fases del Juego")]
    public GameObject robotObject;
    public GameObject golemObject;

    private int score = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (robotObject != null) robotObject.SetActive(true);
        if (golemObject != null) golemObject.SetActive(false);
        if (mensajeFinalText != null) mensajeFinalText.text = "";
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ProcesarClic(Mouse.current.position.ReadValue());
        }
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
            AR_EnemyHealth enemigo = hit.transform.GetComponent<AR_EnemyHealth>();

            if (enemigo != null)
            {
                enemigo.RecibirDaño();

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

    public void EnemigoDerrotado(AR_EnemyHealth enemigoMuerto)
    {
        StartCoroutine(ManejarFase(enemigoMuerto));
    }

    IEnumerator ManejarFase(AR_EnemyHealth enemigo)
    {
        yield return new WaitForSeconds(2f);

        enemigo.gameObject.SetActive(false);

        if (enemigo.esElJefeFinal)
        {
            if (mensajeFinalText != null) mensajeFinalText.text = "¡MISIÓN COMPLETADA!";
        }
        else
        {
            if (golemObject != null)
            {
                golemObject.SetActive(true);
            }
        }
    }
}