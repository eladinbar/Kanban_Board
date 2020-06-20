

namespace Presentation.Model
{
    public class UserModel: NotifiableModelObject
    {
        public string AssociatedBoard;
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                RaisePropertyChanged("Nickname");
            }
        }

        public UserModel(BackendController controller, string email, string nickname, string associatedBoard) : base(controller)
        {
            _email = email;
            _nickname = nickname;
            this.AssociatedBoard = associatedBoard;
        }
    }
}
