using UnityEngine;

public class BalaCodigo : MonoBehaviour
{
    public float velocidade = 10f;
    public float tempoVida = 50f;

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