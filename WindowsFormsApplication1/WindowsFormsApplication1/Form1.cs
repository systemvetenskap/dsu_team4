using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        DataTable Person = new DataTable();
        DataTable Golfer = new DataTable();
        
        

        public Form1()
        {
            InitializeComponent();
            //Person.Columns.Add("Id");
            //Person.Columns.Add("firstName");
            //Person.Columns.Add("lastName");
            //Person.Columns.Add("streetAddres");
            //Person.Columns.Add("postalCode");
            //Person.Columns.Add("city");
            //Person.Columns.Add("email");
            //Person.Columns.Add("gender_ID");
            //Person.Columns.Add("Golfer_ID");
            //Person.Columns.Add("memberType_Id");
            //Person.Columns.Add("PW");

            //Golfer.Columns.Add("Id");
            //Golfer.Columns.Add("golfID");
            //Golfer.Columns.Add("HCP");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cstring = "Data Source=(localdb)\\ProjectsV12;Initial Catalog=mellanhand;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
            SqlConnection conn = new SqlConnection(cstring);
            string q = "SELECT Förnamn, Efternamn, Adress, Postnummer, Ort, Epost, Kön, hcp, golfID, Medlemskategori from mellanHand";

            

            SqlDataAdapter da = new SqlDataAdapter(q, conn);

            da.Fill(Person);
            dataGridView1.DataSource = Person;
            foreach(DataRow r in Person.Rows)
            {
                if (r[9].ToString() == "Senior")
                {
                    r[9] = "4";
                }
                else if (r[9].ToString() == "Junior 13 - 21 år")
                {
                    r[9] = "3";

                }
                else if (r[9].ToString() == "Junior 0 - 12 år")
                {
                    r[9] = "2";
                }
                else if (r[9].ToString() == "Studerande")
                {
                    r[9] = "1";
                }



                if (r[6].ToString() == "Male")
                {
                    r[6] = "1";
                }
                else if (r[6].ToString() == "Female")
                {
                    r[6] = "2";

                }


            }
            dataGridView2.DataSource = Person;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cStringAzure = "Data Source=dsuteam4.database.windows.net;Initial Catalog=dsuteam4;Persist Security Info=True;User ID=dusadmin;Password=Team42016";

            SqlConnection connAzure = new SqlConnection(cStringAzure);

            foreach(DataRow rw in Person.Rows)
            {
                int lastid = 0;

                string qPerson = "INSERT INTO Person(firstName, lastName, streetAddres, postalCode, city, email, gender_ID, memberType_ID, PW) VALUES(@firstName, @lastName, @streetAddres, @postalCode, @city, @email, @gender_ID, @memberType_ID, @PW)";
                string lastID = "SELECT MAX(Id) from Person";
                

                SqlCommand com = new SqlCommand(qPerson, connAzure);

                com.Parameters.AddWithValue("firstName", rw[0].ToString());
                com.Parameters.AddWithValue("lastName", rw[1].ToString());
                com.Parameters.AddWithValue("streetAddres", rw[2].ToString());
                com.Parameters.AddWithValue("postalCode", rw[3].ToString());
                com.Parameters.AddWithValue("city", rw[4].ToString());
                com.Parameters.AddWithValue("email", rw[5].ToString());
                com.Parameters.AddWithValue("gender_ID", rw[6].ToString());
                com.Parameters.AddWithValue("memberType_ID", rw[9].ToString());
                com.Parameters.AddWithValue("PW", "123");

                connAzure.Open();
                com.ExecuteNonQuery();
                connAzure.Close();
                SqlCommand com2 = new SqlCommand(lastID, connAzure);
                connAzure.Open();
                SqlDataReader r = com2.ExecuteReader();

               while(r.Read())
               {
                 lastid = Convert.ToInt16(r[0].ToString());
               }
               connAzure.Close();

                string addGolfer = "INSERT INTO Golfer(golfID, HCP, Person_ID) VALUES(@golfID, @HCP, @Person_ID)";
                SqlCommand com3 = new SqlCommand(addGolfer, connAzure);
                com3.Parameters.AddWithValue("golfID", rw[8].ToString());
                com3.Parameters.AddWithValue("HCP", rw[7].ToString());
                com3.Parameters.AddWithValue("Person_ID", lastid);
                connAzure.Open();
                com3.ExecuteNonQuery();

                connAzure.Close();

            }
            




        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cStringAzure = "Data Source=dsuteam4.database.windows.net;Initial Catalog=dsuteam4;Persist Security Info=True;User ID=dusadmin;Password=Team42016";
            SqlConnection connAzure = new SqlConnection(cStringAzure);
            string addTimes = "INSERT INTO TeeTime(teeTime) VALUES(@teeTime)";
            
            
            for (int i = 7; i < 19; i++)
			{
                string del1;
			 
             if (i < 10)
             {
                del1 = "0"+ i.ToString();
             }
             else
             {
                del1 = i.ToString();
             }
                
                for (int z = 0; z < 60; z += 10)
			    {
                    string del2;
                    if (z == 0)
                    {
                        del2 = "0" + z.ToString();
                    }
                    else
                    {
			        del2 = z.ToString();
                    }
                    string hela = del1 +":"+ del2;
                    SqlCommand com3 = new SqlCommand(addTimes, connAzure);
                    com3.Parameters.AddWithValue("teeTime", hela);
                    connAzure.Open();
                    com3.ExecuteNonQuery();
                    connAzure.Close();
			    }
			
            
            }
        
        }
    }
}
