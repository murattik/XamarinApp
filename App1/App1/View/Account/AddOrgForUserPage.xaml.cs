using App1.Model;
using App1.View.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrgForUserPage : ContentPage
    {
        Picker pickerOrg;
        Picker pickerSklad;
        Button btn;

        public AddOrgForUserPage()
        {
            Title = "Выбор клинта";

            pickerOrg = new Picker
            {
                Title = "Клиент"
            };

            pickerSklad = new Picker
            {
                Title = "Склад",
                IsEnabled = false
            };

            btn = new Button
            {
                Text = "OK",
                IsVisible = false,
                BackgroundColor = Color.DeepSkyBlue
            };

            var org = App.UsersOrgAndSkladDatabase.GetItems().Select(a => a.NameOrg).Distinct();

            if (org != null)
            {
                foreach (string data in org)
                {
                    pickerOrg.Items.Add(data);
                }
            }


            btn.Clicked += ToHomePage;


            pickerOrg.SelectedIndexChanged += picker_SelectedIndexChanged;
            pickerSklad.SelectedIndexChanged += PickerSklad_SelectedIndexChanged;

            this.Content = new StackLayout { Children = { pickerOrg, pickerSklad, btn } };
            
        }

        private void PickerSklad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerSklad.Items[pickerSklad.SelectedIndex] != null)
            {
                btn.IsVisible = true;
            };

            int skladId = App.usersOrgAndSkladDatabase.GetItems().Where(a => a.SkladName == pickerSklad.Items[pickerSklad.SelectedIndex].ToString()).Select(a => a.SkladID).Distinct().FirstOrDefault();

            int userId = App.UsersDatabase.GetItems().Select(a => a.UserId).FirstOrDefault();

            try
            {
                if (userId != 0 && skladId != 0)
                {
                    Users user = App.UsersDatabase.GetItem(userId);
                    user.SkladID = skladId;
                    user.SkladName = pickerSklad.Items[pickerSklad.SelectedIndex].ToString();
                    App.UsersDatabase.SaveItem(user);
                }
            }
            catch
            {
                DisplayAlert("Ошибка", "При добавлении пользователю склада произошла ошибка!", "ОК");
            }

        }

        private async void ToHomePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //orgName.Text = "Клиент: " + pickerOrg.Items[pickerOrg.SelectedIndex];

            int orgId = App.UsersOrgAndSkladDatabase.GetItems().Where(a => a.NameOrg == pickerOrg.Items[pickerOrg.SelectedIndex].ToString()).Select(a => a.OrgID).Distinct().FirstOrDefault();

            int userId = App.UsersDatabase.GetItems().Select(a => a.UserId).FirstOrDefault();


            pickerSklad.IsEnabled = true;

            var pickerSkladData = App.UsersOrgAndSkladDatabase.GetItems().Where(a => a.NameOrg == pickerOrg.Items[pickerOrg.SelectedIndex].ToString()).OrderBy(a => a.SkladName).Select(a => a.SkladName).Distinct();

            try
            {
                foreach (string data in pickerSkladData)
                {
                    pickerSklad.Items.Remove(data);
                }
            }
            catch
            {
                DisplayAlert("Ошибка", "При смене клиента изменение складов завершилось с ошибкой!", "ОК");
            }

            try
            {
                foreach (string data in pickerSkladData)
                {
                    pickerSklad.Items.Add(data);
                }
            }
            catch
            {
                DisplayAlert("Ошибка", "При добавлении складов произошла ошибка!", "ОК");
            }
       

            if (userId != 0 && orgId != 0)
            {
                try
                {
                    Users user = App.UsersDatabase.GetItem(userId);
                    user.OrgID = orgId;
                    user.NameOrg = pickerOrg.Items[pickerOrg.SelectedIndex].ToString();
                    App.UsersDatabase.SaveItem(user);
                }
                catch
                {
                    DisplayAlert("Ошибка", "При добавлении пользователю клиента произошла ошибка!", "ОК");
                }
                
            }
        }
    }
}