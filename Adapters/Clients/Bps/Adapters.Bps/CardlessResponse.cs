using System.Collections.Generic;
using Common.Extensions;

namespace Adapters.Bps
{
    public class CardlessResponse : IResponse<CardlessRequest>
    {
        public long date { get; set; }
        public long prevDate { get; set; }
        public object nextDate { get; set; }
        public Rates rates { get; set; }
        public int typeId { get; set; }
    }

    public class Rates
    {
        public List<List> list { get; set; }
    }

    public class List
    {
        public string rateType { get; set; }
        public string iso { get; set; }
        public object iso2 { get; set; }
        public float buy { get; set; }
        public float deltaBuy { get; set; }
        public float sale { get; set; }
        public float deltaSale { get; set; }
        public object minsum { get; set; }
        public object maxsum { get; set; }
        public int? scale { get; set; }
        public object rate { get; set; }
        public object deltaRate { get; set; }
    }
}
