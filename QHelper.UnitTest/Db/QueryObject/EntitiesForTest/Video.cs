using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHelper.UnitTest.Db.QueryObject.EntitiesForTest
{
    public class Video
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Point { get; set; }

        public long WatchedTimes { get; set; }

        public float length { get; set; }

        public int? lendTimes { get; set; }

        public DateTime? SaledTime { get; set; }
    }
}
