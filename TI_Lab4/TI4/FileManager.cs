using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace TI_Lab4
{
    class FileManager
    {
        public int GetIntFromLastLine(string fileContent)
        {
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                MessageBox.Show("Содержимое файла не может быть пустым!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; 
            }

            string[] lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
            {
                MessageBox.Show("Нет строк в содержимом файла!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            string lastLine = lines.Last().Trim();

            if (!int.TryParse(lastLine, out int result))
            {
                MessageBox.Show("Последняя строка не содержит допустимое целое число!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            return result;
        }

        public string RemoveLastLine(string fileContent)
        {
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                MessageBox.Show("Содержимое файла не может быть пустым!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            string[] lines = fileContent.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            if (lines.Length == 0)
            {
                MessageBox.Show("Нет строк в содержимом файла!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            string lastLine = lines.Reverse().FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim();

            if (lastLine == null)
            {
                MessageBox.Show("Нет строк с данными!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            int lastNonEmptyLineIndex = lines.Length - 1 - lines.Reverse().TakeWhile(string.IsNullOrWhiteSpace).Count();
            return string.Join(Environment.NewLine, lines.Take(lastNonEmptyLineIndex));
        }

        public string ReadFile(OpenFileDialog openFileDialog)
        {
            try
            {
                openFileDialog.Filter = "Все файлы (*.*)|*.*";
                openFileDialog.Title = "Открыть файл";
                openFileDialog.RestoreDirectory = true;

                return openFileDialog.ShowDialog() == DialogResult.OK
                    ? File.ReadAllText(openFileDialog.FileName)
                    : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void SaveFileWithEDS(string content, BigInteger number)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveDialog.Title = "Сохранить файл";
                saveDialog.DefaultExt = ".txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string fullContent = string.IsNullOrEmpty(content)
                            ? number.ToString()
                            : $"{content}{Environment.NewLine}{number}";

                        File.WriteAllText(saveDialog.FileName, fullContent);
                        MessageBox.Show("Файл успешно сохранён!", "Успех",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}