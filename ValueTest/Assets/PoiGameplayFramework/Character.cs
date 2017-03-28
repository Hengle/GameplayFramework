using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 人形角色
    /// </summary>
    public partial class Character : Pawn
    {
        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        public new CharacterInfo DataInfo => dataInfo as CharacterInfo;

        public static void Create(CharacterInfo info)
        {
            var go = GM.Instance.CreatePlayer();
            var character = go.AddComponent<Character>();
            character.Init(info);

            Dic[info.ID] = character;
        }

        public static bool Quit(Quit pks)
        {
            lock (Dic)
            {

                if (Dic.ContainsKey(pks.InstanceID))
                {
                    UI.ShowSystemMsg(Dic[pks.InstanceID].DataInfo);
                    Destroy(Dic[pks.InstanceID].gameObject);
                }
                return Dic.Remove(pks.InstanceID);
            }
        }

        internal static void ChangeMode(LineMode mode)
        {
            if (mode != LineMode.Offline || mode != LineMode.LAN)
            {
                RemoveAllCharater();
            }
        }

        private static void RemoveAllCharater()
        {
            lock (Dic)
            {
                foreach (var item in Dic)
                {
                    if (item.Value != Player.Instance)
                    {
                        Destroy(item.Value.gameObject);
                    }
                }
                Dic.Clear();
            }
        }

        /// <summary>
        /// 所有玩家集合
        /// </summary>
        public static Dictionary<int, Character> Dic => CharacterList.characterDic;

        private class CharacterList
        {
            public static Dictionary<int, Character> characterDic = new Dictionary<int, Character>();
        }
    }
}
