using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes
{
    public class StArray : List<StObject>, ISerializedType
    {
        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static TypedFields.StArrayField AffectedNodes = new StArrayField(Field.AffectedNodes);

        public static TypedFields.StArrayField SigningAccounts = new StArrayField(Field.SigningAccounts);
        public static TypedFields.StArrayField TxnSignatures = new StArrayField(Field.TxnSignatures);
        public static TypedFields.StArrayField Signatures = new StArrayField(Field.Signatures);
        public static TypedFields.StArrayField Template = new StArrayField(Field.Template);
        public static TypedFields.StArrayField Necessary = new StArrayField(Field.Necessary);
        public static TypedFields.StArrayField Sufficient = new StArrayField(Field.Sufficient);

        public StArray()
        {
        }

        object ISerializedType.ToJson()
        {
            return ToJArray();
        }

        public JArray ToJArray()
        {
            var array = new JArray();

            foreach (var so in this)
            {
                array.Add(so.ToJson());
            }

            return array;
        }

        public byte[] ToBytes()
        {
            return InTranslate.ToBytes(this);
        }

        public string ToHex()
        {
            return InTranslate.ToHex(this);
        }

        public void ToBytesSink(IBytesSink to)
        {
            foreach (var stObject in this)
            {
                stObject.ToBytesSink(to);
            }
        }

        public class OutTranslator : OutTypeTranslator<StArray>
        {
            public override StArray FromParser(BinaryParser parser, int? hint)
            {
                var stArray = new StArray();

                while (!parser.End)
                {
                    Field field = parser.ReadField();

                    if (field == Field.ArrayEndMarker)
                    {
                        break;
                    }

                    var outer = new StObject {{field, StObject.OutTranslate.FromParser(parser)}};
                    stArray.Add(outer);
                }

                return stArray;
            }

            public override StArray FromJsonArray(JArray jsonArray)
            {
                var arr = new StArray();

                for (int i = 0; i < jsonArray.Count; i++)
                {
                    try
                    {
                        var o = jsonArray[i];
                        arr.Add(StObject.FromJObject(o.ToObject<JObject>()));
                    }
                    catch (JsonException e)
                    {
                        throw new ApplicationException("Json deserialization failed.", e);
                    }
                }

                return arr;
            }
        }

        public class InTranslator : InTypeTranslator<StArray>
        {
            public override JArray ToJArray(StArray obj)
            {
                return obj.ToJArray();
            }
        }

        private class StArrayField : TypedFields.StArrayField
        {
            private readonly Field _f;

            public StArrayField(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }

    }
}
