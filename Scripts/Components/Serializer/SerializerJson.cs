using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SongLib
{
    public abstract class SerializerJson
    {
        protected JObject m_JsonObject = new JObject();
        protected JArray m_TempJsonArray = (JArray)null;
        protected JObject m_TempJsonInfo = (JObject)null;
        protected int m_nJsonArrayIndex = 0;
        protected List<JArray> m_listJsonArray = new List<JArray>();
        protected int m_nJsonInfoIndex = 0;
        protected List<JObject> m_listJsonInfo = new List<JObject>();

        protected abstract JObject JsonEncode();

        protected abstract void JsonDecode();

        public string GetJsonString() => this.m_JsonObject.ToString();

        public string StartJsonEncodeToString()
        {
            this.StartJsonEncode();
            return this.GetJsonString();
        }

        public JObject StartJsonEncode()
        {
            this.m_JsonObject.RemoveAll();
            this.m_TempJsonArray = (JArray)null;
            this.m_TempJsonInfo = (JObject)null;
            int count1 = this.m_listJsonArray.Count;
            for (int index = 0; index < count1; ++index)
                this.m_listJsonArray[index].RemoveAll();
            int count2 = this.m_listJsonInfo.Count;
            for (int index = 0; index < count2; ++index)
                this.m_listJsonInfo[index].RemoveAll();
            this.m_nJsonArrayIndex = 0;
            this.m_nJsonInfoIndex = 0;
            return this.JsonEncode();
        }

        public virtual bool StartJsonDecode(string _strDecoder)
        {
            if (_strDecoder == "")
                return false;
            try
            {
                this.m_JsonObject = JObject.Parse(_strDecoder);
                this.JsonDecode();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Unable to parse jsonObject : {_strDecoder} : {ex.Message}");
                return false;
            }
        }

        public bool StartJsonDecode(JToken _jsonToken) => this.StartJsonDecode(_jsonToken.ToString());

        protected JArray GetNextJsonArray()
        {
            if (this.m_listJsonArray.Count > this.m_nJsonArrayIndex)
                return this.m_listJsonArray[this.m_nJsonArrayIndex++];
            JArray nextJsonArray = new JArray();
            this.m_listJsonArray.Add(nextJsonArray);
            ++this.m_nJsonArrayIndex;
            return nextJsonArray;
        }

        protected JObject GetNextJsonInfo()
        {
            if (this.m_listJsonInfo.Count > this.m_nJsonInfoIndex)
                return this.m_listJsonInfo[this.m_nJsonInfoIndex++];
            JObject nextJsonInfo = new JObject();
            this.m_listJsonInfo.Add(nextJsonInfo);
            ++this.m_nJsonInfoIndex;
            return nextJsonInfo;
        }

        protected void SetJsonValue(string _propertyName, string _strValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_strValue);
        }

        protected void SetJsonValue(string _propertyName, short _sValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_sValue);
        }

        protected void SetJsonValue(string _propertyName, ushort _usValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_usValue);
        }

        protected void SetJsonValue(string _propertyName, int _nValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_nValue);
        }

        protected void SetJsonValue(string _propertyName, uint _unValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_unValue);
        }

        protected void SetJsonValue(string _propertyName, float _fValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_fValue);
        }

        protected void SetJsonValue(string _propertyName, long _lValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_lValue);
        }

        protected void SetJsonValue(string _propertyName, ulong _ulValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_ulValue);
        }

        protected void SetJsonValue(string _propertyName, double _dValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_dValue);
        }

        protected void SetJsonValue(string _propertyName, bool _bValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_bValue);
        }

        protected void SetJsonValue(string _propertyName, byte _byValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_byValue);
        }

        protected void SetJsonValue(string _propertyName, sbyte _sbValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_sbValue);
        }

        protected void SetJsonValue(string _propertyName, DateTime _dateValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_dateValue);
        }

        protected void SetJsonValue(string _propertyName, TimeSpan _dateValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_dateValue);
        }

        protected void SetJsonEnumValue<T>(string _propertyName, T _enumValue)
        {
            this.m_JsonObject.Add(_propertyName, (JToken)_enumValue.ToString());
        }

        protected string GetJsonValue(string _propertyName)
        {
            JToken jtoken;
            return this.m_JsonObject.TryGetValue(_propertyName, out jtoken) ? jtoken.ToString() : "";
        }

        protected void GetJsonValue(string _propertyName, ref string _strValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _strValue);
        }

        protected void GetJsonValue(string _propertyName, ref short _sValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _sValue);
        }

        protected void GetJsonValue(string _propertyName, ref ushort _usValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _usValue);
        }

        protected void GetJsonValue(string _propertyName, ref int _nValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _nValue);
        }

        protected void GetJsonValue(string _propertyName, ref uint _unValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _unValue);
        }

        protected void GetJsonValue(string _propertyName, ref float _fValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _fValue);
        }

        protected void GetJsonValue(string _propertyName, ref long _lValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _lValue);
        }

        protected void GetJsonValue(string _propertyName, ref ulong _ulValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _ulValue);
        }

        protected void GetJsonValue(string _propertyName, ref double _dValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _dValue);
        }

        protected void GetJsonValue(string _propertyName, ref bool _bValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _bValue);
        }

        protected void GetJsonValue(string _propertyName, ref byte _byValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _byValue);
        }

        protected void GetJsonValue(string _propertyName, ref sbyte _sbValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _sbValue);
        }

        protected void GetJsonValue(string _propertyName, ref DateTime _dateValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _dateValue);
        }

        protected void GetJsonValue(string _propertyName, ref TimeSpan _dateValue)
        {
            SerializerJson.GetJsonValue(this.m_JsonObject, _propertyName, ref _dateValue);
        }

        protected void GetJsonEnumValue<T>(string _propertyName, ref T _enumValue)
        {
            this.GetJsonEnumValue<T>(this.m_JsonObject, _propertyName, ref _enumValue);
        }

        public static void GetJsonValue(
            JObject _jsonObject,
            string _propertyName,
            ref string _strValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _strValue = jtoken.ToString();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref short _sValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _sValue = jtoken.ToObject<short>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref ushort _usValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _usValue = jtoken.ToObject<ushort>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref int _nValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _nValue = jtoken.ToObject<int>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref uint _unValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _unValue = jtoken.ToObject<uint>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref float _fValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _fValue = jtoken.ToObject<float>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref long _lValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _lValue = jtoken.ToObject<long>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref ulong _ulValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _ulValue = jtoken.ToObject<ulong>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref double _dValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _dValue = jtoken.ToObject<double>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref bool _bValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _bValue = jtoken.ToObject<bool>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref byte _byValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _byValue = jtoken.ToObject<byte>();
        }

        public static void GetJsonValue(JObject _jsonObject, string _propertyName, ref sbyte _sbValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken))
                return;
            _sbValue = jtoken.ToObject<sbyte>();
        }

        public static void GetJsonValue(
            JObject _jsonObject,
            string _propertyName,
            ref DateTime _dateValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken) || !(jtoken.ToString() != ""))
                return;
            _dateValue = jtoken.ToObject<DateTime>();
        }

        public static void GetJsonValue(
            JObject _jsonObject,
            string _propertyName,
            ref TimeSpan _dateValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken) || !(jtoken.ToString() != ""))
                return;
            _dateValue = jtoken.ToObject<TimeSpan>();
        }

        public void GetJsonEnumValue<T>(JObject _jsonObject, string _propertyName, ref T _enumValue)
        {
            JToken jtoken;
            if (!_jsonObject.TryGetValue(_propertyName, out jtoken) || !(jtoken.ToString() != ""))
                return;
            _enumValue = jtoken.ToString().ToEnum<T>();
        }

        protected JArray GetJsonArrayValue(string _propertyName)
        {
            string _strValue = "";
            this.GetJsonValue(_propertyName, ref _strValue);
            return _strValue == "" ? (JArray)null : JArray.Parse(_strValue);
        }

        public static JArray GetJsonArrayValue(JObject _jsonObject, string _propertyName)
        {
            string _strValue = "";
            SerializerJson.GetJsonValue(_jsonObject, _propertyName, ref _strValue);
            return _strValue == "" ? (JArray)null : JArray.Parse(_strValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref string _strValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _strValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref short _sValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _sValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref ushort _usValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _usValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref int _nValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _nValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref uint _unValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _unValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref float _fValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _fValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref long _lValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _lValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref ulong _ulValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _ulValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref double _dValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _dValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref bool _bValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _bValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref byte _byValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _byValue);
        }

        public static void GetJsonValue(string _jsonData, string _propertyName, ref sbyte _sbValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _sbValue);
        }

        public static void GetJsonValue(
            string _jsonData,
            string _propertyName,
            ref DateTime _dateValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _dateValue);
        }

        public static void GetJsonValue(
            string _jsonData,
            string _propertyName,
            ref TimeSpan _dateValue)
        {
            SerializerJson.GetJsonValue(JObject.Parse(_jsonData), _propertyName, ref _dateValue);
        }
    }
}