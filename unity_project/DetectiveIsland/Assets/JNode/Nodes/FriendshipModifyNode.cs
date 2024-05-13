using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class FriendshipModifyNode : Node
{
    private FriendshipModify _friendshipModify = new FriendshipModify(true, "", 10);

    public FriendshipModifyNode(string title, Node parentNode): base(title, parentNode)  // Node 클래스의 생성자 호출
    {
        SetNodeRectSize(CalNodeSize());
    }
    public override Element ToElement()
    {
        return _friendshipModify;
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }
    public override void DrawNode()
    {
        base.DrawNode();
        _friendshipModify.IsGain = (bool)CustomField("IsGain : ", _friendshipModify.IsGain, Vector2.up * 200);
        _friendshipModify.Id = (string)CustomField("Id : ", _friendshipModify.Id, Vector2.up * 0);
        _friendshipModify.Amount = (int)CustomField("Amount : ", _friendshipModify.Amount, Vector2.up * -200);


        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColor.friendshipModifyColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColor.friendshipModifyColor);
    }
}
