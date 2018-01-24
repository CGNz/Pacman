using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject startCountDownPrefab;
    public GameObject winPrefab;
    public GameObject gameoverPrefab;
    public AudioClip startClip;
    public Text remainText;
    public Text nowText;
    public Text scoreText;

    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;

    public bool isSuperPacman = false;
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };
    private List<GameObject> pacdotGos = new List<GameObject>();

    private void Awake()
    {
        _instance = this;
        Screen.SetResolution(1024, 768, false);
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
        pacdotNum = GameObject.Find("Maze").transform.childCount;
    }

    //开局10秒后开始生成超级豆子
    private void Start()
    {
        SetGameState(false);
        
    }

    private void Update()
    {
        //判断胜利条件               避免重复加载winPrefab
        if (nowEat == pacdotNum && pacman.GetComponent<PacmanMove>().enabled != false)
        {
            gamePanel.SetActive(false);
            Instantiate(winPrefab);

            //取消所有协程
            StopAllCoroutines();

            SetGameState(false);
        }

        //胜利之后按任意键重新加载场景
        if (nowEat == pacdotNum)
        {
            if (Input.anyKeyDown)       
            {
                SceneManager.LoadScene(0);
            }
        }

        if (gamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:\n\n" + (pacdotNum - nowEat);
            nowText.text = "Eaten:\n\n" + nowEat;
            scoreText.text = "Score:\n\n" + score;
        }
    }

    //点击开始按钮
    public void OnStartButton()
    {
        StartCoroutine(PlayStartCountDown());
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0,0,-10));
        startPanel.SetActive(false);
    }

    IEnumerator PlayStartCountDown()
    {
        GameObject go = Instantiate(startCountDownPrefab);
        //播放动画时间2秒
        yield return new WaitForSeconds(2f);
        Destroy(go);
        SetGameState(true);
        Invoke("CreateSuperPacdot", 10f);
        gamePanel.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    //豆子被吃了，在列表里将自己删除
    public void OnEatPacdot(GameObject go)
    {
        //加分 普通100
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);
    }

    //超级豆子被吃了，变成超级吃豆人
    public void OnEatSuperPacdot()
    {
        //加分 超级豆子200
        score += 200;
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
        //场上剩余豆子树太少则不生成超级豆子
        if (pacdotGos.Count < 10)
        {
            return;
        }
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

    //设置移动状态 开始停止移动 点击开始按钮后可以移动
    private void SetGameState(bool state)
    {
        pacman.GetComponent<PacmanMove>().enabled = state;
        blinky.GetComponent<EnemyMove>().enabled = state;
        clyde.GetComponent<EnemyMove>().enabled = state;
        inky.GetComponent<EnemyMove>().enabled = state;
        pinky.GetComponent<EnemyMove>().enabled = state;
    }
}
