using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Homework14
{
    
    public class ClientRepository<T> : IEnumerable<T> where T : Client, new()
    {
        #region Свойства
        public ObservableCollection<T> ClientList { get; private set; }

        public ActionLog Log { get; private set; }
        #endregion

        #region Конструкторы
        public ClientRepository()
        {
            this.ClientList = new ObservableCollection<T>();
            Log = new ActionLog();
        }
        #endregion

        #region Методы


        /// <summary>
        /// Сериализация Клиентов и Лога
        /// </summary>
        /// <param name="ClientsFile">Файл с клиентами</param>
        /// <param name="LogFile">Файла с логами</param>
        public void SerializeJson(string ClientsFile, string LogFile)
        {

            JsonSerializerSettings serializeSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string json = JsonConvert.SerializeObject(ClientList, Formatting.Indented, serializeSettings);
            File.WriteAllText("Clients.txt", json);

            Log.SerializeJson(LogFile);

        }


        /// <summary>
        /// Десериализация Клиентов и Лога
        /// </summary>
        /// <param name="ClientsFile">Файл с клиентами</param>
        /// <param name="LogFile">Файла с логами</param>
        /// <returns>Возвращает результат выполнения десериализации - true или  false</returns>
        public bool DeserializeJson(string ClientsFile, string LogFile)
        {
            if (!File.Exists(ClientsFile) || (!File.Exists(LogFile)))
            {
                return false;
            }
            string json = File.ReadAllText(ClientsFile);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            ClientList = JsonConvert.DeserializeObject<ObservableCollection<T>>(json, serializerSettings);

            Log.DeserializeJson(LogFile);

            
            for (int i=0; i<ClientList.Count; i++)
            {
                // восстанавливаем подписки 
                ClientList[i].OpenCloseBillEvent += Log.OnOpenCloseBill;
                ClientList[i].BillDepositEvent += Log.OnBillDeposit;
                ClientList[i].TransferEvent += Log.OnTransfer;

                for (int j=0; j < ClientList[i].Bills.Count; j++)
                {
                    ClientList[i].Bills[j].RefillByTransferEvent += ClientList[i].OnRefillBillByTransfer;
                }
            }

            return true;
        }


        /// <summary>
        /// возвращает счет по id
        /// </summary>
        /// <param name="id">id счета</param>
        /// <returns>счет</returns>
        public Bill FindBillById(int id)
        {
            Bill bill = null;

            foreach (Client c in ClientList)
            {
                foreach (Bill b in c.Bills)
                {
                    if (b.Id == id)
                    {
                        bill = b;
                        break;
                    }
                }
            }
            return bill;
        }


        /// <summary>
        /// Добавляет уже созданного клиента в репозиторий
        /// </summary>
        /// <param name="t">Клиент</param>
        public void AddClient(T t) 
        {
            ClientList.Add(t);
            ClientList[ClientList.Count - 1].OpenCloseBillEvent += Log.OnOpenCloseBill;
            ClientList[ClientList.Count - 1].BillDepositEvent += Log.OnBillDeposit;
            ClientList[ClientList.Count - 1].TransferEvent += Log.OnTransfer;
        }

        /// <summary>
        /// Создает и добавляет нового клиента
        /// </summary>
        /// <param name="Name">Имя клиента</param>
        /// <param name="clientType">Тип клиента</param>
        public void AddNewClient(string Name, EClientType clientType)
        {
            switch (clientType)
            {
                case EClientType.Client:
                    T newClient = new T();
                    ClientList.Add(newClient);
                    newClient.Name = Name;
                    break;

                case EClientType.VipClient:
                    VipClient newVipClient = new VipClient();
                    newVipClient.Name = Name;
                    ClientList.Add((T)(object)newVipClient);
                    break;

                case EClientType.LegalPerson:
                    LegalPerson newLegalPerson = new LegalPerson();
                    newLegalPerson.Name = Name;
                    ClientList.Add((T)(object)newLegalPerson);
                    break;

                default: break;
            }
            ClientList[ClientList.Count - 1].OpenCloseBillEvent += Log.OnOpenCloseBill;
            ClientList[ClientList.Count - 1].BillDepositEvent += Log.OnBillDeposit;
            ClientList[ClientList.Count - 1].TransferEvent += Log.OnTransfer;
            ClientList[ClientList.Count - 1].RefillByTransferEvent += Log.OnRefillByTransfer;




        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var e in ClientList)
            {
                yield return e;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)ClientList).GetEnumerator();
        }
        #endregion
    }
}
