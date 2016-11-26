using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hjp.Fair_Die
{
    public class Throws
    {
        List<int> TheThrows = new List<int>();
        public int max { get; set; }
        int[] _Histogram = null;

        public int[] Histogram
        {
            get
            {
                if (_Histogram == null)
                {
                    _Histogram = new int[max + 1];
                    foreach (int t in TheThrows)
                    {
                        _Histogram[t]++;
                    }
                }
                return _Histogram;
            }
        }

        RankedValue[] _RankedValues = null;
        public RankedValue[] RankedValues
        {
            get
            {
                if (_RankedValues == null)
                {
                    _RankedValues = new RankedValue[Histogram.Length - 1];

                    for (int i = 1; i < Histogram.Length; i++)
                    {
                        _RankedValues[i - 1] = new RankedValue() { Value = i, Count = Histogram[i] };
                    }
                    Array.Sort(_RankedValues);

                    for (int i = 0; i < _RankedValues.Length; i++)
                    {
                        _RankedValues[i].Rank = i;
                    }
                }
                return _RankedValues;
            }
        }

        public void Add(int t)
        {
            TheThrows.Add(t);
            if (t > max) max = t;

            // invalidate cached stats
            _Histogram = null;
            _RankedValues = null;
        }

        public Random Rng { get; set; }
        public void AddRandom()
        {
            if (Rng == null)
            {
                Rng = new Random();
            }
            Add(Rng.Next(1, max + 1));
        }
        public int Count
        {
            get
            {
                return TheThrows.Count();
            }
        }
    }

    public class RankedValue
        : IComparable<RankedValue>
    {
        public int Rank;
        public int Value;
        public int Count;

        public int CompareTo(RankedValue other)
        {
            if (this.Count > other.Count)
            {
                return -1;
            }
            else if (this.Count < other.Count)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
    }
}
