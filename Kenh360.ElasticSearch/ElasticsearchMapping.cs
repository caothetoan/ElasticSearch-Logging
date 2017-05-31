using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Elastic
{
   public class ElasticsearchMapping
    {
        /// <summary>
        /// Override this if you require a special type definitoin for your document type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The type used in Elasticsearch for this type</returns>
        public virtual string GetDocumentType(Type type)
        {
            // Adding support for EF types
            if (type.BaseType != null && type.Namespace == "System.Data.Entity.DynamicProxies")
            {
                type = type.BaseType;
            }
            return type.Name.ToLower();
        }

        public virtual Type GetEntityDocumentType(Type type)
        {
            // Adding support for EF types
            if (type.BaseType != null && type.Namespace == "System.Data.Entity.DynamicProxies")
            {
                type = type.BaseType;
            }
            return type;
        }

        /// <summary>
        /// Overide this if you need to define the index for your document. 
        /// Required if your using a child document type.
        /// Default: pluralize the default type
        /// </summary>
        /// <param name="type">Type of class used</param>
        /// <returns>The index used in Elasticsearch for this type</returns>
        public virtual string GetIndexForType(Type type)
        {
            // Adding support for EF types
            if (type.BaseType != null && type.Namespace == "System.Data.Entity.DynamicProxies")
            {
                type = type.BaseType;
            }
            return type.Name.ToLower();// + "s"
        }

        /// <summary>
        /// bool System.Boolean
        /// byte System.Byte
        /// sbyte System.SByte 
        /// char System.Char
        /// decimal System.Decimal => string
        /// double System.Double
        /// float System.Single
        /// int System.Int32
        /// uint System.UInt32
        /// long System.Int64
        /// ulong System.UInt64
        /// short System.Int16
        /// ushort System.UInt16
        /// string System.String 
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns>
        /// string,  boolean, and null.
        /// float, double, byte, short, integer, and long
        /// date
        /// binary
        /// </returns>
        public string GetElasticsearchType(Type propertyType)
        {
            switch (propertyType.FullName)
            {
                case "System.Boolean":
                    return "boolean";
                case "System.Byte":
                    return "byte";
                case "System.SByte":
                    return "byte";
                case "System.Double":
                    return "double";
                case "System.Single":
                    return "float";
                case "System.Int32":
                    return "integer";
                case "System.UInt32":
                    return "integer";
                case "System.Int64":
                    return "long";
                case "System.UInt64":
                    return "long";
                case "System.Int16":
                    return "short";
                case "System.UInt16":
                    return "short";
                default:
                    return "string";
            }
        }
    }
}
