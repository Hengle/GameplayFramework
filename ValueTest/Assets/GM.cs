using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using Poi;
using UnityEngine.SceneManagement;
using System.Net;
using ProtoBuf;

public partial class GM : MonoBehaviour {

    public static GM Instance;

    public static List<PawnController> Controllers => PawnController.Controllers;

    public static PlayerController PlayerController { get; private set; }
    public static float Delay { get; internal set; }
    public static LineMode Mode { get; internal set; }



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
            Instance = this;
        }
    }

    /// <summary>
    /// 防止误创建
    /// </summary>
    /// <returns></returns>
    private bool CheckInitFinish()
    {
        if (Instance && GameObject.Find("GM"))
        {
            return true;
        }
        return false;
    }

    // Use this for initialization
    IEnumerator Start ()
    {
        yield return StartCoroutine(LoadXML());
        yield return StartCoroutine(LoadDB());
        ///等待一帧防止一些组件没有初始化（不是必须）
        yield return new WaitForEndOfFrame();
        ///游戏主入口
        Init();

        ChangeScene(1);
    }

    private void ChangeScene(int sceneID, bool destoryPlayer = true)
    {
        if (destoryPlayer)
        {
            Destroy(PlayerController.Character);
        }
        else
        {
            DontDestroyOnLoad(PlayerController.Character);
        }

        this.Wait(LoadSceneAsync(sceneID), () =>
        {
            if (destoryPlayer)
            {
                CreateTestPlayer();
            }
        });
    }

    private void CreateTestPlayer()
    {
        GameObject go = CreatePlayer();

        var p = go.AddComponent<Player>();

        Player.Instance = p;

        PlayerInfo info = new PlayerInfo()
        {
            Height = 1.6f,
            JumpPower = 9f,
            JumpMaxStep = 2,
            Name = "初音未来" + new System.Random().Next(1000, 9999).ToString(),
            ID = Player.InstanceID,
        };
        info.Run.Max = 10;
        info.AttackCooldown.Max = 0.3f;


        p.Init(info);


        ///朝向初始化
        p.NextTurnToAngle = p.transform.eulerAngles.y;

        PlayerController.Possess(p);
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    void Init()
    {
        UI.Init();

        InitNet();

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
        UI.Update(Time.deltaTime);
	}

    private void FixedUpdate()
    {
        UpdateMesssage(Time.fixedDeltaTime);
    }

    private void OnApplicationQuit()
    {
        Logout();
        DB?.CloseConnection();
    }

    private void OnDestroy()
    {
        
    }



}
