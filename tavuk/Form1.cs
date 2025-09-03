using System.IO.Ports;
namespace tavuk
{

    public partial class Form1 : Form
    {
        private readonly System.Windows.Forms.Timer _timer =new System.Windows.Forms.Timer { Interval = 1000 };
        public Form1()
        {
            InitializeComponent();
            _timer.Tick += Timer_Tick;
            _timer.Start();
            Timer_Tick(this, EventArgs.Empty); // Form açýlýr açýlmaz bir kez güncelle
        }
        private SerialPort port = new SerialPort();
        private void Form1_Load(object sender, EventArgs e)
        {



            //port kýsmý
            if (comboBox1.Items.Count > 0)                    // En az bir port varsa
                comboBox1.SelectedIndex = 0;
            button3.Text = "Baðla";

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }
        int toplamAgirlik = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            int basan = int.Parse(textBox1.Text);
            if (basan == 1)
            {
                Random rnd = new Random();
                int agirlik = rnd.Next(0, 2001);

                toplamAgirlik += agirlik;

                richTextBox1.AppendText("Toplam aðýrlýk: " + toplamAgirlik.ToString() + " gram\n\n");
            }
            else
            {
                richTextBox1.AppendText(textBox1.Text);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            toplamAgirlik = 0;
            richTextBox1.AppendText("Aðýrlýk Sýfýrlandý");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (port.IsOpen)
            {
                port.Close();
                button3.Text = "Baðla";
                comboBox1.Enabled = true;
            }
            else
            {
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen port seçin.");
                    return;
                }
                port.PortName = comboBox1.SelectedItem.ToString();
                port.BaudRate = 9600;
                port.Open();
                button3.Text = "Durdur";
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (port.IsOpen) port.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            label1.Text = DateTime.Now.ToString("HH:mm tt");
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            label1.Text = now.ToString("hh:mm");
            label2.Text = TimeToWords(now);
        }

        private static string TimeToWords(DateTime dt)
        {
            int h12 = dt.Hour % 12;
            int m = dt.Minute;

            int rounded = (int)Math.Round(m / 5.0) * 5;
            if (rounded == 60)
            {
                rounded = 0;
                h12 = (h12 + 1) % 12;
            }

            string[] hours = {
        "TWELVE","ONE","TWO","THREE","FOUR","FIVE",
        "SIX","SEVEN","EIGHT","NINE","TEN","ELEVEN"
    };

            if (rounded == 0)
                return $"IT IS {hours[h12]} O'CLOCK";

            if (rounded <= 30)
                return $"IT IS {MinuteWord(rounded)} PAST {hours[h12]}";

            int toValue = 60 - rounded;
            int nextHour = (h12 + 1) % 12;
            return $"IT IS {MinuteWord(toValue)} TO {hours[nextHour]}";
        }

        private static string MinuteWord(int m)
        {
            // Sadece 5 dakikalýk adýmlar gelecek
            switch (m)
            {
                case 5: return "FIVE";
                case 10: return "TEN";
                case 15: return "QUARTER";
                case 20: return "TWENTY";
                case 25: return "TWENTY-FIVE";
                case 30: return "HALF";
                default: return m.ToString(); // Güvenlik amaçlý (normalde düþmez)
            }

        }
    }
}
