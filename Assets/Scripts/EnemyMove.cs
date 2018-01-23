using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    //路径条数
    public GameObject[] wayPointsGo;
    public float speed = 0.2f;
    //储存所有路径点的transform组件
    private List<Vector3> wayPoints = new List<Vector3>();
    //当前在前往哪个路径点
    private int index = 0;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position + new Vector3(0, 3, 0);
        
        //加载路径
        LoadAPath(wayPointsGo[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder-2]]);

    }

    private void FixedUpdate()
    {
        if (transform.position != wayPoints[index])
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index], speed);
            GetComponent<Rigidbody2D>().MovePosition(temp);
        }
        else
        {
            index++;
            if (index >= wayPoints.Count){
                index = 0;
                LoadAPath(wayPointsGo[Random.Range(0, wayPointsGo.Length)]);
            }
            
        }
        Vector2 dir = wayPoints[index] - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    private void LoadAPath(GameObject go)
    {
        wayPoints.Clear();
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t.position);
        }
        wayPoints.Insert(0, startPos);
        wayPoints.Add(startPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if (GameManager.Instance.isSuperPacman)
            {
                transform.position = startPos - new Vector3(0, 3, 0);
                index = 0;
            } else
            {
                Destroy(collision.gameObject);
            }
           
        }
    }

}
