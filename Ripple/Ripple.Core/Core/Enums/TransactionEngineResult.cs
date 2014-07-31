using System.Collections.Generic;
using System.Linq;
using Ripple.Core.Core.Coretypes.UInt;

namespace Ripple.Core.Core.Enums
{
    public class TransactionEngineResult
    {
        private static readonly IDictionary<string, TransactionEngineResult> ByName = new Dictionary<string, TransactionEngineResult>();
        private static readonly IDictionary<int, TransactionEngineResult> ByCode = new Dictionary<int, TransactionEngineResult>();
        private readonly int _ord;
        private string _human;

        static TransactionEngineResult()
        {
            ByName.Add("telLOCAL_ERROR", telLOCAL_ERROR);
            ByName.Add("telBAD_DOMAIN", telBAD_DOMAIN);
            ByName.Add("telBAD_PATH_COUNT", telBAD_PATH_COUNT);
            ByName.Add("telBAD_PUBLIC_KEY", telBAD_PUBLIC_KEY);
            ByName.Add("telFAILED_PROCESSING", telFAILED_PROCESSING);
            ByName.Add("telINSUF_FEE_P", telINSUF_FEE_P);
            ByName.Add("telNO_DST_PARTIAL", telNO_DST_PARTIAL);
            ByName.Add("temMALFORMED", temMALFORMED);
            ByName.Add("temBAD_AMOUNT", temBAD_AMOUNT);
            ByName.Add("temBAD_AUTH_MASTER", temBAD_AUTH_MASTER);
            ByName.Add("temBAD_CURRENCY", temBAD_CURRENCY);
            ByName.Add("temBAD_FEE", temBAD_FEE);
            ByName.Add("temBAD_EXPIRATION", temBAD_EXPIRATION);
            ByName.Add("temBAD_ISSUER", temBAD_ISSUER);
            ByName.Add("temBAD_LIMIT", temBAD_LIMIT);
            ByName.Add("temBAD_OFFER", temBAD_OFFER);
            ByName.Add("temBAD_PATH", temBAD_PATH);
            ByName.Add("temBAD_PATH_LOOP", temBAD_PATH_LOOP);
            ByName.Add("temBAD_PUBLISH", temBAD_PUBLISH);
            ByName.Add("temBAD_TRANSFER_RATE", temBAD_TRANSFER_RATE);
            ByName.Add("temBAD_SEND_XRP_LIMIT", temBAD_SEND_XRP_LIMIT);
            ByName.Add("temBAD_SEND_XRP_MAX", temBAD_SEND_XRP_MAX);
            ByName.Add("temBAD_SEND_XRP_NO_DIRECT", temBAD_SEND_XRP_NO_DIRECT);
            ByName.Add("temBAD_SEND_XRP_PARTIAL", temBAD_SEND_XRP_PARTIAL);
            ByName.Add("temBAD_SEND_XRP_PATHS", temBAD_SEND_XRP_PATHS);
            ByName.Add("temBAD_SIGNATURE", temBAD_SIGNATURE);
            ByName.Add("temBAD_SRC_ACCOUNT", temBAD_SRC_ACCOUNT);
            ByName.Add("temBAD_SEQUENCE", temBAD_SEQUENCE);
            ByName.Add("temDST_IS_SRC", temDST_IS_SRC);
            ByName.Add("temDST_NEEDED", temDST_NEEDED);
            ByName.Add("temINVALID", temINVALID);
            ByName.Add("temINVALID_FLAG", temINVALID_FLAG);
            ByName.Add("temREDUNDANT", temREDUNDANT);
            ByName.Add("temREDUNDANT_SEND_MAX", temREDUNDANT_SEND_MAX);
            ByName.Add("temRIPPLE_EMPTY", temRIPPLE_EMPTY);
            ByName.Add("temUNCERTAIN", temUNCERTAIN);
            ByName.Add("temUNKNOWN", temUNKNOWN);
            ByName.Add("tefFAILURE", tefFAILURE);
            ByName.Add("tefALREADY", tefALREADY);
            ByName.Add("tefBAD_ADD_AUTH", tefBAD_ADD_AUTH);
            ByName.Add("tefBAD_AUTH", tefBAD_AUTH);
            ByName.Add("tefBAD_CLAIM_ID", tefBAD_CLAIM_ID);
            ByName.Add("tefBAD_GEN_AUTH", tefBAD_GEN_AUTH);
            ByName.Add("tefBAD_LEDGER", tefBAD_LEDGER);
            ByName.Add("tefCLAIMED", tefCLAIMED);
            ByName.Add("tefCREATED", tefCREATED);
            ByName.Add("tefDST_TAG_NEEDED", tefDST_TAG_NEEDED);
            ByName.Add("tefEXCEPTION", tefEXCEPTION);
            ByName.Add("tefGEN_IN_USE", tefGEN_IN_USE);
            ByName.Add("tefINTERNAL", tefINTERNAL);
            ByName.Add("tefNO_AUTH_REQUIRED", tefNO_AUTH_REQUIRED);
            ByName.Add("tefPAST_SEQ", tefPAST_SEQ);
            ByName.Add("tefWRONG_PRIOR", tefWRONG_PRIOR);
            ByName.Add("tefMASTER_DISABLED", tefMASTER_DISABLED);
            ByName.Add("tefMAX_LEDGER", tefMAX_LEDGER);
            ByName.Add("terRETRY", terRETRY);
            ByName.Add("terFUNDS_SPENT", terFUNDS_SPENT);
            ByName.Add("terINSUF_FEE_B", terINSUF_FEE_B);
            ByName.Add("terNO_ACCOUNT", terNO_ACCOUNT);
            ByName.Add("terNO_AUTH", terNO_AUTH);
            ByName.Add("terNO_LINE", terNO_LINE);
            ByName.Add("terOWNERS", terOWNERS);
            ByName.Add("terPRE_SEQ", terPRE_SEQ);
            ByName.Add("terLAST", terLAST);
            ByName.Add("tesSUCCESS", tesSUCCESS);
            ByName.Add("tecCLAIM", tecCLAIM);
            ByName.Add("tecPATH_PARTIAL", tecPATH_PARTIAL);
            ByName.Add("tecUNFUNDED_ADD", tecUNFUNDED_ADD);
            ByName.Add("tecUNFUNDED_OFFER", tecUNFUNDED_OFFER);
            ByName.Add("tecUNFUNDED_PAYMENT", tecUNFUNDED_PAYMENT);
            ByName.Add("tecFAILED_PROCESSING", tecFAILED_PROCESSING);
            ByName.Add("tecDIR_FULL", tecDIR_FULL);
            ByName.Add("tecINSUF_RESERVE_LINE", tecINSUF_RESERVE_LINE);
            ByName.Add("tecINSUF_RESERVE_OFFER", tecINSUF_RESERVE_OFFER);
            ByName.Add("tecNO_DST", tecNO_DST);
            ByName.Add("tecNO_DST_INSUF_XRP", tecNO_DST_INSUF_XRP);
            ByName.Add("tecNO_LINE_INSUF_RESERVE", tecNO_LINE_INSUF_RESERVE);
            ByName.Add("tecNO_LINE_REDUNDANT", tecNO_LINE_REDUNDANT);
            ByName.Add("tecPATH_DRY", tecPATH_DRY);
            ByName.Add("tecUNFUNDED", tecUNFUNDED);
            ByName.Add("tecMASTER_DISABLED", tecMASTER_DISABLED);
            ByName.Add("tecNO_REGULAR_KEY", tecNO_REGULAR_KEY);
            ByName.Add("tecOWNERS", tecOWNERS);

            foreach (var value in Values)
            {
                ByCode.Add(value._ord, value);
            }
        }

        private TransactionEngineResult(int i, string s)
        {
            _human = s;
            _ord = i;
        }

        public static readonly TransactionEngineResult telLOCAL_ERROR = new TransactionEngineResult(-399, "Local failure.");
        public static readonly TransactionEngineResult telBAD_DOMAIN = new TransactionEngineResult(-398, "Domain too long.");
        public static readonly TransactionEngineResult telBAD_PATH_COUNT = new TransactionEngineResult(-397, "Malformed: Too many paths.");
        public static readonly TransactionEngineResult telBAD_PUBLIC_KEY = new TransactionEngineResult(-396, "Public key too long.");
        public static readonly TransactionEngineResult telFAILED_PROCESSING = new TransactionEngineResult(-395, "Failed to correctly process transaction.");
        public static readonly TransactionEngineResult telINSUF_FEE_P = new TransactionEngineResult(-394, "Fee insufficient.");
        public static readonly TransactionEngineResult telNO_DST_PARTIAL = new TransactionEngineResult(-393, "Partial payment to create account not allowed.");
        public static readonly TransactionEngineResult temMALFORMED = new TransactionEngineResult(-299, "Malformed transaction.");
        public static readonly TransactionEngineResult temBAD_AMOUNT = new TransactionEngineResult(-298, "Can only send positive amounts.");
        public static readonly TransactionEngineResult temBAD_AUTH_MASTER = new TransactionEngineResult(-297, "Auth for unclaimed account needs correct master key.");
        public static readonly TransactionEngineResult temBAD_CURRENCY = new TransactionEngineResult(-296, "Malformed: Bad currency.");
        public static readonly TransactionEngineResult temBAD_FEE = new TransactionEngineResult(-295, "Invalid fee, negative or not XRP.");
        public static readonly TransactionEngineResult temBAD_EXPIRATION = new TransactionEngineResult(-294, "Malformed: Bad expiration.");
        public static readonly TransactionEngineResult temBAD_ISSUER = new TransactionEngineResult(-293, "Malformed: Bad issuer.");
        public static readonly TransactionEngineResult temBAD_LIMIT = new TransactionEngineResult(-292, "Limits must be non-negative.");
        public static readonly TransactionEngineResult temBAD_OFFER = new TransactionEngineResult(-291, "Malformed: Bad offer.");
        public static readonly TransactionEngineResult temBAD_PATH = new TransactionEngineResult(-290, "Malformed: Bad path.");
        public static readonly TransactionEngineResult temBAD_PATH_LOOP = new TransactionEngineResult(-289, "Malformed: Loop in path.");
        public static readonly TransactionEngineResult temBAD_PUBLISH = new TransactionEngineResult(-288, "Malformed: Bad publish.");
        public static readonly TransactionEngineResult temBAD_TRANSFER_RATE = new TransactionEngineResult(-287, "Malformed: Transfer rate must be >= 1.0");
        public static readonly TransactionEngineResult temBAD_SEND_XRP_LIMIT = new TransactionEngineResult(-286, "Malformed: Limit quality is not allowed for XRP to XRP.");
        public static readonly TransactionEngineResult temBAD_SEND_XRP_MAX = new TransactionEngineResult(-285, "Malformed: Send max is not allowed for XRP to XRP.");
        public static readonly TransactionEngineResult temBAD_SEND_XRP_NO_DIRECT = new TransactionEngineResult(-284, "Malformed: No Ripple direct is not allowed for XRP to XRP.");
        public static readonly TransactionEngineResult temBAD_SEND_XRP_PARTIAL = new TransactionEngineResult(-283, "Malformed: Partial payment is not allowed for XRP to XRP.");
        public static readonly TransactionEngineResult temBAD_SEND_XRP_PATHS = new TransactionEngineResult(-282, "Malformed: Paths are not allowed for XRP to XRP.");
        public static readonly TransactionEngineResult temBAD_SIGNATURE = new TransactionEngineResult(-281, "Malformed: Bad signature.");
        public static readonly TransactionEngineResult temBAD_SRC_ACCOUNT = new TransactionEngineResult(-280, "Malformed: Bad source account.");
        public static readonly TransactionEngineResult temBAD_SEQUENCE = new TransactionEngineResult(-279, "Malformed: Sequence is not in the past.");
        public static readonly TransactionEngineResult temDST_IS_SRC = new TransactionEngineResult(-278, "Destination may not be source.");
        public static readonly TransactionEngineResult temDST_NEEDED = new TransactionEngineResult(-277, "Destination not specified.");
        public static readonly TransactionEngineResult temINVALID = new TransactionEngineResult(-276, "The transaction is ill-formed.");
        public static readonly TransactionEngineResult temINVALID_FLAG = new TransactionEngineResult(-275, "The transaction has an invalid flag.");
        public static readonly TransactionEngineResult temREDUNDANT = new TransactionEngineResult(-274, "Sends same currency to self.");
        public static readonly TransactionEngineResult temREDUNDANT_SEND_MAX = new TransactionEngineResult(-273, "Send max is redundant.");
        public static readonly TransactionEngineResult temRIPPLE_EMPTY = new TransactionEngineResult(-272, "PathSet with no paths.");
        public static readonly TransactionEngineResult temUNCERTAIN = new TransactionEngineResult(-271, "In process of determining result. Never returned.");
        public static readonly TransactionEngineResult temUNKNOWN = new TransactionEngineResult(-270, "The transactions requires logic not implemented yet.");
        public static readonly TransactionEngineResult tefFAILURE = new TransactionEngineResult(-199, "Failed to apply.");
        public static readonly TransactionEngineResult tefALREADY = new TransactionEngineResult(-198, "The exact transaction was already in this ledger.");
        public static readonly TransactionEngineResult tefBAD_ADD_AUTH = new TransactionEngineResult(-197, "Not authorized to add account.");
        public static readonly TransactionEngineResult tefBAD_AUTH = new TransactionEngineResult(-196, "Transaction's public key is not authorized.");
        public static readonly TransactionEngineResult tefBAD_CLAIM_ID = new TransactionEngineResult(-195, "Malformed: Bad claim id.");
        public static readonly TransactionEngineResult tefBAD_GEN_AUTH = new TransactionEngineResult(-194, "Not authorized to claim generator.");
        public static readonly TransactionEngineResult tefBAD_LEDGER = new TransactionEngineResult(-193, "Ledger in unexpected state.");
        public static readonly TransactionEngineResult tefCLAIMED = new TransactionEngineResult(-192, "Can not claim a previously claimed account.");
        public static readonly TransactionEngineResult tefCREATED = new TransactionEngineResult(-191, "Can't add an already created account.");
        public static readonly TransactionEngineResult tefDST_TAG_NEEDED = new TransactionEngineResult(-190, "Destination tag required.");
        public static readonly TransactionEngineResult tefEXCEPTION = new TransactionEngineResult(-189, "Unexpected program state.");
        public static readonly TransactionEngineResult tefGEN_IN_USE = new TransactionEngineResult(-188, "Generator already in use.");
        public static readonly TransactionEngineResult tefINTERNAL = new TransactionEngineResult(-187, "Internal error.");
        public static readonly TransactionEngineResult tefNO_AUTH_REQUIRED = new TransactionEngineResult(-186, "Auth is not required.");
        public static readonly TransactionEngineResult tefPAST_SEQ = new TransactionEngineResult(-185, "This sequence number has already past.");
        public static readonly TransactionEngineResult tefWRONG_PRIOR = new TransactionEngineResult(-184, "tefWRONG_PRIOR");
        public static readonly TransactionEngineResult tefMASTER_DISABLED = new TransactionEngineResult(-183, "tefMASTER_DISABLED");

        /// <summary>
        /// TODO: Check -182.
        /// </summary>
        public static readonly TransactionEngineResult tefMAX_LEDGER = new TransactionEngineResult(-182, "Ledger sequence too high.");

        public static readonly TransactionEngineResult terRETRY = new TransactionEngineResult(-99, "Retry transaction.");
        public static readonly TransactionEngineResult terFUNDS_SPENT = new TransactionEngineResult(-98, "Can't set password, password set funds already spent.");
        public static readonly TransactionEngineResult terINSUF_FEE_B = new TransactionEngineResult(-97, "AccountID balance can't pay fee.");
        public static readonly TransactionEngineResult terNO_ACCOUNT = new TransactionEngineResult(-96, "The source account does not exist.");
        public static readonly TransactionEngineResult terNO_AUTH = new TransactionEngineResult(-95, "Not authorized to hold IOUs.");
        public static readonly TransactionEngineResult terNO_LINE = new TransactionEngineResult(-94, "No such line.");
        public static readonly TransactionEngineResult terOWNERS = new TransactionEngineResult(-93, "Non-zero owner count.");
        public static readonly TransactionEngineResult terPRE_SEQ = new TransactionEngineResult(-92, "Missing/inapplicable prior transaction.");
        public static readonly TransactionEngineResult terLAST = new TransactionEngineResult(-91, "Process last.");
        public static readonly TransactionEngineResult tesSUCCESS = new TransactionEngineResult(0, "The transaction was applied.");
        public static readonly TransactionEngineResult tecCLAIM = new TransactionEngineResult(100, "Fee claimed. Sequence used. No action.");
        public static readonly TransactionEngineResult tecPATH_PARTIAL = new TransactionEngineResult(101, "Path could not send full amount.");
        public static readonly TransactionEngineResult tecUNFUNDED_ADD = new TransactionEngineResult(102, "Insufficient XRP balance for WalletAdd.");
        public static readonly TransactionEngineResult tecUNFUNDED_OFFER = new TransactionEngineResult(103, "Insufficient balance to fund created offer.");
        public static readonly TransactionEngineResult tecUNFUNDED_PAYMENT = new TransactionEngineResult(104, "Insufficient XRP balance to send.");
        public static readonly TransactionEngineResult tecFAILED_PROCESSING = new TransactionEngineResult(105, "Failed to correctly process transaction.");
        public static readonly TransactionEngineResult tecDIR_FULL = new TransactionEngineResult(121, "Can not add entry to full directory.");
        public static readonly TransactionEngineResult tecINSUF_RESERVE_LINE = new TransactionEngineResult(122, "Insufficient reserve to add trust line.");
        public static readonly TransactionEngineResult tecINSUF_RESERVE_OFFER = new TransactionEngineResult(123, "Insufficient reserve to create offer.");
        public static readonly TransactionEngineResult tecNO_DST = new TransactionEngineResult(124, "Destination does not exist. Send XRP to create it.");
        public static readonly TransactionEngineResult tecNO_DST_INSUF_XRP = new TransactionEngineResult(125, "Destination does not exist. Too little XRP sent to create it.");
        public static readonly TransactionEngineResult tecNO_LINE_INSUF_RESERVE = new TransactionEngineResult(126, "No such line. Too little reserve to create it.");
        public static readonly TransactionEngineResult tecNO_LINE_REDUNDANT = new TransactionEngineResult(127, "Can't set non-existant line to default.");
        public static readonly TransactionEngineResult tecPATH_DRY = new TransactionEngineResult(128, "Path could not send partial amount.");
        public static readonly TransactionEngineResult tecUNFUNDED = new TransactionEngineResult(129, "One of _ADD, _OFFER, or _SEND. Deprecated.");
        public static readonly TransactionEngineResult tecMASTER_DISABLED = new TransactionEngineResult(130, "tecMASTER_DISABLED");
        public static readonly TransactionEngineResult tecNO_REGULAR_KEY = new TransactionEngineResult(131, "tecNO_REGULAR_KEY");
        public static readonly TransactionEngineResult tecOWNERS = new TransactionEngineResult(132, "tecOWNERS");

        public static IEnumerable<TransactionEngineResult> Values
        {
            get
            {
                yield return telLOCAL_ERROR;
                yield return telBAD_DOMAIN;
                yield return telBAD_PATH_COUNT;
                yield return telBAD_PUBLIC_KEY;
                yield return telFAILED_PROCESSING;
                yield return telINSUF_FEE_P;
                yield return telNO_DST_PARTIAL;
                yield return temMALFORMED;
                yield return temBAD_AMOUNT;
                yield return temBAD_AUTH_MASTER;
                yield return temBAD_CURRENCY;
                yield return temBAD_FEE;
                yield return temBAD_EXPIRATION;
                yield return temBAD_ISSUER;
                yield return temBAD_LIMIT;
                yield return temBAD_OFFER;
                yield return temBAD_PATH;
                yield return temBAD_PATH_LOOP;
                yield return temBAD_PUBLISH;
                yield return temBAD_TRANSFER_RATE;
                yield return temBAD_SEND_XRP_LIMIT;
                yield return temBAD_SEND_XRP_MAX;
                yield return temBAD_SEND_XRP_NO_DIRECT;
                yield return temBAD_SEND_XRP_PARTIAL;
                yield return temBAD_SEND_XRP_PATHS;
                yield return temBAD_SIGNATURE;
                yield return temBAD_SRC_ACCOUNT;
                yield return temBAD_SEQUENCE;
                yield return temDST_IS_SRC;
                yield return temDST_NEEDED;
                yield return temINVALID;
                yield return temINVALID_FLAG;
                yield return temREDUNDANT;
                yield return temREDUNDANT_SEND_MAX;
                yield return temRIPPLE_EMPTY;
                yield return temUNCERTAIN;
                yield return temUNKNOWN;
                yield return tefFAILURE;
                yield return tefALREADY;
                yield return tefBAD_ADD_AUTH;
                yield return tefBAD_AUTH;
                yield return tefBAD_CLAIM_ID;
                yield return tefBAD_GEN_AUTH;
                yield return tefBAD_LEDGER;
                yield return tefCLAIMED;
                yield return tefCREATED;
                yield return tefDST_TAG_NEEDED;
                yield return tefEXCEPTION;
                yield return tefGEN_IN_USE;
                yield return tefINTERNAL;
                yield return tefNO_AUTH_REQUIRED;
                yield return tefPAST_SEQ;
                yield return tefWRONG_PRIOR;
                yield return tefMASTER_DISABLED;
                yield return tefMAX_LEDGER;
                yield return terRETRY;
                yield return terFUNDS_SPENT;
                yield return terINSUF_FEE_B;
                yield return terNO_ACCOUNT;
                yield return terNO_AUTH;
                yield return terNO_LINE;
                yield return terOWNERS;
                yield return terPRE_SEQ;
                yield return terLAST;
                yield return tesSUCCESS;
                yield return tecCLAIM;
                yield return tecPATH_PARTIAL;
                yield return tecUNFUNDED_ADD;
                yield return tecUNFUNDED_OFFER;
                yield return tecUNFUNDED_PAYMENT;
                yield return tecFAILED_PROCESSING;
                yield return tecDIR_FULL;
                yield return tecINSUF_RESERVE_LINE;
                yield return tecINSUF_RESERVE_OFFER;
                yield return tecNO_DST;
                yield return tecNO_DST_INSUF_XRP;
                yield return tecNO_LINE_INSUF_RESERVE;
                yield return tecNO_LINE_REDUNDANT;
                yield return tecPATH_DRY;
                yield return tecUNFUNDED;
                yield return tecMASTER_DISABLED;
                yield return tecNO_REGULAR_KEY;
                yield return tecOWNERS;
            }
        }

        public int AsInteger
        {
            get { return _ord; }
        }

        public static TransactionEngineResult FromNumber(Number i)
        {
            return ByCode[i.IntValue()];
        }

        public static TransactionEngineResult FromNumber(int i)
        {
            return ByCode[i];
        }

        public static TransactionEngineResult FromString(string s)
        {
            return ByName[s];
        }

        public string GetName()
        {
            return ByName.Single(q => q.Value == this).Key;
        }

        public Class ResultClass()
        {
            return Class.ForResult(this);
        }

        public class Class
        {
            private readonly int _starts;

            private Class(int i)
            {
                _starts = i;
            }

            public static Class telLOCAL_ERROR = new Class(-399);
            public static Class temMALFORMED = new Class(-299);
            public static Class tefFAILURE = new Class(-199);
            public static Class terRETRY = new Class(-99);
            public static Class tesSUCCESS = new Class(0);
            public static Class tecCLAIMED = new Class(100);

            public static Class ForResult(TransactionEngineResult result)
            {
                if (result._ord >= telLOCAL_ERROR._starts && result._ord < temMALFORMED._starts)
                {
                    return telLOCAL_ERROR;
                }

                if (result._ord >= temMALFORMED._starts && result._ord < tefFAILURE._starts)
                {
                    return telLOCAL_ERROR;
                }

                if (result._ord >= tefFAILURE._starts && result._ord < terRETRY._starts)
                {
                    return telLOCAL_ERROR;
                }

                if (result._ord >= terRETRY._starts && result._ord < tesSUCCESS._starts)
                {
                    return telLOCAL_ERROR;
                }

                if (result._ord >= tesSUCCESS._starts && result._ord < tecCLAIMED._starts)
                {
                    return tesSUCCESS;
                }

                return tecCLAIMED;
            }
        }
    }
}
