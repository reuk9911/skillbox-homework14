using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    public class LegalPerson : Client
    {
        #region Поля и свойства
        
        /// <summary>
        /// Тип клиента
        /// </summary>
        public override string ClientType { get; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public LegalPerson() : base()
        {
            this.ClientType = "Юридическое лицо";
        }
        #endregion
    }
}
