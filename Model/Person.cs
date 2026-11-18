
using System;

namespace LR1_3kyrs_NOVIKOVA_BAI24.Model
{
    public class Person
    {
        public int Id { get; set; }      // код
        public int RoleId { get; set; }    // код должности
        public string FirstName { get; set; } // имя
        public string LastName { get; set; } // фамилия
        public virtual Role Role { get; set; }
        public DateTime Birthday { get; set; } // дата рождения
    }
}
