using System;
using System.Web.UI;
using DevExpress.XtraScheduler;
using DevExpress.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;

public partial class AppointmentForm : SchedulerFormControl {
    public bool CanShowReminders {
        get {
            return ((AppointmentFormTemplateContainer)Parent).Control.Storage.EnableReminders;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        //PrepareChildControls();
        tbSubject.Focus();
    }

    protected void cbValidation_Callback(object source, CallbackEventArgs e) {
        int price = Convert.ToInt32(e.Parameter);

        // Custom validation rule.
        // You can use ((AppointmentFormTemplateContainer)Parent).Control.Storage.Appointments
        // collection here to access attributes of other appointments.
        if (price == 10 || price == 20)
            e.Result = "valid";
        else
            e.Result = "Price must be 10 or 20.";
    }

    public override void DataBind() {
        base.DataBind();

        AppointmentFormTemplateContainer container = (AppointmentFormTemplateContainer)Parent;
        Appointment apt = container.Appointment;
        edtLabel.SelectedIndex = (int)apt.LabelKey;
        edtStatus.SelectedIndex = (int)apt.StatusKey;
        if (!Object.Equals(apt.ResourceId, ResourceEmpty.Id))
            edtResource.Value = apt.ResourceId.ToString();
        else
            edtResource.Value = SchedulerIdHelper.EmptyResourceId;

        AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence;

        if (container.Appointment.HasReminder) {
            cbReminder.Value = container.Appointment.Reminder.TimeBeforeStart.ToString();
            chkReminder.Checked = true;
        }
        else {
            cbReminder.ClientEnabled = false;
        }

        btnOk.ClientSideEvents.Click = string.Format("function() {{ if (confirm('Apply changes?')) ASPx.AppointmentSave('{0}'); }}", container.Control.ClientID);
        btnCancel.ClientSideEvents.Click = container.CancelHandler;
        btnDelete.ClientSideEvents.Click = container.DeleteHandler;
    }

    protected override void PrepareChildControls() {
        AppointmentFormTemplateContainer container = (AppointmentFormTemplateContainer)Parent;
        ASPxScheduler control = container.Control;

        AppointmentRecurrenceForm1.EditorsInfo = new EditorsInfo(control, control.Styles.FormEditors, control.Images.FormEditors, control.Styles.Buttons);
        base.PrepareChildControls();
    }

    protected override ASPxEditBase[] GetChildEditors() {
        ASPxEditBase[] edits = new ASPxEditBase[] {
			lblSubject, tbSubject,
			lblLocation, tbLocation,
			lblLabel, edtLabel,
			lblStartDate, edtStartDate,
			lblEndDate, edtEndDate,
			lblStatus, edtStatus,
			lblAllDay, chkAllDay,
			lblResource, edtResource,
			tbDescription, cbReminder
		};
        return edits;
    }

    protected override ASPxButton[] GetChildButtons() {
        ASPxButton[] buttons = new ASPxButton[] {
			btnOk, btnCancel, btnDelete
		};
        return buttons;
    }
}