using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMove : MonoBehaviour
{
    //吃豆人的移动速度
    public float speed = 0.35f;
    //吃豆人的下一次的移动目的地
    private Vector2 dest = Vector2.zero;


    //插值法 改变位置 交给rigidbody处理
    
    void Start()
    {
        //保证吃豆人刚开始的时候不会动
        dest = this.transform.position;
    }

    
    void FixedUpdate()
    {
        //通过插值得到要移动到dest位置的下一次移动坐标
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        //通过刚体来设置物体的位置 
        GetComponent<Rigidbody2D>().MovePosition(temp);

        //必须先达到上一个dest位置才能发出新目的地指令
        if ((Vector2)transform.position == dest)
        {
                if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))&&Valid(Vector2.up))
                {
                    dest = (Vector2)transform.position + Vector2.up;
                }
                if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))&& Valid(Vector2.down))
                {
                    dest = (Vector2)transform.position + Vector2.down;
                }
                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))&& Valid(Vector2.left))
                {
                    dest = (Vector2)transform.position + Vector2.left;
                }
                if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))&& Valid(Vector2.right))
                {
                    dest = (Vector2)transform.position + Vector2.right;
                }
            //获取移动方向 并把方向给移动动画状态机
            Vector2 dir = dest - (Vector2)transform.position;
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
        }
        
    }
    //检测取得位置是否可以到达 
    
    private bool Valid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        //墙体射线检测 如果是墙返回false 即不可走
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);

        return (hit.collider == GetComponent<Collider2D>());
    }
}
