using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHelper.Db.QueryObject
{
    public class WhereCriteria : WhereQuery
    {
        private string fieldName;
        private CriType criType;
        private object value;
        private string sqlStr;

        public WhereCriteria(ConnectType cType, string fieldName, CriType criType, object value): base(cType)
        {
            this.fieldName = fieldName;
            this.criType = criType;
            this.value = value;

            this.sqlStr = "";
        }

        internal override WhereClause GeneralClauseBy(bool isFirst, int paraNo)
        {
            Init_IsFirst_ParaNo(isFirst, paraNo);
            SetSqlHead(isFirst);
            SetSqlTotal(paraNo);
            SetClause();
            return clause;
        }


        private void SetClause()
        {
            clause.SqlStr = sqlStr;
            clause.paras.Add(value);
        }


        private void SetSqlTotal(int paraNo)
        {
            switch (criType)
            {
                case CriType.Equal:
                    sqlStr += fieldName + " = @" + paraNo + " ";
                    break;
                case CriType.Contains:
                    sqlStr += fieldName + ".Contains(@" + paraNo + ") ";
                    break;
            }
        }


        private string SetSqlHead(bool isFirst)
        {
            if (!isFirst)
            {
                sqlStr = GetAndOr();
            }
            return sqlStr;
        }

    }
}
