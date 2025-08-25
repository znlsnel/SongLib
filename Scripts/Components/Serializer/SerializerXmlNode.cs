using System;
using System.Xml;

namespace SongLib
{
  public abstract class SerializerXmlNode
  {
    protected XmlNode m_node = (XmlNode) null;

    protected abstract void XmlEncode(XmlDocument _doc, XmlNode _nodeparent);

    protected abstract void XmlDecode();

    public void StartXmlEncode(XmlDocument _doc, XmlNode _nodeparent)
    {
      this.XmlEncode(_doc, _nodeparent);
    }

    public bool StartXmlDecode(XmlNode _node)
    {
      if (_node == null)
        return false;
      this.m_node = _node;
      this.XmlDecode();
      this.m_node.RemoveAll();
      this.m_node = (XmlNode) null;
      return true;
    }

    protected void CreateNode(XmlDocument _doc, string _nodeName)
    {
      if (this.m_node != null)
        return;
      this.m_node = (XmlNode) _doc.CreateElement(_nodeName);
    }

    protected XmlNode SelectSingleNode(string _nodePath) => this.m_node.SelectSingleNode(_nodePath);

    protected XmlNodeList SelectNodes(string _nodePath) => this.m_node.SelectNodes(_nodePath);

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, string _strValue)
    {
      if (this.m_node.Attributes[_propertyName] == null)
      {
        XmlAttribute attribute = _doc.CreateAttribute(_propertyName);
        attribute.Value = _strValue;
        this.m_node.Attributes.Append(attribute);
      }
      else
        this.m_node.Attributes[_propertyName].InnerText = _strValue;
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, short _sValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _sValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, ushort _usValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _usValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, int _nValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _nValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, float _fValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _fValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, long _lValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _lValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, ulong _ulValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _ulValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, double _dValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _dValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, bool _bValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _bValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, byte _byValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _byValue.ToString());
    }

    protected void SetNodeAttributeValue(XmlDocument _doc, string _propertyName, sbyte _sbValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _sbValue.ToString());
    }

    protected void SetNodeAttributeValue(
      XmlDocument _doc,
      string _propertyName,
      DateTime _dateValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _dateValue.ToString());
    }

    protected void SetNodeAttributeValue(
      XmlDocument _doc,
      string _propertyName,
      TimeSpan _dateValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _dateValue.ToString());
    }

    protected void SetNodeAttributeValue<T>(XmlDocument _doc, string _propertyName, T _enumValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _enumValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, string _strValue)
    {
      XmlNode xmlNode = this.SelectSingleNode(_propertyName);
      if (xmlNode == null)
      {
        XmlNode element = (XmlNode) _doc.CreateElement(_propertyName);
        element.InnerText = _strValue;
        this.m_node.AppendChild(element);
      }
      else
        xmlNode.InnerText = _strValue;
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, short _sValue)
    {
      this.SetNodeValue(_doc, _propertyName, _sValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, ushort _usValue)
    {
      this.SetNodeValue(_doc, _propertyName, _usValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, int _nValue)
    {
      this.SetNodeValue(_doc, _propertyName, _nValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, float _fValue)
    {
      this.SetNodeValue(_doc, _propertyName, _fValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, long _lValue)
    {
      this.SetNodeValue(_doc, _propertyName, _lValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, ulong _ulValue)
    {
      this.SetNodeValue(_doc, _propertyName, _ulValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, double _dValue)
    {
      this.SetNodeValue(_doc, _propertyName, _dValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, bool _bValue)
    {
      this.SetNodeValue(_doc, _propertyName, _bValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, byte _byValue)
    {
      this.SetNodeValue(_doc, _propertyName, _byValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, sbyte _sbValue)
    {
      this.SetNodeValue(_doc, _propertyName, _sbValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, DateTime _dateValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _dateValue.ToString());
    }

    protected void SetNodeValue(XmlDocument _doc, string _propertyName, TimeSpan _dateValue)
    {
      this.SetNodeAttributeValue(_doc, _propertyName, _dateValue.ToString());
    }

    protected void SetNodeValue<T>(XmlDocument _doc, string _propertyName, T _enumValue)
    {
      this.SetNodeValue(_doc, _propertyName, _enumValue.ToString());
    }

    protected void GetNodeAttributeValue(string _propertyName, ref string _strValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _strValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref short _sValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _sValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref ushort _usValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _usValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref int _nValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _nValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref uint _unValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _unValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref float _fValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _fValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref long _lValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _lValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref ulong _ulValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _ulValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref double _dValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _dValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref bool _bValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _bValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref byte _byValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _byValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref sbyte _sbValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _sbValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref DateTime _dateValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _dateValue);
    }

    protected void GetNodeAttributeValue(string _propertyName, ref TimeSpan _dateValue)
    {
      SerializerXmlNode.GetNodeAttributeValue(this.m_node, _propertyName, ref _dateValue);
    }

    protected void GetNodeAttributeEnumValue<T>(string _propertyName, ref T _enumValue)
    {
      this.GetNodeAttributeEnumValue<T>(this.m_node, _propertyName, ref _enumValue);
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref string _strValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _strValue = _node.Attributes[_propertyName].InnerText;
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref short _sValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _sValue = _node.Attributes[_propertyName].InnerText.ToShort();
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref ushort _usValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _usValue = _node.Attributes[_propertyName].InnerText.ToUShort();
    }

    public static void GetNodeAttributeValue(XmlNode _node, string _propertyName, ref int _nValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _nValue = _node.Attributes[_propertyName].InnerText.ToInt();
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref uint _unValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _unValue = _node.Attributes[_propertyName].InnerText.ToUInt();
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref float _fValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _fValue = _node.Attributes[_propertyName].InnerText.ToFloat();
    }

    public static void GetNodeAttributeValue(XmlNode _node, string _propertyName, ref long _lValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _lValue = _node.Attributes[_propertyName].InnerText.ToLong();
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref ulong _ulValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _ulValue = _node.Attributes[_propertyName].InnerText.ToULong();
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref double _dValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _dValue = _node.Attributes[_propertyName].InnerText.ToDouble();
    }

    public static void GetNodeAttributeValue(XmlNode _node, string _propertyName, ref bool _bValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _bValue = _node.Attributes[_propertyName].InnerText.ToBool();
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref byte _byValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _byValue = _node.Attributes[_propertyName].InnerText.ToByte();
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref sbyte _sbValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _sbValue = _node.Attributes[_propertyName].InnerText.ToSByte();
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref DateTime _dateValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _dateValue = DateTime.Parse(_node.Attributes[_propertyName].InnerText);
    }

    public static void GetNodeAttributeValue(
      XmlNode _node,
      string _propertyName,
      ref TimeSpan _dateValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _dateValue = TimeSpan.Parse(_node.Attributes[_propertyName].InnerText);
    }

    public void GetNodeAttributeEnumValue<T>(XmlNode _node, string _propertyName, ref T _enumValue)
    {
      if (_node.Attributes[_propertyName] == null)
        return;
      _enumValue = _node.Attributes[_propertyName].InnerText.ToEnum<T>();
    }

    protected void GetNodeValue(string _propertyName, ref string _strValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _strValue);
    }

    protected void GetNodeValue(string _propertyName, ref short _sValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _sValue);
    }

    protected void GetNodeValue(string _propertyName, ref ushort _usValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _usValue);
    }

    protected void GetNodeValue(string _propertyName, ref int _nValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _nValue);
    }

    protected void GetNodeValue(string _propertyName, ref uint _unValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _unValue);
    }

    protected void GetNodeValue(string _propertyName, ref float _fValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _fValue);
    }

    protected void GetNodeValue(string _propertyName, ref long _lValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _lValue);
    }

    protected void GetNodeValue(string _propertyName, ref ulong _ulValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _ulValue);
    }

    protected void GetNodeValue(string _propertyName, ref double _dValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _dValue);
    }

    protected void GetNodeValue(string _propertyName, ref bool _bValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _bValue);
    }

    protected void GetNodeValue(string _propertyName, ref byte _byValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _byValue);
    }

    protected void GetNodeValue(string _propertyName, ref sbyte _sbValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _sbValue);
    }

    protected void GetNodeValue(string _propertyName, ref DateTime _dateValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _dateValue);
    }

    protected void GetNodeValue(string _propertyName, ref TimeSpan _dateValue)
    {
      SerializerXmlNode.GetNodeValue(this.m_node, _propertyName, ref _dateValue);
    }

    protected void GetNodeEnumValue<T>(string _propertyName, ref T _enumValue)
    {
      this.GetNodeAttributeEnumValue<T>(this.m_node, _propertyName, ref _enumValue);
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref string _strValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _strValue = _node.InnerText;
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref short _sValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _sValue = _node.InnerText.ToShort();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref ushort _usValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _usValue = _node.InnerText.ToUShort();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref int _nValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _nValue = _node.InnerText.ToInt();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref uint _unValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _unValue = _node.InnerText.ToUInt();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref float _fValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _fValue = _node.InnerText.ToFloat();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref long _lValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _lValue = _node.InnerText.ToLong();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref ulong _ulValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _ulValue = _node.InnerText.ToULong();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref double _dValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _dValue = _node.InnerText.ToDouble();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref bool _bValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _bValue = _node.InnerText.ToBool();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref byte _byValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _byValue = _node.InnerText.ToByte();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref sbyte _sbValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _sbValue = _node.InnerText.ToSByte();
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref DateTime _dateValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _dateValue = DateTime.Parse(_node.InnerText);
    }

    public static void GetNodeValue(XmlNode _node, string _propertyName, ref TimeSpan _dateValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _dateValue = TimeSpan.Parse(_node.InnerText);
    }

    public void GetNodeEnumValue<T>(XmlNode _node, string _propertyName, ref T _enumValue)
    {
      if (!(_node.Name == _propertyName))
        return;
      _enumValue = _node.InnerText.ToEnum<T>();
    }
  }
}
