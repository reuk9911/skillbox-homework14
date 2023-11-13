using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    public class NonDepositBill : DepositBill
    {
        public override string BillType { get; }

        public NonDepositBill() : base()
        {
            this.BillType = "Не депозитный счет";
        }
    }
}
