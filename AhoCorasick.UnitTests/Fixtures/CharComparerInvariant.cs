using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AhoCorasick.UnitTests.Fixtures
{
    public class CharComparerInvariant : IEqualityComparer<char>
    {
        public bool Equals(char x, char y)
        {
            return char.ToLowerInvariant(x).Equals(char.ToLowerInvariant(y));
        }

        public int GetHashCode(char obj)
        {
            return obj.GetHashCode();
        }
    }
}