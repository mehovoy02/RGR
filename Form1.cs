﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkingWithDB
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;//чтобы законектиться к БД создаём объект
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Артём Вячеславович\documents\visual studio 2013\Projects\WorkingWithDB\WorkingWithDB\Database.mdf;Integrated Security=True";

            sqlConnection = new SqlConnection(connectionString);//создаём экземпляр класса sqlConnection

            await sqlConnection.OpenAsync();//параллельное открытие соединения с БД 

            SqlDataReader sqlReader = null;//нужно получить содержимое БД

            SqlCommand command = new SqlCommand("SELECT * FROM [Accounting]", sqlConnection);//пишем первый запрос

            try
            {
                sqlReader = await command.ExecuteReaderAsync();//метод считывания таблицы

                while (await sqlReader.ReadAsync())//метод перемещения к следующей записи таблицы
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Id"]) + "    " + Convert.ToString(sqlReader["Student"]) + "    " + Convert.ToString(sqlReader["Teacher"]) + "    " + Convert.ToString(sqlReader["Discipline"]) + "    " + Convert.ToString(sqlReader["Evaluation"]));//заполняем листбокс
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)//чтобы код в обработчике событий выполнился/невыполнился,закрываем наш sqlReader
                    sqlReader.Close();
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)//чтобы не было утечки данных, закрываем соединение
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)//если соединение не пусто и не закрыто уже
                Application.Exit();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            if (label7.Visible)//обрабатываем исключение
                label7.Visible = false;

            if (!String.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
               !String.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text)&&
                !String.IsNullOrEmpty(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox8.Text)&&
                !String.IsNullOrEmpty(textBox7.Text) && !string.IsNullOrWhiteSpace(textBox7.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Accounting] (Student, Teacher, Discipline, Evaluation)VALUES(@Student, @Teacher, @Discipline, @Evaluation)", sqlConnection);

                command.Parameters.AddWithValue("Student", textBox1.Text);//добавляем значения Student, Teacher, Discipline, Evaluation,чтобы БД их видела

                command.Parameters.AddWithValue("Teacher", textBox2.Text);

                command.Parameters.AddWithValue("Discipline", textBox8.Text);

                command.Parameters.AddWithValue("Evaluation", textBox7.Text);

                await command.ExecuteNonQueryAsync();//т.к. это INSERT И нам не нужно ничего возвращать
            }
            else
            {
                label7.Visible = true;

                label7.Text = "Поля 'Студент', 'Преподаватель','Дисциплина' и 'Оценка' должны быть заполнены!";
            }
        }
        private async void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SELECT * FROM [Accounting]", sqlConnection);

            try
            {
                sqlReader = await command.ExecuteReaderAsync();//считывание таблицы

                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Id"]) + "    " + Convert.ToString(sqlReader["Student"]) + "    " + Convert.ToString(sqlReader["Teacher"]) + "    " + Convert.ToString(sqlReader["Discipline"]) + "    " + Convert.ToString(sqlReader["Evaluation"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            if (label8.Visible)//обрабатываем исключение
                label8.Visible = false;

            if (!String.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) &&
               !String.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text)&&
               !String.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text)&&
               !String.IsNullOrEmpty(textBox9.Text) && !string.IsNullOrWhiteSpace(textBox9.Text)&&
               !String.IsNullOrEmpty(textBox10.Text) && !string.IsNullOrWhiteSpace(textBox10.Text))
            {
                SqlCommand command = new SqlCommand("UPDATE [Accounting] SET [Student]=@Student, [Teacher]=@Teacher, [Discipline]=@Discipline, [Evaluation]=@Evaluation WHERE [Id]=@Id", sqlConnection);

                command.Parameters.AddWithValue("Id",textBox5.Text);
                command.Parameters.AddWithValue("Student", textBox4.Text);
                command.Parameters.AddWithValue("Teacher", textBox3.Text);
                command.Parameters.AddWithValue("Discipline", textBox9.Text);
                command.Parameters.AddWithValue("Evaluation", textBox10.Text);

                await command.ExecuteNonQueryAsync();
            }
            else if (!String.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text))
            {
                label8.Visible = true;

                label8.Text = "ID должен быть заполнен!";
            }
            else
            {
                label8.Visible = true;

                label8.Text = "Поля 'Id', 'Студент', 'Преподаватель','Дисциплина' и 'Оценка' должны быть заполнены!";
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (label9.Visible)//обрабатываем исключение
                label9.Visible = false;

            if (!String.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text)) 
            {
                SqlCommand command = new SqlCommand("DELETE FROM [Accounting] WHERE [Id]=@Id", sqlConnection);

                command.Parameters.AddWithValue("Id", textBox6.Text);

                await command.ExecuteNonQueryAsync();

            }
            else
            {
                label9.Visible = true;

                label9.Text = "Поле 'ID' должно быть заполнено!";
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("WorkingWithDB\nArtem Altinpara, 2020", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}