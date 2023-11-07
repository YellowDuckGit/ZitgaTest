using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavmesh : MonoBehaviour
{


    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator anim;
    [SerializeField] private PointList pointList;

    private int currentTargetIndex = 0;
    private Vector2 prePosition;
    private Vector2 vectorDistance;
    private bool finishPath;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        UpdateAnimationState();
    }

    /// <summary>
    /// Move Agent To Next check Point
    /// </summary>
    public void MoveToNextPoint()
    {
        if (!finishPath && currentTargetIndex < pointList.list.Count) //true if path is not finish
        {
            agent.SetDestination(pointList.list[currentTargetIndex].transform.parent.transform.position);
            currentTargetIndex++;
        }
        else if(currentTargetIndex > pointList.list.Count) //finish path
        {
            Debug.Log("Finish");
            finishPath = true;
          
        }
    }

    /// <summary>
    /// Make Agent start move sequentially follow list Check Point
    /// </summary>
    public void StartMoveToCheckPoints()
    {
        finishPath = false;
        currentTargetIndex = 0;
        if (pointList.list.Count > 0)
        {
            MoveToNextPoint();
        }
    }

    public void UpdateAnimationState()
    {
        var position = transform.position;
        vectorDistance = (Vector2)position - prePosition;
        prePosition = position;

        spriteRenderer.flipX = vectorDistance.x < 0;
        if (Mathf.Abs(vectorDistance.x) >= Mathf.Abs(vectorDistance.y)/6)
        {
            anim.SetInteger("state", 1);
        }
        else if (vectorDistance.y >= 0)
        {
            anim.SetInteger("state", 0);
        }
        else
        {
            anim.SetInteger("state", 2);
        }
    }

    #region Get Set
    public NavMeshAgent Agent { get { return agent; } }
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }

    public Animator Animator { get { return anim; } }

    public PointList PointList { get { return pointList; } set { pointList = value; } }

    public int CurrentTargetIndex {  get { return currentTargetIndex; } }
    #endregion
}
