using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHelper.Db.QueryObject
{
    public abstract class WhereQuery
    {

        private ConnectType cType;
        protected bool isFirst;
        protected int paraNo;
        protected WhereClause clause;


        public WhereQuery(ConnectType cType) {
            this.cType = cType;

            isFirst = true;
            paraNo = 0;
            clause = new WhereClause();
        }



        public virtual void Add(WhereQuery critertia)
        {
            throw new NotImplementedException();
        }

        internal virtual WhereClause GeneralClauseBy(bool isFirst, int paraNo)
        {
            throw new NotImplementedException();
        }

        public WhereClause GeneralClause() {
            return GeneralClauseBy(isFirst, paraNo);
        }




        protected string GetAndOr()
        {
            String connectStr = "";
            switch (cType)
            {
                case ConnectType.And:
                    connectStr = "AND ";
                    break;
                case ConnectType.Or:
                    connectStr = "OR ";
                    break;
            }
            return connectStr;
        }



        protected void Init_IsFirst_ParaNo(bool isFirst, int paraNo)
        {
            this.isFirst = isFirst;
            this.paraNo = paraNo;            
        }


        public virtual ICollection<WhereQuery> GetSubQueries() {
            throw new NotImplementedException();
        }

    }
}
