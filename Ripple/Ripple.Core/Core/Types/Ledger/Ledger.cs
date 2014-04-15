using Ripple.Core.Core.Binary;
using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Coretypes.UInt;
using Ripple.Core.Core.Serialized;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;
using UInt64 = Ripple.Core.Core.Coretypes.UInt.UInt64;

namespace Ripple.Core.Core.Types.Ledger
{
    public class Ledger
    {
        private UInt32 _version; // Always 0x4C475200 (LGR) (Secures signed objects)
        private UInt32 _sequence;        // Ledger Sequence (0 for genesis ledger)           
        private UInt64 _totalXrp;        //                                                  
        private Hash256 _previousLedger;  // The hash of the previous ledger (0 for genesis ledger)
        private Hash256 _transactionHash; // The hash of the transaction tree's root node.    
        private Hash256 _stateHash;       // The hash of the state tree's root node.          
        private UInt32 _parentCloseTime; // The time the previous ledger closed              
        private UInt32 _closeTime;       // UTC minute ledger closed encoded as seconds since 1/1/2000 (or 0 for genesis ledger)
        private UInt8 _closeResolution; // The resolution (in seconds) of the close time    
        private UInt8 _closeFlags;      // Flags

        private RippleDate _closeDate;

        public static Ledger FromParser(BinaryParser parser)
        {
            return FromReader(new BinaryReader(parser));
        }

        private static Ledger FromReader(BinaryReader reader)
        {
            var ledger = new Ledger
            {
                _version = reader.UInt32(),
                _sequence = reader.UInt32(),
                _totalXrp = reader.UInt64(),
                _previousLedger = reader.Hash256(),
                _transactionHash = reader.Hash256(),
                _stateHash = reader.Hash256(),
                _parentCloseTime = reader.UInt32(),
                _closeTime = reader.UInt32(),
                _closeResolution = reader.UInt8(),
                _closeFlags = reader.UInt8()
            };

            ledger._closeDate = RippleDate.FromSecondsSinceRippleEpoch(ledger._closeTime.LongValue());

            return ledger;
        }
    }
}
