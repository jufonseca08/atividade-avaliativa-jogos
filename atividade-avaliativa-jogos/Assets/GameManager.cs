using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Textos da Interface (Arraste do Canvas)")]
    public TextMeshProUGUI textoScore; 
    public TextMeshProUGUI textoAmmo;  
    public TextMeshProUGUI textoTime;  
    public GameObject telaGameOver;    

    [Header("Valores do Jogo")]
    public int score = 0;
    public int municaoAtual = 30;  
    public int municaoMaxima = 30; 
    public float tempo = 60f;      
    private bool jogoAtivo = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        municaoAtual = municaoMaxima; 
        AtualizarTextos();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ReiniciarJogo();
        }

        if (jogoAtivo)
        {
            tempo -= Time.deltaTime;
            AtualizarTextos();

            if (tempo <= 0)
            {
                FimDeJogo();
            }
        }
    }

    public void AdicionarScore(int pontos)
    {
        if (jogoAtivo)
        {
            score += pontos;
            AtualizarTextos();
        }
    }

    public void GastarMunicao()
    {
        if (jogoAtivo && municaoAtual > 0)
        {
            municaoAtual--;
            AtualizarTextos();
        }
    }

    public void RecarregarMunicao()
    {
        if (jogoAtivo)
        {
            municaoAtual = municaoMaxima;
            AtualizarTextos();
        }
    }

    public bool PodeAtirar()
    {
        return (jogoAtivo && municaoAtual > 0);
    }

    public bool PrecisaRecarregar()
    {
        return (municaoAtual < municaoMaxima);
    }

    void AtualizarTextos()
    {
        if (textoScore != null) textoScore.text = "Score: " + score;

        if (textoAmmo != null)
        {
            textoAmmo.text = "Ammo: " + municaoAtual + " / " + municaoMaxima + " |"; 
        }

        if (textoTime != null)
        {
            textoTime.text = "Tempo: " + Mathf.RoundToInt(tempo);
        }
    }

    void FimDeJogo()
    {
        jogoAtivo = false;
        tempo = 0; 
        if (telaGameOver != null) telaGameOver.SetActive(true);
    }

    void ReiniciarJogo()
    {
        string nomeDaCenaAtual = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(nomeDaCenaAtual))
        {
            SceneManager.LoadScene(nomeDaCenaAtual);
        }
    }
}