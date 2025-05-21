using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

namespace TI_Lab4
{
    public partial class MainForm : Form
    {
        private BigInteger p;
        private BigInteger q;
        private BigInteger r;
        private BigInteger rEuler;
        private BigInteger exp; //экспонента 
        private BigInteger d; //секретное значение

        private BigInteger m; //хеш-образ
        private BigInteger s; //электронная подпись

        bool isText = true;

        private string inputData = "";

        private FileManager fileManager = new FileManager();

        public MainForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            radioC.BackColor = Color.LightGreen;
            radioD.BackColor = Color.White;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Выход",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void button_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateButtonStyle(sender as Button, FontStyle.Bold | FontStyle.Underline);
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            UpdateButtonStyle(sender as Button, FontStyle.Regular);
        }

        private void radioButton_Click(object sender, EventArgs e)
        {
            radioC.BackColor = (sender == radioC) ? Color.LightGreen : Color.White;
            radioD.BackColor = (sender == radioD) ? Color.LightGreen : Color.White;

            labelD.Text = radioC.Checked ? "D:" : "E:";
            labelE.Visible = radioC.Checked;
            textBoxE.Visible = radioC.Checked;
            label4.Visible = radioC.Checked;
            textBoxm.Visible = radioC.Checked;

            ClearAll();
        }

        private void ClearAll()
        {
            p = BigInteger.Zero;
            q = BigInteger.Zero;
            r = BigInteger.Zero;        
            rEuler = BigInteger.Zero;    
            exp = BigInteger.Zero;      
            d = BigInteger.Zero;        

            m = BigInteger.Zero;         
            s = BigInteger.Zero;

            inputData = "";

            textBoxR.Clear();
            textBoxREuler.Clear();
            textBoxE.Clear();
            textBoxD.Clear();

            textBoxDataInput.Clear();
            textBoxEDS.Clear();
            textBoxm.Clear();

            isText = true;
            buttonConvert.Text = "10cc";
        }

        private void ProcessEncryption()
        {
            m = EDS.ComputeHash(inputData, r);
            s = DSAlg.ModPow(m, d, r);

            textBoxm.Text = m.ToString(); // хеш-образ
            textBoxEDS.Text = s.ToString(); // ЭЦП
        }

        private void ProcessDecryption()
        {
            BigInteger m1 = DSAlg.ModPow(s, exp, r);
            BigInteger m2 = EDS.ComputeHash((fileManager.RemoveLastLine(inputData)), r);

            MessageBox.Show(
                $"ХЕШ-образ, вычисленный с помощью открытого ключа: {m1}\n" +
                $"ХЕШ-образ, вычисленный на основе текущего сообщения: {m2}\n" +
                $"Подпись {(m1 == m2 ? "корректна" : "НЕКОРРЕКТНА")}!",
                "Результат"
            );
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            if (radioC.Checked)
            {
                if (!ValidateInputsC()) {
                    ShowError("Заполните все поля!");
                    return; 
                }

                p = BigInteger.Parse(DSAlg.CheckForNum(textBoxInputP.Text));
                q = BigInteger.Parse(DSAlg.CheckForNum(textBoxInputQ.Text));

                if (!DSAlg.IsPrime(p) && p > 0)
                {
                    ShowError($"Число {p} не является простым!(P)");
                    return;
                }                
                
                if (!DSAlg.IsPrime(q) && q > 0)
                {
                    ShowError($"Число {q} не является простым!(Q)");
                    return;
                }

                r = p * q;

                if (r < 256)
                {
                    ShowError($"Значение R должно быть не меньше 256!");
                    return;
                }

                rEuler = DSAlg.EulerAlg(r);

                d = BigInteger.Parse(DSAlg.CheckForNum(textBoxD.Text));

                if (d < 1 || d >= rEuler)
                {
                    ShowError($"Значение D должно быть больше 1 и меньше функции Эйлера!");
                    return;
                }

                if (!DSAlg.CheckD(d, rEuler))
                {
                    ShowError($"Число D должно быть взаимнопростым со значением функции Эйлера!");
                    return;
                }

                var resultE = (rEuler > d)
                ? DSAlg.EuclidRev(rEuler, d)
                : DSAlg.EuclidRev(d, rEuler);

                exp = resultE;

                DisplayNums();

                ProcessEncryption();
            }
            else
            {
                if (!ValidateInputsD())
                {
                    ShowError("Заполните все поля!");
                    return;
                }

                p = BigInteger.Parse(DSAlg.CheckForNum(textBoxInputP.Text));
                q = BigInteger.Parse(DSAlg.CheckForNum(textBoxInputQ.Text));

                if (!DSAlg.IsPrime(p) && p > 0)
                {
                    ShowError($"Число {p} не является простым!(P)");
                    return;
                }

                if (!DSAlg.IsPrime(q) && q  >0)
                {
                    ShowError($"Число {q} не является простым!(Q)");
                    return;
                }

                r = p * q;

                if (r < 256)
                {
                    ShowError($"Значение R должно быть не меньше 256!");
                    return;
                }

                rEuler = DSAlg.EulerAlg(r);

                exp = BigInteger.Parse(DSAlg.CheckForNum(textBoxD.Text));

                if (exp < 1 || exp >= rEuler)
                {
                    ShowError($"Значение e должно быть больше 1 и меньше функции Эйлера!");
                    return;
                }

                if (!DSAlg.CheckD(exp, rEuler))
                {
                    ShowError($"Число e должно быть взаимнопростым со значением функции Эйлера!");
                    return;
                }

                DisplayNums();

                ProcessDecryption();
            }
        }

        private void DisplayNums()
        {
            textBoxR.Text = r.ToString();
            textBoxREuler.Text = rEuler.ToString();
            textBoxE.Text = exp.ToString();
            textBoxD.Text = radioC.Checked ? d.ToString() : exp.ToString();
        }

        private void buttonReadData_Click(object sender, EventArgs e)
        {
            string newData = fileManager.ReadFile(openFileDialog);

            if (newData == null) return;

            inputData = newData;

            if (radioD.Checked)
            {
                textBoxDataInput.Text = fileManager.RemoveLastLine(inputData);
                s = fileManager.GetIntFromLastLine(inputData);
                textBoxEDS.Text = s.ToString();
            } else
                textBoxDataInput.Text = inputData;

            isText = true;
            buttonConvert.Text = "10cc";
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            fileManager.SaveFileWithEDS(inputData, s);
        }

        private void UpdateButtonStyle(Button button, FontStyle style)
        {
            if (button != null)
            {
                button.Font = new Font(button.Font.FontFamily, 8.25f, style);
            }
        }

        private bool ValidateInputsC()
        {
            return (DSAlg.CheckForNum(textBoxInputP.Text) != null &&
                DSAlg.CheckForNum(textBoxInputQ.Text) != null &&
                DSAlg.CheckForNum(textBoxD.Text) != null);
        }

        private bool ValidateInputsD()
        {
            return (ValidateInputsC() && inputData != "");
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            if (inputData != null && textBoxDataInput.TextLength != 0) {
                textBoxDataInput.Text = isText? Convert.StringToDecimalBytes(inputData) : Convert.DecimalBytesToString(textBoxDataInput.Text);
                isText = !isText;
                buttonConvert.Text = isText ? "В 10cc" : "В текст";
            }
        }
    }
}