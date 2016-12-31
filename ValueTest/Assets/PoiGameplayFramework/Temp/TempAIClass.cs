using System;
using UnityEngine;

namespace Poi
{
    enum TASKState
    {
        Success,
        Faild,
        Running
    }


    public class TempAIClass
    {
        internal static void Deal(PlayerController playerController, InputCommand next)
        {
            ///解析所转向的角度
            TASKState res1 = ActionTurn(playerController, next);

            ///计算移动
            Vector3 arrow = new Vector3(next.Horizontal,0, next.Vertical);
            if (arrow != Vector3.zero)
            {
                playerController.Character.JiaSudu = arrow.magnitude - 0.5f;
            }

            if (playerController.Pawn.IsArriveDistanation)
            {
                playerController.Pawn.Move();
            }
            else
            {
                playerController.Pawn.Idle();
            }

        }

        private static TASKState ActionTurn(PlayerController playerController, InputCommand next)
        {
            Vector2 arrow = new Vector2(next.Horizontal, next.Vertical);
            if (arrow != Vector2.zero)
            {
                float angle = Vector2.Angle(Vector2.up, arrow);
                if (arrow.x < 0)
                {
                    angle = 360 - angle;
                }

                playerController.Pawn.NextTurnToAngle = angle;
            }

            playerController.Pawn.Turn();

            return  TASKState.Success;
        }
    }
}