using UnityEngine;
using UnityEngine.UI; // Necessário para interagir com o texto da UI antiga do Unity (Text)
// Se você estiver usando o TMPro (TextMeshPro), troque a linha de cima por: using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeMovimento = 5f;
    private CharacterController controller;

    [Header("Configurações de Tiro")]
    public GameObject projetilPrefab; 
    public Transform firePoint;       
    public float velocidadeProjetil = 20f;

    [Header("Sistema de Munição")]
    public int municaoMaxima = 30;
    private int municaoAtual;
    public Text textoMunicao; // Se usar TextMeshPro, mude para: public TextMeshProUGUI textoMunicao;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Começa o jogo com a arma totalmente carregada
        municaoAtual = municaoMaxima;
        AtualizarInterfaceMuniacao();
    }

    void Update()
    {
        Movimentar();

        // Detecta o clique para atirar (Apenas se tiver munição)
        if (Input.GetButtonDown("Fire1"))
        {
            if (municaoAtual > 0)
            {
                Atirar();
            }
            else
            {
                Debug.Log("Sem munição! Pressione R para recarregar.");
            }
        }

        // Detecta a tecla R para recarregar
        if (Input.GetKeyDown(KeyCode.R))
        {
            Recarregar();
        }
    }

    void Movimentar()
    {
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");

        Vector3 movimento = transform.right * x + transform.forward * z;

        if (controller != null)
        {
            controller.Move(movimento * velocidadeMovimento * Time.deltaTime);
        }
        else
        {
            transform.Translate(movimento * velocidadeMovimento * Time.deltaTime, Space.World);
        }
    }

    void Atirar()
    {
        if (projetilPrefab != null && firePoint != null)
        {
            // Gasta 1 de munição
            municaoAtual--;
            AtualizarInterfaceMuniacao();

            GameObject tiroCriado = Instantiate(projetilPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = tiroCriado.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * velocidadeProjetil;
            }
        }
    }

    void Recarregar()
    {
        // Só recarrega se já não estiver cheio
        if (municaoAtual < municaoMaxima)
        {
            Debug.Log("Recarregando...");
            municaoAtual = municaoMaxima;
            AtualizarInterfaceMuniacao();
        }
    }

    // Função que atualiza o texto na tela do jogador
    void AtualizarInterfaceMuniacao()
    {
        if (textoMunicao != null)
        {
            textoMunicao.text = municaoAtual + " / " + municaoMaxima;
        }
    }
}