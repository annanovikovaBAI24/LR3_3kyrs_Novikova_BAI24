using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LR1_3kyrs_NOVIKOVA_BAI24;
using LR1_3kyrs_NOVIKOVA_BAI24.Model;

namespace LR1_3kyrs_NOVIKOVA_BAI24.View
{
    public partial class WindowNewEmployee : Window
    {
        public WindowNewEmployee()
        {
            InitializeComponent();

            using (var context = new CompanyEntities())
            {
                CbRole.ItemsSource = context.Roles.ToList();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            tbBirthday.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            clBirthday.GetBindingExpression(DatePicker.SelectedDateProperty)?.UpdateSource();

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tbBirthday_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)//видимость календаря
        {
            if (tbBirthday.Visibility == Visibility.Hidden)
            {
                clBirthday.Visibility = Visibility.Visible;
            }
            else
            {
                clBirthday.Visibility = Visibility.Hidden;
            }
        }
    }
}
