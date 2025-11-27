using UnityEngine;
using System.Collections;

public class AR_EnemyHealth : MonoBehaviour
{
    [Header("Configuración")]
    public int vidaMaxima = 10;
    public int puntosPorClick = 10;
    public int puntosAlMorir = 500;
    public bool esElJefeFinal = false; // Marca esto true SOLO para el Golem

    private int vidaActual;
    private Animator animator;
    private bool estaMuerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
        animator = GetComponent<Animator>();
    }

    public void RecibirDaño()
    {
        if (estaMuerto) return;

        vidaActual--;

        // Siempre sumamos los puntos del clic
        if (AR_GameManager.instance != null)
        {
            AR_GameManager.instance.SumarPuntos(puntosPorClick);
        }

        // --- AQUÍ ESTÁ EL CAMBIO ---
        
        // Primero preguntamos: ¿Murió con este golpe?
        if (vidaActual <= 0)
        {
            Morir(); // Si murió, ejecutamos Morir (que activa OnDie) y YA NO HACEMOS NADA MÁS.
        }
        else
        {
            // Si SIGUE VIVO, entonces sí activamos la animación de "Hit/Shoot"
            if (animator != null)
            {
                animator.SetTrigger("OnHit");
            }
        }
    }

    void Morir()
    {
        estaMuerto = true;

        // Animación de Muerte
        if (animator != null)
        {
            animator.SetTrigger("OnDie");
        }

        // Puntos Extra
        if (AR_GameManager.instance != null)
        {
            AR_GameManager.instance.SumarPuntos(puntosAlMorir);
            
            // Avisar al Manager que este enemigo murió
            AR_GameManager.instance.EnemigoDerrotado(this);
        }
    }
}