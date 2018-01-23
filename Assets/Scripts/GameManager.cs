using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;

    public bool isSuperPacman = false;
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };
    private List<GameObject> pacdotGos = new List<GameObject>();

    private void Awake()
    {
        _instance = this;
        int tempCount = rawIndex.Count;
        for (int i=0; i<tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }

        //获取全部豆子
        foreach (Transform t in GameObject.Find("Maze").transform)
        {
            pacdotGos.Add(t.gameObject);
        }
    }

    //开局10秒后开始生成超级豆子
    private void Start()
    {
        Invoke("CreateSuperPacdot", 10f);
    }

    //豆子被吃了，在列表里将自己删除
    public void OnEatPacdot(GameObject go)
    {
        pacdotGos.Remove(go);
    }

    //超级豆子被吃了，变成超级吃豆人
    public void OnEatSuperPacdot()
    {
        Invoke("CreateSuperPacdot",10f);
        isSuperPacman = true;
        FreezeEnemy();
        StartCoroutine(RecoverEnemy());
    }

    //超级吃豆人持续3秒
    IEnumerator RecoverEnemy()
    {
        yield return new WaitForSeconds(3f);
        DisFreezeEnemy();
        isSuperPacman = false;
    }

    private void CreateSuperPacdot()
    {
        //选定超级豆子
        int tempIndex = Random.Range(0, pacdotGos.Count);
        pacdotGos[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdotGos[tempIndex].GetComponent<Pacdot>().isSuperPacdot = true;
    }

    private void FreezeEnemy()
    {
        blinky.GetComponent<EnemyMove>().enabled = false;
        clyde.GetComponent<EnemyMove>().enabled = false;
        inky.GetComponent<EnemyMove>().enabled = false;
        pinky.GetComponent<EnemyMove>().enabled = false;

        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }

    private void DisFreezeEnemy()
    {
        blinky.GetComponent<EnemyMove>().enabled = true;
        clyde.GetComponent<EnemyMove>().enabled = true;
        inky.GetComponent<EnemyMove>().enabled = true;
        pinky.GetComponent<EnemyMove>().enabled = true;

        blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); ;
        clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}
