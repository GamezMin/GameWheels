using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFrame.Runtime
{
    public partial class GameFrame
    {
        public FsmController AddFsmComponents(Type type)
        {
            return (FsmController) RootEntity.GetComponent<FsmComponents>().AddComponent(type);
        }

        public void RemoveFsmComponents(FsmController fsm)
        {
            RootEntity.GetComponent<FsmComponents>().RemoveComponent(fsm);
        }
    }
}