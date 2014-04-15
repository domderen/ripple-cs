using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.UInt;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Tx.Result
{
    public class TransactionMeta : StObject, IEnumerable<AffectedNode>
    {
        public UInt32 TransactionIndex
        {
            get { return base[UInt32.TransactionIndex]; } 
        }

        public static bool IsTransactionMeta(StObject source)
        {
            return source.Has(UInt8.TransactionResult) && source.Has(Field.AffectedNodes);
        }

        public TransactionEngineResult TransactionResult()
        {
            return TransactionResult(this);
        }

        public IEnumerable<AffectedNode> AffectedNodes()
        {
            while (GetEnumerator().MoveNext())
            {
                yield return GetEnumerator().Current;
            }
        }

        public new IEnumerator<AffectedNode> GetEnumerator()
        {
            StArray nodes = base[StArray.AffectedNodes];
            return new AffectedNodeEnumerator(nodes.GetEnumerator());
        }

        private class AffectedNodeEnumerator : IEnumerator<AffectedNode>
        {
            private readonly IEnumerator<StObject> _enumerator;

            public AffectedNodeEnumerator(IEnumerator<StObject> enumerator)
            {
                _enumerator = enumerator;
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }

            public AffectedNode Current
            {
                get { return (AffectedNode) _enumerator.Current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                }
            }
        }
    }
}
