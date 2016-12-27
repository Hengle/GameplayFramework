using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Test : MonoBehaviour {

    public UpdateType type { get; set; } = UpdateType.FixedUpdate;
    public int BBB = 10;
    [SerializeField]
    public AA prop = new AA();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


public class AA { 
    [SerializeField]
    public int BBB;


}