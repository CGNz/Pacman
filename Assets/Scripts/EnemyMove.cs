using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                //吃鬼加500分
                GameManager.Instance.score += 500;
            } else
            {
                //新死亡，将角色隐藏
                collision.gameObject.SetActive(false);
                GameManager.Instance.gamePanel.SetActive(false);
                Instantiate(GameManager.Instance.gameoverPrefab);

                //看三秒后重载场景
                Invoke("Restart", 3f);

                //旧死亡
                //Destroy(collision.gameObject);
            }
           
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
