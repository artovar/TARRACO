using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Linq;

public class SpawnPoint : MonoBehaviour
{
    public static string LevelsPath = "Assets/Scripts/LevelXmls/Level";

    private int numPlayers;
    private List<Transform> players = new List<Transform>();

    public GameObject[] enemyPrefabs;
    public GameObject[] bossesPrefabs;
    public Material[] materialsForSpartan;
    public GameObject[] weaponPrefabs;
    public GameObject[] healingPrefabs;

    public GameObject[] points;
    public Transform[] audience;
    public int secondsSpawn = 5;
    private int deathCount = 0;

    private IEnumerator coroutine;
    
    public int maxEnemies;
    private int maxDef;

    private bool spawnedBoss;

    private List<EnemyController> enemies = new List<EnemyController>();
    private List<int> enemiesReady = new List<int>();
    private List<int> weaponsReady = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        //players = GameObject.FindGameObjectsWithTag("Player");
        //numPlayers = players.Length;
        ArenaGameController arena = GameController.Instance.GetComponent<ArenaGameController>();

        maxDef = maxEnemies;
        if(arena != null)
        {
            switch (arena.modeSelected)
            {
                case ModesEnum.AgainsAI:
                    coroutine = ArenaEnemies(secondsSpawn);
                    StartCoroutine(coroutine);
                    break;
                case ModesEnum.KingOfTheHill:
                    coroutine = SpawnWeapons(3, 15);
                    StartCoroutine(coroutine);
                    coroutine = SpawnLife(7, 25);
                    StartCoroutine(coroutine);
                    break;
                case ModesEnum.FreeForAll:
                    coroutine = SpawnWeapons(3, 15);
                    StartCoroutine(coroutine);
                    coroutine = SpawnLife(7, 25);
                    StartCoroutine(coroutine);
                    break;
            }
        }
        else
        {
            LevelGameController level = GameController.Instance.GetComponent<LevelGameController>();
            level.spawn = this;
            if(level.currentLevel == 1) coroutine = SpawnFromFile(secondsSpawn);
            else coroutine = SpawnEnemy(secondsSpawn);
            StartCoroutine(coroutine);
        }
    }
    private IEnumerator SpawnWeapons(float lowerLimit, float upperLimit)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(lowerLimit, upperLimit));
        while (true)
        {
            Transform single = audience[UnityEngine.Random.Range(0, audience.Length - 1)];
            GameObject newWeapon = Instantiate(weaponPrefabs[UnityEngine.Random.Range(0, weaponPrefabs.Length - 1)],
                 single.position, single.rotation);
            newWeapon.GetComponent<Rigidbody>().AddForce(single.forward * 10 * newWeapon.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
            newWeapon.GetComponent<WeaponScript>().DestroyAfterSpawning();
            yield return new WaitForSeconds(UnityEngine.Random.Range(lowerLimit, upperLimit));
        }
    }
    private IEnumerator SpawnLife(float lowerLimit, float upperLimit)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(lowerLimit, upperLimit));
        while (true)
        {
            Transform single = audience[UnityEngine.Random.Range(0, audience.Length - 1)];
            GameObject heal = Instantiate(healingPrefabs[UnityEngine.Random.Range(0, healingPrefabs.Length - 1)],
                single.position, single.rotation);
            heal.GetComponent<Rigidbody>().AddForce(single.forward * 40, ForceMode.Impulse);
            Destroy(heal, 20f);
            yield return new WaitForSeconds(UnityEngine.Random.Range(lowerLimit, upperLimit));
        }
    }
    private IEnumerator SpawnFromFile(float time)
    {
        LevelGameController controller = GameController.Instance.GetComponent<LevelGameController>();
        bool finishedReading = false;
        while (true)
        {
            if(!finishedReading)
            {
                int level = controller.currentLevel;
                string curLevelPath = LevelsPath + level + ".xml";
                XDocument doc = XDocument.Load(curLevelPath);
                int element = 0;
                foreach (XElement node in doc.Root.Elements())
                {
                    print("Im reading " + element + " element");
                    if (spawnedBoss) break;
                    switch (node.Name.LocalName)
                    {
                        case "spawn":
                            while(WaitForSpace(time) > 0) {
                                yield return new WaitForSeconds(.5f);
                            }
                            LevelSpawn(controller, node.Attribute("kind").Value, node.Attribute("wp").Value);
                            yield return new WaitForSeconds(time);
                            break;
                        case "setTime":
                            time = ParseStrToFloat(node.Attribute("t").Value);
                            print(time);
                            break;
                        case "setMax":
                            maxEnemies = int.Parse(node.Attribute("n").Value);
                            print(maxEnemies);
                            break;
                        case "spMult":
                            int num = int.Parse(node.Attribute("num").Value);
                            while(num > 0)
                            {
                                while (WaitForSpace(time) > 0)
                                {
                                    print("Waiting");
                                    yield return new WaitForSeconds(.5f);
                                }
                                LevelSpawn(controller, "r", "r");
                                yield return new WaitForSeconds(time);
                                num--;
                            }
                            break;
                    }
                    element++;
                }
                finishedReading = true;
            }
            while (WaitForSpace(time) > 0)
            {
                yield return new WaitForSeconds(.5f);
            }
            LevelSpawn(controller, "r", "r");
            yield return new WaitForSeconds(time);
            if (spawnedBoss) break;
        }
    }
    float ParseStrToFloat(string s)
    {
        string sleft = s.Substring(0,s.IndexOf("."));
        string srigth = s.Substring(s.IndexOf(".")+1);
        return ((float)int.Parse(sleft) + int.Parse(srigth)/(10f*srigth.Length));        
    }
    void LevelSpawn(LevelGameController controller, string kind, string wp)
    {
        int e = 0;
        GameObject enemy;
        switch (kind)
        {
            case "r":
                e = enemiesReady[UnityEngine.Random.Range(0, enemiesReady.Count)];
                break;
            default:
                e = int.Parse(kind);
                if (!enemiesReady.Contains(e)) enemiesReady.Add(e);
                break;
        }
        enemy = enemyPrefabs[e];
        if (e == 0) enemy.GetComponentInChildren<SkinnedMeshRenderer>().material = materialsForSpartan[UnityEngine.Random.Range(0, materialsForSpartan.Length)];
        GameObject sp = BetterSP();
        //Instanciamos el prefab del enemigo en el punto
        enemies.Add(Instantiate(enemy, sp.transform.position, Quaternion.identity).GetComponent<EnemyController>());
        enemies[enemies.Count - 1].MoveTowardsInSpawn(sp.transform.forward);
        enemies[enemies.Count - 1].SetAccuracy(controller.currentLevel / controller.GetTotalLevels());
        int w = 0;
        switch(wp)
        {
            case "r":
                w = weaponsReady[UnityEngine.Random.Range(0, enemiesReady.Count)];
                break;
            default:
                w = int.Parse(wp);
                if (!weaponsReady.Contains(w)) weaponsReady.Add(w);
                break;
        }
        if(w != 0) Instantiate(weaponPrefabs[w-1], sp.transform.position, Quaternion.identity);
    }

    float WaitForSpace(float intervalo)
    {
        if (enemies.Count >= maxEnemies)
        {
            int i = 0;
            List<int> deadGuys = new List<int>();
            foreach (EnemyController e in enemies)
            {
                if (e.IsDead())
                {
                    deathCount++;
                    deadGuys.Add(i); // ESTO PUEDE DAR PROBLEMAS SI EL TIEMPO DE MUERTE ES MENOR QUE EL INTERVALO DE SPAWN
                }
                i++;
            }
            i = 0;
            foreach (int d in deadGuys)
            {
                enemies.RemoveAt(d - i);
                i++;
            }
            print("Cant come yet");
            return intervalo;
        }
        print("Im here");
        return 0;
    }

    private IEnumerator SpawnEnemy(float time)  {
        LevelGameController controller = GameController.Instance.GetComponent<LevelGameController>();
        while(true) {
            if (!spawnedBoss)
            {
                int i = 0;
                List<int> deadGuys = new List<int>();
                foreach (EnemyController e in enemies)
                {
                    if (e.IsDead())
                    {
                        deathCount++;
                        deadGuys.Add(i); // ESTO PUEDE DAR PROBLEMAS SI EL TIEMPO DE MUERTE ES MENOR QUE EL INTERVALO DE SPAWN
                    }
                    i++;
                }
                i = 0;
                foreach(int d in deadGuys)
                {
                    enemies.RemoveAt(d - i);
                    i++;
                }
                if(enemies.Count < maxEnemies)
                {
                    int e = UnityEngine.Random.Range(0, enemyPrefabs.Length);
                    GameObject enemy = enemyPrefabs[e];
                    if(e == 0) enemy.GetComponentInChildren<SkinnedMeshRenderer>().material = materialsForSpartan[UnityEngine.Random.Range(0, materialsForSpartan.Length)];
                    GameObject sp = BetterSP();
                    //Instanciamos el prefab del enemido en el punto
                    enemies.Add(Instantiate(enemy, sp.transform.position, Quaternion.identity).GetComponent<EnemyController>());
                    enemies[enemies.Count - 1].MoveTowardsInSpawn(sp.transform.forward);
                    enemies[enemies.Count - 1].SetAccuracy(controller.currentLevel/controller.GetTotalLevels());
                    Instantiate(weaponPrefabs[UnityEngine.Random.Range(0, weaponPrefabs.Length)], sp.transform.position, Quaternion.identity);
                }
                maxEnemies = (int) (maxDef * Mathf.Log(1 + deathCount));
            }
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator ArenaEnemies(float time)  {
        while(true) {
            int i = 0;
            List<int> deadGuys = new List<int>();
            foreach (EnemyController e in enemies)
            {
                if (e.IsDead())
                {
                    deathCount++;
                    deadGuys.Add(i); // ESTO PUEDE DAR PROBLEMAS SI EL TIEMPO DE MUERTE ES MENOR QUE EL INTERVALO DE SPAWN
                }
                i++;
            }
            i = 0;
            foreach(int d in deadGuys)
            {
                enemies.RemoveAt(d - i);
                i++;
            }


            if(enemies.Count < maxEnemies)
            {
                int e = UnityEngine.Random.Range(0, enemyPrefabs.Length);
                GameObject enemy = enemyPrefabs[e];
                if(e == 0) enemy.GetComponentInChildren<SkinnedMeshRenderer>().material = materialsForSpartan[UnityEngine.Random.Range(0, materialsForSpartan.Length)];
                GameObject sp = BetterSP();
                //Instanciamos el prefab del enemido en el punto
                enemies.Add(Instantiate(enemy, sp.transform.position, Quaternion.identity).GetComponent<EnemyController>());
                enemies[enemies.Count - 1].MoveTowardsInSpawn(sp.transform.forward);
                enemies[enemies.Count - 1].SetAccuracy(1 - time);
                Instantiate(weaponPrefabs[UnityEngine.Random.Range(0, weaponPrefabs.Length)], sp.transform.position, Quaternion.identity);
            }
            maxEnemies = (int) (maxDef + Mathf.Sqrt(deathCount) / 2);
            time = time - secondsSpawn * 0.01f * deadGuys.Count;
            if (time < 0.1f) time = .1f;
            yield return new WaitForSeconds(time);
        }
    }

    private float Distance(GameObject p1, GameObject p2) {
        float x = (p1.transform.position.x - p2.transform.position.x);
        float y = (p1.transform.position.y - p2.transform.position.y);
        float z = (p1.transform.position.z - p2.transform.position.z);
        return (float)Mathf.Sqrt(x*x + y*y + z*z);
    }

    public void AddPlayer(Transform root)
    {
        players.Add(root);
    }

    private GameObject BetterSP() {
        float betterDistance = 0;
        GameObject betterPoint = null;
        foreach(GameObject point in points) {
            float shortestToPlayers = 100;
            float trying;
            foreach(Transform thePlayer in players) {
                if(thePlayer != null)
                {
                    trying = (point.transform.position - thePlayer.position).magnitude;
                    if (trying < shortestToPlayers)
                    {
                        shortestToPlayers = trying;
                    }
                }
            }
            if(shortestToPlayers > betterDistance) {
                betterDistance = shortestToPlayers;
                betterPoint = point;
            }
        }
        return betterPoint;
    }


    public Transform SpawnBoss(int level)
    {
        spawnedBoss = true;
        GameObject sp = BetterSP();
        //Instanciamos el prefab del enemido en el punto
        if (level > bossesPrefabs.Length)
        {
            print("Debes añadir un nuevo Boss a este nivel");
            level = bossesPrefabs.Length;
        }
        BossController e = (Instantiate(bossesPrefabs[level - 1], sp.transform.position, Quaternion.identity).GetComponent<BossController>());
        e.MoveTowardsInSpawn(sp.transform.forward);
        GameObject garrote = Instantiate(weaponPrefabs[7], sp.transform.position, Quaternion.identity);
        GameObject bow = Instantiate(weaponPrefabs[4], sp.transform.position, Quaternion.identity);
        e.GetBow(bow.transform);
        e.GetGarrote(garrote.transform);
        StopAllCoroutines();
        return e.enemyScript.Root.transform;
    }
}
