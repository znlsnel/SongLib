using System;
using System.Xml;
using UnityEngine;

namespace SongLib
{
  public abstract class SerializerXml : SerializerXmlNode
  {
    private XmlDocument m_doc = (XmlDocument) null;

    protected abstract void XmlEncode(XmlDocument _doc);

    protected abstract override void XmlDecode();

    protected override void XmlEncode(XmlDocument _doc, XmlNode _nodeparent)
    {
    }

    public string StartXmlEncode()
    {
      if (this.m_doc == null)
        this.m_doc = new XmlDocument();
      this.XmlEncode(this.m_doc);
      return this.m_doc.OuterXml;
    }

    /// <summary>에디터에서만 사용을 해야 한다.</summary>
    public virtual bool XmlLoad(string _strFilePath)
    {
      if (_strFilePath == "")
        return false;
      try
      {
        if (this.m_doc == null)
          this.m_doc = new XmlDocument();
        this.m_doc.Load(_strFilePath);
        this.m_node = (XmlNode) this.m_doc;
        this.XmlDecode();
        this.m_doc.RemoveAll();
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) ("Unable to load xml : " + _strFilePath + " : " + ex.Message));
        return false;
      }
    }

    public virtual bool StartXmlDecode(string _strXml)
    {
      if (_strXml == "")
        return false;
      try
      {
        if (this.m_doc == null)
          this.m_doc = new XmlDocument();
        this.m_doc.LoadXml(_strXml);
        this.m_node = (XmlNode) this.m_doc;
        this.XmlDecode();
        this.m_doc.RemoveAll();
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogWarning($"Unable to parse xml : {_strXml} : {ex.Message}");
        return false;
      }
    }
  }
}
