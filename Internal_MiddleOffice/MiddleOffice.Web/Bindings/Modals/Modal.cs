using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Modals
{
    public class Modal
    {
        public string ID { get; set; }
        public string AreaLabelID { get; set; }
        public ModalSize? Size { get; set; }
        public string ModalSizeClass
        {
            get
            {
                switch (this.Size)
                {
                    case ModalSize.Defalut:
                        return "";
                    case ModalSize.Small:
                        return "modal-sm";
                    case ModalSize.Large:
                        return "modal-lg";
                    case ModalSize.ExtraLarge:
                        return "modal-xl";
                    default:
                        return "";
                }
            }
        }
        public ModalColor? Color { get; set; }
        public string ModalColorClass
        {
            get
            {
                switch (this.Color)
                {
                    case ModalColor.None:
                        return "";
                    case ModalColor.Primary:
                        return "card-primary";
                    case ModalColor.Secondary:
                        return "card-secondar";
                    case ModalColor.Success:
                        return "card-success";
                    case ModalColor.Info:
                        return "card-info";
                    case ModalColor.Warning:
                        return "card-warning";
                    case ModalColor.Danger:
                        return "card-danger";
                    case ModalColor.Dark:
                        return "card-dark";
                    default:
                        return "";
                }
            }
        }
    }

    public enum ModalSize
    {
        Defalut,
        Small,
        Large,
        ExtraLarge
    }

    public enum ModalColor
    {
        None,
        Primary,
        Secondary,
        Success,
        Info,
        Warning,
        Danger,
        Dark
    }
}
