using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    public class TransferEventArgs:EventArgs
    {
        #region Свойства
        /// <summary>
        /// Дата и время операции
        /// </summary>
        public DateTime Dt { get; private set; }

        /// <summary>
        /// Имя клиента
        /// </summary>
        public string ClientName { get; private set; }

        /// <summary>
        /// Id клиента
        /// </summary>
        public int ClientId { get; private set; }

        /// <summary>
        /// Id счета отправителя
        /// </summary>
        public int BillIdFrom { get; private set; }


        /// <summary>
        /// Id счета получателя
        /// </summary>
        public int BillIdTo { get; private set; }


        /// <summary>
        /// Сумма перевода
        /// </summary>
        public decimal Sum { get; private set; }
        #endregion


        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Dt">Дата и время операции</param>
        /// <param name="ClientId">Id клиента</param>
        /// <param name="ClientName">Имя клиента</param>
        /// <param name="BillIdFrom">Id счета отправителя</param>
        /// <param name="BillIdTo">Id счета получателя</param>
        /// <param name="Sum">Сумма перевода</param>
        public TransferEventArgs(DateTime Dt, int ClientId, string ClientName, int BillIdFrom, int BillIdTo, decimal Sum)
        {
            this.Dt = Dt;
            this.ClientName = ClientName;
            this.ClientId = ClientId;
            this.BillIdFrom = BillIdFrom;
            this.BillIdTo = BillIdTo;
            this.Sum = Sum;
        }
        #endregion

    }
}
