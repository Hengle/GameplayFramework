using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMode : MonoBehaviour {

    public List<GameObject> Model;
    public RectTransform Panel;

    public GameObject ShowUITemplate;
	// Use this for initialization
	void Start () {
        ShowUITemplate.SetActive(false);
        Init();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Init()
    {
        for (int i = 0; i < Model.Count; i++)
        {
            var item = Model[i];

            var pos = new Vector3(10000 * (i+1), 10000 * (i + 1), 0);

            GameObject go = Instantiate(item);
            go.transform.position = pos;
            var rig = go.GetComponent<Rigidbody>();
            if (rig)
            {
                Destroy(rig);
            }
            go.name = "Show--" + item.name;
            Transform center = go.transform.Find("Center");
            if (center == null)
            {
                var animator = go.GetComponent<Animator>();
                center = animator?.GetBoneTransform(HumanBodyBones.Hips);
                if (center == null)
                {
                    GameObject tempc = new GameObject("Center");
                    tempc.transform.SetParent(go.transform);
                    tempc.transform.position = Vector3.up;
                    center = tempc.transform;
                }
            }

            GameObject camgo = new GameObject("Show--Camera");
            //camgo.transform.SetParent(go.transform);
            camgo.transform.position = center.transform.position + Vector3.forward * 1.85f;
            camgo.transform.Rotate(0, 180, 0);
            camgo.transform.Rotate(7, 0, 0);
            var cam = camgo.AddComponent<Camera>();

            RenderTexture tex = new RenderTexture(512, 1024, 24);
            cam.targetTexture = tex;

            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0, 0, 0, 0);
            cam.cullingMask = 1 << 31;
            //cam.transparencySortMode = TransparencySortMode.Orthographic;

            go.SetLayerOnAll(31);

            GameObject showUI = Instantiate(ShowUITemplate, Panel);
            showUI.name = "ShowUI--" + item.name;
            showUI.transform.localScale = Vector3.one;

            RawImage rawimageUI = showUI.GetComponentInChildren<RawImage>();
            rawimageUI.texture = tex;
            showUI.SetActive(true);
        }
    }
}
