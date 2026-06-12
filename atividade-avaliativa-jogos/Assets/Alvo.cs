using UnityEngine;

public class Alvo : MonoBehaviour
{
    [Header("Configurações")]
    public int pontosGanhos = 10; // Quantos pontos esse alvo vai dar ao morrer

    // Essa função roda automaticamente quando algo bate no Alvo
    void OnCollisionEnter(Collision collision)
    {
        // Verifica se o objeto que bateu tem "projétil" no nome
        if (collision.gameObject.name.Contains("projétil"))
        {
            // 1. Dá pontos para o jogador lá no GameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.AdicionarScore(pontosGanhos);
            }

            // 2. Destrói a bala para ela não continuar voando
            Destroy(collision.gameObject);

            // 3. Destrói o próprio alvo (faz ele desaparecer do jogo)
            Destroy(gameObject);
        }
    }
}