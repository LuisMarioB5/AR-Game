using UnityEngine;
using System.Collections;

public class AR_EnemyHealth : MonoBehaviour
{
    [Header("Configuración")]
    public int vidaMaxima = 10;
    public int puntosPorClick = 10;
    public int puntosAlMorir = 500;
    public bool esElJefeFinal = false;

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

        if (AR_GameManager.instance != null)
        {
            AR_GameManager.instance.SumarPuntos(puntosPorClick);
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("OnHit");
            }
        }
    }

    void Morir()
    {
        estaMuerto = true;

        if (animator != null)
        {
            animator.SetTrigger("OnDie");
        }

        if (AR_GameManager.instance != null)
        {
            AR_GameManager.instance.SumarPuntos(puntosAlMorir);
            
            AR_GameManager.instance.EnemigoDerrotado(this);
        }
    }
}