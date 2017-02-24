using UnityEngine;
using System.Collections;
using System.Xml;
using Framework;

public class CardData : Sample
{
    public string SpriteName;

    public override void InitSample(XmlElement ChildNode)
    {
        base.InitSample(ChildNode);

        this.Id = int.Parse(ChildNode.GetAttribute("id"));
        this.Name = ChildNode.GetAttribute("name");
        this.SpriteName = ChildNode.GetAttribute("spriteName");
    }

    public override void DoDebug()
    {
        Debug.Log(string.Format("ID = {0},Name = {1},SpiteName = {2}", Id, Name, SpriteName));
    }
}
