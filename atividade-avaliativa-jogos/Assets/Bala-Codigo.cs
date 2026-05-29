using UnityEngine;

public class Bala-Código : MonoBehaviour
{
    public float velocidade = 20f;
    public float tempoVida = 3f; // 

    void Start()
    {
        
        Destroy(gameObject, tempoVida);
    }

    void Update()
    {
        
        transform.Translate(Vector3.forward * velocidade * Time.deltaTime);
    }

    
    void OnCollisionEnter(Collision collision)
    {
        
        Destroy(gameObject);
    }
}
}
