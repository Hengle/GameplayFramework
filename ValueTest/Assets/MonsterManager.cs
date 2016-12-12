using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    [SerializeField]
    public GameObject[] Monsters;

    public Transform pos;
	// Use this for initialization
	void Start ()
    {
        CreateMonster();
    }

    private void CreateMonster()
    {
        GameObject mon = Instantiate(Monsters[0], pos.position, pos.rotation);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
