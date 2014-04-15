using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Coretypes.UInt;
using Ripple.Core.Core.Serialized;
using UInt16 = Ripple.Core.Core.Coretypes.UInt.UInt16;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;
using UInt64 = Ripple.Core.Core.Coretypes.UInt.UInt64;

namespace Ripple.Core.Core.Binary
{
    public class BinaryReader
    {
        private readonly BinaryParser _parser;

        public BinaryReader(BinaryParser parser)
        {
            _parser = parser;
        }

        public UInt8 UInt8()
        {
            return Coretypes.UInt.UInt8.Translate.FromParser(_parser);
        }

        public UInt16 UInt16()
        {
            return Coretypes.UInt.UInt16.Translate.FromParser(_parser);
        }

        public UInt32 UInt32()
        {
            return Coretypes.UInt.UInt32.Translate.FromParser(_parser);
        }

        public UInt64 UInt64()
        {
            return Coretypes.UInt.UInt64.Translate.FromParser(_parser);
        }

        public Hash128 Hash128()
        {
            return Coretypes.Hash.Hash128.Translate.FromParser(_parser);
        }

        public Hash160 Hash160()
        {
            return Coretypes.Hash.Hash160.Translate.FromParser(_parser);
        }

        public Currency Currency()
        {
            return Coretypes.Currency.Translate.FromParser(_parser);
        }

        public Hash256 Hash256()
        {
            return Coretypes.Hash.Hash256.Translate.FromParser(_parser);
        }

        public Vector256 Vector256()
        {
            return Coretypes.Vector256.Translate.FromParser(_parser);
        }

        public AccountId AccountId()
        {
            return Coretypes.AccountId.Translate.FromParser(_parser);
        }

        public VariableLength VariableLength()
        {
            int hint = _parser.ReadVlLength();
            return Coretypes.VariableLength.Translate.FromParser(_parser, hint);
        }

        public Amount Amount()
        {
            return Coretypes.Amount.Translate.FromParser(_parser);
        }

        public PathSet PathSet()
        {
            return Coretypes.PathSet.Translate.FromParser(_parser);
        }

        public StObject StObject()
        {
            return Coretypes.StObject.Translate.FromParser(_parser);
        }

        public StArray StArray()
        {
            return Coretypes.StArray.Translate.FromParser(_parser);
        }

        public RippleDate RippleDate()
        {
            return Coretypes.RippleDate.FromParser(_parser);
        }
    }
}
