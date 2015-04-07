using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHelper.Db.QueryObject
{
    public class WhereClause
    {
        public WhereClause() {
            paras = new List<object>();
        }


        public string sqlStr { get; set; }

        public List<object> paras { get; set; }
    }
}
