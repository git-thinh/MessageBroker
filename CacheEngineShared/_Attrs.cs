using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class AttrApiInfo : Attribute
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string Result { set; get; }

        public AttrApiInfo(string title)
        {
            this.Title = title;
            this.Description = string.Empty;
            this.Result = string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class AttrModelInfo : Attribute
    {
        public string Title { set; get; }
        public string ServiceName { set; get; }

        public AttrModelInfo(string title, string serviceName = "")
        {
            this.Title = title;
            this.ServiceName = serviceName;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class AttrFieldInfo : Attribute
    {
        public int Index { set; get; }
        public string Title { set; get; }
        public bool IsKey { set; get; }
        public bool IsIndex { set; get; }
        public bool IsFullTextSearch { set; get; }
        public AttrDataType TypeCode { set; get; }
        public string TypeName { set; get; }

        public string EntityName = string.Empty;

        public string ServiceLink = string.Empty;
        public string ServiceLinkFieldName = string.Empty;

        public string GroupTitle = string.Empty;

        public AttrFieldInfo(int index, string title, AttrDataType typeCode, bool isKey = false, bool isIndex = false, bool isFullTextSearch = false)
        {
            this.Index = index;
            this.IsFullTextSearch = isFullTextSearch;
            this.IsIndex = IsIndex;
            this.IsKey = IsKey;
            this.Title = title;
            this.TypeCode = typeCode;

            switch (typeCode)
            {
                case AttrDataType.INT_ARRAY:
                    TypeName = "int[]";
                    break;
                case AttrDataType.INT:
                case AttrDataType.INT_DATE:
                case AttrDataType.INT_TIME:
                    TypeName = "int";
                    break;

                case AttrDataType.STRING_ARRAY:
                case AttrDataType.STRING_ARRAY_IMAGES:
                    TypeName = "string[]";
                    break;
                case AttrDataType.STRING:
                case AttrDataType.STRING_IMAGE:
                    TypeName = "string";
                    break;

                case AttrDataType.LONG_ARRAY:
                    TypeName = "long[]";
                    break;
                case AttrDataType.LONG:
                case AttrDataType.LONG_DATETIME:
                    TypeName = "long";
                    break;

                case AttrDataType.ENTITY_ARRAY:
                    TypeName = "object[]";
                    break;
                case AttrDataType.ENTITY:
                    TypeName = "object";
                    break;


                default:
                    TypeName = "object";
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
        INT_ARRAY = 10,
        INT = 11,
        INT_DATE = 12, //yyyyMMdd
        INT_TIME = 13, //1hhmmss

        STRING_ARRAY = 20,
        STRING = 21,
        STRING_ARRAY_IMAGES = 22,
        STRING_IMAGE = 23,

        LONG_ARRAY = 30,
        LONG = 31,
        LONG_DATETIME = 32, //yyyyMMddHHmmss

        ENUM_ARRAY = 70,
        ENUM = 71,

        ENTITY_ARRAY = 80,
        ENTITY = 81,

        OBJECT = 90,

        SERVICE_LINK_RESULT = 99
    }

    public class oModelInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string ServiceName { get; set; }

        public List<oModelFielInfo> Properties { get; set; }
        public oModelInfo()
        {
            this.ServiceName = string.Empty;
            this.Name = string.Empty;
            this.Title = string.Empty;
            this.Properties = new List<oModelFielInfo>() { };
        }
    }

    public class oModelFielInfo
    {
        public int Index { set; get; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string TypeName { get; set; }
        public int TypeCode { get; set; }
        public bool IsKey { get; set; }
        public bool IsFullTextSearch { set; get; }
        public bool IsIndex { set; get; }
        public string EntityName { set; get; }
        public string ServiceLink { set; get; }
        public string ServiceLinkFieldName { set; get; }
        public string GroupTitle { set; get; }

        public oModelFielInfo(string name, AttrFieldInfo attr)
        {
            this.Index = attr.Index;
            this.IsFullTextSearch = attr.IsFullTextSearch;
            this.IsIndex = attr.IsIndex;
            this.IsKey = attr.IsKey;
            this.Name = name;
            this.Title = attr.Title;
            this.TypeName = attr.TypeName;
            this.TypeCode = (int)attr.TypeCode;
            this.GroupTitle = attr.GroupTitle;

            switch (attr.TypeCode)
            {
                case AttrDataType.ENTITY:
                case AttrDataType.ENUM:
                    this.TypeName = attr.EntityName;
                    break;
                case AttrDataType.ENTITY_ARRAY:
                case AttrDataType.ENUM_ARRAY:
                    this.TypeName = attr.EntityName + "[]";
                    break;
            }

            this.EntityName = attr.EntityName;
            this.ServiceLink = attr.ServiceLink;
            this.ServiceLinkFieldName = attr.ServiceLinkFieldName;
        }
    }
}
