using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Modals
{
    public class ModalFooter
    {
        public string SubmitButtonText { get; set; } = "儲存";
        public ModalFooterButtonType SubmitButtonType { get; set; } = ModalFooterButtonType.submit;
        public string CancelButtonText { get; set; } = "關閉";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }
        public ModalFooterSubmitButtonColor SubmitButtonColor { get; set; }
        public string SubmitButtonColorClass
        {
            get
            {
                switch (this.SubmitButtonColor)
                {
                    case ModalFooterSubmitButtonColor.Primary:
                        return "btn-primary";
                    case ModalFooterSubmitButtonColor.Secondary:
                        return "btn-secondary";
                    case ModalFooterSubmitButtonColor.Success:
                        return "btn-success";
                    case ModalFooterSubmitButtonColor.Info:
                        return "btn-info";
                    case ModalFooterSubmitButtonColor.Warning:
                        return "btn-warning";
                    case ModalFooterSubmitButtonColor.Danger:
                        return "btn-danger";
                    default:
                        return "btn-default";
                }
            }
        }
    }

    public enum ModalFooterSubmitButtonColor
    {
        Success,
        Primary,
        Secondary,     
        Info,
        Warning,
        Danger
    }

    public enum ModalFooterButtonType
    {
        submit,
        button
    }
}
