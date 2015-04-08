using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHelper.Db.QueryObject
{
    public class WhereQueryCom: WhereQuery
    {
        private ICollection<WhereQuery> queries;
        private string head = "";
        private string tail = "";

        public WhereQueryCom(ConnectType cType)
            : base(cType)
        {
            queries = new List<WhereQuery>();
        }
        

        public override void Add(WhereQuery critertia)
        {
            queries.Add(critertia);
        }


        internal override WhereClause GeneralClauseBy(bool isFirst, int paraNo)
        {
            Init_IsFirst_ParaNo(isFirst, paraNo);
            SetHeadAndTail(isFirst);
            return GetClause();
        }




        private WhereClause GetClause()
        {
            foreach (WhereQuery query in queries)
            {
                SetClause(query);
            }
            SetHeadTailToClause();
            return clause;
        }


        private void SetHeadTailToClause()
        {
            clause.SqlStr = head + clause.SqlStr + tail;
        }



        private void SetClause(WhereQuery query)
        {
            WhereClause clauseFromSub = query.GeneralClauseBy(isFirst, paraNo);
            clause.SqlStr = clause.SqlStr + clauseFromSub.SqlStr;
            clause.paras.AddRange(clauseFromSub.paras);
            SetParaNo(clauseFromSub.paras.Count);
            SetIsFirstToFalse();
        }



        private void SetParaNo(int parasToAdd)
        {
            paraNo += parasToAdd;
        }


        private void SetIsFirstToFalse()
        {
            if (isFirst)
            {
                isFirst = false;
            }
        }



        private void SetHeadAndTail(bool isFirst)
        {
            if (queries.Count > 0 && !isFirst)
            {
                head = GetAndOr() + "(";
                tail += ")";
            }
        }




        public override ICollection<WhereQuery> GetSubQueries()
        {
            return queries;
        }

    }
}
