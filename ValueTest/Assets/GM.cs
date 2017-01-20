using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using Poi;
using UnityEngine.SceneManagement;

public partial class GM : MonoBehaviour {

    public static GM GMInstance;

    //public static List<PlayerController> PlayerControllers { get; private set; }
    //    = new List<PlayerController>();

    [SerializeField,Tooltip("游戏模式")]
    private GameMode gameMode;
    public GameMode GameMode
    {
        get
        {
            return gameMode;
        }

        set
        {
            gameMode = value;
        }
    }

    public static List<PawnController> Controllers => PawnController.Controllers;

    public static PlayerController PlayerController { get; private set; }



    // 加载脚本实例时调用 Awake
    private void Awake()
    {
        if (CheckInitFinish())
        {
            ///不允许GM重复创建
            Destroy(gameObject);
        }
        else
        {
            ///第一次创建GM赋值给Instance；
            GMInstance = this;
        }
    }

    /// <summary>
    /// 防止误创建
    /// </summary>
    /// <returns></returns>
    private bool CheckInitFinish()
    {
        if (GMInstance && GameObject.Find("GM"))
        {
            return true;
        }
        return false;
    }

    // Use this for initialization
    void Start ()
    {
        Init();

        this.Wait(LoadSceneAsync(1), () =>
        {
            GameObject go = GameObject.Instantiate(GameMode.DefaultPawn);

            GameObject start = GameObject.FindGameObjectWithTag($"{PoiTag.PlayerStart.ToString()}");
            go.transform.Apply(start?.transform);
            go.transform.ApplyRY(start?.transform);
            //go.AddComponent<DontDestroyOnLoad>();
            var controller = go.GetComponent<Animator>();

            var p = go.AddComponent<Player>();

            PlayerInfo info = new PlayerInfo()
            {
                Height = 1.6f,
                JumpPower = 9f,
                JumpMaxStep = 2,

            };
            info.Run.Max = 10;
            p.Init(info);

            PlayerController.Possess(p);
        });
    }

    void Init()
    {
        InitUI();

        ///初始化角色控制器
        InitPlayerController();
    }

    private void InitPlayerController()
    {
        PlayerController pc = PawnController.CreateController<PlayerController>();

        pc.gameObject.AddComponent<DontDestroyOnLoad>();

        pc.Init();

        pc.IsFollowPawn = true;

        PlayerController = pc;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
