using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public float hp = 500;
    public float maxHp = 500;
    [Header("角色魔量"), Range(10f, 300f)]
    public float mp = 50;
    public float maxMp = 100f;
    [Header("角色經驗值")]
    private float exp = 100f;
    private float maxExp = 100f;
    [Header("角色等級")]
    private int lv;

    [Header("介面區塊")]
    public Image barHp;
    public Image barMp;
    public Image barExp;

    public Text textLv;
    [Header("結束畫面")]
    public GameObject final;

    [Header("等級陣列")]
    public float[] exps = new float[99];

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

        for (int i = 0; i < exps.Length; i++) exps[i] = 100 * (i + 1);
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

    private void OnTriggerEnter(Collider other)
    {
        //如果 碰到物件的標籤 等於 怪蟲
        if (other.tag == "怪蟲")
        {
            other.GetComponent<Enemy>().Hit(attack, transform);
        }
    }



    #endregion
    #region 事件區域
    /// <summary>
    ///角色移動:移動動畫、前後左右移動
    /// </summary>
    private void Move()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊")) return;                        //當是 "攻擊動畫"  就不做移動                
      //  if (ani.GetCurrentAnimatorStateInfo(0).IsName("翻滾")) return;                        //當是 "翻滾動畫"  就不做移動  

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
    public void Hit(float damage, Transform direction)                  //direction 方向
    {
        hp -= damage;
        ani.SetTrigger("受傷觸發");
        rig.AddForce(direction.forward * 100 + direction.up * 10);      //擊退朝怪物前與上方

        hp = Mathf.Clamp(hp, 0, 9999);                                  //夾住血量不要低於0
        barHp.fillAmount = hp / maxHp;                              //更新血條

        if (hp == 0) Dead();                                            //如果響亮等於零就死
    }

    /// <summary>
    /// 角色死亡:死亡動畫
    /// </summary>
    private void Dead()
    {
        ani.SetBool("死亡開關", true);                  //死亡動畫
        final.SetActive(true);
        enabled = false;                                //關閉此腳本
    }

    /// <summary>
    /// 角色經驗值
    /// </summary>
    /// <summary>
    /// 經驗值
    /// </summary>
    /// <param name="getExp">獲得的經驗值</param>
    public void Exp(float getExp)
    {
        exp += getExp;
        barExp.fillAmount = exp / maxExp;

        while (exp >= maxExp && lv < exps.Length) LeveUp();                  //當 經驗值 > = 經驗值需求 等級 < 經驗需求數量就 持續升級
    }

    private void LeveUp()
    {
        lv++;                                       //等級遞增
        maxHp += 10;                                //血量遞增
        maxMp += 10;                                //魔力遞增
        attack += 10;                               //攻擊遞增


        hp = maxHp;                                 //恢復血量
        mp = maxMp;                                 //恢復魔力
        exp -= maxExp;                              //扣掉最大經驗值保留多餘的經驗值

        maxExp = exps[lv - 1];                      //下一級最大經驗值

        barHp.fillAmount = 1;                       //血條全滿
        barMp.fillAmount = 1;                       //魔力全滿
        barExp.fillAmount = exp / maxExp;           //更新經驗值介面
        textLv.text = "LV" + lv;                    //更新等級介面

    }

   public void Win()
    {
        final.SetActive(true);
    }

    #endregion

}
