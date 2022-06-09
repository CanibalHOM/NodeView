using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NodeView;

namespace Example.AI 
{

    public class AIActionString : BaseNode
    {
        #region Variables
       
        [SerializeField] private string value;

        #endregion

        #region Protected

        protected override void InitChildsNodes()
        {
            childs.Add("Yes", null);
            childs.Add("No", null);
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

