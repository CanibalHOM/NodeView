using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

using UnityEditor;

namespace NodeView
{
    public class InspectorView : VisualElement
    {

        #region Variables
        
        private Editor editor;
       
        #endregion

        #region Constructors

        public new class UXMLFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        public InspectorView() { }

        #endregion

        #region Internal

        internal void UpdateSelection(BaseNodeView nodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);

            editor = Editor.CreateEditor(nodeView.node);

            IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });

            Add(container);
        }

        #endregion
    }

}