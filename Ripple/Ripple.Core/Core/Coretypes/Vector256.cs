using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Coretypes
{
    public class Vector256 : List<Hash256>, ISerializedType
    {
        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static TypedFields.Vector256Field Indexes = new Vector256Field(Field.Indexes);
        public static TypedFields.Vector256Field Hashes = new Vector256Field(Field.Hashes);
        public static TypedFields.Vector256Field Features = new Vector256Field(Field.Features);

        public object ToJson()
        {
            return ToJArray();
        }

        public JArray ToJArray()
        {
            var array = new JArray();

            foreach (var hash256 in this)
            {
                array.Add(hash256.ToString());
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
            foreach (var hash256 in this)
            {
                hash256.ToBytesSink(to);
            }
        }

        public class OutTranslator : OutTypeTranslator<Vector256>
        {
            public override Vector256 FromParser(BinaryParser parser, int? hint)
            {
                var vector256 = new Vector256();
                if (hint == null)
                {
                    hint = parser.Size;
                }

                for (int i = 0; i < hint / 32; i++)
                {
                    vector256.Add(Hash256.OutTranslate.FromParser(parser));
                }

                return vector256;
            }

            public override Vector256 FromJsonArray(JArray jsonArray)
            {
                var vector = new Vector256();

                for (int i = 0; i < jsonArray.Count; i++)
                {
                    try
                    {
                        var hex = jsonArray[i].ToObject<string>();
                        vector.Add(new Hash256(B16.Decode(hex)));
                    }
                    catch (JsonException e)
                    {
                        throw new ApplicationException("Json deserialization failed.", e);
                    }
                }

                return vector;
            }
        }

        public class InTranslator : InTypeTranslator<Vector256>
        {
            public override JArray ToJArray(Vector256 obj)
            {
                return obj.ToJArray();
            }
        }

        private class Vector256Field : TypedFields.Vector256Field
        {
            private readonly Field _f;

            public Vector256Field(Field f)
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
