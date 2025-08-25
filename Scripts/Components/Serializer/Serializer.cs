// Decompiled with JetBrains decompiler
// Type: isnaraLib.SerializerLib
// Assembly: isnaraLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92E2FF1B-3182-4FF1-AB1E-B02078B93783
// Assembly location: /Users/gui/Desktop/KingdomMatch/KingdomMatch-client/Assets/IsnaraLib/Plugins/isnaraLib.dll
// XML documentation location: /Users/gui/Desktop/KingdomMatch/KingdomMatch-client/Assets/IsnaraLib/Plugins/isnaraLib.xml

using System.IO;
using System.Text;
using UnityEngine;

#nullable disable
namespace SongLib
{
  public class SerializerLib
  {
    public static byte[] DecryptByte(byte[] _buffer)
    {
      return Security.Decrypt3DEString(_buffer, Security.MakeMD5Hash("dev0by1isnara1CasinoDream5)!!%ky"));
    }

    public static byte[] Load(string _strLoadFileName)
    {
      TextAsset textAsset = (TextAsset)Resources.Load(_strLoadFileName, typeof(TextAsset));
      if (!((Object)textAsset == (Object)null))
        return textAsset.bytes;
      Debug.LogWarning((object)string.Format("File is null - Path :{0}", (object)_strLoadFileName));
      return (byte[])null;
    }

    public static string[] LoadToText(string _strFileName)
    {
      return !File.Exists(_strFileName) ? (string[])null : File.ReadAllLines(_strFileName);
    }

    public static bool LoadToBinaryJson(
      string _strFileName,
      SerializerJson _data,
      bool _bDecrypt = false,
      string _strDebugFileName = "")
    {
      if (!File.Exists(_strFileName))
      {
        Debug.LogWarning((object)string.Format("File is null - Path :{0}", (object)_strFileName));
        return false;
      }

      byte[] numArray = File.ReadAllBytes(_strFileName);
      SerializerLib.LoadToBinaryJson(_data, numArray, _bDecrypt);
      if (_strDebugFileName != "")
      {
        StreamWriter text = File.CreateText(_strDebugFileName);
        text.WriteLine(Encoding.UTF8.GetString(SerializerLib.DecryptByte(numArray)));
        text.Close();
      }

      return true;
    }

    public static bool LoadResourceToBinaryJson(
      string _strFileName,
      SerializerJson _data,
      bool _bDecrypt = false)
    {
      byte[] _bufferResult = SerializerLib.Load(_strFileName);
      if (_bufferResult == null)
      {
        Debug.LogWarning((object)string.Format("File is null - Path :{0}", (object)_strFileName));
        return false;
      }

      SerializerLib.LoadToBinaryJson(_data, _bufferResult, _bDecrypt);
      return true;
    }

    public static void LoadToBinaryJson(SerializerJson _data, byte[] _bufferResult, bool _bDecrypt = false)
    {
      if (_bDecrypt)
        _bufferResult = SerializerLib.DecryptByte(_bufferResult);
      _data.StartJsonDecode(Encoding.UTF8.GetString(_bufferResult));
    }
    
    public static bool LoadResourceToBinaryXml(
      string _strFileName,
      SerializerXml _data,
      bool _bDecrypt = false)
    {
      byte[] _bufferResult = SerializerLib.Load(_strFileName);
      if (_bufferResult == null)
      {
        Debug.LogWarning((object) string.Format("File is null - Path :{0}", (object) _strFileName));
        return false;
      }
      SerializerLib.LoadToBinaryXml(_data, _bufferResult, _bDecrypt);
      return true;
    }

    public static void LoadToBinaryXml(SerializerXml _data, byte[] _bufferResult, bool _bDecrypt = false)
    {
      if (_bDecrypt)
        _bufferResult = SerializerLib.DecryptByte(_bufferResult);
      _data.StartXmlDecode(Encoding.UTF8.GetString(_bufferResult));
    }

    public static byte[] EncryptByte(byte[] _buffer)
    {
      return Security.Encrypt3DEString(_buffer, Security.MakeMD5Hash("dev0by1isnara1CasinoDream5)!!%ky"));
    }
  }
}
