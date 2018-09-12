using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class MySql
    {
        Function Controls = new Function();

        static string SERVER = "Localhost";
        static string PORT = "3306";
        static string USER = "root";
        static string PASS = "";
        static string DATABASE = "db_module_test";
        static string CONNECTIONSTRING = "SERVER=" + SERVER + ";PORT=" + PORT + ";UID=" + USER + ";PWD=" + PASS + ";DATABASE=" + DATABASE;
        static public MySqlConnection Koneksi = new MySqlConnection(CONNECTIONSTRING);
        static public MySqlCommand MysqlCmd;
        static public MySqlDataAdapter MysqlDA;
        static public MySqlDataReader MysqlDR;
        static public DataSet DATASET;
        static public Boolean ReaderHasRow;
        static public string[] ReaderData = new string[999];
        static public string QUERY;
        static string Field_;

        public static void BukaKoneksi()
        {
            try
            {                
                Koneksi.Open();
                //Controls.Pesan("Koneksi Berhasil");
            } catch (Exception ex)
            {
                ERRORDB(ex);
            }
        }

        public static void TutupKoneksi()
        {
            try
            {
                Koneksi.Dispose();
                //Controls.Pesan("Koneksi Tertutup");
            } catch (Exception ex)
            {
                ERRORDB(ex);
            }
        }

        private static void ClearReaderData()
        {
            for (int i = 0; i <= ReaderData.Length - 1; i++)
            {
                ReaderData[i] = "";
            }
        }

        public static void ShowDataQuery(string query, DataGridView grid)
        {
            try
            {
                BukaKoneksi();

                QUERY = query;
                MysqlCmd = new MySqlCommand(QUERY, Koneksi);
                MysqlDA = new MySqlDataAdapter(MysqlCmd);
                DATASET = new DataSet();
                MysqlDA.Fill(DATASET);

                if (DATASET.Tables.Count > 0)
                {
                    KeDGV(grid);
                }

                TutupKoneksi();
            } catch(Exception ex)
            {
                ERRORDB(ex);
            }
        }

        public static void KeDGV(DataGridView grid)
        {
            grid.DataSource = DATASET.Tables[0];
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AllowUserToAddRows = false;
        }

        public static void Reader(string query)
        {
            ClearReaderData();
            try
            {
                BukaKoneksi();

                MysqlCmd = new MySqlCommand(query, Koneksi);
                MysqlDR = MysqlCmd.ExecuteReader();
                MysqlDR.Read();

                if (MysqlDR.HasRows == true)
                {
                    for (int i = 0; i <= MysqlDR.FieldCount - 1; i++)
                    {
                        ReaderData[i] = Convert.ToString(MysqlDR.GetValue(i));
                    }
                    ReaderHasRow = true;
                } else
                {
                    ReaderHasRow = false;
                }

                TutupKoneksi();
            } catch (Exception ex)
            {
                ERRORDB(ex);
            }
        }

        public static Boolean Create(string Table, string Data)
        {
            try
            {
                BukaKoneksi();

                QUERY = "INSERT INTO `" + Table + "` " + Data;
                MysqlCmd = new MySqlCommand(QUERY, Koneksi);
                MysqlCmd.ExecuteNonQuery();

                TutupKoneksi();
                return true;
            } catch (Exception ex)
            {
                ERRORDB(ex);
                return false;
            }
        }

        public static Boolean Update(string Table, string Data, string Field, string value)
        {
            try
            {
                BukaKoneksi();

                QUERY = "UPDATE `" + Table + "` " + Data + "    WHERE " + Field + "  = '" + EscapeString(value) + "'";
                MysqlCmd = new MySqlCommand(QUERY, Koneksi);
                MysqlCmd.ExecuteNonQuery();

                TutupKoneksi();
                return true;
            } catch (Exception ex)
            {
                ERRORDB(ex);
                return false;
            }
        }

        public static Boolean Delete(string Table, string Field, string id)
        {
            try
            {
                BukaKoneksi();

                QUERY = "DELETE FROM " + Table + " WHERE " + Field + " = '" + EscapeString(id) + "'";
                MysqlCmd = new MySqlCommand(QUERY, Koneksi);
                MysqlCmd.ExecuteNonQuery();

                TutupKoneksi();
                return true;
            } catch (Exception ex)
            {
                ERRORDB(ex);
                return false;
            }
        }

        public static string INSERT_(string field , Control control, string format_ = "")
        {
            Field_ += field + ", ";
            if (control is DateTimePicker)
            {
                DateTimePicker datetimep = control as DateTimePicker;
                return "'" + EscapeString(datetimep.Value.ToString(format_)) + "', ";
            }
            else
            {
                return "'" + EscapeString(control.Text) + "', ";
            }
        }

        public static string INSERT_S(string field, string text)
        {
            Field_ += field + ", ";
            return "'" + EscapeString(text) + "', ";
        }

        public static string UPDATE_(string field, Control control, string format_ = "")
        {
            if (control is DateTimePicker)
            {
                DateTimePicker datetimep = control as DateTimePicker;
                return field + " = '" + EscapeString(datetimep.Value.ToString(format_)) + "', ";
            } else
            {
                return field + " = '" + string.Format(EscapeString(control.Text)) + "', ";
            }
        }

        public static string UPDATE_S(string field, string text, string format_ = "")
        {
            return field + " = '" + EscapeString(text) + "', ";
        
        }

        public static string DATA_(Array array)
        {
            string data = "";
            foreach (string values in array)
            {
                data += values;
            }
            return data.Substring(0, data.Length - 2);
        }

        public static string EscapeString(string text)
        {
            return MySqlHelper.EscapeString(text);
        }

        public static void ERRORDB(Exception ex, String JUDUL = "Gagal")
        {
            Function.Pesan.Error("Kesalahan : " + ex.Message + "\n\n" + QUERY);
        }
    }
}
