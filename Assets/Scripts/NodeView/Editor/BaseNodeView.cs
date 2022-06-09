using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NodeView
{
    public class BaseNodeView : Node
    {
        #region Variables
        
        public Action<BaseNodeView> OnNodeSelected;
        public BaseNode node;
        public Port input;
        public List<Port> outputs;

        #endregion

        #region Constructors

        public BaseNodeView(BaseNode node)
        {
            this.node = node;
            this.title = node.name;

            this.viewDataKey = node.getGUID;

            style.left = node.getPosition.x;
            style.top = node.getPosition.y;

            titleContainer.style.backgroundColor = (node.get—olor + Color.black) / 2;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            if (input != null)
            {
                input.portName = "in";
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts()
        {
            outputs = new List<Port>();

            foreach(var node in node.GetChildsNodes())
            {
                Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

                if (output != null)
                {
                    output.portName = $"out {node.Key}";
                    output.name     = node.Key;

                    outputContainer.Add(output);
                    outputs.Add(output);
                }
            }
        }

        #endregion

        #region Public

        public override void SetPosition(Rect newPosition)
        {
            base.SetPosition(newPosition);
            node.setPosition = new Vector2(newPosition.xMin, newPosition.yMin);

            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();

            if (OnNodeSelected != null)
                OnNodeSelected.Invoke(this);
        }

        #endregion


    }
}
