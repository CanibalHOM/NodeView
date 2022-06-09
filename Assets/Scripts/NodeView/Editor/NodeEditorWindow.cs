using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;

namespace NodeView
{

    [CustomEditor(typeof(BaseNodeBehavior))]
    public class NodeEditorWindow : EditorWindow
    {

        #region Variables

        private GraphTreeView   graphTreeView;
        private TextField       graphViewHeader;
        private InspectorView   inspectorView;

        #endregion

        #region Static
        public static NodeEditorWindow OpenWindow()
        {
            NodeEditorWindow window = GetWindow<NodeEditorWindow>();
            window.titleContent = new GUIContent("NodeEditorWindow");

            return window;
        }

        public static NodeEditorWindow OpenWindow(BaseNodeBehavior behavior)
        {
            NodeEditorWindow window = OpenWindow();

            window.SetBaseNodeBehavior(behavior, window);

            return window;
        }

        [OnOpenAssetAttribute(1)]
        public static bool OpenWindow(int instanceID, int line)
        {
            bool windowIsOpen = EditorWindow.HasOpenInstances<NodeEditorWindow>();

            if (!windowIsOpen)
                EditorWindow.CreateWindow<NodeEditorWindow>();
            else
                EditorWindow.FocusWindowIfItsOpen<NodeEditorWindow>();

            return false;
        }

        [OnOpenAssetAttribute(2)]
        public static bool CreateAndOpenWindow(int instanceID, int line)
        {
            BaseNodeBehavior behavior = EditorUtility.InstanceIDToObject(instanceID) as BaseNodeBehavior;
            OpenWindow(behavior);

            return true;
        }

        #endregion

        #region Public

        public void CreateGUI()
        {

            VisualElement root = rootVisualElement;

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/NodeView/Editor/NodeEditorWindow.uxml");
            visualTree.CloneTree(root);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/NodeView/Editor/NodeEditorWindow.uss");
            root.styleSheets.Add(styleSheet);

            graphTreeView = root.Q<NodeView.GraphTreeView>();
            inspectorView = root.Q<NodeView.InspectorView>();

            graphViewHeader = root.Q<TextField>("GraphViewHeader");
            graphViewHeader.value   = "...";

            graphTreeView.OnNodeSelected = OnNodeSelectionChanged;

            OnSelectionChange();
        }

        public void SetBaseNodeBehavior(BaseNodeBehavior behavior, NodeEditorWindow window = null)
        {
            if (behavior && AssetDatabase.CanOpenAssetInEditor(behavior.GetInstanceID()))
            {
                graphTreeView.PopulateView(behavior);
                graphViewHeader.value = behavior.name;

                if (window)
                    window.title = $"{behavior.GetType().Name} : {behavior.name}";
            }

        }

        #endregion

        #region Private

        private void OnSelectionChange()
        {
            BaseNodeBehavior behavior = Selection.activeObject as BaseNodeBehavior;
            SetBaseNodeBehavior(behavior);
        }

        private void OnNodeSelectionChanged(BaseNodeView nodeView)
        {
            inspectorView.UpdateSelection(nodeView);
        }

        #endregion

    }

}



