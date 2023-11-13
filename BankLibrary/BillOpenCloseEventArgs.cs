using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    

    public class BillOpenCloseEventArgs : EventArgs
    {
        #region  Свойства
        /// <summary>
        /// Тип операции открытие/закрытие счета
        /// </summary>
        public OperationTypeEnum Type { get; private set; }

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
        /// <param name="Type">Тип операции открытие/закрытие счета</param>
        /// <param name="Dt">Дата и время операции</param>
        /// <param name="BillId">Id счета</param>
        public BillOpenCloseEventArgs(OperationTypeEnum Type, DateTime Dt, int BillId)
        {
            this.Type = Type;
            this.Dt = Dt;
            this.BillId = BillId;
        }
        #endregion


    }
}
