using System;
using System.Collections;
using System.Text;

namespace FWBS.OMS.FileManagement
{
    internal static class Utils
    {
        public static ArgumentException ThrowMaximumLengthExceededException(int expected)
        {
            return new ArgumentException(String.Format(Session.CurrentSession.Resources.GetMessage("ERRFMMAXLENGTH2", "The value specified is too long. Maximum length is '{0}'.", "").Text, expected));
        }

        public static void MsgBox(Exception ex)
        {
            FWBS.OMS.UI.Windows.MessageBox.Show(ex.Message.ToString(), Global.ApplicationName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
        }

        public static Exception ThrowCantResetPlan(ArrayList reasons)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(Session.CurrentSession.Resources.GetResource("FMCANTRESETPLAN", "Unable to reset milestone plan.", "").Text);
            FormatReasonMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

        public static Exception ThrowCantRemovePlan(ArrayList reasons)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(Session.CurrentSession.Resources.GetResource("FMCANTREMPLAN", "Unable to remove milestone plan.", "").Text);
            FormatReasonMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

        public static Exception ThrowCantUnCompleteStage(byte stage, ArrayList reasons)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(String.Format(Session.CurrentSession.Resources.GetResource("FMCANTUNCOMPSTG", "Cannot uncomplete stage '{0}'.", "").Text, stage));
            FormatReasonMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

        public static Exception ThrowCantUnCompleteTask(Milestones.Task task, ArrayList reasons)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(String.Format(Session.CurrentSession.Resources.GetResource("FMCANTUNCOMPTSK", "Cannot uncomplete task '{0}'.", "").Text, task.Description));
            FormatReasonMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

        public static  Exception ThrowCantUnCompleteStageTasks(byte stage, ArrayList reasons)
        {
            StringBuilder sb = new StringBuilder(String.Format(Session.CurrentSession.Resources.GetResource("FMCANTUNCOMPSTTK", "Unable to uncomplete stage '{0}' tasks.", "").Text, stage));
            FormatReasonMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

        public static Exception ThrowCantCompleteStage(byte stage, ArrayList reasons)
        {
            StringBuilder sb = new StringBuilder(String.Format(Session.CurrentSession.Resources.GetResource("FMCANTCOMPSTAGE", "Unable to complete stage '{0}'.", "").Text, stage));
            FormatReasonMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

		public static Exception ThrowCantCompleteTask(Milestones.Task task, ArrayList reasons)
		{
			StringBuilder sb = new StringBuilder(String.Format(Session.CurrentSession.Resources.GetResource("FMCANTCOMPTASK", "Unable to complete task '{0}'.", "").Text, task.Description));
			FormatReasonMessage(sb, reasons);
			return new Exception(sb.ToString());
		}

		public static Exception ThrowCantChangeStageDue(Milestones.MilestoneStage stage, ArrayList reasons)
        {
			StringBuilder sb = new StringBuilder(String.Format(Session.CurrentSession.Resources.GetResource("FMCANTCHGSTGDUE", "Unable to change stage '{0}' due date.", "").Text, stage.Description));
            FormatReasonMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

        public static Exception ThrowMultipleExceptions(ArrayList reasons)
        {
            StringBuilder sb = new StringBuilder();
            FormatErrorMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

        public static Exception ThrowCantCompleteStageTasks(byte stage, ArrayList reasons)
        {
            StringBuilder sb = new StringBuilder(String.Format(Session.CurrentSession.Resources.GetResource("FMCANTCOMPSTTSK", "Unable to complete stage '{0}' tasks.", "").Text, stage));
            FormatReasonMessage(sb, reasons);
            return new Exception(sb.ToString());
        }

        internal static void FormatErrorMessage(System.Text.StringBuilder msg, System.Collections.ArrayList reasons)
        {
            if (reasons.Count > 0)
            {
                msg.Append(Environment.NewLine);
                msg.Append(Environment.NewLine);
                
                foreach (object r in reasons)
                {
                    msg.Append(Convert.ToString(r));
                    msg.Append(Environment.NewLine);
                }
            }

        }

        internal static void FormatReasonMessage(System.Text.StringBuilder msg, System.Collections.ArrayList reasons)
        {
            if (reasons.Count > 0)
            {
                msg.Append(Environment.NewLine);
                msg.Append(Environment.NewLine);
                int ctr = 0;
                foreach (object r in reasons)
                {
                    ctr++;
                    msg.Append(String.Format("{0}. ", ctr));
                    msg.Append(Convert.ToString(r));
                    msg.Append(Environment.NewLine);
                }
            }

        }
    }
}
