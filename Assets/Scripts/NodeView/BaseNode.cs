using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace NodeView
{

    public class BaseNode : ScriptableObject
    {

        [Serializable]
        public class ChildDictionary : UnitySerializedDictionary<string, BaseNode> { }

        #region Variables

        [SerializeField] private string guid = "";

        [SerializeField, HideInInspector] private Vector2 position;
        [SerializeField, HideInInspector] private Color color;

        [SerializeField, HideInInspector] protected ChildDictionary childs = new ChildDictionary();

        #endregion

        #region Properties

        public string getGUID { get { return guid; } }
        public Color get—olor { get { return color; } }

        public bool isRunning { get { return state == State.Running; } }
        public bool isSuccess { get { return state == State.Success; } }
        public State state { get; private set; }
        public Vector2 getPosition { get { return position; } }
        public Vector2 setPosition { set { position = value; } }
        public BaseNode next { get { return childs["next"]; } }

        #endregion

        #region Public

        public virtual BaseNode Init(Color color, string name = "")
        {
            if (guid == "")
                guid = GUID.Generate().ToString();

            if (name == "")
                name = guid;

            this.name   = name;
            this.color  = color;

            InitChildsNodes();

            return this;
        }

        public virtual int GetOutputCount()
        {
            return childs.Count;
        }

        public virtual Dictionary<string, BaseNode> GetChildsNodes()
        {
            return childs;
        }
      
        public void AddLink(string portName, BaseNode childNode)
        {
            childs[portName] = childNode;
        }

        public void DeleteLink(string portName)
        {
            childs[portName] = null;
        }

        public BaseNode GetLink(string portName)
        {
            return childs[portName];
        }

        public virtual BaseNode Clone()
        {
            return Instantiate(this);
        }

        public State Update()
        {
            if (state == State.Empty)
                Start();

            if (state == State.Success)
            {
                Empty();
                return State.Success;
            }

            return state;
        }

        public void Success()
        {
            state = State.Success;
        }

        public void Start()
        {
            state = State.Running;
            OnEnter();
        }

        public void Empty()
        {
            state = State.Empty;
            OnExit();
        }

        #endregion

        #region Protected

        protected virtual void InitChildsNodes()
        {
            childs.Add("next", null);
        }

        protected virtual void OnEnter()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnExit()
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}