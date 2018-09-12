using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Tulpep.NotificationWindow;

namespace WindowsFormsApp1
{
    public class Function
    {

        public static Boolean KOSONGTEXT;
        public static Boolean KOSONGKOMBO;
        public static string[] USERDATA = new string[4];
        static private string textboxval;
        static private string comboboxval;
        static private string udahtext;
        static private string udahcombo;
        static private string Pengecualian = "ket, keterangan, cari, pencarian";

        public class Hash
        {
            public static string MD5(string text, int jumlah = 1)
            {
                string hash = "";
                for (int i = 1; i <= jumlah; i++)
                {
                    var MD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    Byte[] bytes = MD5.ComputeHash(Encoding.ASCII.GetBytes(text));
                    foreach (Byte by in bytes)
                    {
                        hash += by.ToString("x2");
                    }
                }
                return hash;
            }
        }

        public class Encrypt
        {
            public static string Base64(string text, int jumlah = 1)
            {
                string result = text;
                try
                {
                    for (int i = 1; i <= jumlah; i++)
                    {
                        Byte[] enc = Encoding.UTF8.GetBytes(result);
                        result = Convert.ToBase64String(enc);
                    }
                    return result;
                } catch 
                {
                    return null;
                }
            }
        }

        public class Decrypt
        {
            public static string Base64(string text, int jumlah = 1)
            {
                string result = text;
                for (int i = 1; i <= jumlah; i++)
                {
                    Byte[] dec = Convert.FromBase64String(result);
                    result = Encoding.UTF8.GetString(dec);
                }
                return result;
            }
        }

        public class Pesan
        {
            public static void Info(string pesan, string judul = "Informasi")
            {
                MessageBox.Show(pesan, judul, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            public static void Peringatan(string pesan, string judul = "Peringatan")
            {
                MessageBox.Show(pesan, judul, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            public static void Error(string pesan, string judul = "Kesalahan")
            {
                MessageBox.Show(pesan, judul, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            public static DialogResult Konfirmasi(string pesan, string judul = "Konfirmasi")
            {
                return MessageBox.Show(pesan, judul, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            public static void Pupup(string Content, string title = "Information")
            {
                PopupNotifier pupup = new PopupNotifier();
                pupup.Image = Properties.Resources.info;
                pupup.ContentText = Content;
                pupup.TitleText = title;
                pupup.Popup();
            }
        }

        public static void BukaForm(Form formna, FormStartPosition posisi = FormStartPosition.WindowsDefaultLocation)
        {
            Form forma = new Form();
            forma = formna;
            formna.StartPosition = posisi;
            formna.Show();
        }

        public static void ClearUserData()
        {
            for (int i = 0; i <= USERDATA.Length - 1; i++)
            {
                USERDATA[i] = "";
            }
        }

        public static void CekTextboxComboBox(Control control, ErrorProvider errorp)
        {
            errorp.Dispose();
            CekCombobox(control, errorp);
            CekTexbox(control, errorp);
        }

        public static string GetAllInput(Control control, string prefix = "input_")
        {
            string field, value, resultfield, resultvalue, namecontrol, data, ismd5, isbase64enc;
            field = "";
            value = "";
            foreach (Control inputcontrol in control.Controls)
            {
                if (inputcontrol.Name.Length > prefix.Length)
                {
                    namecontrol = inputcontrol.Name.Substring(0, prefix.Length);
                    if (namecontrol == prefix)
                    {
                        ismd5 = inputcontrol.Name.Substring((prefix.Length - 1), 5);
                        isbase64enc = inputcontrol.Name.Substring((prefix.Length - 1), 5);
                        if (ismd5.ToLower() == "_md5_")
                        {
                            field += "`" + inputcontrol.Name.Substring((prefix.Length + 4)) + "`,";
                            value += "'" + Hash.MD5(inputcontrol.Text) + "',";
                        }
                        else if (isbase64enc.ToLower() == "_base64enc_")
                        {
                            field += "`" + inputcontrol.Name.Substring((prefix.Length + 10)) + "`,";
                            value += "'" + Decrypt.Base64(inputcontrol.Text) + "',";
                        }
                        else if (inputcontrol is DateTimePicker)
                        {
                            DateTimePicker dateTimePicker = inputcontrol as DateTimePicker;
                            field += "`" + inputcontrol.Name.Substring(prefix.Length) + "`,";
                            value += "'" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "',";
                        }
                        else
                        {
                            field += "`" + inputcontrol.Name.Substring(prefix.Length) + "`,";
                            value += "'" + MySql.EscapeString(inputcontrol.Text) + "',";
                        }
                    }
                }     
            }
            resultfield = field.Substring(0, field.Length - 1);
            resultvalue = value.Substring(0, value.Length - 1);
            data = "(" + resultfield + ") VALUES (" + resultvalue + ")";
            return data;
        }

        public static string GetAllUpdate(Control control, string prefix = "input_")
        {
            string field, value, resultfield, resultvalue, result, namecontrol, data, ismd5;
            result = "";
            foreach (Control inputcontrol in control.Controls)
            {
                if (inputcontrol.Name.Length > prefix.Length)
                {
                    namecontrol = inputcontrol.Name.Substring(0, prefix.Length);
                    if (namecontrol == prefix)
                    {
                        ismd5 = inputcontrol.Name.Substring((prefix.Length - 1), 5);
                        if (ismd5.ToLower() == "_md5_")
                        {
                            field = "`" + inputcontrol.Name.Substring((prefix.Length + 4)) + "`,";
                            value = "'" + Hash.MD5(inputcontrol.Text) + "',";
                        }
                        else if (inputcontrol is DateTimePicker)
                        {
                            DateTimePicker dateTimePicker = inputcontrol as DateTimePicker;
                            field = "`" + inputcontrol.Name.Substring(prefix.Length) + "`,";
                            value = "'" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "',";
                        }
                        else
                        {
                            field = "`" + inputcontrol.Name.Substring(prefix.Length) + "`,";
                            value = "'" + MySql.EscapeString(inputcontrol.Text) + "',";
                        }
                        resultfield = field.Substring(0, field.Length - 1);
                        resultvalue = value.Substring(0, value.Length - 1);
                        result += resultfield + " = " + resultvalue + ", ";
                    }
                }
            }
            data = result.Substring(0, result.Length - 2);
            return data;
        }

        static void CekTexbox(Control control, ErrorProvider errorp)
        {
            textboxval = "";
            foreach (Control controlss in control.Controls)
            {
                if (controlss is TextBox)
                {
                    if (Pengecualian.ToLower().IndexOf(controlss.Name.ToLower()) < 0)
                    {
                        if (controlss.Text.Trim() == "")
                        {
                            string ss = "B";
                            controlss.Select();
                            errorp.SetError(controlss, "Harap isi bidang ini !");
                            textboxval += ss;
                            udahtext = new string(Convert.ToChar("U"), textboxval.Length);
                            CekText();
                        } else
                        {
                            string ss = "U";
                            textboxval += ss;
                            udahtext = new string(Convert.ToChar("U"), textboxval.Length);
                            CekText();
                        }
                    }
                }
            }
        }

        static void CekText()
        {
            if (udahtext != textboxval)
            {
                KOSONGTEXT = true;
            }
            else
            {
                KOSONGTEXT = false;
            }
        }

        static void CekCombobox(Control control, ErrorProvider errorp)
        {
            comboboxval = "";
            foreach (Control controlss in control.Controls)
            {
                if (controlss is ComboBox)
                {
                    if (Pengecualian.ToLower().IndexOf(controlss.Name.ToLower()) < 0)
                    {
                        if (controlss.Text.Trim() == "")
                        {
                            string ss = "B";
                            controlss.Select();
                            errorp.SetError(controlss, "Harap isi bidang ini !");
                            comboboxval += ss;
                            udahcombo = new string(Convert.ToChar("U"), comboboxval.Length);
                            CekCombo();
                        }
                        else
                        {
                            string ss = "U";
                            comboboxval += ss;
                            udahcombo = new string(Convert.ToChar("U"), comboboxval.Length);
                            CekCombo();
                        }
                    }
                }
            }
        }

        static void CekCombo()
        {
            if (udahcombo != comboboxval)
            {
                KOSONGKOMBO = true;
            }
            else
            {
                KOSONGKOMBO = false;
            }
        }

        public static string AmbilData(Control control, int subitem, int CurrentRow = -1)
        {
            if (control is DataGridView)
            {
                DataGridView dgv = control as DataGridView;
                if (CurrentRow == -1) {
                    CurrentRow = dgv.CurrentRow.Index;
                }
                return dgv.Rows[CurrentRow].Cells[subitem].Value.ToString();
            }
            else if (control is ListView)
            {
                ListView LV = control as ListView;
                return LV.SelectedItems[0].SubItems[subitem].Text;
            }
            else
            {
                return null;
            }
        }

        static public void ClearTextBoxComboBox(Control control)
        {
            ClearTextBox(control);
            ClearComboBox(control);
        }

        static void ClearTextBox(Control control)
        {
             foreach (Control controlss in control.Controls)
            {
                if (controlss is TextBox)
                {
                    if (Pengecualian.ToLower().IndexOf(controlss.Name.ToLower()) < 0)
                    {
                        controlss.Text = "";
                    }
                }
            }
        }

        static void ClearComboBox(Control control)
        {
            foreach (Control controlss in control.Controls)
            {
                if (controlss is ComboBox)
                {
                    if (Pengecualian.ToLower().IndexOf(controlss.Name.ToLower()) < 0)
                    {
                        controlss.Text = "";
                    }
                }
            }
        }
    }
}
