using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Poi;

public class Map : MonoBehaviour
{
    public List<GameObject> MonsterList;
    public List<Transform> posList;

    private void Start()
    {
        foreach (var item in posList)
        {
            ///临时创建怪物
            
            GameObject monsterObj = GameObject.Instantiate(MonsterList[0]);
            var monster = monsterObj.AddComponent<Monster>();
            monster.transform.Apply(item);

            MonsterInfo minfo = new MonsterInfo()
            {
                Height = 1,
                Name = "小熊",
            };
            monster.Init(minfo);
        }
    }


}