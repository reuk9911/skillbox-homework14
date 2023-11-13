using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    public class Bill : IDisposable, INotifyPropertyChanged
    {
        #region Поля и Свойства

        /// <summary>
        /// Id клиента
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Баланс счета
        /// </summary>
        public decimal Balance/* { get; private set; }*/
        {
            get => this.balance;
            set
            {
                if (balance != value)
                {
                    balance = value;
                    OnPropertyChanged("Balance");
                }
            }
        }

        /// <summary>
        /// Тип счета
        /// </summary>
        public virtual string BillType { get; }

        /// <summary>
        /// Статический генератор Id
        /// </summary>
        private static IdGenerator IdGen;

        /// <summary>
        /// Баланс счета
        /// </summary>
        private decimal balance;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static Bill()
        {
            IdGen = new IdGenerator("Bill");
        }


        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Bill()
        {
            this.BillType = "Bill";
            this.balance = 0.0m;
            this.Id = IdGen.GetNewId();
        }
        #endregion

        #region События

        public delegate void RefillByTransferDelegate(Object Sender, RefillByTransferEventArgs Args);

        /// <summary>
        /// Происходит, когда на счет переводят деньги
        /// </summary>
        public event RefillByTransferDelegate RefillByTransferEvent;

        #endregion


        #region Методы

        /// <summary>
        /// пополнение владельцем
        /// </summary>
        /// <param name="sum">Сумма</param>
        public void Deposit(decimal sum)
        {
            if (sum <= 0)
                throw new NegativeSumException();
            Balance += sum;
        }

        /// <summary>
        /// Перевод на другой счет 
        /// </summary>
        /// <param name="sum"></param>
        public void Transfer(decimal sum)
        {
            if (sum <= 0)
                throw new NegativeSumException();
            Balance -= sum;
        }

        /// <summary>
        /// Переводит средства от другого клиента
        /// </summary>
        /// <param name="FromClient">Клиент отправитель</param>
        /// <param name="sum">Сумма</param>
        public void RefillByTransfer(Client FromClient, decimal sum)
        {
            Balance += sum;
            RefillByTransferEvent?.Invoke(this,
                new RefillByTransferEventArgs(DateTime.Now, FromClient.Name, FromClient.Id, this.Id, sum));

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        public void Dispose()
        {
            IdGen.UsedIds.Remove(this.Id);
        }
        #endregion
    }
}
