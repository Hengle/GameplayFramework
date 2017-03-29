using System.Collections;
using System.Collections.Generic;
using System.Net;
using Poi;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

    [SerializeField]
    private GameObject onoff;
    [SerializeField]
    private InputField nameInput;

    public bool IsShow { get; set; }

    private void Awake()
    {
        UI.Setting = this;
        onoff.SetActive(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (IsShow != onoff.activeSelf)
        {
            onoff.SetActive(IsShow);
        }
	}

    public void SetName()
    {
        if (nameInput && !string.IsNullOrEmpty(nameInput.text))
        {
            Player.SetName(nameInput.text);
            UI.NameLabel.Reset();
        }
    }

    #region IP

    public InputField IPInput;
    public void Login()
    {
        if (!IPAddress.TryParse(IPInput.text, out IPAddress ip))
        {
            var list = Dns.GetHostAddresses("www.mikumikufight.top");
            try
            {
                ip = list[0];
            }
            catch (System.Exception)
            {
            }
        }
        GM.Login(ip);
    }

    public void Logout()
    {
        GM.Logout();
    }
    #endregion
}
