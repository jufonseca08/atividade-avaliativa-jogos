using UnityEngine;
using System.Collections; // Precisamos disso para fazer o Unity saber contar o tempo!
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform position;         // Posição do spawn (objeto vazio)
        public GameObject targetPrefab;    // Qual target vai nascer
        public int quantity = 1;           // Quantos targets
        public Vector3 scale = Vector3.one; // Tamanho
        public Vector3 rotation = Vector3.zero; // Rotação

        // ADICIONADO: O nosso freio para o apocalipse zumbi!
        public float respawnDelay = 3f; 

        // Movimento
        public bool moveHorizontal = false;
        public bool moveVertical = false;
        public float moveSpeed = 3f;
        public float moveRange = 5f;

        public int health = 1;
        public int pointsValue = 10;

        // Variável invisível só para o Spawner não se perder na contagem
        [HideInInspector]
        public int currentlySpawning = 0; 
    }

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    private List<GameObject> spawnedTargets = new List<GameObject>();

    void Start()
    {
        // Ao dar Play, os primeiros alvos nascem instantaneamente
        SpawnAllTargets();
    }

    void Update()
    {
        // Remove targets destruídos da lista
        spawnedTargets.RemoveAll(t => t == null);

        // Conta targets ativos por spawn point
        foreach (SpawnPoint point in spawnPoints)
        {
            int currentCount = 0;
            foreach (GameObject target in spawnedTargets)
            {
                if (target != null)
                {
                    Target targetScript = target.GetComponent<Target>();
                    if (targetScript != null && targetScript.spawnPoint == point)
                        currentCount++;
                }
            }

            // Se o número de alvos vivos + alvos "na fila" de espera for menor que o limite, inicia a contagem!
            if ((currentCount + point.currentlySpawning) < point.quantity)
            {
                StartCoroutine(SpawnWithDelay(point));
            }
        }
    }

    void SpawnAllTargets()
    {
        foreach (SpawnPoint point in spawnPoints)
        {
            for (int i = 0; i < point.quantity; i++)
            {
                SpawnTarget(point);
            }
        }
    }

    // ADICIONADO: A rotina que obriga o jogo a esperar antes de fabricar outro alvo
    IEnumerator SpawnWithDelay(SpawnPoint point)
    {
        // Coloca o alvo na "fila" para não criar 50 cópias acidentalmente enquanto espera
        point.currentlySpawning++;

        // O SEGREDO ESTÁ AQUI: O jogo cruza os braços e espera os segundos configurados!
        yield return new WaitForSeconds(point.respawnDelay);

        // Nasce o alvo
        SpawnTarget(point);

        // Tira o alvo da "fila"
        point.currentlySpawning--;
    }

    void SpawnTarget(SpawnPoint point)
    {
        if (point.position == null || point.targetPrefab == null) return;

        // Cria o target
        GameObject target = Instantiate(point.targetPrefab, point.position.position, Quaternion.Euler(point.rotation));
        target.transform.localScale = point.scale;

        // Configura o target
        Target targetScript = target.GetComponent<Target>();
        if (targetScript != null)
        {
            targetScript.spawnPoint = point;
            targetScript.moveHorizontal = point.moveHorizontal;
            targetScript.moveVertical = point.moveVertical;
            targetScript.moveSpeed = point.moveSpeed;
            targetScript.moveRange = point.moveRange;
            targetScript.health = point.health;
            targetScript.pointsValue = point.pointsValue;
        }

        spawnedTargets.Add(target);
    }
}