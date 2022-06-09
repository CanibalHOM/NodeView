using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.Experimental.GraphView;


namespace NodeView
{

    public class GraphTreeView : GraphView
    {

        public new class UXMLFactory : UxmlFactory<GraphTreeView, GraphView.UxmlTraits> { }

        #region Variables

        public Action<BaseNodeView> OnNodeSelected;
        private BaseNodeBehavior behavior;

        #endregion

        #region Constructors

        public GraphTreeView()
        {

            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());


            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/NodeView/Editor/GraphTreeView.uss");
            styleSheets.Add(styleSheet);

        }

        #endregion

        #region Public

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        { 
            var nodeTypes = behavior.NodeTypes();

            Vector2 mousePosition = contentViewContainer.WorldToLocal(evt.mousePosition);

            foreach (var nodeType in nodeTypes)
            {
               var type = nodeType.Item2;
               evt.menu.AppendAction($"Create {nodeType.Item1}", (graphView) => CreateNode(type, mousePosition, nodeType.Item1));
            }
     
        }

        #endregion

        #region Internal

        internal void PopulateView(BaseNodeBehavior behavior)
        {
            this.behavior = behavior;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            behavior.Init();

            behavior.nodes.ForEach(node => CreateNodeView(node));
            behavior.nodes.ForEach(node => LinkNodeView(node));

        }

        #endregion

        #region Private

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(element => {
                    BaseNodeView nodeView = element as BaseNodeView;

                    if (nodeView != null)
                        behavior.DeleteNode(nodeView.node);

                    Edge edge = element as Edge;

                    if (edge != null)
                    {
                        BaseNodeView mainNodeView = edge.output.node as BaseNodeView;
                        behavior.DeleteLink(mainNodeView.node, edge.output.name);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    BaseNodeView nodeView = edge.output.node as BaseNodeView;
                    BaseNodeView nextNodeView = edge.input.node as BaseNodeView;

                    behavior.AddLink(nodeView.node, edge.output.name, nextNodeView.node);
                });
            }

            return graphViewChange;
        }

        private void CreateNode(Type type, Vector2 position, string name = "")
        {
            if (!behavior)
                return;

            BaseNode node = behavior.CreateNode(type, name);
            
            node.setPosition = position;

            CreateNodeView(node);
        }

        private void CreateNode(Type type, string name = "")
        {
            Vector2 position = Vector2.zero;
            CreateNode(type, position, name);
        }

        private void CreateNodeView(BaseNode node)
        {
            BaseNodeView nodeView = new BaseNodeView(node);
            
            nodeView.OnNodeSelected = OnNodeSelected;

            AddElement(nodeView);

        }

        private void LinkNodeView(BaseNode node)
        {
            BaseNodeView nodeView = FindNodeView(node);

            int index = 0;

            foreach(var child in node.GetChildsNodes())
            {

                BaseNode nextNode = child.Value;

                if (!nextNode)
                {
                    index++;
                    continue;
                }    


                BaseNodeView nextNodeView = FindNodeView(nextNode);

                Edge edge = nodeView.outputs[index].ConnectTo(nextNodeView.input);

                AddElement(edge);

                index++;

            }

        }

        private BaseNodeView FindNodeView(BaseNode node)
        {
            return GetNodeByGuid(node.getGUID) as BaseNodeView;
        }

        #endregion

    }
}


