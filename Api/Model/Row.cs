using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Api.Model
{
    public class Row
    {
        public Key Key;

        public string Col1 { get; set; }
    }


    public class Key
    {
    
        //public string Colname { get; set; }
        public string Value { get; set; }
    }


    public class KeyComparer : IEqualityComparer<Key>
    {
        public bool Equals([AllowNull] Key x, [AllowNull] Key y)
        {
            // this method is never called
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] Key key)
        {
            var ret = Convert.ToInt32(key.Value);
            return ret;
        }
    }
}
