using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class AttrModelInfo : Attribute
    {
        public string Title { set; get; }

        public AttrModelInfo(string title)
        {
            this.Title = title;
        }

        public string GetTitle()
        {
            return Title;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class AttrFieldInfo : Attribute
    {
        public string Title { set; get; }
        public bool IsKey { set; get; }
        public bool IsIndex { set; get; }
        public string DataType { set; get; }

        public AttrFieldInfo(string title, AttrDataType dataType, bool isKey = false, bool isIndex = false)
        {
            this.IsIndex = IsIndex;
            this.IsKey = IsKey;
            this.Title = title;
            switch (dataType)
            {
                case AttrDataType.INT:
                    DataType = "int";
                    break;
                case AttrDataType.INT_ARRAY:
                    DataType = "int[]";
                    break;
                case AttrDataType.STRING:
                    DataType = "string";
                    break;
                case AttrDataType.LONG:
                    DataType = "long";
                    break;
                case AttrDataType.STRING_ARRAY:
                    DataType = "string[]";
                    break;
                default:
                    DataType = "object";
                    break;
            }
        }

        public string GetTitle()
        {
            return Title;
        }
    }

    public enum AttrDataType
    {
        INT = 1,
        STRING = 2,
        INT_ARRAY = 3,
        STRING_ARRAY = 4,
        LONG = 5,
        OBJECT = 99
    }

    public class oModelInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public List<oModelFielInfo> Properties { get; set; }
        public oModelInfo()
        {
            this.Name = string.Empty;
            this.Title = string.Empty;
            this.Properties = new List<oModelFielInfo>() { };
        }
    }

    public class oModelFielInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsKey { get; set; }
        public bool IsIndex { set; get; }

        public oModelFielInfo(string name, AttrFieldInfo attr)
        {
            this.IsIndex = attr.IsIndex;
            this.IsKey = attr.IsKey;
            this.Name = name;
            this.Title = attr.Title;
            this.Type = attr.DataType;
        }
    }
}
