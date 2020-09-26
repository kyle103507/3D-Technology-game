using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位區域

    [Header("角色移動速度"), Range(10f, 50f)]
    public float speed = 10f;
    [Header("旋轉速度"), Range(0, 10000)]
    public float turn = 60;
    [Header("角色攻擊力"), Range(10f, 50f)]
    private float attack = 10f;
    [Header("角色血量"), Range(10f, 500f)]
    public float hp = 500f;
    [Header("角色魔量"), Range(10f, 300f)]
    public float mp = 100f;
    [Header("角色經驗值")]
    private float exp = 100f;
    [Header("角色等級")]
    private int lv;

    [Header("剛體")]
    private Rigidbody rig;
    [Header("動畫")]
    private Animator ani;

    private Transform cam;

    #endregion

    #region 方法區域
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").transform;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Attack();
        RotMove();
    }



    #endregion
    #region 事件區域
    /// <summary>
    ///角色移動:移動動畫、前後左右移動
    /// </summary>
    private void Move()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 pos = cam.forward * v + cam.right * h;
        rig.MovePosition(transform.position + pos * speed);

        ani.SetFloat("移動", Mathf.Abs(v) + Mathf.Abs(h));

        if (v != 0 || h != 0)
        {
            pos.y = 0;
            Quaternion angle = Quaternion.LookRotation(pos);
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, turn);
        }
    }

    /// <summary>
    /// 翻滾移動:閃避、翻滾動畫
    /// </summary>
    private void RotMove()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ani.SetTrigger("翻滾");
        }
    }

    /// <summary>
    /// 角色攻擊:攻擊動畫
    /// </summary>
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetTrigger("攻擊觸發");
        }
    }

    /// <summary>
    /// 角色技能
    /// </summary>
    private void Skill()
    {

    }

    /// <summary>
    /// 角色受傷:受傷動畫
    /// </summary>
    private void Hit()
    {

    }

    /// <summary>
    /// 角色死亡:死亡動畫
    /// </summary>
    private void Dead()
    {

    }

    /// <summary>
    /// 角色經驗值
    /// </summary>
    private void Exp()
    {

    }

    #endregion

}
