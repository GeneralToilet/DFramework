using System;
using System.Xml.Serialization;

public class ResItem
{
    private string assetPath = string.Empty;
    private string assetBundle = string.Empty;
    private string[] dependence = new string[0];

    [XmlAttribute]
    public string AssetPath { get { return assetPath; } }
    [XmlAttribute]
    public string AssetBundle { get { return assetBundle; } }
    [XmlArray]
    public string[] Dependence { get { return dependence; } }

    public ResItem(string assetPath, string assetBundle, string[] dependence)
    {
        this.assetPath = assetPath;
        this.assetBundle = assetBundle;
        this.dependence = dependence;
    }
}
