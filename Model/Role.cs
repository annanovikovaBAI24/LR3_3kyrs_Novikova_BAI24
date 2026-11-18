using System.Collections.Generic;

namespace LR1_3kyrs_NOVIKOVA_BAI24.Model
{
    /// <summary>
    /// класс должность сотрудника
    /// </summary>
    public class Role
    {
        /// <summary>
        /// код должности
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// наименование должности
        /// </summary>
        public string NameRole { get; set; }

        public Role()
        {
            this.Persons = new HashSet<Person>();
        }

        /// <summary>
        /// коллекция Persons для связи с классом Person
        /// </summary>
        public virtual ICollection<Person> Persons { get; set; }
    }
}
