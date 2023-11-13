using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{


    public class BillDepositEventArgs : EventArgs
    {
        #region  Свойства
        /// <summary>
        /// сумма пополнения счета
        /// </summary>
        public decimal Sum { get; private set; }

        /// <summary>
        /// Дата и время операции
        /// </summary>
        public DateTime Dt { get; private set; }

        /// <summary>
        /// Id счета
        /// </summary>
        public int BillId { get; private set; }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BillId">Id пополняемого счета</param>
        /// <param name="Dt">Дата и время операции</param>
        /// <param name="Sum">сумма пополнения счета</param>
        public BillDepositEventArgs(int BillId, DateTime Dt, decimal Sum)
        {
            this.Sum = Sum;
            this.Dt = Dt;
            this.BillId = BillId;
        }
        #endregion


    }
}
