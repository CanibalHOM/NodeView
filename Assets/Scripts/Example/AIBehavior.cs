using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEditor;

using NodeView;

namespace Example.AI
{

    [CreateAssetMenu(fileName = "NewAIBehavior", menuName = "AI/AIBehavior", order = 1)]
    public class AIBehavior : BaseNodeBehavior
    {

        #region Public

        public override List<Tuple<string, Type>> NodeTypes()
        {
            List<Tuple<string, Type>> result = new();

            //result.Add(new Tuple<string, Type>("Action", typeof(AIAction)));
            result.Add(new Tuple<string, Type>("Action", typeof(AIAction)));

            result.Add(new Tuple<string, Type>("String Action", typeof(AIActionString)));

            return result;
        }

        public override BaseNode Update()
        { 
            return currentNode;
        }

        public BaseNode Update(bool isCheckEnemy)
        {

            if (!currentNode)
            {
                Set—urrentNodeAndUpdate(rootNode);
                return currentNode;
            }

            if (currentNode == exitNode)
            {
               Set—urrentNodeAndUpdate(rootNode); 
               return currentNode;
            }


            if (currentNode.Update() == State.Success)
            {
                currentNode = currentNode.next;
                currentNode.Start();

                return currentNode;
            }

            return null;

        }

        #endregion

        #region Protected

        protected override void CreateBaseNodes()
        {

            if (rootNode != null)
                return;

            exitNode    = CreateNode(typeof(AIAction), Color.cyan, "Exit");
            rootNode    = CreateNode(typeof(AIAction), Color.green, "Root");

            EditorUtility.SetDirty(rootNode);
            EditorUtility.SetDirty(exitNode);

            EditorUtility.SetDirty(this);

        }

        #endregion

    }

}
