using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.CONFIG
{
    public class ConstConfig
    {

        /// <summary>
        /// 签名密钥
        /// </summary>
        public const string SECRET_KEY = "BAMEENG20161021";


#if DEBUG
        /// <summary>
        /// 商户编号(对应商城的编号)
        /// </summary>
        public const int storeId = 296;
#else
        public const int storeId = 88888;
#endif
        //public const string Bearer = "Bearer ";

    }
}
