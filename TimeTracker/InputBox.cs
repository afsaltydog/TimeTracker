using System;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

namespace TimeTracker
{
    public class InputBox
    {
        public static DialogResult Show(string title, string promptText, ref string value)
        {
            return Show(title, promptText, ref value, null);
        }

        public static DialogResult Show(string title, string promptText, ref string value, InputBoxValidation validation)
        {
            Form form = new Form();
            form = ConfigureForm(form);

            form.Text = title;
            form.Controls[0].Text = promptText;
            form.Controls[1].Text = value;

            if (validation != null)
            {
                form.FormClosing += delegate (object sender, FormClosingEventArgs e)
                {
                    if (form.DialogResult == DialogResult.OK)
                    {
                        string errorText = validation(form.Controls[1].Text);
                        if (e.Cancel = (errorText != ""))
                        {
                            MessageBox.Show(form, errorText, "Validation Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            form.Controls[1].Focus();
                        }
                    }
                };
            }

            DialogResult dialogResult = form.ShowDialog();
            value = form.Controls[1].Text;

            return dialogResult;
        }

        public static Form ConfigureForm(Form form)
        {
            Label label = new Label();
            label = ConfigureLabel(label);

            TextBox textBox = new TextBox();
            textBox = ConfigureTextBox(textBox);

            Button btnOk = new Button();
            btnOk = ConfigureButton(btnOk, "Ok");
            Button btnCancel = new Button();
            btnCancel = ConfigureButton(btnCancel, "Cancel");

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, btnOk, btnCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            return form;
        }

        public static Label ConfigureLabel(Label label)
        {
            label.SetBounds(9, 20, 372, 13);
            label.AutoSize = true;
            return label;
        }

        public static TextBox ConfigureTextBox(TextBox textBox)
        {
            textBox.SetBounds(12, 36, 372, 20);
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;

            return textBox;
        }

        public static Button ConfigureButton(Button button, string name)
        {
            button.Text = name;

            switch (name)
            {
                case "Ok":
                case "OK":
                case "ok":
                    button.DialogResult = DialogResult.OK;
                    button.SetBounds(228, 72, 75, 23);
                    break;
                case "Cancel":
                case "CANCEL":
                case "cancel":
                    button.DialogResult = DialogResult.Cancel;
                    button.SetBounds(309, 72, 75, 23);
                    break;
            }

            button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            return button;
        }

        public delegate string InputBoxValidation(string errorMessage);
    }
}
