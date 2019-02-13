using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;

using Plugin.ContactService;

using RallyUp.Models;

namespace RallyUp.ViewModel
{
    class ContactsViewModel : INotifyPropertyChanged
    {
        #region Property

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        public MultiSelectObservableCollection<Contact> Contacts { get; }

        public ICommand DisplayNameCommand => new Command<Contact>(async contact =>
        {
            await Application.Current.MainPage.DisplayAlert("Selected Contact", contact.Name, "OK");
        });

        public ContactsViewModel(IList<Plugin.ContactService.Shared.Contact> contactList)
        {
            Contacts = new MultiSelectObservableCollection<Contact>();

            for (int i = 0; i < contactList.Count; i++)
            {
                Contacts.Add(new Contact(contactList[i].Name, contactList[i].Email));
            }
        }
    }
}
