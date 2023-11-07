using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SpawnerNavmesh : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button button20;
    [SerializeField] private Button button50;
    [SerializeField] private Button button100;
    [SerializeField] private List<PointList> CheckPoints = new List<PointList>();
    [SerializeField] private List<EnemyNavmesh> listEnemy;
    [SerializeField] private float delay = 0.5f;

    private int spawnAmount = 0;
    private void Start()
    {
        button20.onClick.AddListener(() => spawnCommand(20));
        button50.onClick.AddListener(() => spawnCommand(50));
        button100.onClick.AddListener(() => spawnCommand(100));

    }

    /// <summary>
    /// Spawn Number of Enemy 
    /// </summary>
    /// <param name="number"></param>
    /// <param name="enemy"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator Spawn(int number,EnemyNavmesh enemy, int indexAgent) 
    {
        int priorityIndex = 0;
        if (number > 0)
        {
            for (int i = 0; i < number; i++)
            {
                priorityIndex++;
                if (priorityIndex == 99) priorityIndex = 0;

                GameObject gameobject = ObjectPooler.SharedInstance.GetPooledObject("EnemyPath" +(indexAgent + 1).ToString());
                gameobject.transform.position = new Vector3(this.transform.position.x + (indexAgent / 1.5f) ,this.transform.position.y + (priorityIndex/100));
                EnemyNavmesh enemyNavmesh = gameobject.GetComponent<EnemyNavmesh>();
                enemyNavmesh.PointList = CheckPoints[indexAgent];
                enemyNavmesh.Agent.avoidancePriority = priorityIndex; //Set Priority to agent
                gameobject.SetActive(true);
                enemyNavmesh.StartMoveToCheckPoints();                //Start Moving
                yield return new WaitForSeconds(delay);
            }

        }
    }

    /// <summary>
    /// Call Command spawn number of enemy by divide the spawn number based on type of agent
    /// Each Agent Type have a different check point
    /// </summary>
    /// <param name="number"></param>
    public void spawnCommand(int number)
    {
        spawnAmount = number;
        int typeAgent = listEnemy.Count;
        int spawnAmountInPath = spawnAmount / typeAgent; 
        for (int i = 0; i < typeAgent; i++)
        {
            if (i == typeAgent && number % 2 == 0 && !(typeAgent % 2 == 0))
                spawnAmountInPath++;

            if (spawnAmount <= 0)
            {
                spawnAmountInPath = 1;
            }
            spawnAmount -= spawnAmountInPath;
            StartCoroutine(Spawn(spawnAmountInPath, listEnemy[i], i));
            if (spawnAmount <= 0) break;
        }
    }
}
