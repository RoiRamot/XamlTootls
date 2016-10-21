using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageOverlayTool.Annotations;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ImageOverlayTool
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67
    }
    class ViewModel:INotifyPropertyChanged
    {
        private ImageSource _image;
        private bool _isTopMost;


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ImageSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
           
        }

        public ICommand LoadImageCommand
        {
            get
            {
                return new DelegateCommand(LoadImage);
            }
        }

        public bool IsTopMost
        {
            get { return _isTopMost; }
            set
            {
                _isTopMost = value;
                OnPropertyChanged();
            }
        }

        private void LoadImage()
        {
            var openDialog = new OpenFileDialog();
            openDialog.ShowDialog();
            string path=openDialog.FileName;
            Image = new BitmapImage(new Uri(path));
            //if (openDialog.ShowDialog() == DialogResult.OK)
            //{
            //    try
            //    {

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            //    }
            //}
        }
    }
}
