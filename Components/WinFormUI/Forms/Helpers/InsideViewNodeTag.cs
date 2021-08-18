#nullable enable

using System.Windows.Forms;

namespace Slipstream.Components.WinFormUI.Forms.Helpers
{
    internal class InsideViewNodeTag
    {
        public NodeTypeEnum NodeType { get; private set; }
        public bool EventFilter { get => NodeType == NodeTypeEnum.Instance || NodeType == NodeTypeEnum.Dependency || NodeType == NodeTypeEnum.LuaScripts; }

        public InsideViewNodeTag(NodeTypeEnum type)
        {
            NodeType = type;
        }

        public static TreeNode InstanceNode(string text)
        {
            return new TreeNode(text)
            {
                Name = text,
                Tag = new InsideViewNodeTag(NodeTypeEnum.Instance),
            };
        }

        public static TreeNode DependencyNode(string text)
        {
            return new TreeNode(text)
            {
                Name = text,
                Tag = new InsideViewNodeTag(NodeTypeEnum.Dependency),
            };
        }
    }
}