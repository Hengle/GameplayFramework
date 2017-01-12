using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using Poi;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

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
        GMInstance = this;
    }

    // Use this for initialization
    void Start () {
        
        this.Wait(LoadSceneAsync(1), () =>
        {
            GameObject go = GameObject.Instantiate(GameMode.DefaultPawn);

            GameObject start = GameObject.FindGameObjectWithTag($"{PoiTag.PlayerStart.ToString()}");
            go.transform.Apply(start?.transform);

            //go.AddComponent<DontDestroyOnLoad>();
            var controller = go.GetComponent<Animator>();

            var p = go.AddComponent<Player>();

            PlayerInfo info = new PlayerInfo()
            {
                Height = 1.6f,
                JumpPower = 12f,
                JumpMaxStep = 2,
                
            };
            info.Run.Max = 10;
            p.Init(info);

            PlayerController pc = PawnController.CreateController<PlayerController>();

            pc.Init();

            pc.IsFollowPawn = true;

            PlayerController = pc;

            pc.Possess(p);
        });
	}

    // Update is called once per frame
    void Update () {
		
	}
}
