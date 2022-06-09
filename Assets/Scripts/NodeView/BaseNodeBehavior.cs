using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace NodeView
{

    public class BaseNodeBehavior : ScriptableObject
    {

        #region Variables

        [SerializeField] public BaseNode rootNode;
        [SerializeField] public BaseNode exitNode;

        [SerializeField] public List<BaseNode> nodes = new List<BaseNode>();

        protected BaseNode currentNode = null;

        #endregion

        #region Public

        public virtual BaseNode Update()
        {
            if (!currentNode || currentNode == exitNode)
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

        public virtual List<Tuple<string, Type>> NodeTypes() 
        {
            List<Tuple<string, Type>> result = new List<Tuple<string, Type>>();

            result.Add(new Tuple<string, Type>("Base",  typeof(BaseNode)));

            return result;
        }

        public virtual BaseNode CreateNode(Type type, Color color, string nodeName = "")
        {
            BaseNode node = ScriptableObject.CreateInstance(type) as BaseNode;
            node.Init(color, nodeName);

            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);

            EditorUtility.SetDirty(node);
            EditorUtility.SetDirty(this);
            
            AssetDatabase.SaveAssets();

            return node;
        }

        public virtual BaseNode CreateNode(Type type, string nodeName = "")
        {
            Color color = Color.gray;
            return CreateNode(type, color, nodeName);
        }

        public virtual void Init()
        {
            CreateBaseNodes();
        }

       public void DeleteNode(BaseNode node)
        {
            nodes.Remove(node);

            AssetDatabase.RemoveObjectFromAsset(node);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

        }

        public void AddLink(BaseNode current, string portName, BaseNode next)
        {
            current.AddLink(portName, next);

            EditorUtility.SetDirty(current);
            EditorUtility.SetDirty(this);
        }

        public void DeleteLink(BaseNode current, string portName)
        {
            current.DeleteLink(portName);

            EditorUtility.SetDirty(current);
            EditorUtility.SetDirty(this);
        }

        public BaseNode GetLink(BaseNode current, string portName)
        {
            return current.GetLink(portName);
        }

        #endregion

        #region Protected
       
        protected virtual void CreateBaseNodes()
        {
            if (rootNode != null)
                return;

            rootNode = CreateNode(typeof(BaseNode), "Root");
            exitNode = CreateNode(typeof(BaseNode), "Exit");

            EditorUtility.SetDirty(exitNode);
            EditorUtility.SetDirty(rootNode);

            EditorUtility.SetDirty(this);

            AssetDatabase.SaveAssets();
        }

        protected virtual void Set—urrentNodeAndUpdate(BaseNode node)
        {
            if (currentNode)
                currentNode.Empty();

            currentNode = node;
            currentNode.Start();
        }


        #endregion
    }

}
