using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace NodeView
{

    public class SplitView : TwoPaneSplitView
    {
        public new class UXMLFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }

}