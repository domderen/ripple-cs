using System.Collections.Generic;
using System.Linq;

namespace Ripple.Core.Core.Enums
{
    public class TransactionType
    {
        private static readonly IDictionary<string, TransactionType> ByName = new Dictionary<string, TransactionType>();
        private static readonly IDictionary<int, TransactionType> ByCode = new Dictionary<int, TransactionType>();
        private readonly int _ord;

        static TransactionType()
        {
            ByName.Add("Invalid", Invalid);
            ByName.Add("Payment", Payment);
            ByName.Add("Claim", Claim);
            ByName.Add("WalletAdd", WalletAdd);
            ByName.Add("AccountSet", AccountSet);
            ByName.Add("PasswordFund", PasswordFund);
            ByName.Add("SetRegularKey", SetRegularKey);
            ByName.Add("NickNameSet", NickNameSet);
            ByName.Add("OfferCreate", OfferCreate);
            ByName.Add("OfferCancel", OfferCancel);
            ByName.Add("Contract", Contract);
            ByName.Add("RemoveContract", RemoveContract);
            ByName.Add("TrustSet", TrustSet);
            ByName.Add("EnableFeature", EnableFeature);
            ByName.Add("SetFee", SetFee);

            foreach (var value in Values)
            {
                ByCode.Add(value._ord, value);
            }
        }

        private TransactionType(int i)
        {
            _ord = i;
        }

        public static readonly TransactionType Invalid = new TransactionType(-1);
        public static readonly TransactionType Payment = new TransactionType(0);

        /// <summary>
        /// Open.
        /// </summary>
        public static readonly TransactionType Claim = new TransactionType(1);
        public static readonly TransactionType WalletAdd = new TransactionType(2);
        public static readonly TransactionType AccountSet = new TransactionType(3);

        /// <summary>
        /// Open.
        /// </summary>
        public static readonly TransactionType PasswordFund = new TransactionType(4);
        public static readonly TransactionType SetRegularKey = new TransactionType(5);

        /// <summary>
        /// Open.
        /// </summary>
        public static readonly TransactionType NickNameSet = new TransactionType(6);
        public static readonly TransactionType OfferCreate = new TransactionType(7);
        public static readonly TransactionType OfferCancel = new TransactionType(8);
        public static readonly TransactionType Contract = new TransactionType(9);

        /// <summary>
        /// Can we use the same msg as offer cancel.
        /// </summary>
        public static readonly TransactionType RemoveContract = new TransactionType(10);
        public static readonly TransactionType TrustSet = new TransactionType(20);
        public static readonly TransactionType EnableFeature = new TransactionType(100);
        public static readonly TransactionType SetFee = new TransactionType(101);

        public static IEnumerable<TransactionType> Values
        {
            get
            {
                yield return Invalid;
                yield return Payment;
                yield return Claim;
                yield return WalletAdd;
                yield return AccountSet;
                yield return PasswordFund;
                yield return SetRegularKey;
                yield return NickNameSet;
                yield return OfferCreate;
                yield return OfferCancel;
                yield return Contract;
                yield return RemoveContract;
                yield return TrustSet;
                yield return EnableFeature;
                yield return SetFee;
            }
        }

        public int AsInteger
        {
            get { return _ord; }
        }

        public static TransactionType FromNumber(int i)
        {
            return ByCode[i];
        }

        public static TransactionType FromString(string s)
        {
            return ByName[s];
        }

        public string GetName()
        {
            return ByName.Single(q => q.Value == this).Key;
        }
    }
}
