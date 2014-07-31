using System;
using System.Collections.Generic;
using Ripple.Core.Core.Fields;

namespace Ripple.Core.Core.Formats
{
    public class Format
    {
        private readonly IDictionary<Field, Requirement> _requirementEnumMap = new Dictionary<Field, Requirement>();

        public Format(Object[] args)
        {
            if ((args.Length % 2 != 0) || args.Length < 2)
            {
                throw new InvalidOperationException("Varargs length should be a minimum multiple of 2.");
            }

            for (int i = 0; i < args.Length; i += 2)
            {
                var f = (Field)args[i];
                var r = (Requirement)args[i + 1];
                Put(f, r);
            }
        }

        public virtual void AddCommonFields()
        {
        }

        protected void Put(Field f, Requirement r)
        {
            _requirementEnumMap.Add(f, r);
        }

        public IDictionary<Field, Requirement> RequirementEnumMap
        {
            get
            {
                return this._requirementEnumMap;
            }
        }
    }
}
