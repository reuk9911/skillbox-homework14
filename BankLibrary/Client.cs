using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
//using System.Windows.Documents;
//using System.Windows.Media.TextFormatting;

namespace Homework14
{


    public class Client : IDisposable, /*IDeposit<Bill>,*/ ITransfer<Bill>
    {
        #region Поля и Свойства
        /// <summary>
        /// Id клиента
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Коллекция счетов клиента
        /// </summary>
        public ObservableCollection<Bill> Bills { get; }

        /// <summary>
        /// Сообщения из методов для подписчиков
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Имя Клиента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип Клиента
        /// </summary>
        public virtual string ClientType { get; }

        /// <summary>
        /// Генератор Id
        /// </summary>
        private static IdGenerator IdGen;
        #endregion

        #region Конструкторы

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static Client()
        {
            IdGen = new IdGenerator("Client");
        }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Client()
        {
            this.ClientType = "Физическое лицо";
            this.Message = "";
            this.Bills = new ObservableCollection<Bill>();
            this.Id = IdGen.GetNewId();
            this.Name = "";

        }

        #endregion

        #region События

        public delegate void OpenCloseDelegate(object Sender, BillOpenCloseEventArgs Args);

        /// <summary>
        /// Происходит, когда клиент открывает и закрывает счет
        /// </summary>
        public event OpenCloseDelegate OpenCloseBillEvent;

        public delegate void BillDepositDelegate(object Sender, BillDepositEventArgs Args);

        /// <summary>
        /// Происходит при пополнении счета клиентом
        /// </summary>
        public event BillDepositDelegate BillDepositEvent;

        public delegate void RefillByTransferDelegate(object Sender, RefillByTransferEventArgs Args);

        /// <summary>
        /// Происходит, когда на счет переводят деньги
        /// </summary>
        public event RefillByTransferDelegate RefillByTransferEvent;

        public delegate void TransferDelegate(object Sender, TransferEventArgs Args);
        public event TransferDelegate TransferEvent;

        #endregion

        #region Методы
        public void OnRefillBillByTransfer(Object Sender, RefillByTransferEventArgs Args)
        {
            RefillByTransferEvent?.Invoke(this, Args);
        }

        /// <summary>
        /// Открывает новый счет заданного типа
        /// </summary>
        /// <param name="bType">Тип счета</param>
        public void OpenBill(EBillType bType)
        {
            if (bType == EBillType.DepositBill)
                Bills.Add(new DepositBill() as Bill);
            else
            if (bType == EBillType.NonDepositBill)
                Bills.Add(new NonDepositBill() as Bill);
            else return;

            OpenCloseBillEvent?.Invoke(this,
                new BillOpenCloseEventArgs(OperationTypeEnum.Open, DateTime.Now, Bills[Bills.Count - 1].Id));
            Bills[Bills.Count - 1].RefillByTransferEvent += OnRefillBillByTransfer;
        }

        /// <summary>
        /// Закрывает счет
        /// </summary>
        /// <param name="BillId">Id счета</param>
        /// <returns>true, если счет закрылся, иначе false</returns>
        public bool CloseBill(int BillId)
        {
            int index = FindBillIndex(BillId);
            if (index == -1)
            {
                Message = "Счет с указанным Id не найден";
                return false;
            }
            if (Bills[index].Balance < 0.0m)
            {
                Message = "Нельзя закрыть счет с отрицательным балансом";
                return false;
            }
            else
            {
                OpenCloseBillEvent?.Invoke(this,
                    new BillOpenCloseEventArgs(OperationTypeEnum.Close, DateTime.Now, Bills[Bills.Count - 1].Id));
                Bills[index].RefillByTransferEvent -= OnRefillBillByTransfer;
                Bills[index].Dispose();
                Bills.RemoveAt(index);
                Message = "Счет удален";
                return true;
            }

        }


        /// <summary>
        /// Вносит средства на счет
        /// </summary>
        /// <param name="BillId">Id счета</param>
        /// <param name="Sum">Сумма</param>
        /// <returns>Возвращает счет, на который вносятся деньги</returns>
        public /*Bill*/ bool Deposit(int BillId, decimal Sum)
        {
            if (Bills == null) return false;
            int k = -1;
            for (int i = 0; i < Bills.Count; i++)
            {
                if (Bills[i].Id == BillId)
                {
                    k = i;
                    break;
                }
            }
            if (k == -1) return /*null*/false;
            else
            {
                try
                {
                    Bills[k].Deposit(Sum);
                }
                catch (NegativeSumException e)
                {
                    this.Message = e.Message;
                    return /*null*/false;
                }
                BillDepositEvent?.Invoke(this, new BillDepositEventArgs(Bills[k].Id, DateTime.Now, Sum));
                //if (Bills[k] is DepositBill) return (DepositBill)Bills[k];
                //if (Bills[k] is NonDepositBill) return (NonDepositBill)Bills[k];
                //return Bills[k];
                return true;
            }

        }

        /// <summary>
        /// Переводит средства
        /// </summary>
        /// <param name="BillFrom">Счет, с котрорго отправляются деньги</param>
        /// <param name="BillTo">Счет зачисления</param>
        /// <param name="sum">Сумма</param>
        /// <returns>результат выполнения переавода</returns>
        public bool Transfer(Bill BillFrom, Bill BillTo, decimal sum)
        {
            if (BillFrom.Balance < sum)
            {
                this.Message = "Перевод не прошел! Не достаточно средств!";
                return false;
            }
            try
            {
                BillFrom.Transfer(sum);
            }
            catch (NegativeSumException e)
            {
                this.Message = e.Message;
                return false;
            }
            TransferEvent?.Invoke(this, new TransferEventArgs(DateTime.Now, Id, Name, BillFrom.Id, BillTo.Id, sum));

            BillTo.RefillByTransfer(this, sum);

            this.Message = "Перевод прошел успешно!";
            return true;
        }

        /// <summary>
        /// Ищет индекс счета в Bills по Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private int FindBillIndex(int Id)
        {
            for (int i = 0; i < Bills.Count; i++)
            {
                if (Bills[i].Id == Id)
                {
                    return i;
                }
            }
            return -1;
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
