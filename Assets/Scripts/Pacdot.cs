using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour {
    //判定是否为超级豆子
    public bool isSuperPacdot = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if (isSuperPacdot)
            {
                //告诉gamemanager是超级豆子且被吃了
                GameManager.Instance.OnEatPacdot(gameObject);
                GameManager.Instance.OnEatSuperPacdot();
                Destroy(gameObject);
                //让吃豆人变成超级吃豆人能吃鬼
                
            } else
            {
                GameManager.Instance.OnEatPacdot(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
