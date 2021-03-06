Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI
Imports DevExpress.XtraScheduler
Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal

Partial Public Class AppointmentForm
	Inherits SchedulerFormControl
	Public ReadOnly Property CanShowReminders() As Boolean
		Get
			Return (CType(Parent, AppointmentFormTemplateContainer)).Control.Storage.EnableReminders
		End Get
	End Property

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		'PrepareChildControls();
		tbSubject.Focus()
	End Sub

	Protected Sub cbValidation_Callback(ByVal source As Object, ByVal e As CallbackEventArgs)
		Dim price As Integer = Convert.ToInt32(e.Parameter)

		' Custom validation rule.
		' You can use ((AppointmentFormTemplateContainer)Parent).Control.Storage.Appointments
		' collection here to access attributes of other appointments.
		If price = 10 OrElse price = 20 Then
			e.Result = "valid"
		Else
			e.Result = "Price must be 10 or 20."
		End If
	End Sub

	Public Overrides Sub DataBind()
		MyBase.DataBind()

		Dim container As AppointmentFormTemplateContainer = CType(Parent, AppointmentFormTemplateContainer)
		Dim apt As Appointment = container.Appointment
        edtLabel.SelectedIndex = apt.LabelKey
        edtStatus.SelectedIndex = apt.StatusKey
        If (Not Object.Equals(apt.ResourceId, ResourceEmpty.Id)) Then
            edtResource.Value = apt.ResourceId.ToString()
        Else
            edtResource.Value = SchedulerIdHelper.EmptyResourceId
        End If

		AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence

		If container.Appointment.HasReminder Then
			cbReminder.Value = container.Appointment.Reminder.TimeBeforeStart.ToString()
			chkReminder.Checked = True
		Else
			cbReminder.ClientEnabled = False
		End If

        btnOk.ClientSideEvents.Click = String.Format("function() {{ if (confirm('Apply changes?')) ASPx.AppointmentSave('{0}'); }}", container.Control.ClientID)
		btnCancel.ClientSideEvents.Click = container.CancelHandler
		btnDelete.ClientSideEvents.Click = container.DeleteHandler
	End Sub

	Protected Overrides Sub PrepareChildControls()
		Dim container As AppointmentFormTemplateContainer = CType(Parent, AppointmentFormTemplateContainer)
		Dim control As ASPxScheduler = container.Control

		AppointmentRecurrenceForm1.EditorsInfo = New EditorsInfo(control, control.Styles.FormEditors, control.Images.FormEditors, control.Styles.Buttons)
		MyBase.PrepareChildControls()
	End Sub

	Protected Overrides Function GetChildEditors() As ASPxEditBase()
		Dim edits() As ASPxEditBase = { lblSubject, tbSubject, lblLocation, tbLocation, lblLabel, edtLabel, lblStartDate, edtStartDate, lblEndDate, edtEndDate, lblStatus, edtStatus, lblAllDay, chkAllDay, lblResource, edtResource, tbDescription, cbReminder }
		Return edits
	End Function

	Protected Overrides Function GetChildButtons() As ASPxButton()
		Dim buttons() As ASPxButton = { btnOk, btnCancel, btnDelete }
		Return buttons
	End Function
End Class