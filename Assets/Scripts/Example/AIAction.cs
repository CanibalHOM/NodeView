using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NodeView;

namespace Example.AI 
{

    public class AIAction : BaseNode
    {
        #region Variables
       
        [SerializeField] private int value;

        #endregion

        #region Protected

        protected override void InitChildsNodes()
        {
            childs.Add("next", null);
        }

        protected override void OnEnter()
        {

        }

        protected override void OnExit()
        {

        }

        #endregion

    }

}

