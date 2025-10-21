using UnityEngine;

public class EnemySpawner : MonoBehaviour, IHaveDamage
{
    [SerializeField] ParticleSystem damageParticles;


    private float health = 100f;
    private readonly float spawnSpeed = 10f;
    private float spawnTimer = 0f;

    private readonly int maxSpawnCount = 5;
    private int spawnCount = 0;
    private bool alive = true;
    private bool paused = false;

    private void Start()
    {

        GameInputProcessor.Instance.OnGamePause += GameInputProcessor_OnGamePause;
    }

    private void GameInputProcessor_OnGamePause(object sender, System.EventArgs e)
    {
        paused = !paused;
    }



    // Update is called once per frame
    void Update()
    {
        if (paused) { return; }

        spawnTimer += Time.deltaTime * Random.value;

        if (spawnTimer > spawnSpeed && spawnCount < maxSpawnCount)
        {
            //Debug.Log(spawnTimer.ToString());
            //Debug.Log("Spawned Enemy");
            spawnTimer = 0f;
            spawnCount += 1;

        }


        if (!alive)
        {
            Destroy(gameObject);
        }
    }


    public bool Damage(float damage)
    {
        health -= damage;
        damageParticles.Play();

        if (health <= 0f)
        {
            alive = false;
            return true;

        }

        return false;

    }
}
