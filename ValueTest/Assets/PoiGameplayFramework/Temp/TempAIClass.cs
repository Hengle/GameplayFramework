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


            if (playerController.Pawn.IsArriveDistanation)
            {
                playerController.Pawn.Move();
            }
            else
            {
                playerController.Pawn.Idle();
            }

            ///计算移动
            Vector2 arrow = new Vector2(next.Horizontal, next.Vertical);
            



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