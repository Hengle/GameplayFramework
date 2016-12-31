using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class PlayerMoveFSM : StateMachine<Character, MoveState, IPlayerMove>
    {
        public static void Init()
        {
            StateList.Add(MoveState.Idle, new PlayerMoveIdleState());
            StateList.Add(MoveState.MoveStart, new PlayerMoveStartState());
            StateList.Add(MoveState.MoveStop, new PlayerMoveStopState());
            StateList.Add(MoveState.Moving, new PlayerMoveMovingState());
            isInit = true;
        }
    }

    

    public interface IPlayerMove : IMove
    {
        float CurrentMoveStateSpeed { get; }
        float JiaSudu { get; }
    }

    public class PlayerMoveIdleState : BaseState<MoveState, IPlayerMove>
    {
        public override MoveState State => MoveState.Idle;

        public override void OnUpdate(IPlayerMove agent, float deltaTime)
        {
            if (agent.JiaSudu == 0)
            {
                ///没有目标保持Idle
                return;
            }
            else
            {
                PlayerMoveFSM.ChangeState(agent, State, MoveState.MoveStart, deltaTime);
            }
        }
    }

    public class PlayerMoveStartState : BaseState<MoveState, IPlayerMove>
    {
        public override MoveState State => MoveState.MoveStart;

        public override void OnEnter(IPlayerMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override void OnUpdate(IPlayerMove agent, float deltaTime)
        {
            base.OnUpdate(agent, deltaTime);

            const float V = 0.5f;

            if (agent.JiaSudu < 0)
            {
                if (agent.CurrentMoveStateSpeed < V)
                {
                    PlayerMoveFSM.ChangeState(agent, State, MoveState.MoveStop, deltaTime);
                }
                else
                {
                    agent.MoveStart(deltaTime);
                }
            }
            else
            {
                if (agent.CurrentMoveStateSpeed > V)
                {
                    PlayerMoveFSM.ChangeState(agent, State, MoveState.Moving, deltaTime);
                }
                else
                {
                    agent.MoveStart(deltaTime);
                }
            }

        }
    }

    public class PlayerMoveMovingState : BaseState<MoveState, IPlayerMove>
    {
        public override MoveState State => MoveState.Moving;

        public override void OnEnter(IPlayerMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override void OnUpdate(IPlayerMove agent, float deltaTime)
        {
            base.OnUpdate(agent, deltaTime);

            if (agent.JiaSudu < 0 && agent.CurrentMoveStateSpeed < 0.5f)
            {
                PlayerMoveFSM.ChangeState(agent, State, MoveState.MoveStop, deltaTime);
            }
            else
            {
                agent.Moving(deltaTime);
            }
        }
    }

    public class PlayerMoveStopState : BaseState<MoveState, IPlayerMove>
    {
        public override MoveState State => MoveState.MoveStop;

        public override void OnEnter(IPlayerMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override void OnUpdate(IPlayerMove agent, float deltaTime)
        {
            base.OnUpdate(agent, deltaTime);

            if (agent.CurrentMoveStateSpeed < 0.02f)
            {
                PlayerMoveFSM.ChangeState(agent, State, MoveState.MoveStop, deltaTime);
            }
            else
            {
                agent.MoveStop(deltaTime);
            }
        }
    }
}
