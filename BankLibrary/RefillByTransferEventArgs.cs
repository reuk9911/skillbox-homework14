using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{


    public class RefillByTransferEventArgs : EventArgs
    {
        #region  Свойства
        /// <summary>
        /// сумма перевода
        /// </summary>
        public decimal Sum { get; private set; }

        /// <summary>
        /// Дата и время операции
        /// </summary>
        public DateTime Dt { get; private set; }

        /// <summary>
        /// Имя клиента отправителя
        /// </summary>
        public string SenderName { get; private set; }

        /// <summary>
        /// Id клиента отправителя
        /// </summary>
        public int SenderId { get; private set; }

        public int RecieverBillId { get; private set; }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Dt">Дата и время операции</param>
        /// <param name="Sum">сумма перевода</param>
        public RefillByTransferEventArgs(DateTime Dt, string SenderName, int SenderId, int RecieverBillId, decimal Sum)
        {
            this.SenderName = SenderName;
            this.SenderId = SenderId;
            this.RecieverBillId = RecieverBillId;
            this.Sum = Sum;
            this.Dt = Dt;
        }
        #endregion


    }
}
