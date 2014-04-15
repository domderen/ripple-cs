using System;
using System.Collections.Generic;

namespace Ripple.Core.Core.Coretypes.Hash.Prefixes
{
    public class LedgerSpace : IPrefix
    {
        private readonly byte[] _bytes;

        private LedgerSpace(char a)
        {
            byte[] intBytes = BitConverter.GetBytes(a);
            Array.Reverse(intBytes);

            _bytes = intBytes;
        }

        public static LedgerSpace Account = new LedgerSpace('a');
        public static LedgerSpace DirNode = new LedgerSpace('d');
        public static LedgerSpace Generator = new LedgerSpace('g');
        public static LedgerSpace Nickname = new LedgerSpace('n');
        public static LedgerSpace Ripple = new LedgerSpace('r');

        /// <summary>
        /// Entry for an offer.
        /// </summary>
        public static LedgerSpace Offer = new LedgerSpace('a');

        /// <summary>
        /// Directory of things owned by account.
        /// </summary>
        public static LedgerSpace OwnerDir = new LedgerSpace('O');

        /// <summary>
        /// Directory of order books.
        /// </summary>
        public static LedgerSpace BookDir = new LedgerSpace('B');
        public static LedgerSpace Contract = new LedgerSpace('c');
        public static LedgerSpace SkipList = new LedgerSpace('s');
        public static LedgerSpace Feature = new LedgerSpace('f');
        public static LedgerSpace Fee = new LedgerSpace('e');

        public static IEnumerable<LedgerSpace> Values
        {
            get
            {
                yield return Account;
                yield return DirNode;
                yield return Generator;
                yield return Nickname;
                yield return Ripple;
                yield return Offer;
                yield return OwnerDir;
                yield return BookDir;
                yield return Contract;
                yield return SkipList;
                yield return Feature;
                yield return Fee;
            }
        }

        public byte[] Bytes
        {
            get { return _bytes; }
        }
    }
}
