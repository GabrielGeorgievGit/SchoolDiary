﻿using MySql.Data.MySqlClient;
using WebAPI.DB_connection;
using WebAPI.Models;

namespace WebAPI.Queries
{
    public class SchoolQuery
    {
        private DBConnection connection;
        public SchoolQuery() {
            this.connection = new DBConnection();
        }
        User user;
        public School readSchool()
        {

            string query = "SELECT * FROM school WHERE id_school=@ishoolId";
            connection.open();

            MySqlCommand command = new MySqlCommand(query, connection.conn);
            command.Parameters.Add("@shooId", MySqlDbType.Int32).Value = user.schoolId;

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                School school = new School(reader.GetString("name"), reader.GetString("type"));
                reader.Close();
                connection.close();
                return school;
            }
            reader.Close();
            connection.close();
           
            Console.WriteLine("\n  school of user " + user.name + "is not found\n");
            return new School("N/A", "N/A");
        }

        public School registerSchool(School school) {
            string query = "INSERT INTO SCHOOL(name, type) VALUES(@name, @type);";
            this.connection.open();

            MySqlCommand cmd = new MySqlCommand(query, connection.conn);
            cmd.Parameters.Add("@name", MySqlDbType.String).Value = school.Name;
            cmd.Parameters.Add("@type", MySqlDbType.String).Value = school.Type;

            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            connection.close();

            Finder.school = school;
            setAdminSchool(cmd.LastInsertedId);
            fillSchoolClasses(findSchoolId(school.Name));

            return school;
        }

        public static int findSchoolId(String name) {
            DBConnection connection = new DBConnection();
            MySqlCommand command;
            connection.open();
            string query = "SELECT ID_SCHOOL FROM SCHOOL WHERE NAME=@name";
            MySqlDataReader reader;
            command = new MySqlCommand(query, connection.conn);
            command.Parameters.Add("@name", MySqlDbType.VarChar, 45).Value = name;

            reader = command.ExecuteReader();
            int id = -50;
            if (reader.Read()) {
                id = reader.GetInt32("ID_SCHOOL");
            }
            reader.Close();
            connection.close();
            return id;
        }

        public void fillSchoolClasses(int id) {
            int[] classes = new int[12];
            for(int i = 1; i < 13; ++i) {
                classes[i-1] = i;
            }
            DBConnection connection = new DBConnection();
            MySqlCommand command;
            connection.open();
            string query = "INSERT INTO GRADE(CLASS, GRADE, ID_SCHOOL, ID_CLASS_TEACHER) VALUES(@class, @grade, @idSchool, NULL)";
            string[] grades = new string[5] { "a", "b", "c", "d", "e" };
            for(int i = 1; i < 13; ++i) {
                for (int j = 0; j < 5; ++j) {
                    command = new MySqlCommand(query, connection.conn);
                    command.Parameters.Add("@class", MySqlDbType.Int32).Value = i;
                    command.Parameters.Add("@grade", MySqlDbType.String).Value = grades[j];
                    command.Parameters.Add("@idSchool", MySqlDbType.Int32).Value = id;
                    command.ExecuteReader().Close();
                }
            }

            connection.close();
        }

        public void setAdminSchool(long idSchool)
        {
            this.user = Signer.user;
            string query = "UPDATE SCHOOL_ADMINISTRATOR set id_school=@id WHERE id_school_administrator=@admin_id;";
            this.connection.open();

            MySqlCommand cmd = new MySqlCommand(query, connection.conn);
            cmd.Parameters.Add("@id", MySqlDbType.String).Value = idSchool;
            cmd.Parameters.Add("@admin_id", MySqlDbType.String).Value = user.id;

            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            connection.close();

            Signer.user.schoolId = (int)(idSchool);
        }

        public void schoolEdit(string name, string type)
        {
            this.user = Signer.user;
            DBConnection connection = new DBConnection();
            string query = "UPDATE school SET name=@name , type=@type WHERE id_school=@schoolId";
            connection.open();
            MySqlCommand comand = new MySqlCommand(query, connection.conn);
            comand.Parameters.Add("@name", MySqlDbType.VarChar, 45).Value = name;
            comand.Parameters.Add("@type", MySqlDbType.VarChar, 3).Value = type;
            comand.Parameters.Add("@schoolId", MySqlDbType.Int32).Value = user.schoolId;
            comand.ExecuteNonQuery();
            connection.close();
        }
        public void schoolDelite()
        {
            DBConnection connection = new DBConnection();
            string query = "DELITE FROM school WHERE id_shool=@ischoolId";
            connection.open();
            MySqlCommand comand = new MySqlCommand(query, connection.conn);
            comand.Parameters.Add("@shooId", MySqlDbType.Int32).Value = user.schoolId;
            comand.ExecuteNonQuery();
            connection.close();
        }

    }
}
