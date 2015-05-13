using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hamming
{
    public partial class _Default : Page
    {
        char[] v_txt;
        char[] v_code;
        double v_length;
        double v_amount_of_control_symbols;
        char[,] v_Matrix;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public void fill_in_the_matrix()
        {
            v_Matrix = new char[(int)v_amount_of_control_symbols, v_code.Length];
            // предварительное заполнение проверочной матрицы нулями
            for (int i = 0; i < (int)v_amount_of_control_symbols; i++)
            {
                for (int j = 0; j < v_code.Length; j++)
                {
                    v_Matrix[i, j] = '0';
                }
            }
            // заполнение проверочной матрицы (левая часть)
            for (int j = 0, l_num = 3; j < v_length; j++, l_num++)
            {
                //int v_num = j + 1;
                int l_tmp = l_num;
                int l_weight = 0;
                for (int i = 0; i < (int)v_amount_of_control_symbols; i++)
                {
                    if (l_tmp > 0)
                    {
                        int v_res = l_tmp % 2;
                        if (v_res == 1)
                        {
                            v_Matrix[i, j] = '1';
                            l_weight++;
                        }
                        l_tmp /= 2;
                    }
                }
                if (l_weight < 2)
                    j--;
            }
            //
            for (int i = 0; i < (int)v_amount_of_control_symbols; i++)
            {
                int l_weight = 0;
                for (int j = 0; j < v_length; j++)
                {
                    if (v_Matrix[i, j] == '1')
                        l_weight++;
                }
                if (l_weight == 0)
                    v_Matrix[i, (int)v_length - 1] = '1';
            }
            // заполнение проверочной матрицы (правая часть)
            for (int i = 0, j = (int)v_length; i < (int)v_amount_of_control_symbols; i++, j++)
                v_Matrix[i, j] = '1';
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            v_txt = null;
            v_length = 0;
            v_amount_of_control_symbols = 0;
            ListBox1.Items.Clear();
            // считыванте файла
            string path = Server.MapPath("~/Content/text.txt");
           // using (StreamReader rfile = new StreamReader(path))
           // {
               // v_txt = rfile.ReadToEnd().ToLower().ToCharArray();
            v_txt = TextBox1.Value.ToLower().ToCharArray();
           // }
            v_code = v_txt;
            v_length = v_txt.Length; // длина сообщения
            //вычисление количество проверочных символов
            //v_amount_of_control_symbols = Math.Ceiling(Math.Log((v_length + 1) + Math.Ceiling(Math.Log(v_length + 1, 2)), 2));
            v_amount_of_control_symbols = Math.Ceiling(Math.Log(v_length, 2.0) + 1);
            // массив с номерами позиций проверочных символов
            ///v_check_positions = new int[(int)v_amount_of_control_symbols]; 
            // массив со значениями проверочных символов
            int[] v_symbols = new int[(int)v_amount_of_control_symbols];
            // увеличиваем размер кода на величину количества проверочных символов
            Array.Resize<char>(ref v_code, v_code.Length + (int)v_amount_of_control_symbols);
            for (double i = 0; i < v_amount_of_control_symbols; i++)
            {
                // вычисление позиции (необходимо для
                // приведения проверочной матрицы к 
                // каноническому виду)
                ///double v_position = Math.Pow(2, i);
                ///v_check_positions[(int)i] = (int)v_position;
                // добавление мест для проверочных символов
                v_code[(int)v_length + (int)i] = '0';
            }
            fill_in_the_matrix();
            // заполнение проверочных символов
            for (int i = 0; i < (int)v_amount_of_control_symbols; i++)
            {
                v_symbols[i] = 0;
                for (int j = 0; j < v_code.Length; j++)
                {
                    v_symbols[i] += ((Convert.ToInt32(v_code[j]) - 48) * (Convert.ToInt32(v_Matrix[i, j]) - 48));
                }
                v_symbols[i] %= 2;
            }
            // вставка проверочных символов в код
            for (int i = (int)v_length, j = 0; i < v_code.Length; i++, j++)
                v_code[i] = Convert.ToChar(v_symbols[j].ToString());
            // вывод проверочной матрицы
            ListBox1.Items.Add(new string(v_code).Insert((int)v_length, "") + "");
            string v_separator = "";
            for (int i = 0; i < v_code.Length; i++)
            {
                if (i == (int)v_length)
                    v_separator += " ";
                else v_separator += " ";
            }
            ListBox1.Items.Add(v_separator + "");
            for (int i = 0; i < (int)v_amount_of_control_symbols; i++)
            {
                string v_str = "";
                for (int j = 0; j < v_code.Length; j++)
                {
                    if (j == (int)v_length)
                        v_str += "" + v_Matrix[i, j];
                    else v_str += v_Matrix[i, j];
                }
                ListBox1.Items.Add(v_str + " r" + i.ToString() + "= " + v_symbols[i]);
            }
            //groupBox1.Visible = true;
            //label2.Visible = true;
            //label3.Visible = true;
            //label4.Visible = true;
            //label5.Visible = true;
            //label6.Visible = true;
            //label7.Visible = true;

          //  TextBox1.Text = new string(v_txt);
            Label1.Text = "Длина сообщения:      " + v_length;
            Label2.Text = "Количество проверочных символов: " + v_amount_of_control_symbols;
            Label3.Text = "Длина кодового слова:     " + v_code.Length;

            //button2.Visible = true;
           // textBox1.Visible = true;
            Session["v_code"] = v_code;
            Session["v_txt"] = v_txt;
            Session["v_length"] = v_length;
            Session["v_amount_of_control_symbols"] = v_amount_of_control_symbols;
            Session["v_Matrix"] = v_Matrix;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            v_code = (char[])Session["v_code"];
            v_txt = (char[])Session["v_txt"];
            v_length = (double)Session["v_length"];
            v_amount_of_control_symbols = (double) Session["v_amount_of_control_symbols"];
            v_Matrix = (char[,]) Session["v_Matrix"];

            char[] v_received_msg = (char[])v_code.Clone();
            ListBox1.Items.Add("       ");
            if (TextBox2.Text.Length > 0)
            {
                // чтение позиций, в которых будут ошибки
                string[] v_positions = TextBox2.Text.Split(' ');
                // инвертирование символов
                for (int i = 0; i < v_positions.Length; i++)
                    if (Convert.ToInt32(v_positions[i]) <= v_received_msg.Length)
                        if (v_received_msg[Convert.ToInt32(v_positions[i]) - 1] == '1')
                            v_received_msg[Convert.ToInt32(v_positions[i]) - 1] = '0';
                        else
                            v_received_msg[Convert.ToInt32(v_positions[i]) - 1] = '1';
            }
            // нахождение синдромов
            int[] v_syndroms = new int[(int)v_amount_of_control_symbols];
            for (int i = 0; i < (int)v_amount_of_control_symbols; i++)
            {
                v_syndroms[i] = 0;
                for (int j = 0; j < v_code.Length; j++)
                {
                    v_syndroms[i] += ((Convert.ToInt32(v_received_msg[j]) - 48) * (Convert.ToInt32(v_Matrix[i, j]) - 48));
                }
                v_syndroms[i] %= 2;
            }
            // нахождение позиции ошибки
            int v_error_position = 0;
            string v_syndrom = "";
            for (int i = 0; i < v_amount_of_control_symbols; i++)
                v_syndrom += v_syndroms[i].ToString();
            for (int j = 0; j < v_received_msg.Length; j++)
            {
                string v_column = "";
                for (int i = 0; i < v_amount_of_control_symbols; i++)
                    v_column += v_Matrix[i, j];
                if (v_syndrom == v_column)
                {
                    v_error_position = j + 1;
                    break;
                }
            }
            // вывод синдромов
            ListBox1.Items.Add(new string(v_received_msg));
            string v_separator1 = "";
            string v_separator2 = "";
            for (int i = 0; i < v_received_msg.Length; i++)
            {
                if (i == v_error_position - 1)
                    v_separator1 += "=";
                else v_separator1 += "  ";
                v_separator2 += " ";
            }
            ListBox1.Items.Add(v_separator1 + "   - обозначена позиция ошибки");
            ListBox1.Items.Add(v_separator2 + " ");
            for (int i = 0; i < (int)v_amount_of_control_symbols; i++)
            {
                string v_str = "";
                for (int j = 0; j < v_code.Length; j++)
                {
                    if (j == (int)v_length)
                        v_str += "" + v_Matrix[i, j];
                    else v_str += v_Matrix[i, j];
                }
                ListBox1.Items.Add(v_str + " s" + i.ToString() + "= " + v_syndroms[i]);
            }
            string v_error_vector = "";
            int v_error_multiplicity = 0;
            // вычислене вектора ошибок и кратности ошибок
            for (int i = 0; i < v_received_msg.Length; i++)
                if (i == v_error_position - 1)
                {
                    v_error_vector += "1";
                    v_error_multiplicity++;
                }
                else v_error_vector += "0";
            ListBox1.Items.Add("         ");
            ListBox1.Items.Add("Вектор ошибок:");
            ListBox1.Items.Add(v_error_vector);
            ListBox1.Items.Add("Кратность ошибок: " + v_error_multiplicity.ToString());
            char[] v_correct_msg = (char[])v_received_msg.Clone();
            // исправление кода
            if (v_error_position > 0)
                if (v_correct_msg[v_error_position - 1] == '1')
                    v_correct_msg[v_error_position - 1] = '0';
                else v_correct_msg[v_error_position - 1] = '1';
            string v_information = "";
            // извлечение информационного сообщения
            for (int i = 1; i <= v_correct_msg.Length - v_amount_of_control_symbols; i++)
                v_information += v_correct_msg[i - 1];
            ListBox1.Items.Add("Информационное сообщение:");
            ListBox1.Items.Add(v_information);
        }
    }
}