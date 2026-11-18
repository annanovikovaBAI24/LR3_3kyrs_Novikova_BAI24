
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using LR1_3kyrs_NOVIKOVA_BAI24.Helper;
using LR1_3kyrs_NOVIKOVA_BAI24.Model;
using LR1_3kyrs_NOVIKOVA_BAI24.View;

namespace LR1_3kyrs_NOVIKOVA_BAI24.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Role> ListRole { get; set; } =
            new ObservableCollection<Role>();

        private Role selectedRole;

        // Свойство для хранения выбранной в таблице должности
        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged("SelectedRole");
                EditRole.CanExecute(true);
            }
        }

        public string Error { get; set; }

        public RoleViewModel()
        {
            ListRole = GetRoles(); // Загружаем данные при старте
        }

        // Метод загрузки данных по должностям из базы данных
        public ObservableCollection<Role> GetRoles()
        {
            using (var context = new CompanyEntities())
            {
                var roles = context.Roles.ToList();
                return new ObservableCollection<Role>(roles);
            }
        }

        // Обновление коллекции должностей
        private void RefreshRoles()
        {
            ListRole.Clear();

            foreach (Role role in GetRoles())
            {
                ListRole.Add(role);
            }
        }

        #region command AddRole

        /// команда добавления новой должности
        private RelayCommand _addRole;

        public RelayCommand AddRole
        {
            get
            {
                return _addRole ??
                (_addRole = new RelayCommand(obj =>
                {
                    Role newRole = new Role();

                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Новая должность",
                        DataContext = newRole
                    };

                    wnRole.ShowDialog();

                    if (wnRole.DialogResult == true)
                    {
                        using (var context = new CompanyEntities())
                        {
                            try
                            {
                                context.Roles.Add(newRole);
                                context.SaveChanges();

                                RefreshRoles();

                                SelectedRole = ListRole.FirstOrDefault(
                                    r => r.Id == newRole.Id);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(
                                    "\nОшибка добавления данных!\n" + ex.Message,
                                    "Предупреждение");
                            }
                        }
                    }
                }));
            }
        }

        #endregion

        #region command EditRole

        /// команда редактирования должности
        private RelayCommand _editRole;

        public RelayCommand EditRole
        {
            get
            {
                return _editRole ??
                (_editRole = new RelayCommand(obj =>
                {
                    Role selected = SelectedRole;

                    if (selected == null)
                        return;

                    // Создаем отдельный экземпляр для редактирования
                    Role editRole = new Role
                    {
                        Id = selected.Id,
                        NameRole = selected.NameRole
                    };

                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Редактирование должности",
                        DataContext = editRole
                    };

                    wnRole.ShowDialog();

                    if (wnRole.DialogResult == true)
                    {
                        using (var context = new CompanyEntities())
                        {
                            Role role = context.Roles.Find(editRole.Id);

                            if (role != null)
                            {
                                if (role.NameRole != editRole.NameRole)
                                    role.NameRole = editRole.NameRole.Trim();

                                try
                                {
                                    context.SaveChanges();
                                    RefreshRoles();

                                    SelectedRole = ListRole.FirstOrDefault(
                                        r => r.Id == editRole.Id);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(
                                        "\nОшибка редактирования данных!\n" +
                                        ex.Message,
                                        "Предупреждение");
                                }
                            }
                        }
                    }
                },
                (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        #endregion

        #region command DeleteRole

        /// команда удаления должности
        private RelayCommand _deleteRole;

        public RelayCommand DeleteRole
        {
            get
            {
                return _deleteRole ??
                (_deleteRole = new RelayCommand(obj =>
                {
                    Role selected = SelectedRole;

                    if (selected == null)
                        return;

                    MessageBoxResult result = MessageBox.Show(
                        "Удалить данные по должности: " + selected.NameRole,
                        "Предупреждение",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                    {
                        using (var context = new CompanyEntities())
                        {
                            try
                            {
                                Role delRole = context.Roles.Find(selected.Id);

                                if (delRole != null)
                                {
                                    context.Roles.Remove(delRole);
                                    context.SaveChanges();

                                    ListRole.Remove(selected);
                                    SelectedRole = null;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(
                                    "\nОшибка удаления данных!\n" + ex.Message,
                                    "Предупреждение");
                            }
                        }
                    }
                },
                (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
