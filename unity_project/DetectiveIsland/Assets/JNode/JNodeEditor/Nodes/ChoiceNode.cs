using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceNode : Node
{
    public string Content = "선택지 내용";
    public List<Node> Nodes = new List<Node>();

    public float ChoicesTotalSizeY => NodeService.CalNodesSize(Nodes).y;
    
    public ChoiceNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Content = (string)CustomField("Title : ", Content, Vector2.down * 0f);
        float accumulatedHeight = 0f;
        for(int i = 0 ; i < Nodes.Count ; i++){
            Node node = Nodes[i];
            float xPos = NodeRect.center.x - NodeRect.width * 0.5f;
            float yPos = NodeRect.position.y + 100 + accumulatedHeight;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            accumulatedHeight += node.CalNodeSize().y + 20;
        }
    }



}
