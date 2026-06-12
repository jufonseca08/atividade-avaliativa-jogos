using UnityEngine;

public class ArmaController : MonoBehaviour
{
    [Header("Referências")]
    public GameObject prefabProjetil; 
    public Transform firePoint;       

    [Header("Configurações")]
    public float forcaDoTiro = 40f;   

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TentarAtirar();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TentarRecarregar();
        }
    }

    void TentarAtirar()
    {
        // TRAVA DE SEGURANÇA: Se você esquecer de colocar a bala, o jogo te avisa sem travar
        if (prefabProjetil == null)
        {
            Debug.LogError("⚠️ Ei! Você esqueceu de arrastar o Prefab da bala para a caixinha da Arma no Inspector!");
            return;
        }

        if (GameManager.instance != null && GameManager.instance.PodeAtirar())
        {
            GameManager.instance.GastarMunicao();

            GameObject bala = Instantiate(prefabProjetil, firePoint.position, firePoint.rotation);

            Rigidbody rb = bala.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * forcaDoTiro, ForceMode.Impulse);
            }
        }
    }
    void TentarRecarregar()
    {
        if (GameManager.instance != null && GameManager.instance.PrecisaRecarregar())
        {
            GameManager.instance.RecarregarMunicao();
        }
    }
}