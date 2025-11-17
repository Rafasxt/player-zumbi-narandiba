using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        [Tooltip("Quantos inimigos no total nessa horda")]
        public int totalEnemies = 10;

        [Tooltip("Tempo entre um spawn e outro (segundos)")]
        public float spawnInterval = 2f;

        [Range(0f, 1f)]
        [Tooltip("Chance de nascer um zumbi FAST (0 = só slow, 1 = só fast)")]
        public float fastChance = 0.3f;
    }

    public static EnemySpawner Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject slowPrefab;
    public GameObject fastPrefab;

    [Header("Configuração das Hordas")]
    public Wave[] waves;

    [Header("Onde spawnar")]
    [Tooltip("Raio em volta do player onde os zumbis vão nascer")]
    public float spawnRadius = 10f;

    private PlayerController player;
    private int currentWaveIndex = -1;
    private int enemiesAlive = 0;
    private int enemiesSpawnedThisWave = 0;
    private bool isSpawning = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        StartNextWave();
    }

    public void OnEnemySpawned()
    {
        enemiesAlive++;
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

        // se já acabou de spawnar todos da wave e não tem mais vivos -> próxima wave
        if (!isSpawning && enemiesAlive <= 0)
        {
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            // todas as hordas foram derrotadas -> vitória
            PlayerController p = FindObjectOfType<PlayerController>();
            if (p != null)
                p.SetWin();

            Debug.Log("VITÓRIA: todas as hordas foram derrotadas!");
            SceneManager.LoadScene("VictoryScene");
            return;
        }

        Wave w = waves[currentWaveIndex];
        Debug.Log("Iniciando horda " + (currentWaveIndex + 1) + " - Total de inimigos: " + w.totalEnemies);

        enemiesSpawnedThisWave = 0;
        isSpawning = true;
        StopAllCoroutines();
        StartCoroutine(SpawnWaveRoutine());
    }

    System.Collections.IEnumerator SpawnWaveRoutine()
    {
        Wave w = waves[currentWaveIndex];

        while (enemiesSpawnedThisWave < w.totalEnemies)
        {
            SpawnOne(w);
            enemiesSpawnedThisWave++;
            yield return new WaitForSeconds(w.spawnInterval);
        }

        // terminou de spawnar todos dessa wave
        isSpawning = false;

        // se por acaso todos já morreram, já chama a próxima
        if (enemiesAlive <= 0)
        {
            StartNextWave();
        }
    }

    void SpawnOne(Wave w)
    {
        if (slowPrefab == null || fastPrefab == null)
        {
            Debug.LogWarning("Spawner sem prefabs configurados!");
            return;
        }

        // Posição base: em volta do player. Se não tiver player por algum motivo, usa o próprio spawner.
        Vector2 center = player != null ? (Vector2)player.transform.position : (Vector2)transform.position;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
        Vector2 spawnPos = center + offset;

        GameObject prefabToSpawn = (Random.value < w.fastChance) ? fastPrefab : slowPrefab;

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        // EnemyBasic chama OnEnemySpawned() no Awake, então enemiesAlive sobe lá.
    }
}
