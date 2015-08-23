using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Infrustructure
{
    public class ViewSystemMessage
    {
        public SystemMessage? message { get; private set; }
        public string messageText { get; private set; }
        public string htmlClass { get; private set; }
        public ViewSystemMessage (SystemMessage? Message)
        {
            message = Message;
            switch (message)
            {
                case SystemMessage.SaveAccountInfoSuccess:
                    messageText = "Личная информация успешно сохранена";
                    htmlClass = "alert alert-success";
                    break;
                case SystemMessage.ChangePasswordSuccess:
                    messageText = "Пароль успешно изменён";
                    htmlClass = "alert alert-success";
                    break;
                case SystemMessage.Error:
                    messageText = "При выполнении действия произошла ошибка";
                    htmlClass = "alert alert-danger";
                    break;
                default:
                    messageText = "";
                    htmlClass = "";
                    break;
            }
        }

    }

    public enum SystemMessage
    {
        SaveAccountInfoSuccess,
        ChangePasswordSuccess,
        Error
    }
}