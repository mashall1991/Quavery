using UnityEngine;
using System.Collections;
using System.Xml;
using Framework;

public class TxtData : Sample
{
    public override void InitSample(XmlElement ChildNode)
    {
        base.InitSample(ChildNode);

        this.Id = int.Parse(ChildNode.GetAttribute("Id"));
        this.Name = ChildNode.GetAttribute("Name");
    }


}
