using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[System.Serializable]
public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject point;

    private void Start()
    {
        point = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyNavmesh enemyNavmesh = collision.transform.parent.GetComponent<EnemyNavmesh>();
        if(enemyNavmesh.PointList.list.Contains(this) //true if agent have this checkpoint in list
            && enemyNavmesh.CurrentTargetIndex == enemyNavmesh.PointList.list.IndexOf(this) + 1) //prevent trigger 1 checkpoint two time
        {
            enemyNavmesh.MoveToNextPoint();
        }
    }
}

[System.Serializable]
public class PointList
{
    public List<CheckPoint> list;
}
