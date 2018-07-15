using UnityEngine;
using System.Collections;
using ETModel;
using UnityEngine.UI;

public class PlayerMoveControl : MonoBehaviour
{    private Transform _mTransform;    private Joystick mJoystick;

    public float moveSpeed = 0.05f;    //public SceneType sceneType;
    public delegate void MoveDelegate();    public static MoveDelegate moveEnd;    public static MoveDelegate moveStart;    public static PlayerMoveControl Instance;    private CharacterController cc;    private Animator animator;    //private PlayerManager playerManager;
    // private Text name;

    // Use this for initialization
    void Awake()    {        Instance = this;        _mTransform = transform;

        moveEnd = OnMoveEnd;        moveStart = OnMoveStart;        cc = this.GetComponent<CharacterController>();        animator = this.GetComponent<Animator>();        mJoystick = GameObject.Find("Controller").GetComponent<Joystick>();
        
    }    void Start()
    {

    }    void OnMoveEnd()    {        _turnBase = false;    }

    void OnMoveStart()    {        _turnBase = true;    }

    // Update is called once per frame
    private float angle;    private bool _turnBase = false;
    private Vector3 curPos = new Vector3();    void Update()    {        if (_turnBase)
        {
            Vector3 vecMove = mJoystick.MovePosiNorm * Time.deltaTime * moveSpeed;
            //Log.Debug($"--position--{mJoystick.MovePosiNorm.ToString()}");
            //Debug.Log(Time.time);
            //if (Mathf.Abs(vecMove.x) > 0.01f || Mathf.Abs(vecMove.y) > 0.01f)
            {
                //animator.SetBool("Run", true);
                angle = Mathf.Atan2(mJoystick.MovePosiNorm.x, mJoystick.MovePosiNorm.z) * Mathf.Rad2Deg - 10;
                _mTransform.localRotation = Quaternion.Euler(Vector3.back * angle);
                //this.transform.position += (transform.forward * moveSpeed) ;
                //Log.Debug($"--position--{vecMove.ToString()}");
                curPos.x = this.transform.position.x + mJoystick.MovePosiNorm.x * this.moveSpeed / 10;
                curPos.y = this.transform.position.y + mJoystick.MovePosiNorm.y * this.moveSpeed / 10;
                //this.transform.position += mJoystick.MovePosiNorm * this.moveSpeed / 10;
                this.transform.position = this.curPos;
                /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    cc.SimpleMove(transform.forward * moveSpeed);
                }*/
            }
            //else
            {
                //animator.SetBool("Run", false);
            }
        }


        if (!_turnBase)        {            //animator.SetBool("Run", false);        }    }    void OnGUI()    {                //GUI.Label(new Rect(200, 10, 100, 60), playerManager._playerState.ToString());    }}