using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    public class NegativeSumException:Exception
    {
        public override string Message => base.Message;
        public NegativeSumException():base("Сумма не может быть <= 0") { }
    }
}
