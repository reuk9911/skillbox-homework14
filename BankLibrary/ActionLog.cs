using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Homework14
{
    public class ActionLog:IEnumerable<string>
    {
        #region Поля и свойства
        /// <summary>
        /// Лист с логами транзакций
        /// </summary>
        public ObservableCollection<string> Logs { get; private set; }
        #endregion

        #region Конструкторы
        public ActionLog()
        {
            this.Logs = new ObservableCollection<string>();
        }

        #endregion

        #region Методы
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Args"></param>
        public void OnOpenCloseBill(Object Sender, BillOpenCloseEventArgs Args)
        {
            if (Args.Type == OperationTypeEnum.Open)
                Logs.Add($"{Args.Dt.ToShortTimeString()} Клиент {((Client)Sender).Name} открыл счет {Args.BillId}");
            else
                Logs.Add($"{Args.Dt.ToShortTimeString()} Клиент {((Client)Sender).Name} закрыл счет {Args.BillId}");

        }

        public void OnBillDeposit(Object Sender, BillDepositEventArgs Args)
        {
            Logs.Add($"{Args.Dt.ToShortTimeString()} Клиент {((Client)Sender).Name} пополнил счет " +
                $"{Args.BillId} на {Args.Sum}");
        }

        public void OnTransfer(Object Sender, TransferEventArgs Args)
        {
            Logs.Add($"{Args.Dt.ToShortTimeString()} Клиент {((Client)Sender).Name} " +
                $"перевел {Args.Sum} на счет {Args.BillIdTo}");
        }

        public void OnRefillByTransfer(object Sender, RefillByTransferEventArgs Args)
        {
            Logs.Add($"{Args.Dt.ToShortTimeString()} Поступление {Args.Sum} на счет {Args.RecieverBillId} " +
                $" от клиента { Args.SenderName }");
        }

        public bool DeserializeJson(string LogPath)
        {
            if  (!File.Exists(LogPath))
            {
                return false;
            }
            string json = File.ReadAllText(LogPath);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            Logs = JsonConvert.DeserializeObject<ObservableCollection<string>>(json, serializerSettings);
            return true;
        }

        public void SerializeJson(string LogPath)
        {

            JsonSerializerSettings serializeSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string json = JsonConvert.SerializeObject(Logs, Formatting.Indented, serializeSettings);
            File.WriteAllText(LogPath, json);

        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var e in Logs)
            {
                yield return e;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<string>)Logs).GetEnumerator();
        }

        #endregion


    }
}
